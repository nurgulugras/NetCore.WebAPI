using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class LicenseProductDto : IDtoEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }

        [Required]
        public int LicenseId { get; set; }
        public string LicenseName { get; set; }
        public string LicenseNo { get; set; }

        [Required]
        public int AppProductId { get; set; }
        public string AppProductName { get; set; }

    }
}