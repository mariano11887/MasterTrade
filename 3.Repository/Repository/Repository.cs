using System.Data.Entity;
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

        public IQueryable<TEntity> GetQuery(string includes = "")
        {
            IQueryable<TEntity> query = context.Set<TEntity>().AsQueryable();
            if (!string.IsNullOrEmpty(includes))
            {
                query = query.Include(includes);
            }

            return query;
        }

        public void Insert(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
