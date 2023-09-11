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

        public ServiceStrategy()
        {
            repositoryStrategy = new RepositoryStrategy();
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
            }

            repositoryStrategy.SaveChanges();

            return strategy.Id;
        }

        public DTOStrategy GetById(int id, int userId)
        {
            DTOStrategy strategy = repositoryStrategy.GetQuery().Where(s => s.Id == id && s.UserId == userId).Select(s => new DTOStrategy
            {
                Id = s.Id,
                UserId = s.UserId,
                Name = s.Name,
                IsComplete = s.IsComplete
            }).FirstOrDefault();
            
            return strategy;
        }
    }
}
