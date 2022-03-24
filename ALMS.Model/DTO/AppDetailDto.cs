
using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class AppDetailDto : IDtoEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }

        [Required]
        public string Name { get; set; }

        // /// <summary>
        // /// Bu uygulama için onay sistmei kullanılacak mı?
        // /// </summary>
        // /// <value></value>
        // public bool IsApprovalActive { get; set; }

        /// <summary>
        /// Uygulama Token için api anahtarı
        /// </summary>
        /// <value></value>
        public string ApiKey { get; set; }

        /// <summary>
        /// Uygulama Token için api şifre anahtarı
        /// </summary>
        /// <value></value>
        public string ApiSecretKey { get; set; }

        public bool IsActive { get; set; }
    }
}