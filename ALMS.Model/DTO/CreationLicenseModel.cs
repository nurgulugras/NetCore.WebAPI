using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class CreationLicenseModel : IDtoEntity
    {
        public int Id { get; set; }
        public int CreateUserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AppId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CompanyId { get; set; }

        [Required]
        public string LicenseNo { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public LicensePeriodType LicensePeriodType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int LicensePeriod { get; set; }
        public int SessionLimit { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }
    }
}