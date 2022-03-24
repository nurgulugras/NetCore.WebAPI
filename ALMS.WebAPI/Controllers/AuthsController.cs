using System;
using System.Threading.Tasks;
using ALMS.Model;
using ALMS.Service;
using ALMS.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.WebAPI.Controllers
{
    /// <summary>
    /// Auths Endpoint
    /// </summary>
    [Route("api/v1/auths")]
    [ApiController]
    public class AuthsController : ControllersBase
    {
        public AuthsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginRequestModel">Login parameters</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponseParameter<string>>> Login([FromBody] UserLoginRequestParameter loginRequestModel)
        {
            using (var authenticationService = ServiceProvider.GetService<IAuthenticationService>())
            {
                var operationResult = await authenticationService.LoginForUIAsync(loginRequestModel);
                return APIResult(operationResult);
            }
        }

        
    }
}