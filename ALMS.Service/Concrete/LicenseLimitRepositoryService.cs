using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public class LicenseLimitRepositoryService : IServiceRepositoryBase<LicenseLimit>, ILicenseLimitRepositoryService
    {
        public LicenseLimitRepositoryService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<LicenseLimit> GetLicenseLimitByIdAsync(int licenseLimitId)
        {
            return _repository.GetByIdAsync(licenseLimitId);
        }
        public Task<IQueryable<LicenseLimit>> GetLicenseLimitsAsync(Expression<Func<LicenseLimit, bool>> predicate = null)
        {
            return _repository.GetAsync(predicate);
        }

        public Task<OperationResult<LicenseLimit>> SaveLicenseLimitAsync(LicenseLimit licenseLimit)
        {
            return base.SaveAsync(licenseLimit);
        }

        public Task<OperationResult<LicenseLimit>> UpdateLicenseLimitAsync(LicenseLimit licenseLimit, LicenseLimit existLicenseLimit)
        {
            return base.UpdateAsync(licenseLimit, existLicenseLimit);
        }

        public Task<OperationResult<bool>> DeleteLicenseLimitAsync(int licenseLimitId, LicenseLimit existLicenseLimit)
        {
            return base.DeleteAsync(licenseLimitId, existLicenseLimit);
        }
    }
}