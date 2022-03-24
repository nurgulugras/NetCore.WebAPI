using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Elsa.NNF.Data.ORM;

namespace ALMS.Service
{
    public interface IEntityCacheService<TEntity, TEntityPrimaryKey> where TEntity : class, IEntity, IEntity<TEntityPrimaryKey>
    {
        bool IsCacheCreated();
        IEnumerable<TEntity> GetCollection(Expression<Func<TEntity, bool>> predicate = null);
        IEnumerable<TEntity> CreateEntityCollection(IEnumerable<TEntity> entities, DateTimeOffset? absoluteExpiration = null);
        void AddEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        void DeleteEntity(TEntity entity);
        void RemoveThisCacheSet();
    }
}