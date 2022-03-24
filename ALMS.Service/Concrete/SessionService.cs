using System;
using System.Linq;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Model;

namespace ALMS.Service
{
    public class SessionService : IServiceRepositoryBase<Session>, ISessionService
    {
        private readonly ILicenseService _licenseService;
        private readonly ISessionUpdateQueueService _sessionUpdateQueueService;

        public SessionService(IServiceProvider serviceProvider, ILicenseService licenseService, ISessionUpdateQueueService sessionUpdateQueueService) : base(serviceProvider)
        {
            _licenseService = licenseService;
            _sessionUpdateQueueService = sessionUpdateQueueService;
        }

        public async Task<IQueryable<Session>> GetActiveSessionsAsync(SessionListParameter sessionListParameter)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;

            if (await _licenseService.GetValidLicenseAsync(currentApp.Id, sessionListParameter.LicenseNo) == null)
                throw new Exception(Messages.InvalidLicenseMessage);
            return await base.GetEntitiesFromCacheAsync(x => x.AppId == currentApp.Id && x.EndDate == null && x.License.LicenseNo == sessionListParameter.LicenseNo);
        }

        public async Task<SessionInfo> CreateNewSessionAsync(SessionCreationParameter sessionCreationParameter)
        {
            var contextUser = _contextUserIdentity.GetContextUser();
            var currentApp = contextUser.App;
            var license = await _licenseService.GetValidLicenseAsync(currentApp.Id, sessionCreationParameter.LicenseNo);
            if (license == null) throw new Exception(Messages.InvalidLicenseMessage);

            if (sessionCreationParameter.CloseAllSessionsOfThisUser)
            {
                await CloseAllSessionsOfUser(license.Id, sessionCreationParameter.Username);
            }

            var userSessionLimitInfo = await GetLicenseUserLimitInfoAsync(license);
            if (userSessionLimitInfo.AvailableSessionsCount < 1)
            {
                throw new Exception(Messages.LimitExcessMessage);
            }

            var session = new Session(currentApp.Id, license.Id, sessionCreationParameter.Username, contextUser.RequestUserInfo);

            var sessionSaveOperation = await base.SaveAsync(session);

            if (!sessionSaveOperation.IsSuccess) throw new Exception(sessionSaveOperation.Message);

            await AddNewEntityToCacheAsync(session.Id);

            return await GetSessionInfoAsync(session.SessionUId);
        }
        public async Task CloseSession(SessionCloseParameter sessionCloseParameter)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;
            if (await _licenseService.GetValidLicenseAsync(currentApp.Id, sessionCloseParameter.LicenseNo) == null)
                throw new Exception(Messages.InvalidLicenseMessage);

            var session = (await base.GetEntitiesFromCacheAsync(x => x.AppId == currentApp.Id && x.SessionUId.ToString() == sessionCloseParameter.SessionUID.ToString() && x.Username == sessionCloseParameter.Username)).SingleOrDefault();
            if (session == null) throw new Exception(Messages.InvalidSessionMessage);

            if (session.IsClosesSession) return;

            await CloseSessionBase(session);
        }

        public async Task CloseAllSessionsOfUser(int licenseId, string username)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;
            var sessionsOfUser = (await base.GetEntitiesFromCacheAsync(x => x.AppId == currentApp.Id && x.LicenseId == licenseId && x.Username == username.Trim() && x.EndDate == null)).ToList();
            foreach (var session in sessionsOfUser)
            {
                await CloseSessionBase(session);
            }
        }
        public async Task CloseAllSessionsOfLicense(string licenseNo)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;
            if (await _licenseService.GetValidLicenseAsync(currentApp.Id, licenseNo) == null)
                throw new Exception(Messages.InvalidLicenseMessage);

            var sessionsOfLicense = await GetActiveSessionsAsync(new SessionListParameter { LicenseNo = licenseNo });
            foreach (var session in sessionsOfLicense)
            {
                await CloseSessionBase(session);
            }
        }
        public async Task CloseAllSessionsOfApp(int appId)
        {
            var sessionsOfLicense = await base.GetEntitiesFromCacheAsync(x => x.AppId == appId && x.EndDate == null);
            foreach (var session in sessionsOfLicense)
            {
                await CloseSessionBase(session);
            }
        }
        public async Task CloseAllSessionsOfCompany(int companyId)
        {
            var sessionsOfLicense = await base.GetEntitiesFromCacheAsync(x => x.License.CompanyId == companyId && x.EndDate == null);
            foreach (var session in sessionsOfLicense)
            {
                await CloseSessionBase(session);
            }
        }
        public async Task CloseAllSessionsOfOrganization(int organizationId)
        {
            var sessionsOfLicense = await base.GetEntitiesFromCacheAsync(x => x.License.Company.OrganizationId == organizationId && x.EndDate == null);
            foreach (var session in sessionsOfLicense)
            {
                await CloseSessionBase(session);
            }
        }

        private async Task CloseSessionBase(Session session)
        {
            session.EndDate = DateTime.Now;

            session.App = null;
            session.License = null;

            var updateSessionResult = await base.UpdateAsync(session);
            if (updateSessionResult.IsSuccess)
            {
                _entityCacheService.DeleteEntity(session);
                return;
            }

            throw new Exception(updateSessionResult.Message);
        }
        public Task<OperationResult<Session>> UpdateSessionAsync(Session session)
        {
            return base.UpdateAsync(session);
        }
        private async Task<SessionInfo> GetSessionInfoAsync(Guid sessionUID)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;

            var session = (await base.GetEntitiesFromCacheAsync(x => x.AppId == currentApp.Id && x.SessionUId.ToString() == sessionUID.ToString())).SingleOrDefault();
            if (session == null) throw new Exception(Messages.InvalidSessionMessage);

            return new SessionInfo
            {
                CreatedAt = session.StartDate,
                SessionUID = session.SessionUId,
                Username = session.Username,
                UserSessionLimitInfo = await GetLicenseUserLimitInfoAsync(session.License),
                LastActivityDate = session.LastActivityDate
            };
        }
        public async Task<LicenseUserLimitInfo> GetLicenseUserLimitInfoAsync(string licenseNo)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;
            var license = await _licenseService.GetValidLicenseAsync(currentApp.Id, licenseNo);
            if (license == null)
                throw new Exception(Messages.InvalidLicenseMessage);

            return await GetLicenseUserLimitInfoAsync(license);
        }
        private async Task<LicenseUserLimitInfo> GetLicenseUserLimitInfoAsync(License license)
        {
            var limitInfo = new LicenseUserLimitInfo();
            limitInfo.UserSessionsLimit = license.SessionLimit;
            limitInfo.ActiveSessionsCount = await GetActiveSessionUsageCountOfLicense(license.LicenseNo);
            return limitInfo;
        }

        private async Task<int> GetActiveSessionUsageCountOfLicense(string licenseNo)
        {
            var sessionsOfLicense = (await GetActiveSessionsAsync(new SessionListParameter { LicenseNo = licenseNo })).ToList();
            return sessionsOfLicense == null ? 0 : sessionsOfLicense.Count();
        }
        public async Task<bool> CheckSessionIsActiveAsync(SessionCheckParameter sessionCheckParameter)
        {
            var currentApp = _contextUserIdentity.GetContextUser().App;
            var session = (await GetEntitiesFromCacheAsync(x => x.AppId == currentApp.Id && x.License.IsRegistered && x.SessionUId.ToString().Equals(sessionCheckParameter.SessionUID.ToString()))).SingleOrDefault();

            if (session != null)
            {
                session.LastActivityDate = DateTime.Now;
                _sessionUpdateQueueService.AddQueue(session);
            }
            return session != null;
        }


    }
}