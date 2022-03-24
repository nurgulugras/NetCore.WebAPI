using System;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public class LicenseLimitService : ILicenseLimitService
    {
        private readonly ILicenseService _licenseService;
        private readonly ILicenseLimitRepositoryService _licenseLimitRepositoryService;
        private readonly IContextUserIdentity _contextUserIdentity;

        public LicenseLimitService(IServiceProvider serviceProvider, ILicenseService licenseService, ILicenseLimitRepositoryService licenseLimitRepositoryService, IContextUserIdentity contextUserIdentity)
        {
            this._licenseService = licenseService;
            this._licenseLimitRepositoryService = licenseLimitRepositoryService;
            this._contextUserIdentity = contextUserIdentity;
        }

        public Task<IQueryable<LicenseLimit>> GetLicenseLimitsAsync(int licenseId)
        {
            return _licenseLimitRepositoryService.GetLicenseLimitsAsync(x => x.LicenseId == licenseId);
        }

        public async Task<OperationResult<LicenseLimit>> SaveLicenseLimitAsync(LicenseLimit licenseLimit)
        {
            var license = await _licenseService.GetLicenseAsync(licenseLimit.LicenseId);
            if (license == null)
                return new Model.OperationResult<LicenseLimit>($"{licenseLimit.LicenseId} numaralı lisans mevcut değil!", true);
            if (license.IsRegistered)
                return new Model.OperationResult<LicenseLimit>($"Kayıt edilmiş bir lisans üzerinde değişiklik yapılamaz!", true);

            licenseLimit.CreateDate = DateTime.Now;
            licenseLimit.CreateUserId = _contextUserIdentity.GetContextUser().User.Id;

            var operationResult = await _licenseLimitRepositoryService.SaveLicenseLimitAsync(licenseLimit);
            if (operationResult.IsSuccess)
                await _licenseService.UpdateLicenseFromCache(license.Id);

            return operationResult;
        }

        public async Task<OperationResult<LicenseLimit>> UpdateLicenseLimitAsync(LicenseLimit licenseLimit)
        {
            var existsLicenseLimit = await _licenseLimitRepositoryService.GetLicenseLimitByIdAsync(licenseLimit.Id);
            if (existsLicenseLimit == null)
            {
                return new OperationResult<LicenseLimit>($"{licenseLimit.Id} numaralı kayıt mevcut değil!", true);
            }

            var license = await _licenseService.GetLicenseAsync(licenseLimit.LicenseId);
            if (license == null)
                return new Model.OperationResult<LicenseLimit>($"{licenseLimit.LicenseId} numaralı lisans mevcut değil!", true);
            if (license.IsRegistered)
                return new Model.OperationResult<LicenseLimit>($"Kayıt edilmiş bir lisans üzerinde değişiklik yapılamaz!", true);

            licenseLimit.CreateDate = existsLicenseLimit.CreateDate;
            licenseLimit.CreateUserId = existsLicenseLimit.CreateUserId;

            var operationResult = await _licenseLimitRepositoryService.UpdateLicenseLimitAsync(licenseLimit, existsLicenseLimit);
            if (operationResult.IsSuccess)
                await _licenseService.UpdateLicenseFromCache(license.Id);

            return operationResult;
        }
        public async Task<OperationResult<bool>> DeleteLicenseLimitAsync(int licenseLimitId)
        {
            var existsLicenseLimit = await _licenseLimitRepositoryService.GetLicenseLimitByIdAsync(licenseLimitId);
            if (existsLicenseLimit == null)
            {
                return new OperationResult<bool>($"{licenseLimitId} numaralı kayıt mevcut değil!", true);
            }
            var license = await _licenseService.GetLicenseAsync(licenseLimitId);
            if (license == null)
                return new Model.OperationResult<bool>($"{licenseLimitId} numaralı lisans mevcut değil!", true);
            if (license.IsRegistered)
                return new Model.OperationResult<bool>($"Kayıt edilmiş bir lisans üzerinde değişiklik yapılamaz!", true);

            var operationResult = await _licenseLimitRepositoryService.DeleteLicenseLimitAsync(licenseLimitId, existsLicenseLimit);
            if (operationResult.IsSuccess)
                await _licenseService.UpdateLicenseFromCache(license.Id);

            return operationResult;
        }

        public void Dispose()
        {
        }
    }
}