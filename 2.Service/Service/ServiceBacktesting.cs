using _2.Service.Indicator.Interface;
using _3.Repository;
using _3.Repository.Repository;
using _4.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using IndicatorMeta = _2.Service.Indicator.Interface.IndicatorMeta;

namespace _2.Service.Service
{
    public class ServiceBacktesting
    {
        private readonly RepositoryCandle repositoryCandle;
        private readonly RepositoryCryptoPair repositoryCryptoPair;
        private readonly RepositoryTemporality repositoryTemporality;
        private readonly RepositoryBacktestingBatch repositoryBacktestingBatch;
        private readonly RepositoryBacktesting repositoryBacktesting;

        private readonly ServiceIndicator serviceIndicator;
        private readonly ServiceStrategy serviceStrategy;

        public ServiceBacktesting()
        {
            repositoryCandle = new RepositoryCandle();
            repositoryCryptoPair = new RepositoryCryptoPair();
            repositoryTemporality = new RepositoryTemporality();
            repositoryBacktestingBatch = new RepositoryBacktestingBatch();
            repositoryBacktesting = new RepositoryBacktesting();

            serviceIndicator = new ServiceIndicator();
            serviceStrategy = new ServiceStrategy();
        }

        public DTOBacktestingWithRangesResult ExecuteWithRanges(DTOBacktestingWithRangesParameters parameters)
        {
            // Creo una iteración de cada combinación de los rangos elegidos, los ejecuto todos y obtengo el mejor resultado
            List<List<decimal>> configurationsValues = new List<List<decimal>>();
            foreach (DTOBacktestingWithRangesIndicatorConfiguration indicatorConfiguration in parameters.IndicatorConfigurations)
            {
                List<decimal> indicatorValues = new List<decimal>();
                for (decimal value = indicatorConfiguration.MinValue; value <= indicatorConfiguration.MaxValue; value += indicatorConfiguration.Increment)
                {
                    indicatorValues.Add(value);
                }
                configurationsValues.Add(indicatorValues);
            }

            List<Candle> groupedCandles = GetGroupedCandles(parameters);
            List<DTOBacktestingResult> backtestingsResults = new List<DTOBacktestingResult>();
            List<List<decimal>> combinations = GenerateCombinations(configurationsValues);

            BacktestingBatch backtestingBatch = new BacktestingBatch
            {
                Date = DateTime.Now,
                StrategyId = parameters.Strategy.Id,
                TemporalityId = parameters.TemporalityId,
                DateFrom = parameters.DateFrom,
                DateTo = parameters.DateTo
            };

            foreach (List<decimal> combination in combinations)
            {
                DTOBacktestingParameters combinationParameters = new DTOBacktestingParameters
                {
                    CryptoPairId = parameters.CryptoPairId,
                    DateFrom = parameters.DateFrom,
                    DateTo = parameters.DateTo,
                    TemporalityId = parameters.TemporalityId,
                    Strategy = parameters.Strategy
                };

                int index = 0;

                foreach (DTOStrategyCondition condition in combinationParameters.Strategy.Conditions)
                {
                    index = 0;
                    if (condition.FirstIndicatorMeta.Indicator.Configurations.Any())
                    {
                        condition.FirstIndicatorMeta.Indicator.Configurations.First().Value = combination[index++].ToString();
                    }
                    if (condition.SecondIndicatorMeta.Indicator.Configurations.Any())
                    {
                        condition.SecondIndicatorMeta.Indicator.Configurations.First().Value = combination[index].ToString();
                    }
                }

                index = 0;
                if (combinationParameters.Strategy.Indicators[0].Configurations.Any())
                {
                    combinationParameters.Strategy.Indicators[0].Configurations.First().Value = combination[index++].ToString();
                }
                if (combinationParameters.Strategy.Indicators.Count > 1 && combinationParameters.Strategy.Indicators[1].Configurations.Any())
                {
                    combinationParameters.Strategy.Indicators[1].Configurations.First().Value = combination[index].ToString();
                }

                DTOBacktestingResult combinationResult = Execute(combinationParameters, groupedCandles, backtestingBatch);
                backtestingsResults.Add(combinationResult);
            }

            try
            {
                repositoryBacktestingBatch.Insert(backtestingBatch);
                repositoryBacktestingBatch.SaveChanges();
            }
            catch { }

            // De los backtestings, obtengo el que mejor rendimiento tuvo
            decimal bestRevenue = decimal.MinValue;
            Backtesting bestBacktesting = null;
            foreach (Backtesting backtesting in backtestingBatch.Backtestings)
            {
                BacktestingOperation lastOperation = backtesting.BacktestingOperations.OrderBy(bo => bo.OpenDate).Last();
                decimal revenue = lastOperation.InitialCapital + lastOperation.Revenue;
                if (revenue > bestRevenue)
                {
                    bestRevenue = revenue;
                    bestBacktesting = backtesting;
                }
            }

            bestBacktesting = repositoryBacktesting.GetQuery().First(b => b.Id == bestBacktesting.Id); // Para cargar todos los datos del backtesting
            DTOStrategy strategyDTO = serviceStrategy.GetById(parameters.Strategy.Id, parameters.Strategy.UserId);
            int c = 0;
            DTOBacktestingWithRangesResult result = new DTOBacktestingWithRangesResult
            {
                OptimalIndicators = bestBacktesting.Indicators.Where(i => i.IndicatorMetas.Any()).Select(i => new DTOBacktestingWithRangesResultOptimalIndicator
                {
                    IndicatorName = strategyDTO.Indicators[c++].ToString(),
                    ConfigurationName = i.IndicatorMetas.First().Name,
                    ConfigurationValue = decimal.Parse(i.IndicatorMetas.First().Value)
                }).ToList(),
                Backtestings = backtestingBatch.Backtestings.OrderByDescending(b => b.BacktestingOperations.OrderBy(bo => bo.OpenDate).Last().InitialCapital + b.BacktestingOperations.OrderBy(bo => bo.OpenDate).Last().Revenue).Select(b => new DTOBacktestingWithRangesResultBacktesting
                {
                    BacktestingId = b.Id,
                    InitialCapital = b.BacktestingOperations.OrderBy(bo => bo.OpenDate).First().InitialCapital,
                    FinalCapital = b.BacktestingOperations.OrderBy(bo => bo.OpenDate).Last().InitialCapital + b.BacktestingOperations.OrderBy(bo => bo.OpenDate).Last().Revenue
                }).ToList()
            };

            return result;
        }

        private List<List<decimal>> GenerateCombinations(List<List<decimal>> configurationsValues)
        {
            List<List<decimal>> results = new List<List<decimal>>();
            GenerateCominationsRecursive(configurationsValues, results, new List<decimal>(), 0);
            return results;
        }

        private void GenerateCominationsRecursive(List<List<decimal>> configurationsValues, List<List<decimal>> results, List<decimal> currentCombination, int currentList)
        {
            if (currentList == configurationsValues.Count)
            {
                results.Add(currentCombination.ToList());
                return;
            }

            foreach (decimal value in configurationsValues[currentList])
            {
                currentCombination.Add(value);
                GenerateCominationsRecursive(configurationsValues, results, currentCombination, currentList + 1);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        public DTOBacktestingResult Execute(DTOBacktestingParameters parameters)
        {
            BacktestingBatch backtestingBatch = new BacktestingBatch
            {
                Date = DateTime.Now,
                StrategyId = parameters.Strategy.Id,
                TemporalityId = parameters.TemporalityId,
                DateFrom = parameters.DateFrom,
                DateTo = parameters.DateTo
            };

            DTOBacktestingResult result = Execute(parameters, GetGroupedCandles(parameters), backtestingBatch);

            try
            {
                repositoryBacktestingBatch.Insert(backtestingBatch);
                repositoryBacktestingBatch.SaveChanges();
            }
            catch { }

            return result;
        }

        public DTOBacktestingResult Execute(DTOBacktestingParameters parameters, List<Candle> groupedCandles, BacktestingBatch backtestingBatch)
        {
            List<Operation> operations = new List<Operation>();
            Operation currentOperation = null;

            // Voy recorriendo las velas agrupadas y chequeando si se cumplen las condiciones de apertura y cierre
            List<DTOStrategyCondition> openConditions = parameters.Strategy.Conditions.Where(c => c.IsOpenCondition).ToList();
            List<DTOStrategyCondition> closeConditions = parameters.Strategy.Conditions.Where(c => !c.IsOpenCondition).ToList();

            for (int i = 0; i < groupedCandles.Count; i++)
            {
                Candle candle = groupedCandles[i];

                if (currentOperation == null)
                {
                    // Busco que condiciones de apertura que se cumplan
                    int matchingConditions = 0;

                    foreach (DTOStrategyCondition condition in openConditions)
                    {
                        bool conditionMatches = ConditionMatches(condition, groupedCandles, i);
                        matchingConditions += conditionMatches ? 1 : 0;
                    }

                    if (matchingConditions == openConditions.Count)
                    {
                        currentOperation = new Operation
                        {
                            OpenCandle = candle
                        };
                    }
                }
                else
                {
                    // Busco condiciones de cierre
                    foreach (DTOStrategyCondition condition in closeConditions)
                    {
                        bool conditionMatches = ConditionMatches(condition, groupedCandles, i);
                        if (conditionMatches)
                        {
                            currentOperation.CloseCandle = candle;
                            operations.Add(currentOperation.Clone());
                            currentOperation = null;
                            break;
                        }
                    }
                }
            }

            // Preparo las entidades de Backtesting para su guardado
            Backtesting backtesting = new Backtesting
            {
                Date = DateTime.Now
            };

            foreach (DTOIndicator indicatorDTO in parameters.Strategy.Indicators)
            {
                _3.Repository.Indicator indicator = new _3.Repository.Indicator
                {
                    TypeId = indicatorDTO.TypeId
                };

                foreach (DTOIndicatorConfiguration configuration in indicatorDTO.Configurations)
                {
                    indicator.IndicatorMetas.Add(new _3.Repository.IndicatorMeta
                    {
                        Name = configuration.Name,
                        Value = configuration.Value,
                        IndicatorMetaDataTypeId = (int)configuration.Type
                    });
                }

                backtesting.Indicators.Add(indicator);
            }

            // Calculo los resultados
            decimal currentCapital = 1000;
            DTOBacktestingResult result = new DTOBacktestingResult
            {
                InitialCapital = currentCapital,
                StrategyName = parameters.Strategy.Name,
                CryptoPair = repositoryCryptoPair.GetQuery().Where(cp => cp.Id == parameters.CryptoPairId).Select(cp => cp.Name).FirstOrDefault(),
                Temporality = repositoryTemporality.GetQuery().First(t => t.Id == parameters.TemporalityId).Description
            };

            decimal peakCapital = currentCapital;
            decimal? troughCapital = null;
            int winOperations = 0;

            foreach (Operation operation in operations)
            {
                decimal operationAmount = parameters.Strategy.InvestmentAmount ?? parameters.Strategy.InvestmentPercentage.Value * currentCapital / 100;
                decimal finalCapital = currentCapital - operationAmount + operationAmount * operation.CloseCandle.Close / operation.OpenCandle.Close;

                peakCapital = finalCapital > peakCapital ? finalCapital : peakCapital;

                if (!troughCapital.HasValue && finalCapital < currentCapital)
                {
                    troughCapital = finalCapital;
                }
                else
                {
                    troughCapital = finalCapital < troughCapital ? finalCapital : troughCapital;
                }

                winOperations += finalCapital > currentCapital ? 1 : 0;

                backtesting.BacktestingOperations.Add(new BacktestingOperation
                {
                    OpenDate = operation.OpenCandle.StartDate,
                    CloseDate = operation.CloseCandle.StartDate,
                    InitialCapital = currentCapital,
                    Revenue = finalCapital - currentCapital
                });

                result.Operations.Add(new DTOBacktestingOperation
                {
                    OpenDate = operation.OpenCandle.StartDate,
                    CloseDate = operation.CloseCandle.StartDate,
                    InitialCapital = currentCapital,
                    FinalCapital = finalCapital
                });

                currentCapital = finalCapital;
            }

            backtestingBatch.Backtestings.Add(backtesting);
            
            result.FinalCapital = currentCapital;
            result.MaxDrawdown = troughCapital.HasValue ? (troughCapital.Value - peakCapital) / peakCapital : 0;
            result.WinRate = (decimal)winOperations / operations.Count;

            return result;
        }

        private List<Candle> GetGroupedCandles(DTOBacktestingParameters parameters)
        {
            // Obtengo los datos de las velas del rango de fechas
            List<Candle> candles = repositoryCandle.GetQuery()
                                                   .Where(c => c.CryptoPairId == parameters.CryptoPairId && c.StartDate >= parameters.DateFrom && c.StartDate <= parameters.DateTo)
                                                   .OrderBy(c => c.StartDate)
                                                   .ToList();

            // Las agrupo según la temporalidad elegida
            Temporality temporality = repositoryTemporality.GetQuery().First(t => t.Id == parameters.TemporalityId);
            List<Candle> groupedCandles = new List<Candle>();
            int counter = 0;
            foreach (Candle candle in candles)
            {
                if (counter == 0)
                {
                    groupedCandles.Add(new Candle
                    {
                        StartDate = candle.StartDate,
                        High = candle.High,
                        Low = candle.Low,
                        Open = candle.Open,
                        Close = candle.Close
                    });
                }
                counter++;
                Candle lastGroupedCandle = groupedCandles.Last();

                if (lastGroupedCandle.High < candle.High)
                {
                    lastGroupedCandle.High = candle.High;
                }
                if (lastGroupedCandle.Low > candle.Low)
                {
                    lastGroupedCandle.Low = candle.Low;
                }

                if (counter == temporality.CandlesGroupingAmount)
                {
                    counter = 0;
                    lastGroupedCandle.Close = candle.Close;
                }
            }

            return groupedCandles;
        }

        private bool ConditionMatches(DTOStrategyCondition condition, List<Candle> groupedCandles, int candleIndex)
        {
            IIndicator firstIndicator = serviceIndicator.GetIndicator(condition.FirstIndicatorMeta.Indicator.TypeId);
            List<IndicatorMeta> firstIndicatorValues = firstIndicator.GetCurrentValues(groupedCandles, candleIndex, condition.FirstIndicatorMeta.Indicator.Configurations);
            if (firstIndicatorValues == null)
            {
                return false;
            }
            IndicatorMeta firstIndicatorValue = firstIndicatorValues.FirstOrDefault(v => v.Name == condition.FirstIndicatorMeta.Name);
            decimal castedFirstValue = decimal.Parse(firstIndicatorValue.Value);

            IIndicator secondInicator = serviceIndicator.GetIndicator(condition.SecondIndicatorMeta.Indicator.TypeId);
            List<IndicatorMeta> secondIndicatorValues = secondInicator.GetCurrentValues(groupedCandles, candleIndex, condition.SecondIndicatorMeta.Indicator.Configurations);
            if (secondIndicatorValues == null)
            {
                return false;
            }
            IndicatorMeta secondIndicatorValue = secondIndicatorValues.FirstOrDefault(v => v.Name == condition.SecondIndicatorMeta.Name);
            decimal castedSecondValue = decimal.Parse(secondIndicatorValue.Value);

            bool conditionMatches = false;
            switch (condition.Comparer)
            {
                case _4.DTO.Enums.Comparer.Equal:
                    conditionMatches = castedFirstValue == castedSecondValue;
                    break;
                case _4.DTO.Enums.Comparer.LowerOrEqual:
                    conditionMatches = castedFirstValue <= castedSecondValue;
                    break;
                case _4.DTO.Enums.Comparer.GreaterOrEqual:
                    conditionMatches = castedFirstValue >= castedSecondValue;
                    break;
                case _4.DTO.Enums.Comparer.Lower:
                    conditionMatches = castedFirstValue < castedSecondValue;
                    break;
                case _4.DTO.Enums.Comparer.Greater:
                    conditionMatches = castedFirstValue > castedSecondValue;
                    break;
            }

            return conditionMatches;
        }

        private class Operation
        {
            public Candle OpenCandle { get; set; }
            public Candle CloseCandle { get; set; }

            public Operation Clone()
            {
                return new Operation
                {
                    OpenCandle = OpenCandle,
                    CloseCandle = CloseCandle
                };
            }
        }

        public DTOBacktestingResult GetById(int backtestingId, int userId)
        {
            Backtesting backtesting = repositoryBacktesting.GetQuery().FirstOrDefault(b => b.Id == backtestingId);
            if (backtesting == null)
            {
                return null;
            }
            DTOStrategy strategyDTO = serviceStrategy.GetById(backtesting.BacktestingBatch.StrategyId, userId);
            int c = 0;

            DTOBacktestingResult result = new DTOBacktestingResult
            {
                IndicatorsConfig = backtesting.Indicators.Where(i => i.IndicatorMetas.Any()).Select(i => new DTOBacktestingResultIndicatorConfig
                {
                    IndicatorName = strategyDTO.Indicators[c++].ToString(),
                    ConfigurationName = i.IndicatorMetas.First().Name,
                    ConfigurationValue = decimal.Parse(i.IndicatorMetas.First().Value)
                }).ToList(),
                InitialCapital = backtesting.BacktestingOperations.OrderBy(bo => bo.OpenDate).First().InitialCapital,
                FinalCapital = backtesting.BacktestingOperations.OrderBy(bo => bo.OpenDate).Last().InitialCapital + backtesting.BacktestingOperations.OrderBy(bo => bo.OpenDate).Last().Revenue,
            };

            decimal peakCapital = result.InitialCapital;
            decimal? troughCapital = null;
            int winOperations = 0;
            foreach (BacktestingOperation operation in backtesting.BacktestingOperations)
            {
                decimal finalCapital = operation.InitialCapital + operation.Revenue;
                peakCapital = finalCapital > peakCapital ? finalCapital : peakCapital;

                if (!troughCapital.HasValue)
                {
                    troughCapital = finalCapital;
                }
                else
                {
                    troughCapital = finalCapital < troughCapital ? finalCapital : troughCapital;
                }

                winOperations += finalCapital > operation.InitialCapital ? 1 : 0;

                result.Operations.Add(new DTOBacktestingOperation
                {
                    OpenDate = operation.OpenDate,
                    CloseDate = operation.CloseDate,
                    InitialCapital = operation.InitialCapital,
                    FinalCapital = finalCapital
                });
            }
            result.MaxDrawdown = troughCapital.HasValue ? (troughCapital.Value - peakCapital) / peakCapital : 0;
            result.WinRate = (decimal)winOperations / backtesting.BacktestingOperations.Count;

            return result;
        }
    }
}
