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
    /// Apps Endpoint
    /// </summary>
    [Route("api/v1/apps")]
    [ApiController]
    public class AppsController : ControllersBase
    {
        public AppsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// Get App
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<AppDecryptedDto>>> GetAppById([Required] int id)
        {
            using (var appService = ServiceProvider.GetService<IAppService>())
            {
                var app = await appService.GetAppByIdAsync(id);
                return APIResult<App, AppDecryptedDto>(app);
            }
        }

        /// <summary>
        /// Get Full Apps
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<AppDto>>>> GetFullApps()
        {
            using (var appService = ServiceProvider.GetService<IAppService>())
            {
                var appList = (await appService.GetFullAppsAsync()).ToList();
                return APIResult<List<App>, List<AppDto>>(appList);
            }
        }

        /// <summary>
        /// Get Active Apps
        /// </summary>
        /// <returns></returns>
        [HttpGet("actives")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<AppDto>>>> GetActiveApps()
        {
            using (var appService = ServiceProvider.GetService<IAppService>())
            {
                var appList = (await appService.GetActiveAppsAsync()).ToList();
                return APIResult<List<App>, List<AppDto>>(appList);
            }
        }

        /// <summary>
        /// Save Active Apps
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<AppDto>>> SaveApps([FromBody] CreationAppModel creationAppModel)
        {
            using (var appService = ServiceProvider.GetService<IAppService>())
            {
                var result = await appService.SaveApplicationAsync(creationAppModel.ConvertTo<App>());
                return APIResult<App, AppDto>(result);
            }
        }

        /// <summary>
        /// Update Active Apps
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<AppDto>>> UpdateApps([Required] int id, [FromBody] CreationAppModel creationAppModel)
        {
            if (id != creationAppModel.Id)
                return APIBadRequestResult();

            using (var appService = ServiceProvider.GetService<IAppService>())
            {
                var result = await appService.UpdateApplicationAsync(creationAppModel.ConvertTo<App>());
                return APIResult<App, AppDto>(result);
            }
        }
    }
}