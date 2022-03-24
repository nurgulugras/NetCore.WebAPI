using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ALMS.Data;
using Elsa.NNF.Data.ORM;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.Service
{
    /// <summary>
    /// Repository katmanında rutin yapılan İnsert, Update vb. işlemler için yazıldı. Özelleştirme olması durumunda overide ile içeriği değiştirilerek kullanılabilir.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class IServiceRepositoryBase<TEntity> : IServiceRepositoryCoreBase<TEntity, int>
        where TEntity : class, IEntity, IEntity<int>, new()
    {
        public IServiceRepositoryBase(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #region [ Cache ]

        protected async Task<TEntity> GetCleanEntityByIdFromNewRepository(int entityId)
        {
            using (var newRepository = _serviceProvider.GetService<IEMSRepository<TEntity, int>>())
            {
                var entity = await newRepository.GetByIdAsync(entityId);
                if (entity != null) entity = _mapper.Map<TEntity>(entity);
                return entity;
            }
        }

        protected async Task<IQueryable<TEntity>> GetEntitiesFromCacheAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (_entityCacheService.IsCacheCreated())
            {
                return _entityCacheService.GetCollection(predicate).AsQueryable();
            }
            else
            {
                using (var newRepository = _serviceProvider.GetService<IEMSRepository<TEntity, int>>())
                {
                    var entities = (await newRepository.GetAsync()).ToList();
                    entities = _mapper.Map<List<TEntity>, List<TEntity>>(entities);
                    _entityCacheService.CreateEntityCollection(entities.ToList());
                    return predicate == null ? entities.AsQueryable() : entities.AsQueryable().Where(predicate);
                }
            }
        }
        protected async Task<TEntity> AddNewEntityToCacheAsync(int entityId)
        {
            var entity = await GetCleanEntityByIdFromNewRepository(entityId);
            _entityCacheService.AddEntity(entity);
            return entity;
        }
        protected async Task<TEntity> UpdateEntityOnCacheAsync(int entityId)
        {
            var entity = await GetCleanEntityByIdFromNewRepository(entityId);
            _entityCacheService.UpdateEntity(entity);
            return entity;
        }
        protected void DeleteEntityOnCache(TEntity entity)
        {
            _entityCacheService.DeleteEntity(entity);
        }

        protected void ClearEntityCache()
        {
            _entityCacheService.RemoveThisCacheSet();
        }

        #endregion



    }
}