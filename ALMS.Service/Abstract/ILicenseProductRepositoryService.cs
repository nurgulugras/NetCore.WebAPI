using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface ILicenseProductRepositoryService : IServiceBase
    {

        Task<LicenseProduct> GetLicenseProductByIdAsync(int licenseProductId);
        Task<IQueryable<LicenseProduct>> GetLicenseProductsAsync(Expression<Func<LicenseProduct, bool>> predicate = null);
        Task<OperationResult<LicenseProduct>> SaveLicenseProductAsync(LicenseProduct licenseProduct);
        Task<OperationResult<LicenseProduct>> UpdateLicenseProductAsync(LicenseProduct licenseProduct, LicenseProduct existLicenseProduct);
        Task<OperationResult<bool>> DeleteLicenseProductAsync(int licenseProductId, LicenseProduct existLicenseProduct);
    }
}