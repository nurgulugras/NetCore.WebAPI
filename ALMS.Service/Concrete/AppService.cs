using System;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Model;

namespace ALMS.Service
{
    public class AppService : IServiceRepositoryBase<App>, IAppService
    {
        private readonly IUserCredentialService _userCredentialService;
        private readonly ISessionService _sessionService;

        public AppService(IServiceProvider serviceProvider, IUserCredentialService userCredentialService, ISessionService sessionService) : base(serviceProvider)
        {
            _userCredentialService = userCredentialService;
            _sessionService = sessionService;
        }

        public async Task<App> GetAppByIdAsync(int id)
        {
            return (await base.GetEntitiesFromCacheAsync(x => x.Id == id)).SingleOrDefault();
        }
        public Task<IQueryable<App>> GetFullAppsAsync()
        {
            return base.GetEntitiesFromCacheAsync();
        }
        public Task<IQueryable<App>> GetActiveAppsAsync()
        {
            return base.GetEntitiesFromCacheAsync(x => x.IsActive);
        }

        public async Task<OperationResult<App>> SaveApplicationAsync(App app)
        {
            app.CreateDate = DateTime.Now;
            app.CreateUserId = _contextUserIdentity.GetContextUser().User.Id;
            app.ApiKey = CryptographyHelper.SymmetricEncrypt(_userCredentialService.GenerateRandomPassword(12));
            app.ApiSecretKey = CryptographyHelper.SymmetricEncrypt(_userCredentialService.GenerateRandomPassword(12));
            app.PasswordHashCode = _userCredentialService.GenerateRandomPassword(4);

            var operationResult = await base.SaveAsync(app);

            if (operationResult.IsSuccess)
                await base.AddNewEntityToCacheAsync(app.Id);

            return operationResult;
        }

        public async Task<OperationResult<App>> UpdateApplicationAsync(App app)
        {
            var existsApp = await GetAppByIdAsync(app.Id);
            if (existsApp == null)
            {
                return new OperationResult<App>($"{app.Id} numaralı kayıt mevcut değil!", true);
            }

            app.CreateDate = existsApp.CreateDate;
            app.CreateUserId = existsApp.CreateUserId;
            app.ApiKey = existsApp.ApiKey;
            app.ApiSecretKey = existsApp.ApiSecretKey;
            app.PasswordHashCode = existsApp.PasswordHashCode;

            var operationResult = await base.UpdateAsync(app, existsApp);

            if (operationResult.IsSuccess)
            {
                await base.UpdateEntityOnCacheAsync(app.Id);
                // uygulama pasife alınır ise altında bağlı olan açılmış tüm oturumlar kapatılır
                if (!app.IsActive && existsApp.IsActive != app.IsActive)
                {
                    await _sessionService.CloseAllSessionsOfApp(app.Id);
                }
            }

            return operationResult;
        }

        public async Task<App> GetActiveAppByKeyAndSecretAsync(string apiKey, string apiSecretKey)
        {
            var encryptedApiKey = CryptographyHelper.SymmetricEncrypt(apiKey);
            var encryptedApiSecretKey = CryptographyHelper.SymmetricEncrypt(apiSecretKey);
            return (await base.GetEntitiesFromCacheAsync(x =>
                        x.IsActive &&
                        x.ApiKey.Equals(encryptedApiKey) &&
                        x.ApiSecretKey.Equals(encryptedApiSecretKey)
            )).SingleOrDefault();
        }

        public async Task<bool> IsApprovalActiveOnApp(int appId)
        {
            var app = await GetAppByIdAsync(appId);
            return app == null ? false : app.IsActive && app.IsApprovalActive;
        }
    }
}