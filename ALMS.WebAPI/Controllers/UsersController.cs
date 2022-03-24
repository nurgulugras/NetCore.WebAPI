using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Model;
using ALMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.WebAPI.Controllers
{
    /// <summary>
    /// UserLimits Endpoint
    /// </summary>
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllersBase
    {
        public UsersController(IServiceProvider serviceProvider) : base(serviceProvider) { }

       /// <summary>
        /// Get User
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<UserDto>>> GetCurrentUser()
        {
            using (var userService = ServiceProvider.GetService<IUserService>())
            {
                var user = await userService.GetCurrentUserAsync();
                return APIResult<User, UserDto>(user);
            }
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<UserDto>>> GetUserById([Required] int id)
        {
            using (var userService = ServiceProvider.GetService<IUserService>())
            {
                var user = await userService.GetUserByIdAsync(id);
                return APIResult<User, UserDto>(user);
            }
        }


        /// <summary>
        /// Get Full Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<List<UserDto>>>> GetFullUsers()
        {
            using (var userService = ServiceProvider.GetService<IUserService>())
            {
                var userList = (await userService.GetFullUsersAsync()).ToList();
                return APIResult<List<User>, List<UserDto>>(userList);
            }
        }

        /// <summary>
        /// Get Active Users
        /// </summary>
        /// <returns></returns>
        [HttpGet("actives")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<UserDto>>>> GetActiveUsers()
        {
            using (var userService = ServiceProvider.GetService<IUserService>())
            {
                var userList = (await userService.GetActiveUsersAsync()).ToList();
                return APIResult<List<User>, List<UserDto>>(userList);
            }
        }

        /// <summary>
        /// Save Active Users
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<UserDto>>> SaveUsers([FromBody] UserDto creationUserModel)
        {
            using (var userService = ServiceProvider.GetService<IUserService>())
            {
                var result = await userService.SaveUserAsync(creationUserModel.ConvertTo<User>());
                return APIResult<User, UserDto>(result);
            }
        }

        /// <summary>
        /// Update Active Users
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<UserDto>>> UpdateUsers([Required] int id, [FromBody] UserDto creationUserModel)
        {
            if (id != creationUserModel.Id)
                return APIBadRequestResult();

            using (var userService = ServiceProvider.GetService<IUserService>())
            {
                var result = await userService.UpdateUserAsync(creationUserModel.ConvertTo<User>());
                return APIResult<User, UserDto>(result);
            }
        }


        /// <summary>
        /// Update Active Users
        /// </summary>
        /// <returns></returns>
        [HttpPut("me/change-password")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<bool>>> ChangePasswordOfUser([FromBody] UserPassChangeModel changeModel)
        {
            using (var userCredentialService = ServiceProvider.GetService<IUserCredentialService>())
            {
                var result = await userCredentialService.ChangeCurrentUserPasswordAsync(changeModel.OldPassword, changeModel.NewPassword);
                return APIResult<bool>(result);
            }
        }


        [HttpPatch("{userId}/generate-password")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<bool>>> GeneratePassword([Required] int userId)
        {
            using (var userCredentialService = ServiceProvider.GetService<IUserCredentialService>())
            {
                var contextUserService = ServiceProvider.GetService<IContextUserIdentity>();
                var result = await userCredentialService.ResetUserPasswordAsync(userId);
                return APIResult<bool>(result);
            }
        }

    }
}