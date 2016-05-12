using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public abstract class AsiaMoneyerRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider;
        IDbSet<TEntity> dbSet;

        protected AsiaMoneyerDbContext DbContext
        {
            get {
                return this.dbContextProvider.DbContext;
            }
        }

        protected AsiaMoneyerRepositoryBase(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
        {
            this.dbContextProvider = dbContextProvider;

            this.dbSet = this.dbContextProvider.DbContext.Set<TEntity>();
        }

        //add common methods for all repositories
        public IEnumerable<TEntity> List
        {
            get
            {
                return this.dbSet;
            }

        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            return dbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);            
        }

        public virtual void Delete(TPrimaryKey id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.DbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Save(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            this.DbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            Dictionary<object, object> originalValues = new Dictionary<object, object>();
            TEntity entityToUpdate = dbSet.Find(entity.Id);

            foreach (var property in properties)
            {
                var val = this.DbContext.Entry(entityToUpdate).Property(property).OriginalValue;
                originalValues.Add(property, val);
            }

            this.DbContext.Entry(entityToUpdate).State = EntityState.Detached;

            this.DbContext.Entry(entity).State = EntityState.Unchanged;
            foreach (var property in properties)
            {
                this.DbContext.Entry(entity).Property(property).OriginalValue = originalValues[property];
                this.DbContext.Entry(entity).Property(property).IsModified = true;
            }
        }
    }

    public abstract class AsiaMoneyerRepositoryBase<TEntity> : AsiaMoneyerRepositoryBase<TEntity, string>
        where TEntity : class, IEntity<string>
    {
        protected AsiaMoneyerRepositoryBase(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
