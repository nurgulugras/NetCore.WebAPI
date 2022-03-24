using ALMS.Model;

namespace ALMS.Service
{
    public interface ILicenseValidations
    {
        bool IsValidToStartApprovalFlowLicense(License license, out string errorMessage, bool throwException = false);
        bool IsValidToDeleteUnregister(License license, out string errorMessage, bool throwException = false);
        bool IsValidLicense(License license, out string errorMessage, bool throwException = false);
        bool IsValidLicenseToRequestedUser(License license, out string errorMessage, bool throwException = false);
    }
}