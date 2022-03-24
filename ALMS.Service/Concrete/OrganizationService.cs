using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;
using Microsoft.EntityFrameworkCore;

namespace ALMS.Service
{
    public class OrganizationService : IServiceRepositoryBase<Organization>, IOrganizationService
    {
        private readonly ISessionService _sessionService;
        private readonly ICompanyService _companyService;

        public OrganizationService(IServiceProvider serviceProvider, ISessionService sessionService, ICompanyService companyService) : base(serviceProvider)
        {
            _sessionService = sessionService;
            _companyService = companyService;
        }

        public async Task<Organization> GetOrganizationByIdAsync(int id)
        {
            return (await _repository.GetAsync(x => x.Id == id)).Include(x => x.CreateUser).AsNoTracking().SingleOrDefault();
        }

        public async Task<List<Organization>> GetFullOrganizationsAsync()
        {
            return (await _repository.GetAsync()).Include(x => x.CreateUser).ToList();
        }
        public async Task<List<Organization>> GetActiveOrganizationsAsync()
        {
            return (await _repository.GetAsync(x => x.IsActive)).Include(x => x.CreateUser).ToList();
        }

        public Task<OperationResult<Organization>> SaveOrganizationAsync(Organization organization)
        {
            organization.CreateDate = DateTime.Now;
            organization.CreateUserId = _contextUserIdentity.GetContextUser().User.Id;
            return base.SaveAsync(organization);
        }

        public async Task<OperationResult<Organization>> UpdateOrganizationAsync(Organization organization)
        {
            var existsOrganization = await _repository.GetByIdAsNoTrackingAsync(organization.Id);
            if (existsOrganization == null)
            {
                return new OperationResult<Organization>($"{organization.Id} numaralı kayıt mevcut değil!", true);
            }
            organization.CreateDate = existsOrganization.CreateDate;
            organization.CreateUserId = existsOrganization.CreateUserId;

            var operationResult = await base.UpdateAsync(organization);
            if (operationResult.IsSuccess)
            {
                _companyService.ClearCompanyCache();

                // organizasyon pasife alınır ise altında bağlı olan açılmış tüm oturumlar kapatılır
                if (!organization.IsActive && existsOrganization.IsActive != organization.IsActive)
                {
                    await _sessionService.CloseAllSessionsOfOrganization(organization.Id);
                }
            }
            return operationResult;
        }

    }
}