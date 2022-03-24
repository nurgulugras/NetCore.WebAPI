using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ALMS.Core;
using Elsa.NNF.Data.ORM;

namespace ALMS.Service
{
    public class MicrosoftCacheService<TEntity, TEntityPrimaryKey> : IEntityCacheService<TEntity, TEntityPrimaryKey>
        where TEntity : class, IEntity, IEntity<TEntityPrimaryKey>, new()
    {
        private SingletonMemoryCache _singletonMemoryCache;

        public MicrosoftCacheService()
        {
            _singletonMemoryCache = SingletonMemoryCache.Instance;
        }

        public IEnumerable<TEntity> CreateEntityCollection(IEnumerable<TEntity> entities, DateTimeOffset? absoluteExpiration = null)
        {
            UpdateFromCache(entities, absoluteExpiration);
            return entities;
        }

        public IEnumerable<TEntity> GetCollection(Expression<Func<TEntity, bool>> predicate = null)
        {
            var values = _singletonMemoryCache.Read<IEnumerable<TEntity>>(Key);
            if (values == null) return new List<TEntity>();
            return predicate == null ? values : values.Where(predicate.Compile());
        }

        public bool IsCacheCreated()
        {
            return _singletonMemoryCache.IsCacheCreated(Key);
        }

        public void AddEntity(TEntity entity)
        {
            if (!IsCacheCreated())
                return;

            var entityCollection = GetCollection().ToList();
            entityCollection.Add(entity);

            UpdateFromCache(entityCollection);
        }
        public void UpdateEntity(TEntity entity)
        {
            if (!IsCacheCreated())
                return;
            var entityCollection = GetCollection().ToList();
            var existsItemIndex = entityCollection.ToList().FindIndex(x => x.Id.Equals(entity.Id));
            entityCollection.RemoveAt(existsItemIndex);
            UpdateFromCache(entityCollection);
            AddEntity(entity);
        }

        public void DeleteEntity(TEntity entity)
        {
            if (!IsCacheCreated())
                return;
            var entityCollection = GetCollection().ToList();
            var existsItemIndex = entityCollection.ToList().FindIndex(x => x.Id.Equals(entity.Id));
            if (existsItemIndex < 0) return;
            entityCollection.RemoveAt(existsItemIndex);
            UpdateFromCache(entityCollection);
        }
        private void UpdateFromCache(IEnumerable<TEntity> entities, DateTimeOffset? absoluteExpiration = null)
        {
            if (absoluteExpiration == null)
                _singletonMemoryCache.Write(Key, entities);
            else
                _singletonMemoryCache.Write(Key, entities, (DateTimeOffset)absoluteExpiration);

        }
        public void RemoveThisCacheSet()
        {
            _singletonMemoryCache.Remove(Key);
        }

        private string Key => $"{typeof(TEntity).Namespace}_{typeof(TEntity).Name}_Collection ";
    }
}