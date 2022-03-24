using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class SessionCreationParameter : APILicenseRequestBase
    {
        /// <summary>
        /// Oturum açılacak kullanıcı
        /// </summary>
        /// <value></value>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Kullanıcının açık olan tüm oturumlarının kapatılması
        /// </summary>
        /// <value></value>
        public bool CloseAllSessionsOfThisUser { get; set; }
    }
}