using System.Linq;
using System.Threading.Tasks;
using ALMS.Model;

namespace ALMS.Service
{
    public interface ILicenseService : IServiceBase
    {
        Task<string> GenerateLicenseNumberAsync();
        Task<License> GetLicenseAsync(int licenseId);
        Task<License> GetLicenseAsync(int appId, string licenseNo);
        Task<License> GetValidLicenseAsync(int licenseId);
        Task<License> GetValidLicenseAsync(int appId, string licenseNo);

        /// <summary>
        /// Lisans bilgisi
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<LicenseInfo> GetLicenseInfoAsync(LicenseInfoParameter parameter);

        /// <summary>
        /// Get licences of App
        /// </summary>
        /// <param name="appId">App ID</param>
        /// <returns></returns>
        Task<IQueryable<License>> GetLicensesAsync(int appId);

        /// <summary>
        /// Yeni lisans kaydı oluşturur. Lisans geçerli sayılmaz. 
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        Task<OperationResult<License>> SaveNewLicenseAsync(CreationLicenseModel license);

        /// <summary>
        /// Kayıt işlemi tamamlanmamış lisansı günceller. 
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        Task<OperationResult<License>> UpdateUnregisteredLicenseAsync(CreationLicenseModel license);

        /// <summary>
        /// Lisans kaydetmek.
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        Task<bool> RegisterLicenseAsync(string licenseNo);

        /// <summary>
        /// Kayıt işlemi tamamlanmamış lisansı siler. 
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        Task<OperationResult<bool>> DeleteUnregisteredLicenseAsync(int licenseId);

        /// <summary>
        /// Lisansı kaydet ve onay sürecine gönder. Onaydan sonra lisans resmilik kazanır. 
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns></returns>
        Task<OperationResult<bool>> SaveAndStartAppFlowToLicenseAsync(int licenseId);

        Task UpdateLicenseFromCache(int licenseId);

        /// <summary>
        /// Lisans geçerlilik kontrolü
        /// </summary>
        /// <param name="licenseId"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool IsValidLicense(int licenseId, out string errorMessage);
    }
}