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
    public class LicenseProductsController : ControllersBase
    {
        public LicenseProductsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// Get License Apps
        /// </summary>
        /// <returns></returns>
        [HttpGet("{licenseId}/products")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<LicenseProductDto>>>> GetLicenseProducts(int licenseId)
        {
            using (var licenseProductService = ServiceProvider.GetService<ILicenseProductService>())
            {
                var result = (await licenseProductService.GetLicenseProductsAsync(licenseId)).ToList();
                return APIResult<List<LicenseProduct>, List<LicenseProductDto>>(result);
            }
        }


        /// <summary>
        /// Save License Products Apps
        /// </summary>
        /// <returns></returns>
        [HttpPost("{licenseId}/products")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<LicenseProductDto>>> SaveLicenseProduct(int licenseId, [FromBody] LicenseProductDto licenseProductDto)
        {
            if (licenseId != licenseProductDto.LicenseId)
                return APIBadRequestResult();
            using (var licenseProductService = ServiceProvider.GetService<ILicenseProductService>())
            {
                var result = await licenseProductService.SaveLicenseProductAsync(licenseProductDto.ConvertTo<LicenseProduct>());
                return APIResult<LicenseProduct, LicenseProductDto>(result);
            }
        }

        /// <summary>
        /// Update License Products Apps
        /// </summary>
        /// <returns></returns>
        [HttpPut("{licenseId}/products/{licenseProductId}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<LicenseProductDto>>> UpdateLicenseProduct(int licenseId, int licenseProductId, [FromBody] LicenseProductDto licenseProductDto)
        {
            if (licenseId != licenseProductDto.LicenseId)
                return APIBadRequestResult();
            if (licenseProductId != licenseProductDto.Id)
                return APIBadRequestResult();

            using (var licenseProductService = ServiceProvider.GetService<ILicenseProductService>())
            {
                var result = await licenseProductService.UpdateLicenseProductAsync(licenseProductDto.ConvertTo<LicenseProduct>());
                return APIResult<LicenseProduct, LicenseProductDto>(result);
            }
        }


        /// <summary>
        /// Delete License Products Apps
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{licenseId}/products/{licenseProductId}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<bool>>> DeleteLicenseProduct(int licenseId, int licenseProductId)
        {
            using (var licenseProductService = ServiceProvider.GetService<ILicenseProductService>())
            {
                var result = await licenseProductService.DeleteLicenseProductAsync(licenseProductId);
                return APIResult<bool>(result);
            }
        }
    }
}