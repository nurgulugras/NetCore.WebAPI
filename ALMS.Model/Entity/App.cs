using System.ComponentModel.DataAnnotations;
using Elsa.NNF.Common.Library;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Model
{
    [Logger("Uygulama")]
    [KeyValue("Name", "Uygulama")]
    public class App : EntityBase
    {
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Bu uygulama için onay sistmei kullanılacak mı?
        /// </summary>
        /// <value></value>
        public bool IsApprovalActive { get; set; }

        /// <summary>
        /// Uygulama Token için api anahtarı
        /// </summary>
        /// <value></value>
        [Required]
        public string ApiKey { get; set; }

        /// <summary>
        /// Uygulama Token için api şifre anahtarı
        /// </summary>
        /// <value></value>
        [Required]
        public string ApiSecretKey { get; set; }

        [Required]
        public string PasswordHashCode { get; set; }

        public bool IsActive { get; set; }
    }
}