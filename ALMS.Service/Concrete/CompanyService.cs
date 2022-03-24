using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public class CompanyService : IServiceRepositoryBase<Company>, ICompanyService
    {
        private readonly ISessionService _sessionService;

        public CompanyService(IServiceProvider serviceProvider, ISessionService sessionService) : base(serviceProvider)
        {
            _sessionService = sessionService;
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return (await GetEntitiesFromCacheAsync(x => x.Id == id && x.Organization.IsActive)).SingleOrDefault();
        }

        public async Task<List<Company>> GetFullCompanysAsync()
        {
            return (await GetEntitiesFromCacheAsync(x => x.Organization.IsActive)).ToList();
        }
        public async Task<List<Company>> GetActiveCompanysAsync()
        {
            return (await GetEntitiesFromCacheAsync(x => x.Organization.IsActive && x.IsActive)).ToList();
        }

        public async Task<OperationResult<Company>> SaveCompanyAsync(Company company)
        {
            company.CreateDate = DateTime.Now;
            company.CreateUserId = _contextUserIdentity.GetContextUser().User.Id;

            var operationResult = await base.SaveAsync(company);
            if (operationResult.IsSuccess)
                await base.AddNewEntityToCacheAsync(company.Id);

            return operationResult;
        }

        public async Task<OperationResult<Company>> UpdateCompanyAsync(Company company)
        {
            var existsCompany = await GetCompanyByIdAsync(company.Id);
            if (existsCompany == null)
            {
                return new OperationResult<Company>($"{company.Id} numaralı kayıt mevcut değil!", true);
            }
            company.CreateDate = existsCompany.CreateDate;
            company.CreateUserId = existsCompany.CreateUserId;

            var operationResult = await base.UpdateAsync(company);

            if (operationResult.IsSuccess)
            {
                await base.UpdateEntityOnCacheAsync(company.Id);
                // şirket pasife alınır ise altında bağlı olan açılmış tüm oturumlar kapatılır
                if (!company.IsActive && existsCompany.IsActive != company.IsActive)
                {
                    await _sessionService.CloseAllSessionsOfCompany(company.Id);
                }
            }
            return operationResult;
        }

        public void ClearCompanyCache()
        {
            base.ClearEntityCache();
        }

        public async Task<List<Company>> GetFullCompaniesOfOrganizationAsync(int organizationId)
        {
          return (await GetEntitiesFromCacheAsync(x => x.OrganizationId==organizationId && x.Organization.IsActive)).ToList();

        }

        public async Task<List<Company>> GetActiveCompaniesOfOrganizationAsync(int organizationId)
        {
         return (await GetEntitiesFromCacheAsync(x => x.OrganizationId== organizationId && x.Organization.IsActive && x.IsActive)).ToList();

        }
    }
}