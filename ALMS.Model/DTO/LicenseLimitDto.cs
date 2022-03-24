using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class LicenseLimitDto : IDtoEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }

        [Required]
        public int LicenseId { get; set; }
        // public string LicenseName { get; set; }
        // public string LicenseNo { get; set; }

        [Required]
        public int AppLimitId { get; set; }
        public string AppLimitName { get; set; }
        public string AppLimitDescription { get; set; }

        [Required]
        public string Limit { get; set; }
    }
}