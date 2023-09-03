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

        public void Insert(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
