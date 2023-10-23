using _3.Repository.Repository;
using _4.DTO;
using System.Collections.Generic;
using System.Linq;

namespace _2.Service.Service
{
    public class ServiceTemporality
    {
        private readonly RepositoryTemporality repositoryTemporality;

        public ServiceTemporality()
        {
            repositoryTemporality = new RepositoryTemporality();
        }

        public List<DTOTemporality> GetAll(int cryptoPairId)
        {
            List<DTOTemporality> temporalities = repositoryTemporality.GetQuery().Where(t => t.CryptoPairId == cryptoPairId).Select(t => new DTOTemporality
            {
                Id = t.Id,
                CryptoPairId = cryptoPairId,
                Description = t.Description,
                CandlesGroupingAmount = t.CandlesGroupingAmount
            }).ToList();

            return temporalities;
        }
    }
}
