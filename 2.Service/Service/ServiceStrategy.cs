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
    }
}
