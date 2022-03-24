using System;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;
namespace ALMS.Service
{
    public class UserService : IServiceRepositoryBase<User>, IUserService
    {
        public UserService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<User> GetCurrentUserAsync()
        {
            var currentUserId=base._contextUserIdentity.GetContextUser().User.Id;
            return (await base.GetEntitiesFromCacheAsync(x => x.Id == currentUserId)).SingleOrDefault();
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return (await base.GetEntitiesFromCacheAsync(x => x.Id == id)).SingleOrDefault();
        }

        public Task<IQueryable<User>> GetFullUsersAsync()
        {
            return base.GetEntitiesFromCacheAsync();
        }
        public Task<IQueryable<User>> GetActiveUsersAsync()
        {
            return base.GetEntitiesFromCacheAsync(x => x.IsActive);
        }

        public async Task<OperationResult<User>> SaveUserAsync(User user)
        {
            user.Id = 0;
            user.CreateDate = DateTime.Now;

            var operationResult = await base.SaveAsync(user);
            if (operationResult.IsSuccess)
                await base.AddNewEntityToCacheAsync(user.Id);

            return operationResult;
        }

        public async Task<OperationResult<User>> UpdateUserAsync(User user)
        {
            var existsUser = await GetUserByIdAsync(user.Id);
            if (existsUser == null)
            {
                return new OperationResult<User>($"{user.Id} numaralı kayıt mevcut değil!", true);
            }

            user.CreateDate = existsUser.CreateDate;
            user.Password = existsUser.Password;
            user.PasswordHashCode = existsUser.PasswordHashCode;

            var operationResult = await base.UpdateAsync(user);
            if (operationResult.IsSuccess)
                await base.UpdateEntityOnCacheAsync(user.Id);

            return operationResult;
        }

        public async Task<User> CheckUserIsValid(string username, string hash)
        {
            var user = (await base.GetEntitiesFromCacheAsync(x => x.IsActive && x.Email.Equals(username) && x.PasswordHashCode.Equals(hash))).SingleOrDefault();
            return user != null && user.IsActive ? user : null;
        }
        public async Task<User> GetActiveUserByUsernameAsync(string username)
        {
            username = username?.Trim();
            if (string.IsNullOrEmpty(username)) return null;
            return (await base.GetEntitiesFromCacheAsync(x => x.IsActive && x.Email == username)).SingleOrDefault();
        }
    }
}