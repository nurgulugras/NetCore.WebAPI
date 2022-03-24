using System;
using ALMS.Core;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.Service
{
    public class CompanyValidations : ICompanyValidations
    {
        private readonly IServiceProvider _serviceProvider;

        public CompanyValidations(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool IsValidToCompanyAndOrganization(int companyId, out string errorMessage, bool throwException = false)
        {
            var company = GetCompanyService().GetCompanyByIdAsync(companyId).GetAwaiter().GetResult();

            if (company == null)
            {
                errorMessage = Messages.InvalidCompanyMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }
            if (company.Organization == null)
            {
                errorMessage = Messages.InvalidOrganizationyMessage;
                if (throwException) throw new Exception(errorMessage);
            }
            if (!company.Organization.IsActive)
            {
                errorMessage = Messages.InvalidOrganizationyMessage;
                if (throwException) throw new Exception(errorMessage);
            }

            errorMessage = string.Empty;
            return true;
        }

        private ICompanyService GetCompanyService()
        {
            return _serviceProvider.GetService<ICompanyService>();
        }

    }
}