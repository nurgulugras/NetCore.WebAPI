using System;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Data;
using ALMS.Model;
using AutoMapper;
using Elsa.NNF.Data.ORM;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.Service
{
    public class IServiceRepositoryCoreBase<TEntity, TEntityPrimaryKey>
        where TEntity : class, IEntity, IEntity<TEntityPrimaryKey>, new()
    {
        protected readonly IEMSRepository<TEntity, TEntityPrimaryKey> _repository;
        protected readonly IContextUserIdentity _contextUserIdentity;
        protected readonly IMapper _mapper;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ALMS.Service.IEntityCacheService<TEntity, TEntityPrimaryKey> _entityCacheService;
        public IServiceRepositoryCoreBase(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetService<IEMSRepository<TEntity, TEntityPrimaryKey>>();
            _contextUserIdentity = serviceProvider.GetService<IContextUserIdentity>();
            _mapper = serviceProvider.GetService<IMapper>();
            _serviceProvider = serviceProvider;
            _entityCacheService = serviceProvider.GetService<ALMS.Service.IEntityCacheService<TEntity, TEntityPrimaryKey>>();
        }

        #region [ Save ]
        public virtual OperationResult<TEntity> Save(TEntity entity)
        {
            var operationResult = new OperationResult<TEntity>();
            var affectedRowCount = _repository.Save(entity);
            if (affectedRowCount == 0)
            {
                operationResult.IsSuccess = false;
                operationResult.Message = $"'Kullanıcı' kayıt işleminde hata oluştu.";
            }
            else
            {
                operationResult.IsSuccess = true;
                operationResult.DataModel = entity;
            }
            return operationResult;
        }
        #endregion

        #region [ SaveAsync ]
        public virtual async Task<OperationResult<TEntity>> SaveAsync(TEntity entity)
        {
            var operationResult = new OperationResult<TEntity>();
            try
            {
                var affectedRowCount = await _repository.SaveAsync(entity);
                if (affectedRowCount == 0)
                {
                    operationResult.Message = $"{entity.GetEntityName()} kayıt işleminde hata oluştu.";
                }
                else
                {
                    operationResult.IsSuccess = true;
                    operationResult.DataModel = entity;
                }
            }
            catch (System.Exception exception)
            {
                var message = exception.TryResolveExceptionMessage();
                operationResult.Message = $"{entity.GetEntityName()} kayıt işleminde hata oluştu. Mesaj: " + message;
            }

            return operationResult;
        }
        #endregion

        #region [ Update ]
        public virtual OperationResult<TEntity> Update(TEntity entity, TEntity entityFromDb = null)
        {
            var operationResult = new OperationResult<TEntity>();
            try
            {
                entityFromDb = entityFromDb ?? _repository.GetByIdAsNoTracking(entity.Id);
                if (entityFromDb == null) { return new OperationResult<TEntity>() { Message = $"Veritabanında {entity.GetEntityName()} kaydı bulunamadı.", NoContent = true }; }

                var affectedRowCount = _repository.Update(entityFromDb, entity);
                if (affectedRowCount == 0)
                {
                    operationResult.IsSuccess = false;
                }
                else
                {
                    operationResult.IsSuccess = true;
                    operationResult.DataModel = entity;
                }
            }
            catch (Exception exception)
            {
                var message = exception.TryResolveExceptionMessage();
                operationResult.Message = $"{entity.GetEntityName()} kayıt işleminde hata oluştu. Mesaj: " + message;
            }

            return operationResult;
        }
        #endregion

        #region [ UpdateAsync ]
        public virtual async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity, TEntity entityFromDb = null)
        {
            var operationResult = new OperationResult<TEntity>();
            try
            {
                entityFromDb = entityFromDb ?? await _repository.GetByIdAsNoTrackingAsync(entity.Id);
                if (entityFromDb == null) { return new OperationResult<TEntity>() { Message = $"Veritabanında {entity.GetEntityName()} kaydı bulunamadı.", NoContent = true }; }

                var affectedRowCount = await _repository.UpdateAsync(entityFromDb, entity);
                if (affectedRowCount == 0)
                {
                    operationResult.IsSuccess = false;
                }
                else
                {
                    operationResult.IsSuccess = true;
                    operationResult.DataModel = entity;
                }
            }
            catch (Exception exception)
            {
                var message = exception.TryResolveExceptionMessage();
                operationResult.Message = $"{entity.GetEntityName()} kayıt işleminde hata oluştu. Mesaj: " + message;
            }

            return operationResult;
        }
        #endregion

        #region [ Delete ]
        public virtual OperationResult<bool> Delete(TEntityPrimaryKey entityId, TEntity entityFromDb = null)
        {
            var operationResult = new OperationResult<bool>();
            try
            {
                entityFromDb = entityFromDb ?? _repository.GetByIdAsNoTracking(entityId);
                if (entityFromDb == null) { return new OperationResult<bool>() { Message = $"Veritabanında {new TEntity().GetEntityName()} kaydı bulunamadı.", NoContent = true }; }

                var affectedRowCount = _repository.Delete(entityFromDb);
                if (affectedRowCount == 0)
                {
                    operationResult.IsSuccess = false;
                    operationResult.Message = $"{new TEntity().GetEntityName()} güncelleme işleminde hata oluştu.";
                }
                else
                {
                    operationResult.IsSuccess = true;
                    operationResult.DataModel = true;
                }
                return operationResult;

            }
            catch (Exception exception)
            {
                var message = exception.TryResolveExceptionMessage();
                operationResult.Message = $"{new TEntity().GetEntityName()} kayıt işleminde hata oluştu. Mesaj: " + message;
            }
            return operationResult;
        }
        #endregion

        #region [ DeleteAsync ]
        public virtual async Task<OperationResult<bool>> DeleteAsync(TEntityPrimaryKey entityId, TEntity entityFromDb = null)
        {
            var operationResult = new OperationResult<bool>();
            try
            {
                entityFromDb = entityFromDb ?? await _repository.GetByIdAsNoTrackingAsync(entityId);
                if (entityFromDb == null) { return new OperationResult<bool>() { Message = $"Veritabanında {new TEntity().GetEntityName()} kaydı bulunamadı.", NoContent = true }; }

                var affectedRowCount = await _repository.DeleteAsync(entityFromDb);
                if (affectedRowCount == 0)
                {
                    operationResult.IsSuccess = false;
                    operationResult.Message = $"{entityFromDb.GetEntityName()} güncelleme işleminde hata oluştu.";
                }
                else
                {
                    operationResult.IsSuccess = true;
                    operationResult.DataModel = true;
                }
                return operationResult;

            }
            catch (Exception exception)
            {
                var message = exception.TryResolveExceptionMessage();
                operationResult.Message = $"{new TEntity().GetEntityName()} kayıt işleminde hata oluştu. Mesaj: " + message;
            }
            return operationResult;
        }
        #endregion

        #region [ Dispose ]
        public void Dispose()
        {
            this._repository.Dispose();
        }
        #endregion

    }
}