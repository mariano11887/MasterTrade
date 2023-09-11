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
        private readonly RepositoryIndicatorMeta repositoryIndicatorMeta;

        public ServiceStrategy()
        {
            repositoryStrategy = new RepositoryStrategy();
            repositoryIndicator = new RepositoryIndicator();
            repositoryIndicatorMeta = new RepositoryIndicatorMeta();
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
                    IsComplete = dto.IsComplete
                };
                repositoryStrategy.Insert(strategy);
            }
            else
            {
                strategy = repositoryStrategy.GetQuery("Indicators.IndicatorMetas").FirstOrDefault(s => s.Id == dto.Id && s.UserId == dto.UserId);
                if (strategy == null)
                {
                    return 0;
                }

                strategy.Name = dto.Name;
                strategy.IsComplete = dto.IsComplete;

                foreach (DTOIndicator indicator in dto.Indicators.Where(i => i.Id == 0))
                {
                    strategy.Indicators.Add(new _3.Repository.Indicator
                    {
                        TypeId = indicator.TypeId,
                        IndicatorMetas = indicator.Metas.Select(m => new IndicatorMeta
                        {
                            Name = m.Name,
                            Value = m.Value,
                            IndicatorMetaDataTypeId = (int)m.Type
                        }).ToList()
                    });
                }

                foreach (DTOIndicator indicator in dto.Indicators.Where(i => i.Removed))
                {
                    _3.Repository.Indicator strategyIndicator = repositoryIndicator.GetQuery("IndicatorMetas").First(i => i.Id == indicator.Id);
                    repositoryIndicator.Remove(strategyIndicator);
                    repositoryIndicator.SaveChanges();
                }
            }

            repositoryStrategy.SaveChanges();

            return strategy.Id;
        }

        public DTOStrategy GetById(int id, int userId)
        {
            DTOStrategy strategy = repositoryStrategy.GetQuery("Indicators.IndicatorMetas").Where(s => s.Id == id && s.UserId == userId).Select(s => new DTOStrategy
            {
                Id = s.Id,
                UserId = s.UserId,
                Name = s.Name,
                IsComplete = s.IsComplete,
                Indicators = s.Indicators.Select(i => new DTOIndicator
                {
                    Id = i.Id,
                    Name = i.IndicatorType.Description,
                    TypeId = i.TypeId,
                    Metas = i.IndicatorMetas.Select(im => new DTOIndicatorMeta
                    {
                        Name = im.Name,
                        Value = im.Value,
                        Type = (_4.DTO.Enums.IndicatorMetaDataType)im.IndicatorMetaDataTypeId
                    }).ToList()
                }).ToList()
            }).FirstOrDefault();
            
            return strategy;
        }
    }
}
