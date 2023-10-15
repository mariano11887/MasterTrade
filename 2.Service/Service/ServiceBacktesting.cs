using _2.Service.Indicator.Interface;
using _3.Repository;
using _3.Repository.Repository;
using _4.DTO;
using System.Collections.Generic;
using System.Linq;
using IndicatorMeta = _2.Service.Indicator.Interface.IndicatorMeta;

namespace _2.Service.Service
{
    public class ServiceBacktesting
    {
        private readonly RepositoryCandle repositoryCandle;
        private readonly RepositoryCryptoPair repositoryCryptoPair;

        private readonly ServiceIndicator serviceIndicator;

        public ServiceBacktesting()
        {
            repositoryCandle = new RepositoryCandle();
            repositoryCryptoPair = new RepositoryCryptoPair();

            serviceIndicator = new ServiceIndicator();
        }

        public DTOBacktestingResult Execute(DTOBacktestingParameters parameters)
        {
            // Obtengo los datos de las velas del rango de fechas
            List<Candle> candles = repositoryCandle.GetQuery()
                                                   .Where(c => c.CryptoPairId == parameters.CryptoPairId && c.StartDate >= parameters.DateFrom && c.StartDate <= parameters.DateTo)
                                                   .OrderBy(c => c.StartDate)
                                                   .ToList();

            // Las agrupo según la temporalidad elegida
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

                if (counter == parameters.CandlesGroupingAmount)
                {
                    counter = 0;
                    lastGroupedCandle.Close = candle.Close;
                }
                else
                {
                    if (lastGroupedCandle.High < candle.High)
                    {
                        lastGroupedCandle.High = candle.High;
                    }
                    if (lastGroupedCandle.Low > candle.Low)
                    {
                        lastGroupedCandle.Low = candle.Low;
                    }
                }
            }

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

            // Calculo los resultados
            decimal currentCapital = 1000;
            DTOBacktestingResult result = new DTOBacktestingResult
            {
                InitialCapital = currentCapital,
                StrategyName = parameters.Strategy.Name,
                CryptoPair = repositoryCryptoPair.GetQuery().Where(cp => cp.Id == parameters.CryptoPairId).Select(cp => cp.Name).FirstOrDefault()
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

                result.Operations.Add(new DTOBacktestingOperation
                {
                    OpenDate = operation.OpenCandle.StartDate,
                    CloseDate = operation.CloseCandle.StartDate,
                    InitialCapital = currentCapital,
                    FinalCapital = finalCapital
                });

                currentCapital = finalCapital;
            }

            result.FinalCapital = currentCapital;
            result.MaxDrawdown = troughCapital.HasValue ? (troughCapital.Value - peakCapital) / peakCapital : 0;
            result.WinRate = (decimal)winOperations / operations.Count;

            return result;
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
    }
}
