using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface IAppLimitService : IServiceBase
    {
        Task<IQueryable<AppLimit>> GetFullAppLimitsAsync(int appId);
        Task<IQueryable<AppLimit>> GetActiveAppLimitsAsync(int appId);
        Task<OperationResult<AppLimit>> SaveAppLimitAsync(AppLimit appLimit);
        Task<OperationResult<AppLimit>> UpdateAppLimitAsync(AppLimit appLimit);
    }
}