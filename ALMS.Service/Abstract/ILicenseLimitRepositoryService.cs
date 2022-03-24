using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface ILicenseLimitRepositoryService : IServiceBase
    {
        Task<LicenseLimit> GetLicenseLimitByIdAsync(int licenseLimitId);
        Task<IQueryable<LicenseLimit>> GetLicenseLimitsAsync(Expression<Func<LicenseLimit, bool>> predicate = null);
        Task<OperationResult<LicenseLimit>> SaveLicenseLimitAsync(LicenseLimit licenseLimit);
        Task<OperationResult<LicenseLimit>> UpdateLicenseLimitAsync(LicenseLimit licenseLimit, LicenseLimit existLicenseLimit);
        Task<OperationResult<bool>> DeleteLicenseLimitAsync(int licenseLimitId, LicenseLimit existLicenseLimit);
    }
}