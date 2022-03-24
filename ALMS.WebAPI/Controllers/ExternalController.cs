using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;
using ALMS.Service;
using ALMS.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace ALMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/external")]
    [Authorized(RoleType.API)]
    public class ExternalController : ControllersBase
    {
        public ExternalController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region [ Auths ]

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginRequestModel">Login parameters</param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResultModel<string>>> LoginForAPI([FromBody] APILoginRequestParameter loginRequestModel)
        {
            var apiResult = new APIResultModel<string>();
            try
            {
                using (var authenticationService = ServiceProvider.GetService<IAuthenticationService>())
                {
                    var operationResult = await authenticationService.LoginForAPIAsync(loginRequestModel);
                    if (!operationResult.IsSuccess)
                    {
                        return Unauthorized(operationResult);
                    }

                    apiResult.IsSuccess = operationResult.IsSuccess;
                    apiResult.Result = operationResult.DataModel;
                    apiResult.ErrorMessage = operationResult.Message;
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }

        #endregion

        #region [ Licenses ]

        /// <summary>
        /// Get App
        /// </summary>
        /// <returns></returns>
        [HttpPatch("licenses/register")]
        [Authorized(RoleType.API)]
        public async Task<ActionResult<APIResultModel<bool>>> RegisterLicense([FromBody] LicenseInfoParameter parameter)
        {
            var apiResult = new APIResultModel<bool>();
            try
            {
                using (var appService = ServiceProvider.GetService<ILicenseService>())
                {
                    var licenseInfo = await appService.RegisterLicenseAsync(parameter.LicenseNo);
                    apiResult.IsSuccess = licenseInfo;
                    apiResult.Result = licenseInfo;
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }

        /// <summary>
        /// Get App
        /// </summary>
        /// <returns></returns>
        [HttpPost("licenses/info")]
        [Authorized(RoleType.API)]
        public async Task<ActionResult<APIResultModel<LicenseInfo>>> GetLicenseInfo([FromBody] LicenseInfoParameter parameter)
        {
            var apiResult = new APIResultModel<LicenseInfo>();
            try
            {
                using (var appService = ServiceProvider.GetService<ILicenseService>())
                {
                    var licenseInfo = await appService.GetLicenseInfoAsync(parameter);
                    apiResult.IsSuccess = true;
                    apiResult.Result = licenseInfo;
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }

        /// <summary>
        /// Get App
        /// </summary>
        /// <returns></returns>
        [HttpPost("licenses/limits/status")]
        [Authorized(RoleType.API)]
        public async Task<ActionResult<APIResultModel<LicenseUserLimitInfo>>> GetLicenseUserLimitInfo([FromBody] LicenseInfoParameter parameter)
        {
            var apiResult = new APIResultModel<LicenseUserLimitInfo>();
            try
            {
                using (var sessionService = ServiceProvider.GetService<ISessionService>())
                {
                    var licenseInfo = await sessionService.GetLicenseUserLimitInfoAsync(parameter.LicenseNo);
                    apiResult.IsSuccess = licenseInfo != null;
                    apiResult.Result = licenseInfo;
                    apiResult.ErrorMessage = licenseInfo == null ? Messages.InvalidLicenseMessage : string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }

        #endregion

        #region [ Sessions ]

        /// <summary>
        /// Get Active Sessions
        /// </summary>
        /// <returns></returns>
        [HttpPost("licenses/sessions/get")]
        [Authorized(RoleType.API)]
        public async Task<ActionResult<APIResultModel<List<SessionApiDto>>>> GetActiveSessions([FromBody] SessionListParameter sessionListParameter)
        {
            var apiResult = new APIResultModel<List<SessionApiDto>>();
            try
            {
                using (var sessionService = ServiceProvider.GetService<ISessionService>())
                {
                    var sessions = (await sessionService.GetActiveSessionsAsync(sessionListParameter)).ToList();
                    apiResult.IsSuccess = true;
                    apiResult.Result = Mapper.Map<List<SessionApiDto>>(sessions);
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }

        /// <summary>
        /// Create Sessions
        /// </summary>
        /// <returns></returns>
        [HttpPost("licenses/sessions/create")]
        [Authorized(RoleType.API)]
        public async Task<ActionResult<APIResultModel<SessionInfo>>> CreateSessions([FromBody] SessionCreationParameter sessionCreationParameter)
        {
            var apiResult = new APIResultModel<SessionInfo>();
            try
            {
                using (var sessionService = ServiceProvider.GetService<ISessionService>())
                {
                    var sessionInfo = await sessionService.CreateNewSessionAsync(sessionCreationParameter);
                    apiResult.IsSuccess = true;
                    apiResult.Result = sessionInfo;
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }

        /// <summary>
        /// Get Active Sessions
        /// </summary>
        /// <returns></returns>
        [HttpPost("licenses/sessions/check")]
        [Authorized(RoleType.API)]
        public async Task<ActionResult<APIResultModel<bool>>> CheckActiveSessions([FromBody] SessionCheckParameter sessionCheckParameter)
        {
            var apiResult = new APIResultModel<bool>();
            try
            {
                using (var sessionService = ServiceProvider.GetService<ISessionService>())
                {
                    apiResult.IsSuccess = true;
                    apiResult.Result = await sessionService.CheckSessionIsActiveAsync(sessionCheckParameter); ;
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }

        /// <summary>
        /// Close Sessions
        /// </summary>
        /// <returns></returns>
        [HttpDelete("licenses/sessions/close")]
        [Authorized(RoleType.API)]
        public async Task<ActionResult<APIResultModel<bool>>> CloseSession([FromBody] SessionCloseParameter sessionCloseParameter)
        {
            var apiResult = new APIResultModel<bool>();
            try
            {
                using (var sessionService = ServiceProvider.GetService<ISessionService>())
                {
                    await sessionService.CloseSession(sessionCloseParameter);
                    apiResult.IsSuccess = true;
                    apiResult.Result = true;
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }


        /// <summary>
        /// Close All Sessions
        /// </summary>
        /// <returns></returns>
        [HttpDelete("licenses/sessions/close/all")]
        [Authorized(RoleType.API)]
        public async Task<ActionResult<APIResultModel<bool>>> CloseAllSessions([FromBody] LicenseSessionCloseParameter licenseSessionCloseParameter)
        {
            var apiResult = new APIResultModel<bool>();
            try
            {
                using (var sessionService = ServiceProvider.GetService<ISessionService>())
                {
                    await sessionService.CloseAllSessionsOfLicense(licenseSessionCloseParameter.LicenseNo);
                    apiResult.IsSuccess = true;
                    apiResult.Result = true;
                }
            }
            catch (System.Exception ex)
            {
                apiResult.ErrorMessage = ex.TryResolveExceptionMessage();
            }
            return Ok(apiResult);
        }

        #endregion


    }
}