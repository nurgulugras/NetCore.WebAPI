using System.Runtime.Serialization;

namespace ALMS.Model
{
    public enum LicensePeriodType
    {
        [EnumMember(Value = "Yıllık")]
        Yearly,
        [EnumMember(Value = "Aylık")]
        Monthly,
        [EnumMember(Value = "Günlük")]
        Daily
    }
}
