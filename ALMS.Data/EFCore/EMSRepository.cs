using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ALMS.Core;
using Elsa.NNF.Data.ORM;

namespace ALMS.Data.EFCore
{

    /// <summary>
    /// Identifier kolonunun tipini sabit 'int' şekliyle verilerek kullanılması örneği
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EMSRepository<TEntity> : EMSRepository<TEntity, int>, IEMSRepository<TEntity>
        where TEntity : class, IEntity, IEntity<int>
    {
        public EMSRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
    }
    public class EMSRepository<TEntity, TEntityPrimaryKey> : IEMSRepository<TEntity, TEntityPrimaryKey>
        where TEntity : class, IEntity, IEntity<TEntityPrimaryKey>
    {
        private IEntityRepository<TEntity, TEntityPrimaryKey> _repository;
        private bool _isECacheableCompanyBaseType;
        private readonly IServiceProvider serviceProvider;

        public EMSRepository(IServiceProvider serviceProvider)
        {
            LoadNNFRepository(serviceProvider);
            _isECacheableCompanyBaseType = typeof(TEntity).IsCacheableEntityType();

            this.serviceProvider = serviceProvider;
        }

        public TEntity GetById(TEntityPrimaryKey id)
        {
            return _repository.GetById(id);
        }

        public async Task<TEntity> GetByIdAsync(TEntityPrimaryKey id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public TEntity GetByIdAsNoTracking(TEntityPrimaryKey id)
        {
            return _repository.GetByIdAsNoTracking(id);
        }

        public async Task<TEntity> GetByIdAsNoTrackingAsync(TEntityPrimaryKey id)
        {
            return await _repository.GetByIdAsNoTrackingAsync(id);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate == null ? (_repository.All()) : (_repository.Where(predicate));
        }

        public async Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate == null ? (await _repository.AllAsync()) : (await _repository.WhereAsync(predicate));
        }

        public int Save(TEntity entity)
        {
            return _repository.Save(entity);
        }

        public Task<int> SaveAsync(TEntity entity)
        {
            return _repository.SaveAsync(entity);
        }

        public int Update(TEntity entity)
        {
            return _repository.Update(entity);
        }

        public int Update(TEntity oldEntity, TEntity newEntity)
        {
            return _repository.Update(oldEntity, newEntity);
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<int> UpdateAsync(TEntity oldEntity, TEntity newEntity)
        {
            if (_isECacheableCompanyBaseType)
                _repository.ClearCache();
            return await _repository.UpdateAsync(oldEntity, newEntity);
        }

        public int Delete(TEntity entity)
        {
            return _repository.Delete(entity);
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            if (_isECacheableCompanyBaseType)
                _repository.ClearCache();
            return _repository.DeleteAsync(entity);
        }

        public TEntity CreateProxy()
        {
            return _repository.CreateProxy();
        }
        public void Dispose()
        {
            _repository.Dispose();
        }
        private void LoadNNFRepository(IServiceProvider serviceProvider)
        {
            _repository = RepositoryBuilder.GenerateRepository<TEntity, TEntityPrimaryKey>(serviceProvider, GetBuilder());
        }
        private EntityRepositoryBuilder GetBuilder()
        {
            var efCoreBuilder = new EFCoreBuilder<ApiContext>();
            return new EntityRepositoryBuilder(efCoreBuilder) { CacheExpiryTime = DateTime.Now.AddSeconds(10) };
        }
    }
}