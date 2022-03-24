using System.Runtime.Serialization;

namespace ALMS.Model
{
    public enum RoleType
    {
        [EnumMember(Value ="API")]
        API = 6,
        [EnumMember(Value ="User")]
        User = 7,
        [EnumMember(Value ="Admin")]
        Admin = 9,
    }
}
