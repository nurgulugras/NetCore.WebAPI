using System;
using System.Collections.Generic;

namespace ALMS.Model
{
    public class LicenseInfo
    {
        public string LicenseNo { get; set; }
        public string Name { get; set; }
        public LicensePeriodType LicensePeriodType { get; set; }
        public int LicensePeriod { get; set; }
        public int UserSessionLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public List<string> LicenseProducts { get; set; }
        public List<LicenseLimitDto2> LicenseLimits { get; set; }
    }
}