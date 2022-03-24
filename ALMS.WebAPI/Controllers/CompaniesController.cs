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
    /// Companies Endpoint
    /// </summary>
    [Route("api/v1")]
    [ApiController]
    public class CompaniesController : ControllersBase
    {
        public CompaniesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// Get Company
        /// </summary>
        /// <returns></returns>
        [HttpGet("companies/{id}")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<CompanyDto>>> GetCompanyById([Required] int id)
        {
            using (var companyService = ServiceProvider.GetService<ICompanyService>())
            {
                var company = await companyService.GetCompanyByIdAsync(id);
                return APIResult<Company, CompanyDto>(company);
            }
        }

        /// <summary>
        /// Get Full Companys
        /// </summary>
        /// <returns></returns>
        [HttpGet("organizations/{organizationId}/companies")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<List<CompanyDto>>>> GetFullCompanies([Required] int organizationId)
        {
            using (var companyService = ServiceProvider.GetService<ICompanyService>())
            {
                var companyList = (await companyService.GetFullCompaniesOfOrganizationAsync(organizationId)).ToList();
                return APIResult<List<Company>, List<CompanyDto>>(companyList);
            }
        }

        /// <summary>
        /// Get Active Companys
        /// </summary>
        /// <returns></returns>
        [HttpGet("organizations/{organizationId}/companies/actives")]
        [Authorized(RoleType.Admin, RoleType.User)]
        public async Task<ActionResult<ApiResponseParameter<List<CompanyDto>>>> GetActiveCompanies([Required] int organizationId)
        {
            using (var companyService = ServiceProvider.GetService<ICompanyService>())
            {
                var companyList = (await companyService.GetActiveCompaniesOfOrganizationAsync(organizationId)).ToList();
                return APIResult<List<Company>, List<CompanyDto>>(companyList);
            }
        }

        /// <summary>
        /// Save Active Companys
        /// </summary>
        /// <returns></returns>
        [HttpPost("organizations/{organizationId}/companies")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<CompanyDto>>> SaveCompany(int organizationId, [FromBody] CompanyDto creationCompanyModel)
        {
            if (organizationId != creationCompanyModel.OrganizationId)
                return APIBadRequestResult();

            using (var companyService = ServiceProvider.GetService<ICompanyService>())
            {
                var result = await companyService.SaveCompanyAsync(creationCompanyModel.ConvertTo<Company>());
                return APIResult<Company, CompanyDto>(result);
            }
        }

        /// <summary>
        /// Update Active Companys
        /// </summary>
        /// <returns></returns>
        [HttpPut("organizations/{organizationId}/companies/{companyId}")]
        [Authorized(RoleType.Admin)]
        public async Task<ActionResult<ApiResponseParameter<CompanyDto>>> UpdateCompany(int organizationId, [Required] int companyId, [FromBody] CompanyDto creationCompanyModel)
        {
            if (companyId != creationCompanyModel.Id)
                return APIBadRequestResult();
            if (organizationId != creationCompanyModel.OrganizationId)
                return APIBadRequestResult();

            using (var companyService = ServiceProvider.GetService<ICompanyService>())
            {
                var result = await companyService.UpdateCompanyAsync(creationCompanyModel.ConvertTo<Company>());
                return APIResult<Company, CompanyDto>(result);
            }
        }

    }
}