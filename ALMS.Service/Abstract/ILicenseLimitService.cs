using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface ILicenseLimitService : IServiceBase
    {
        Task<IQueryable<LicenseLimit>> GetLicenseLimitsAsync(int licenseId);
        Task<OperationResult<LicenseLimit>> SaveLicenseLimitAsync(LicenseLimit licenseLimit);
        Task<OperationResult<LicenseLimit>> UpdateLicenseLimitAsync(LicenseLimit licenseLimit);
        Task<OperationResult<bool>> DeleteLicenseLimitAsync(int licenseLimitId);
    }
}