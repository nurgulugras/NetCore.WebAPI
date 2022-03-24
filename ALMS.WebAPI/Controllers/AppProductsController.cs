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
    /// AppProducts Endpoint
    /// </summary>
    [Route("api/v1/apps")]
    [ApiController]
    public class AppProductsController : ControllersBase
    {
        public AppProductsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// Get Full AppProducts
        /// </summary>
        /// <returns></returns>
        [HttpGet("{appId}/products")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<AppProductDto>>>> GetAppProducts([Required] int appId)
        {
            using (var appProductService = ServiceProvider.GetService<IAppProductService>())
            {
                var appProductList = (await appProductService.GetFullAppProductsAsync(appId)).ToList();
                return APIResult<List<AppProduct>, List<AppProductDto>>(appProductList);
            }
        }

        /// <summary>
        /// Get Active AppProducts
        /// </summary>
        /// <returns></returns>
        [HttpGet("{appId}/products/actives")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<AppProductDto>>>> GetActiveAppProducts([Required] int appId)
        {
            using (var appProductService = ServiceProvider.GetService<IAppProductService>())
            {
                var appProductList = (await appProductService.GetActiveAppProductsAsync(appId)).Include(x => x.CreateUser).ToList();
                return APIResult<List<AppProduct>, List<AppProductDto>>(appProductList);
            }
        }

        /// <summary>
        /// Save Active AppProducts
        /// </summary>
        /// <returns></returns>
        [HttpPost("{appId}/products")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<AppProductDto>>> SaveAppProducts(int appId, [FromBody] AppProductDto creationAppProductModel)
        {
            if (appId != creationAppProductModel.AppId)
                return APIBadRequestResult();

            using (var appProductService = ServiceProvider.GetService<IAppProductService>())
            {
                var result = await appProductService.SaveAppProductAsync(creationAppProductModel.ConvertTo<AppProduct>());
                return APIResult<AppProduct, AppProductDto>(result);
            }
        }

        /// <summary>
        /// Update Active AppProducts
        /// </summary>
        /// <returns></returns>
        [HttpPut("{appId}/products/{id}")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<AppProductDto>>> UpdateAppProducts(int appId, [Required] int id, [FromBody] AppProductDto creationAppProductModel)
        {
            if (id != creationAppProductModel.Id)
                return APIBadRequestResult();
            if (appId != creationAppProductModel.AppId)
                return APIBadRequestResult();

            using (var appProductService = ServiceProvider.GetService<IAppProductService>())
            {
                var result = await appProductService.UpdateAppProductAsync(creationAppProductModel.ConvertTo<AppProduct>());
                return APIResult<AppProduct, AppProductDto>(result);
            }
        }


    }
}