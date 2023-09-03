using System.Linq;

namespace _3.Repository.Repository
{
    public abstract class Repository<TEntity> where TEntity : class
    {
        protected MasterTradeEntities context;

        public Repository()
        {
            context = new MasterTradeEntities();
        }

        public IQueryable<TEntity> GetQuery()
        {
            return context.Set<TEntity>().AsQueryable();
        }
    }
}
