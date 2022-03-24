using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Elsa.ApprovalFlowManagement;

namespace ALMS.Model
{
    public class LicenseDto : IDtoEntity, IAFEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }

        [Required]
        public int AppId { get; set; }
        public string AppName { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        [Required]
        public string LicenseNo { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public LicensePeriodType LicensePeriodType { get; set; }
        public string LicensePeriodTypeDesc { get; set; }

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
        public bool IsRegistered { get; set; }
        public List<AppProductDto2> LicenseProducts { get; set; }
        public List<AppLimitDto2> LicenseLimits { get; set; }
    }
}