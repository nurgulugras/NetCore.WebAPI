using System;
namespace ALMS.Model
{
    public class SessionInfo : IDtoEntity
    {
        public Guid SessionUID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; }
        public LicenseUserLimitInfo UserSessionLimitInfo { get; set; }
        public DateTime LastActivityDate { get; set; }
    }
}