using System.Collections.Generic;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface ICompanyService : IServiceBase
    {
        Task<Company> GetCompanyByIdAsync(int id);
        Task<List<Company>> GetFullCompaniesOfOrganizationAsync(int organizationId);
        Task<List<Company>> GetActiveCompaniesOfOrganizationAsync(int organizationId);
        Task<OperationResult<Company>> SaveCompanyAsync(Company company);
        Task<OperationResult<Company>> UpdateCompanyAsync(Company company);
        void ClearCompanyCache();
    }
}