using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface IAuthenticationService : IServiceBase
    {
        Task<OperationResult<string>> LoginForUIAsync(UserLoginRequestParameter loginRequestModel);
        Task<OperationResult<string>> LoginForAPIAsync(APILoginRequestParameter loginRequestModel);
    }
}