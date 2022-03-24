using System;
using System.Linq;
using ALMS.Core;
using ALMS.Model;
using Elsa.ApprovalFlowManagement;

namespace ALMS.Service
{
    public class LicenseValidations : ILicenseValidations
    {
        private readonly IContextUserIdentity _contextUserIdentity;
        private readonly ICompanyValidations _companyValidations;

        public LicenseValidations(IContextUserIdentity contextUserIdentity, ICompanyValidations companyValidations)
        {
            _contextUserIdentity = contextUserIdentity;
            _companyValidations = companyValidations;
        }

        public bool IsValidToStartApprovalFlowLicense(License license, out string errorMessage, bool throwException = false)
        {
            if (license == null)
            {
                errorMessage = Messages.NotExistsLicenseMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            if (!_companyValidations.IsValidToCompanyAndOrganization(license.CompanyId, out string errorMessageToCompnay))
            {
                errorMessage = errorMessageToCompnay;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            if (!license.IsValidDateRange)
            {
                errorMessage = Messages.InvalidLicenseDateRangeMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            if (license.AFStatus == AFStatus.Accepted)
            {
                errorMessage = Messages.ReApprovedLicenseMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            if (license.LicenseProductList == null || !license.LicenseProductList.Any())
            {
                errorMessage = Messages.MinLicenseProdutRequiredMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool IsValidToDeleteUnregister(License license, out string errorMessage, bool throwException = false)
        {
            if (license == null)
            {
                errorMessage = Messages.NotExistsLicenseMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }
            if (license.IsRegistered)
            {
                errorMessage = Messages.RegisteredLicenseDeleteMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public bool IsValidLicense(License license, out string errorMessage, bool throwException = false)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;

            if (license == null)
            {
                errorMessage = Messages.NotExistsLicenseMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            if (license.AppId != currentApp.Id)
            {
                errorMessage = Messages.NotExistsLicenseMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }
            if (!license.IsRegistered)
            {
                errorMessage = Messages.UnregisterLicenseMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }
            if (!license.IsValidDateRange)
            {
                errorMessage = Messages.InvalidLicenseDateRangeMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            if (!_companyValidations.IsValidToCompanyAndOrganization(license.CompanyId, out string errorMessageToCompnay))
            {
                errorMessage = errorMessageToCompnay;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool IsValidLicenseToRequestedUser(License license, out string errorMessage, bool throwException = false)
        {
            var contextUser = _contextUserIdentity.GetContextUser();
            if (license == null)
            {
                errorMessage = Messages.NotExistsLicenseMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            if (license.RegisteredIP != contextUser.RequestUserInfo.IP)
            {
                errorMessage = Messages.InvalidLicenseUserMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }
            if (license.RegisteredMechineName != contextUser.RequestUserInfo.MechineName)
            {
                errorMessage = Messages.InvalidLicenseUserMessage;
                if (throwException) throw new Exception(errorMessage);
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }


    }
}