using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface ISessionService : IServiceBase
    {
        /// <summary>
        /// Lisansın aktif oturum kullanım bilgisini verir
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        Task<LicenseUserLimitInfo> GetLicenseUserLimitInfoAsync(string licenseNo);

        /// <summary>
        /// Aktif oturumları verir
        /// </summary>
        /// <param name="sessionListParameter"></param>
        /// <returns></returns>
        Task<IQueryable<Session>> GetActiveSessionsAsync(SessionListParameter sessionListParameter);

        /// <summary>
        /// Kullanıcı için bir oturum başlatır
        /// </summary>
        /// <param name="sessionCreationParameter"></param>
        /// <returns></returns>
        Task<SessionInfo> CreateNewSessionAsync(SessionCreationParameter sessionCreationParameter);

        /// <summary>
        ///  Kullanıcı için açık olan oturumu sonlandırır
        /// </summary>
        /// <param name="sessionCloseParameter"></param>
        /// <returns></returns>
        Task CloseSession(SessionCloseParameter sessionCloseParameter);

        /// <summary>
        ///  Kullanıcının açık olan tüm oturumlarını sonlandırılır
        /// </summary>
        /// <param name="licenseId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task CloseAllSessionsOfUser(int licenseId, string username);

        /// <summary>
        /// Lisans içiin açılmış tüm oturumları kapatır
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        Task CloseAllSessionsOfLicense(string licenseNo);

        /// <summary>
        /// Oturum güncelleme
        /// </summary>
        /// <param name="nextSession"></param>
        /// <returns></returns>
        Task<OperationResult<Session>> UpdateSessionAsync(Session session);

        /// <summary>
        /// Uygulama için açılmış tüm oturumları kapatır
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        Task CloseAllSessionsOfApp(int appId);

        /// <summary>
        /// Şirket için açılmış tüm oturumları kapatır
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task CloseAllSessionsOfCompany(int companyId);

        /// <summary>
        /// Organizasyon için açılmış tüm oturumları kapatır
        /// </summary>
        /// <param name="organizasyonId"></param>
        /// <returns></returns>
        Task CloseAllSessionsOfOrganization(int organizationId);

        /// <summary>
        /// Oturumun aktif olup olmadığı kontrolü
        /// </summary>
        /// <param name="sessionCheckParameter"></param>
        /// <returns>true: Aktif, oturum geçerli | false: Pasif, oturum mevcut değil</returns>
        Task<bool> CheckSessionIsActiveAsync(SessionCheckParameter sessionCheckParameter);
    }
}