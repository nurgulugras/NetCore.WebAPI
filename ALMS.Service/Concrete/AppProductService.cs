
using System;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public class AppProductService : IServiceRepositoryBase<AppProduct>, IAppProductService
    {
        public AppProductService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<IQueryable<AppProduct>> GetActiveAppProductsAsync(int appId)
        {
            return _repository.GetAsync(x => x.AppId == appId && x.IsActive);
        }

        public Task<IQueryable<AppProduct>> GetFullAppProductsAsync(int appId)
        {
            return _repository.GetAsync(x => x.AppId == appId);
        }

        public Task<OperationResult<AppProduct>> SaveAppProductAsync(AppProduct appProduct)
        {
            appProduct.CreateDate = DateTime.Now;
            appProduct.CreateUserId = _contextUserIdentity.GetContextUser().User.Id;
            return base.SaveAsync(appProduct);
        }

        public async Task<OperationResult<AppProduct>> UpdateAppProductAsync(AppProduct appProduct)
        {
            var existsAppProduct = await _repository.GetByIdAsNoTrackingAsync(appProduct.Id);
            if (existsAppProduct == null)
            {
                return new OperationResult<AppProduct>($"{appProduct.Id} numaralı kayıt mevcut değil!", true);
            }
            appProduct.CreateDate = existsAppProduct.CreateDate;
            appProduct.CreateUserId = existsAppProduct.CreateUserId;
            return await base.UpdateAsync(appProduct, existsAppProduct);
        }

    }
}