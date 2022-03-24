using System;

namespace ALMS.Model
{
    public class SessionApiDto : IDtoEntity
    {
        public Guid SessionUId { get; set; }
        public DateTime CreateAt { get; set; }
        public string Username { get; set; }
        public DateTime LastActivityDate { get; set; }
    }
}