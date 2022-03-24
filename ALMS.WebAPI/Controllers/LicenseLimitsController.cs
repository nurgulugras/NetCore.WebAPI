using System;
using System.Collections.Generic;
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
    /// License Endpoint
    /// </summary>
    [Route("api/v1/licenses")]
    [ApiController]
    public class LicenseLimitsController : ControllersBase
    {
        public LicenseLimitsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// Get License Apps
        /// </summary>
        /// <returns></returns>
        [HttpGet("{licenseId}/limits")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<LicenseLimitDto>>>> GetLicenseLimits(int licenseId)
        {
            using (var licenseLimitService = ServiceProvider.GetService<ILicenseLimitService>())
            {
                var result = (await licenseLimitService.GetLicenseLimitsAsync(licenseId)).Include(x => x.AppLimit).Include(x => x.License).ToList();
                return APIResult<List<LicenseLimit>, List<LicenseLimitDto>>(result);
            }
        }

        /// <summary>
        /// Save License Products Apps
        /// </summary>
        /// <returns></returns>
        [HttpPost("{licenseId}/limits")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<LicenseLimitDto>>> SaveLicenseLimit(int licenseId, [FromBody] LicenseLimitDto licenseLimitDto)
        {
            if (licenseId != licenseLimitDto.LicenseId)
                return APIBadRequestResult();
            using (var licenseLimitService = ServiceProvider.GetService<ILicenseLimitService>())
            {
                var result = await licenseLimitService.SaveLicenseLimitAsync(licenseLimitDto.ConvertTo<LicenseLimit>());
                return APIResult<LicenseLimit, LicenseLimitDto>(result);
            }
        }

        /// <summary>
        /// Update License Products Apps
        /// </summary>
        /// <returns></returns>
        [HttpPut("{licenseId}/limits/{licenseLimitId}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<LicenseLimitDto>>> UpdateLicenseLimit(int licenseId, int licenseLimitId, [FromBody] LicenseLimitDto licenseLimitDto)
        {
            if (licenseId != licenseLimitDto.LicenseId)
                return APIBadRequestResult();
            if (licenseLimitId != licenseLimitDto.Id)
                return APIBadRequestResult();

            using (var licenseLimitService = ServiceProvider.GetService<ILicenseLimitService>())
            {
                var result = await licenseLimitService.UpdateLicenseLimitAsync(licenseLimitDto.ConvertTo<LicenseLimit>());
                return APIResult<LicenseLimit, LicenseLimitDto>(result);
            }
        }

        /// <summary>
        /// Delete License Products Apps
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{licenseId}/limits/{licenseLimitId}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<bool>>> DeleteLicenseLimit(int licenseId, int licenseLimitId)
        {
            using (var licenseLimitService = ServiceProvider.GetService<ILicenseLimitService>())
            {
                var result = await licenseLimitService.DeleteLicenseLimitAsync(licenseLimitId);
                return APIResult<bool>(result);
            }
        }
    }
}