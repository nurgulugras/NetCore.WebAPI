using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface ILicenseProductService : IServiceBase
    {
        Task<IQueryable<LicenseProduct>> GetLicenseProductsAsync(int licenseId);
        Task<OperationResult<LicenseProduct>> SaveLicenseProductAsync(LicenseProduct licenseProduct);
        Task<OperationResult<LicenseProduct>> UpdateLicenseProductAsync(LicenseProduct licenseProduct);
        Task<OperationResult<bool>> DeleteLicenseProductAsync(int licenseProductId);
    }
}