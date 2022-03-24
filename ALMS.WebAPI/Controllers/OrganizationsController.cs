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
    /// Organizations Endpoint
    /// </summary>
    [Route("api/v1/organizations")]
    [ApiController]
    public class OrganizationsController : ControllersBase
    {
        public OrganizationsController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// Get Organization
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<OrganizationDto>>> GetOrganizationById([Required] int id)
        {
            using (var organizationService = ServiceProvider.GetService<IOrganizationService>())
            {
                var organization = await organizationService.GetOrganizationByIdAsync(id);
                return APIResult<Organization, OrganizationDto>(organization);
            }
        }

        /// <summary>
        /// Get Full Organizations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<List<OrganizationDto>>>> GetFullOrganizations()
        {
            using (var organizationService = ServiceProvider.GetService<IOrganizationService>())
            {
                var organizationList = (await organizationService.GetFullOrganizationsAsync()).ToList();
                return APIResult<List<Organization>, List<OrganizationDto>>(organizationList);
            }
        }

        /// <summary>
        /// Get Active Organizations
        /// </summary>
        /// <returns></returns>
        [HttpGet("actives")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<OrganizationDto>>>> GetActiveOrganizations()
        {
            using (var organizationService = ServiceProvider.GetService<IOrganizationService>())
            {
                var organizationList = (await organizationService.GetActiveOrganizationsAsync()).ToList();
                return APIResult<List<Organization>, List<OrganizationDto>>(organizationList);
            }
        }

        /// <summary>
        /// Save Active Organizations
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<OrganizationDto>>> SaveOrganizations([FromBody] OrganizationDto creationOrganizationModel)
        {
            using (var organizationService = ServiceProvider.GetService<IOrganizationService>())
            {
                var result = await organizationService.SaveOrganizationAsync(creationOrganizationModel.ConvertTo<Organization>());
                return APIResult<Organization, OrganizationDto>(result);
            }
        }

        /// <summary>
        /// Update Active Organizations
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<OrganizationDto>>> UpdateOrganizations([Required] int id, [FromBody] OrganizationDto creationOrganizationModel)
        {
            if (id != creationOrganizationModel.Id)
                return APIBadRequestResult();

            using (var organizationService = ServiceProvider.GetService<IOrganizationService>())
            {
                var result = await organizationService.UpdateOrganizationAsync(creationOrganizationModel.ConvertTo<Organization>());
                return APIResult<Organization, OrganizationDto>(result);
            }
        }


    }
}