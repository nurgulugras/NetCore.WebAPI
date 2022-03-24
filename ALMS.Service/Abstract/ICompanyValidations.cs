namespace ALMS.Service
{
    public interface ICompanyValidations
    {
        bool IsValidToCompanyAndOrganization(int companyId, out string errorMessage, bool throwException = false);
    }
}