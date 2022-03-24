using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elsa.NNF.Common.Library;
namespace ALMS.Model
{
    [KeyValue("Name", "Oturum")]
    public class Session : IEntityBase
    {
        public Session() { }
        public Session(int appId, int licenseId, string username, HttpRequestUserInfo requestUserInfo)
        {
            AppId = appId;
            LicenseId = licenseId;
            CreateDate = DateTime.Now;
            StartDate = DateTime.Now;
            SessionUId = Guid.NewGuid();
            Username = username;
            LastActivityDate = DateTime.Now;
            IP = requestUserInfo.IP ?? "";
            MechineName = requestUserInfo.MechineName ?? "";
            UserAgent = requestUserInfo.UserAgent ?? "";
        }

        public int Id { get; set; }

        [Required]
        [ForeignKey("App")]
        public int AppId { get; set; }
        public virtual App App { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("License")]
        public int LicenseId { get; set; }
        public virtual License License { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public Guid SessionUId { get; set; }

        [Required]
        public string Username { get; set; }

        [NotMapped]
        public bool IsClosesSession => EndDate != null;
        public DateTime LastActivityDate { get; set; } = DateTime.Now;

        [Required]
        public string IP { get; set; }
        [Required]
        public string MechineName { get; set; }
        [Required]
        public string UserAgent { get; set; }

        public Session Shallowcopy()
        {
            return (Session)this.MemberwiseClone();
        }
    }
}