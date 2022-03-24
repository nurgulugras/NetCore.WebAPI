using System.Collections.Generic;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface IOrganizationService : IServiceBase
    {
        Task<Organization> GetOrganizationByIdAsync(int id);
        Task<List<Organization>> GetFullOrganizationsAsync();
        Task<List<Organization>> GetActiveOrganizationsAsync();
        Task<OperationResult<Organization>> SaveOrganizationAsync(Organization organization);
        Task<OperationResult<Organization>> UpdateOrganizationAsync(Organization organization);
    }
}