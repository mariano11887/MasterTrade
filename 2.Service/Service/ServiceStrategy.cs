using _3.Repository;
using _3.Repository.Repository;
using _4.DTO;
using System.Collections.Generic;
using System.Linq;

namespace _2.Service.Service
{
    public class ServiceStrategy
    {
        private readonly RepositoryStrategy repositoryStrategy;
        private readonly RepositoryIndicator repositoryIndicator;
        private readonly RepositoryIndicatorConfiguration repositoryIndicatorConfiguration;
        private readonly RepositoryStrategyCondition repositoryStrategyCondition;

        public ServiceStrategy()
        {
            repositoryStrategy = new RepositoryStrategy();
            repositoryIndicator = new RepositoryIndicator();
            repositoryIndicatorConfiguration = new RepositoryIndicatorConfiguration();
            repositoryStrategyCondition = new RepositoryStrategyCondition();
        }

        public List<DTOStrategy> GetUserStrategies(int userId)
        {
            List<DTOStrategy> strategies = repositoryStrategy.GetQuery().Where(s => s.UserId == userId).Select(s => new DTOStrategy
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return strategies;
        }

        public bool CheckStrategyName(int userId, string strategyName)
        {
            bool isAvailable = !repositoryStrategy.GetQuery().Any(s => s.UserId == userId && s.Name == strategyName);
            return isAvailable;
        }

        public int Save(DTOStrategy dto)
        {
            Strategy strategy;
            if (dto.Id == 0)
            {
                strategy = new Strategy
                {
                    Name = dto.Name,
                    UserId = dto.UserId,
                    IsComplete = dto.IsComplete,
                };
                repositoryStrategy.Insert(strategy);
            }
            else
            {
                strategy = repositoryStrategy.GetQuery("Indicators.IndicatorConfigurations").FirstOrDefault(s => s.Id == dto.Id && s.UserId == dto.UserId);
                if (strategy == null)
                {
                    return 0;
                }

                strategy.Name = dto.Name;
                strategy.InvestmentAmount = dto.InvestmentAmount;
                strategy.InvestmentPercentage = dto.InvestmentPercentage;
                strategy.IsComplete = dto.IsComplete;

                foreach (DTOIndicator indicator in dto.Indicators.Where(i => i.Id == 0))
                {
                    strategy.Indicators.Add(new _3.Repository.Indicator
                    {
                        TypeId = indicator.TypeId,
                        IndicatorConfigurations = indicator.Configurations.Select(m => new IndicatorConfiguration
                        {
                            Name = m.Name,
                            Value = m.Value,
                            DataTypeId = (int)m.Type
                        }).ToList()
                    });
                }

                foreach (DTOIndicator indicator in dto.Indicators.Where(i => i.Removed))
                {
                    List<IndicatorConfiguration> indicatorConfigurations = repositoryIndicatorConfiguration.GetQuery().Where(im => im.IndicatorId == indicator.Id).ToList();
                    foreach (IndicatorConfiguration indicatorConfiguration in indicatorConfigurations)
                    {
                        repositoryIndicatorConfiguration.Remove(indicatorConfiguration);
                    }
                    repositoryIndicatorConfiguration.SaveChanges();

                    _3.Repository.Indicator strategyIndicator = repositoryIndicator.GetQuery("IndicatorConfigurations").First(i => i.Id == indicator.Id);
                    repositoryIndicator.Remove(strategyIndicator);
                    repositoryIndicator.SaveChanges();
                }

                foreach (DTOStrategyCondition condition in dto.Conditions.Where(c => c.Id == 0))
                {
                    strategy.StrategyConditions.Add(new StrategyCondition
                    {
                        ExecutionMomentId = (int)condition.ExecutionMoment,
                        FirstIndicatorMeta = new IndicatorMeta
                        {
                            IndicatorId = condition.FirstIndicatorMeta.Indicator.Id,
                            Name = condition.FirstIndicatorMeta.Name
                        },
                        SecondIndicatorMeta = new IndicatorMeta
                        {
                            IndicatorId = condition.SecondIndicatorMeta.Indicator.Id,
                            Name = condition.SecondIndicatorMeta.Name
                        },
                        ComparerId = (int)condition.Comparer,
                        IsOpenCondition = condition.IsOpenCondition
                    });
                }

                foreach (DTOStrategyCondition condition in dto.Conditions.Where(c => c.Removed))
                {
                    StrategyCondition strategyCondition = repositoryStrategyCondition.GetQuery().FirstOrDefault(sc => sc.Id == condition.Id);
                    repositoryStrategyCondition.Remove(strategyCondition);
                }
                repositoryStrategyCondition.SaveChanges();
            }

            repositoryStrategy.SaveChanges();

            return strategy.Id;
        }

        public DTOStrategy GetById(int id, int userId)
        {
            DTOStrategy strategy = repositoryStrategy.GetQuery("Indicators.IndicatorConfigurations").Where(s => s.Id == id && s.UserId == userId).Select(s => new DTOStrategy
            {
                Id = s.Id,
                UserId = s.UserId,
                Name = s.Name,
                InvestmentAmount = s.InvestmentAmount,
                InvestmentPercentage = s.InvestmentPercentage,
                IsComplete = s.IsComplete,
                Indicators = s.Indicators.Select(i => new DTOIndicator
                {
                    Id = i.Id,
                    Name = i.IndicatorType.Description,
                    TypeId = i.TypeId,
                    Configurations = i.IndicatorConfigurations.Select(im => new DTOIndicatorConfiguration
                    {
                        Name = im.Name,
                        Value = im.Value,
                        Type = (_4.DTO.Enums.IndicatorMetaDataType)im.DataTypeId
                    }).ToList()
                }).ToList(),
                Conditions = s.StrategyConditions.Select(sc => new DTOStrategyCondition
                {
                    Id = sc.Id,
                    ExecutionMoment = (_4.DTO.Enums.ExecutionMoment)sc.ExecutionMomentId,
                    FirstIndicatorMeta = new DTOIndicatorMeta
                    {
                        Indicator = new DTOIndicator
                        {
                            Id = sc.FirstIndicatorMeta.Indicator.Id,
                            Name = sc.FirstIndicatorMeta.Indicator.IndicatorType.Description,
                            TypeId = sc.FirstIndicatorMeta.Indicator.TypeId,
                            Configurations = sc.FirstIndicatorMeta.Indicator.IndicatorConfigurations.Select(im => new DTOIndicatorConfiguration
                            {
                                Name = im.Name,
                                Value = im.Value,
                                Type = (_4.DTO.Enums.IndicatorMetaDataType)im.DataTypeId
                            }).ToList()
                        },
                        Name = sc.FirstIndicatorMeta.Name
                    },
                    SecondIndicatorMeta = new DTOIndicatorMeta
                    {
                        Indicator = new DTOIndicator
                        {
                            Id = sc.SecondIndicatorMeta.Indicator.Id,
                            Name = sc.SecondIndicatorMeta.Indicator.IndicatorType.Description,
                            TypeId = sc.SecondIndicatorMeta.Indicator.TypeId,
                            Configurations = sc.SecondIndicatorMeta.Indicator.IndicatorConfigurations.Select(im => new DTOIndicatorConfiguration
                            {
                                Name = im.Name,
                                Value = im.Value,
                                Type = (_4.DTO.Enums.IndicatorMetaDataType)im.DataTypeId
                            }).ToList()
                        },
                        Name = sc.SecondIndicatorMeta.Name
                    },
                    Comparer = (_4.DTO.Enums.Comparer)sc.ComparerId,
                    IsOpenCondition = sc.IsOpenCondition
                }).ToList()
            }).FirstOrDefault();
            
            return strategy;
        }
    }
}
