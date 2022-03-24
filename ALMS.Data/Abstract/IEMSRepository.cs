using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Elsa.NNF.Data.ORM;

namespace ALMS.Data
{
    public interface IEMSRepository<TEntity> : IEMSRepository<TEntity, int>
        where TEntity : class, IEntity, IEntity<int>
    { }
    public interface IEMSRepository<TEntity, TEntityPrimaryKey> : IDisposable
    where TEntity : class, IEntity, IEntity<TEntityPrimaryKey>
    {
        TEntity GetById(TEntityPrimaryKey id);
        Task<TEntity> GetByIdAsync(TEntityPrimaryKey id);
        TEntity GetByIdAsNoTracking(TEntityPrimaryKey id);
        Task<TEntity> GetByIdAsNoTrackingAsync(TEntityPrimaryKey id);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null);
        Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null);
        int Save(TEntity entity);
        Task<int> SaveAsync(TEntity entity);
        int Update(TEntity entity);
        int Update(TEntity oldEntity, TEntity newEntity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity oldEntity, TEntity newEntity);
        int Delete(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        TEntity CreateProxy();
    }
}