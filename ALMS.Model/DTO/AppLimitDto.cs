
using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class AppLimitDto : IDtoEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }

        [Required]
        public int AppId { get; set; }
        public string AppName { get; set; }

        [Required]
        public string LimitName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}