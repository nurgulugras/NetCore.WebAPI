using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface IUserCredentialService : IServiceBase
    {
        OperationResult<User> CheckUserIsValid(User user, string password);
        Task<OperationResult<bool>> ChangeCurrentUserPasswordAsync(string oldPassword, string newPassword);
        // Task<bool> CheckContextUserIsValid(string username, string passwordHash);

        Task<OperationResult<bool>> ResetUserPasswordAsync(int userId);
        // Task<OperationResult<bool>> ResetUserPassword(User user);
        string GenerateRandomPassword(int lenght);

    }
}