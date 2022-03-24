using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface IAppService : IServiceBase
    {
        Task<App> GetAppByIdAsync(int id);
        Task<App> GetActiveAppByKeyAndSecretAsync(string apiKey, string apiSecretKey);
        Task<IQueryable<App>> GetFullAppsAsync();
        Task<IQueryable<App>> GetActiveAppsAsync();
        Task<OperationResult<App>> SaveApplicationAsync(App app);
        Task<OperationResult<App>> UpdateApplicationAsync(App app);
        Task<bool> IsApprovalActiveOnApp(int appId);
    }
}