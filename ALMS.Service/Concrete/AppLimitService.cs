using System;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public class AppLimitService : IServiceRepositoryBase<AppLimit>, IAppLimitService
    {
        public AppLimitService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<IQueryable<AppLimit>> GetActiveAppLimitsAsync(int appId)
        {
            return _repository.GetAsync(x => x.AppId == appId && x.IsActive);
        }

        public Task<IQueryable<AppLimit>> GetFullAppLimitsAsync(int appId)
        {
            return _repository.GetAsync(x => x.AppId == appId);
        }

        public Task<OperationResult<AppLimit>> SaveAppLimitAsync(AppLimit appLimit)
        {
            appLimit.CreateDate = DateTime.Now;
            appLimit.CreateUserId = _contextUserIdentity.GetContextUser().User.Id;
            return base.SaveAsync(appLimit);
        }

        public async Task<OperationResult<AppLimit>> UpdateAppLimitAsync(AppLimit appLimit)
        {
            var existsAppLimit = await _repository.GetByIdAsNoTrackingAsync(appLimit.Id);
            if (existsAppLimit == null)
            {
                return new OperationResult<AppLimit>($"{appLimit.Id} numaralı kayıt mevcut değil!", true);
            }
            appLimit.CreateDate = existsAppLimit.CreateDate;
            appLimit.CreateUserId = existsAppLimit.CreateUserId;
            return await base.UpdateAsync(appLimit, existsAppLimit);
        }
    }
}