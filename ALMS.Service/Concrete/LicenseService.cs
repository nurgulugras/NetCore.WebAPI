using System;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Model;
using Elsa.ApprovalFlowManagement;

namespace ALMS.Service
{
    public class LicenseService : IServiceRepositoryBase<License>, ILicenseService
    {
        private readonly IApprovalFlowService _approvalFlowService;
        private readonly ILicenseProductRepositoryService _licenseProductRepositoryService;
        private readonly ILicenseLimitRepositoryService _licenseLimitRepositoryService;
        private readonly ILicenseValidations _licenseValidations;
        private readonly ICompanyValidations _companyValidations;

        public LicenseService(IServiceProvider serviceProvider,
            IApprovalFlowService approvalFlowService,
            ILicenseProductRepositoryService licenseProductRepositoryService,
            ILicenseLimitRepositoryService licenseLimitRepositoryService,
            ILicenseValidations licenseValidations,
            ICompanyValidations companyValidations
            ) : base(serviceProvider)
        {
            _approvalFlowService = approvalFlowService;
            _licenseProductRepositoryService = licenseProductRepositoryService;
            _licenseLimitRepositoryService = licenseLimitRepositoryService;
            _licenseValidations = licenseValidations;
            _companyValidations = companyValidations;
        }

        public Task<string> GenerateLicenseNumberAsync()
        {
            return Task.Run(() => SerialKeyGenerater.GenerateLicenseKey());
        }

        public async Task<License> GetLicenseAsync(int licenseId)
        {
            return (await GetEntitiesFromCacheAsync(x => x.Id == licenseId)).SingleOrDefault();
        }
        public async Task<License> GetValidLicenseAsync(int licenseId)
        {
            var license = await GetLicenseAsync(licenseId);
            return _licenseValidations.IsValidLicense(license, out string errorMessage) ? license : null;
        }
        public async Task<License> GetLicenseAsync(int appId, string licenseNo)
        {
            return (await GetEntitiesFromCacheAsync(x => x.AppId == appId && x.LicenseNo == licenseNo)).SingleOrDefault();
        }
        public async Task<License> GetValidLicenseAsync(int appId, string licenseNo)
        {
            var license = await GetLicenseAsync(appId, licenseNo);
            return _licenseValidations.IsValidLicense(license, out string errorMessage) ? license : null;
        }
        public async Task<LicenseInfo> GetLicenseInfoAsync(LicenseInfoParameter parameter)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;
            var license = await GetValidLicenseAsync(currentApp.Id, parameter.LicenseNo);
            if (license == null) throw new Exception(Messages.InvalidLicenseMessage);

            _licenseValidations.IsValidLicenseToRequestedUser(license, out string errorMessage, true);

            return _mapper.Map<LicenseInfo>(license);
        }
        public Task<IQueryable<License>> GetLicensesAsync(int appId)
        {
            return GetEntitiesFromCacheAsync(x => x.AppId == appId);
        }
        public async Task<Model.OperationResult<License>> SaveNewLicenseAsync(CreationLicenseModel licenseCreation)
        {
            _companyValidations.IsValidToCompanyAndOrganization(licenseCreation.CompanyId, out string errorMessage, true);

            var license = _mapper.Map<License>(licenseCreation);
            license.CreateUserId = _contextUserIdentity.GetContextUser().User.Id;
            license.CreateDate = DateTime.Now;
            license.EndDate = GetEndDate(license);

            var operationResult = await base.SaveAsync(license);
            if (operationResult.IsSuccess)
                await base.AddNewEntityToCacheAsync(license.Id);

            return operationResult;
        }

        public async Task<Model.OperationResult<License>> UpdateUnregisteredLicenseAsync(CreationLicenseModel licenseCreation)
        {
            var existsLicense = await GetLicenseAsync(licenseCreation.Id);
            if (existsLicense == null)
            {
                return new Model.OperationResult<License>($"{licenseCreation.Id} numaralı kayıt mevcut değil!", true);
            }

            _companyValidations.IsValidToCompanyAndOrganization(licenseCreation.CompanyId, out string errorMessage, true);

            if (existsLicense.IsRegistered)
            {
                return new Model.OperationResult<License>($"Kayıt edilmiş bir lisans güncellenemez!", true);
            }

            var license = _mapper.Map<License>(licenseCreation);
            license.CreateUserId = existsLicense.CreateUserId;
            license.CreateDate = existsLicense.CreateDate;
            license.EndDate = GetEndDate(license);
            license.AFStatus = AFStatus.NotStarted;

            license.RegisteredDate = existsLicense.RegisteredDate;
            license.RegisteredIP = existsLicense.RegisteredIP;
            license.RegisteredUserAgent = existsLicense.RegisteredUserAgent;

            var operationResult = await base.UpdateAsync(license);
            if (operationResult.IsSuccess)
                await base.UpdateEntityOnCacheAsync(license.Id);

            return operationResult;
        }

        public async Task<Model.OperationResult<bool>> SaveAndStartAppFlowToLicenseAsync(int licenseId)
        {
            var operationResult = new Model.OperationResult<bool>();

            var license = await GetLicenseAsync(licenseId);

            if (!_licenseValidations.IsValidToStartApprovalFlowLicense(license, out string errorMessage, false))
                return new Model.OperationResult<bool> { Message = errorMessage };

            // Onay süreci var ise;
            if (license.App.IsApprovalActive)
            {
                var applicationUser = _mapper.Map<ApplicationUser>(_contextUserIdentity.GetContextUser().User);
                var licenseDto = _mapper.Map<LicenseDto>(license);

                var flowStartRequestModel = new FlowStartRequestModel
                {
                    StartedUser = applicationUser,
                    EntityId = licenseId.ToString(),
                    Entity = licenseDto,
                    FormName = GlobalKeys.AppFlowFormNameToCreateNewLicenseForm,
                };

                var flowResult = await this._approvalFlowService.StartAprovalFlowAsync(flowStartRequestModel);
                operationResult.IsSuccess = flowResult.IsSuccess;
                operationResult.DataModel = flowResult.IsSuccess;
                operationResult.Message = flowResult.IsSuccess ? "Lisans kaydı için onay akışı başlatılmıştır. Onay tamamlandığında tarafınıza bilgi geçilecektir." : flowResult.Message;
            }
            else
            {
                // onay süreci yok ise direk kaydet ve resmilik kazandır
                license.AFStatus = AFStatus.Accepted;

                license.App = null;
                license.Company = null;
                license.CreateUser = null;
                license.LicenseProductList = null;
                license.LicenseLimitList = null;

                var updateOperationResult = await base.UpdateAsync(license);
                if (updateOperationResult.IsSuccess)
                    license = await base.UpdateEntityOnCacheAsync(license.Id);

                operationResult.IsSuccess = updateOperationResult.IsSuccess;
                operationResult.DataModel = updateOperationResult.IsSuccess;
                operationResult.Message = updateOperationResult.IsSuccess ? "Lisans kaydı tamamlandı. Müşteri ile paylaşabilirsiniz." : updateOperationResult.Message;
            }
            return operationResult;
        }
        public async Task<Model.OperationResult<bool>> DeleteUnregisteredLicenseAsync(int licenseId)
        {
            var existsLicense = await GetLicenseAsync(licenseId);

            if (!_licenseValidations.IsValidToDeleteUnregister(existsLicense, out string errorMessage))
                return new Model.OperationResult<bool> { Message = errorMessage };

            var operationResult = await base.DeleteAsync(licenseId);
            if (operationResult.IsSuccess)
                base.DeleteEntityOnCache(existsLicense);

            return operationResult;
        }
        private DateTime GetEndDate(License license)
        {
            switch (license.LicensePeriodType)
            {
                case LicensePeriodType.Yearly:
                    return license.StartDate.AddYears(license.LicensePeriod);
                case LicensePeriodType.Monthly:
                    return license.StartDate.AddMonths(license.LicensePeriod);
                case LicensePeriodType.Daily:
                    return license.StartDate.AddDays(license.LicensePeriod);
                default:
                    throw new Exception($"Tanımsız LisanPeriyotTip bilgisi: {license.LicensePeriodType.ToString()}");
            }

        }
        public async Task UpdateLicenseFromCache(int licenseId)
        {
            await base.UpdateEntityOnCacheAsync(licenseId);
        }
        public bool IsValidLicense(int licenseId, out string errorMessage)
        {
            var license = GetLicenseAsync(licenseId).GetAwaiter().GetResult();
            return _licenseValidations.IsValidLicense(license, out errorMessage);
        }
        public async Task<bool> RegisterLicenseAsync(string licenseNo)
        {
            var contextUser = _contextUserIdentity.GetContextUser();
            var license = await GetLicenseAsync(contextUser.App.Id, licenseNo);
            if (license == null) throw new Exception(Messages.InvalidLicenseMessage);

            if (license.AFStatus != AFStatus.Accepted)
                throw new Exception(Messages.UnapprovedLicenseMessage);

            if (license.RegisteredDate != null)
                throw new Exception(Messages.ReRegisterLicenseMessage);

            license.RegisteredDate = DateTime.Now;
            license.RegisteredIP = contextUser.RequestUserInfo.IP;
            license.RegisteredMechineName = contextUser.RequestUserInfo.MechineName;
            license.RegisteredUserAgent = contextUser.RequestUserInfo.UserAgent;

            license.App = null;
            license.Company = null;
            license.CreateUser = null;
            license.LicenseLimitList = null;
            license.LicenseProductList = null;

            var updateOperationResult = await base.UpdateAsync(license);
            if (updateOperationResult.IsSuccess)
                license = await base.UpdateEntityOnCacheAsync(license.Id);
            else throw new Exception(updateOperationResult.Message);

            return true;
        }
    }
}