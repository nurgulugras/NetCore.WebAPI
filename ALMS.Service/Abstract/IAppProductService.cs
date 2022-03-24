using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface IAppProductService : IServiceBase
    {
        Task<IQueryable<AppProduct>> GetFullAppProductsAsync(int appId);
        Task<IQueryable<AppProduct>> GetActiveAppProductsAsync(int appId);
        Task<OperationResult<AppProduct>> SaveAppProductAsync(AppProduct appProduct);
        Task<OperationResult<AppProduct>> UpdateAppProductAsync(AppProduct appProduct);
    }
}