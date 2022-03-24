using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class UserDto : IDtoEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public RoleType Role { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
    }
}