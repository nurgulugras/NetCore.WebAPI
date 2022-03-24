using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface IUserService : IServiceBase
    {
        Task<User> GetCurrentUserAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<IQueryable<User>> GetFullUsersAsync();
        Task<IQueryable<User>> GetActiveUsersAsync();
        Task<OperationResult<User>> SaveUserAsync(User user);
        Task<OperationResult<User>> UpdateUserAsync(User user);
        // Task<OperationResult<User>> ChangeUserPasswordAsync(UserPassChangeModel userPassChangeModel);
        Task<User> GetActiveUserByUsernameAsync(string username);
        Task<User> CheckUserIsValid(string username, string hash);
    }
}