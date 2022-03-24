using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;
using ALMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.WebAPI.Controllers
{

    /// <summary>
    /// License Endpoint
    /// </summary>
    [Route("api/v1/licenses")]
    [ApiController]
    public class LicensesController : ControllersBase
    {
        public LicensesController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// Get App
        /// </summary>
        /// <returns></returns>
        [HttpGet("generate-license-key")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<string>>> GenerateLicenseNo()
        {
            using (var appService = ServiceProvider.GetService<ILicenseService>())
            {
                var generatedSerialKey = await appService.GenerateLicenseNumberAsync();
                return APIResult(generatedSerialKey);
            }
        }


        /// <summary>
        /// Get App
        /// </summary>
        /// <returns></returns>
        [HttpGet("apps/{appId}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<LicenseDto>>>> GetLicensesByApp(int appId)
        {
            using (var appService = ServiceProvider.GetService<ILicenseService>())
            {
                var licenses = (await appService.GetLicensesAsync(appId)).Include(x => x.Company).Include(x => x.App).Include(x => x.CreateUser).ToList();
                return APIResult<List<License>, List<LicenseDto>>(licenses);
            }
        }

        /// <summary>
        /// Get License
        /// </summary>
        /// <returns></returns>
        [HttpGet("{licenseId}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<LicenseDto>>> GetLicense(int licenseId)
        {
            using (var appService = ServiceProvider.GetService<ILicenseService>())
            {
                var license = await appService.GetLicenseAsync(licenseId);
                return APIResult<License, LicenseDto>(license);
            }
        }

        /// <summary>
        /// Save License Apps
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<LicenseDto>>> SaveNewLicense([FromBody] CreationLicenseModel creationLicenseModel)
        {
            if (!ModelState.IsValid)
                return APIBadRequestResult();
            using (var licenseService = ServiceProvider.GetService<ILicenseService>())
            {
                var result = await licenseService.SaveNewLicenseAsync(creationLicenseModel);
                return APIResult<License, LicenseDto>(result);
            }
        }

        /// <summary>
        /// Update License Apps
        /// </summary>
        /// <returns></returns>
        [HttpPut("{licenseId}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<LicenseDto>>> UpdateUnregisteredLicense(int licenseId, [FromBody] CreationLicenseModel creationLicenseModel)
        {
            if (licenseId != creationLicenseModel.Id)
                return APIBadRequestResult();

            using (var licenseService = ServiceProvider.GetService<ILicenseService>())
            {
                var result = await licenseService.UpdateUnregisteredLicenseAsync(creationLicenseModel);
                return APIResult<License, LicenseDto>(result);
            }
        }

        /// <summary>
        /// Save License Apps
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{licenseId}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<bool>>> DeleteUnregisteredLicense(int licenseId)
        {
            using (var licenseService = ServiceProvider.GetService<ILicenseService>())
            {
                var result = await licenseService.DeleteUnregisteredLicenseAsync(licenseId);
                return APIResult<bool>(result);
            }
        }


        /// <summary>
        /// Update License Apps
        /// </summary>
        /// <returns></returns>
        [HttpPost("{licenseId}/registrations/start-workflow")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<bool>>> SaveAndStartAppFlowToLicense(int licenseId)
        {
            using (var licenseService = ServiceProvider.GetService<ILicenseService>())
            {
                var result = await licenseService.SaveAndStartAppFlowToLicenseAsync(licenseId);
                return APIResult<bool>(result);
            }
        }
    }
}