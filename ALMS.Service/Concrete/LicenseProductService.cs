using System;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public class LicenseProductService : ILicenseProductService
    {
        private readonly ILicenseService _licenseService;
        private readonly ILicenseProductRepositoryService _licenseProductRepositoryService;
        private readonly IContextUserIdentity _contextUserIdentity;

        public LicenseProductService(ILicenseService licenseService, ILicenseProductRepositoryService licenseProductRepositoryService, IContextUserIdentity contextUserIdentity)
        {
            this._licenseService = licenseService;
            this._licenseProductRepositoryService = licenseProductRepositoryService;
            this._contextUserIdentity = contextUserIdentity;
        }

        public Task<IQueryable<LicenseProduct>> GetLicenseProductsAsync(int licenseId)
        {
            return _licenseProductRepositoryService.GetLicenseProductsAsync(x => x.LicenseId == licenseId);
        }

        public async Task<OperationResult<LicenseProduct>> SaveLicenseProductAsync(LicenseProduct licenseProduct)
        {
            var license = await _licenseService.GetLicenseAsync(licenseProduct.LicenseId);
            if (license == null)
                return new Model.OperationResult<LicenseProduct>($"{licenseProduct.LicenseId} numaralı lisans mevcut değil!", true);
            if (license.IsRegistered)
                return new Model.OperationResult<LicenseProduct>($"Kayıt edilmiş bir lisans üzerinde değişiklik yapılamaz!", true);

            licenseProduct.CreateDate = DateTime.Now;
            licenseProduct.CreateUserId = _contextUserIdentity.GetContextUser().User.Id;

            var operationResult = await _licenseProductRepositoryService.SaveLicenseProductAsync(licenseProduct);
            if (operationResult.IsSuccess)
                await _licenseService.UpdateLicenseFromCache(license.Id);

            return operationResult;
        }

        public async Task<OperationResult<LicenseProduct>> UpdateLicenseProductAsync(LicenseProduct licenseProduct)
        {
            var existsLicenseProduct = await _licenseProductRepositoryService.GetLicenseProductByIdAsync(licenseProduct.Id);
            if (existsLicenseProduct == null)
            {
                return new OperationResult<LicenseProduct>($"{licenseProduct.Id} numaralı kayıt mevcut değil!", true);
            }

            var license = await _licenseService.GetLicenseAsync(licenseProduct.LicenseId);
            if (license == null)
                return new Model.OperationResult<LicenseProduct>($"{licenseProduct.LicenseId} numaralı lisans mevcut değil!", true);
            if (license.IsRegistered)
                return new Model.OperationResult<LicenseProduct>($"Kayıt edilmiş bir lisans üzerinde değişiklik yapılamaz!", true);

            licenseProduct.CreateDate = existsLicenseProduct.CreateDate;
            licenseProduct.CreateUserId = existsLicenseProduct.CreateUserId;

            var operationResult = await _licenseProductRepositoryService.UpdateLicenseProductAsync(licenseProduct, existsLicenseProduct);
            if (operationResult.IsSuccess)
                await _licenseService.UpdateLicenseFromCache(license.Id);

            return operationResult;
        }
        public async Task<OperationResult<bool>> DeleteLicenseProductAsync(int licenseProductId)
        {
            var existsLicenseProduct = await _licenseProductRepositoryService.GetLicenseProductByIdAsync(licenseProductId);
            if (existsLicenseProduct == null)
            {
                return new OperationResult<bool>($"{licenseProductId} numaralı kayıt mevcut değil!", true);
            }
            var license = await _licenseService.GetLicenseAsync(existsLicenseProduct.LicenseId);
            if (license == null)
                return new Model.OperationResult<bool>($"{existsLicenseProduct.LicenseId} numaralı lisans mevcut değil!", true);
            if (license.IsRegistered)
                return new Model.OperationResult<bool>($"Kayıt edilmiş bir lisans üzerinde değişiklik yapılamaz!", true);

            var operationResult = await _licenseProductRepositoryService.DeleteLicenseProductAsync(licenseProductId, existsLicenseProduct);
            if (operationResult.IsSuccess)
                await _licenseService.UpdateLicenseFromCache(license.Id);

            return operationResult;
        }

        public void Dispose()
        {
        }
    }
}