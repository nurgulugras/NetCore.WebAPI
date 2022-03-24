using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elsa.ApprovalFlowManagement;
using Elsa.NNF.Common.Library;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Model
{
    [Logger("Lisans")]
    [KeyValue("Name", "Lisans")]
    public class License : EntityBase, IAFEntity
    {
        [Required]
        [ForeignKey("App")]
        public int AppId { get; set; }
        public virtual App App { get; set; }

        [Required]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Required]
        public string LicenseNo { get; set; }

        [Required]
        public string Name { get; set; }
        public string LicenseProducts { get; set; }

        [Required]
        public LicensePeriodType LicensePeriodType { get; set; }

        [Required]
        public int LicensePeriod { get; set; }
        public int SessionLimit { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public string AFUid { get; set; }
        public AFStatus AFStatus { get; set; }
        public string AFMessage { get; set; }

        [NotMapped]
        public bool IsRegistered => AFStatus == AFStatus.Accepted && RegisteredDate != null;
        public virtual ICollection<LicenseProduct> LicenseProductList { get; set; }
        public virtual ICollection<LicenseLimit> LicenseLimitList { get; set; }

        public string RegisteredIP { get; set; }
        public string RegisteredMechineName { get; set; }
        public string RegisteredUserAgent { get; set; }

        [NotMapped]
        public bool IsExpiredLicenseDate => DateTime.Now.Date > EndDate.Date;

        [NotMapped]
        public bool IsValidDateRange => StartDate.Date <= DateTime.Now.Date && EndDate.Date >= DateTime.Now.Date;
    }
}