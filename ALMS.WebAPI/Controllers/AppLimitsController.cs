using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Model;
using ALMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.WebAPI.Controllers
{
    /// <summary>
    /// AppLimits Endpoint
    /// </summary>
    [Route("api/v1/apps")]
    [ApiController]
    public class AppLimitsController : ControllersBase
    {
        public AppLimitsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// Get Full AppLimits
        /// </summary>
        /// <returns></returns>
        [HttpGet("{appId}/limits")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<AppLimitDto>>>> GetAppLimits(int appId)
        {
            using (var appLimitService = ServiceProvider.GetService<IAppLimitService>())
            {
                var appLimitList = (await appLimitService.GetFullAppLimitsAsync(appId)).Include(x => x.CreateUser).ToList();
                return APIResult<List<AppLimit>, List<AppLimitDto>>(appLimitList);
            }
        }

        /// <summary>
        /// Get Active AppLimits
        /// </summary>
        /// <returns></returns>
        [HttpGet("{appId}/limits/actives")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<AppLimitDto>>>> GetActiveAppLimits(int appId)
        {
            using (var appLimitService = ServiceProvider.GetService<IAppLimitService>())
            {
                var appLimitList = (await appLimitService.GetActiveAppLimitsAsync(appId)).Include(x => x.CreateUser).ToList();
                return APIResult<List<AppLimit>, List<AppLimitDto>>(appLimitList);
            }
        }

        /// <summary>
        /// Save Active AppLimits
        /// </summary>
        /// <returns></returns>
        [HttpPost("{appId}/limits")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<AppLimitDto>>> SaveAppLimits(int appId, [FromBody] AppLimitDto creationAppLimitModel)
        {
            if (appId != creationAppLimitModel.AppId)
                return APIBadRequestResult();

            using (var appLimitService = ServiceProvider.GetService<IAppLimitService>())
            {
                var result = await appLimitService.SaveAppLimitAsync(creationAppLimitModel.ConvertTo<AppLimit>());
                return APIResult<AppLimit, AppLimitDto>(result);
            }
        }

        /// <summary>
        /// Update Active AppLimits
        /// </summary>
        /// <returns></returns>
        [HttpPut("{appId}/limits/{id}")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<AppLimitDto>>> UpdateAppLimits(int appId, [Required] int id, [FromBody] AppLimitDto creationAppLimitModel)
        {
            if (id != creationAppLimitModel.Id)
                return APIBadRequestResult();
            if (appId != creationAppLimitModel.AppId)
                return APIBadRequestResult();

            using (var appLimitService = ServiceProvider.GetService<IAppLimitService>())
            {
                var result = await appLimitService.UpdateAppLimitAsync(creationAppLimitModel.ConvertTo<AppLimit>());
                return APIResult<AppLimit, AppLimitDto>(result);
            }
        }


    }
}