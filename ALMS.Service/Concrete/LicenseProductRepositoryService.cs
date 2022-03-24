using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public class LicenseProductRepositoryService : IServiceRepositoryBase<LicenseProduct>, ILicenseProductRepositoryService
    {
        public LicenseProductRepositoryService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<LicenseProduct> GetLicenseProductByIdAsync(int licenseProductId)
        {
            return _repository.GetByIdAsync(licenseProductId);
        }
        public Task<IQueryable<LicenseProduct>> GetLicenseProductsAsync(Expression<Func<LicenseProduct, bool>> predicate = null)
        {
            return _repository.GetAsync(predicate);
        }

        public Task<OperationResult<LicenseProduct>> SaveLicenseProductAsync(LicenseProduct licenseProduct)
        {
            return base.SaveAsync(licenseProduct);
        }

        public Task<OperationResult<LicenseProduct>> UpdateLicenseProductAsync(LicenseProduct licenseProduct, LicenseProduct existLicenseProduct)
        {
            return base.UpdateAsync(licenseProduct, existLicenseProduct);
        }

        public Task<OperationResult<bool>> DeleteLicenseProductAsync(int licenseProductId, LicenseProduct existLicenseProduct)
        {
            return base.DeleteAsync(licenseProductId, existLicenseProduct);
        }
    }
}