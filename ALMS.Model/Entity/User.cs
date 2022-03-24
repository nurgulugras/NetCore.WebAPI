using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elsa.NNF.Common.Library;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Model
{
    [Logger("Kullanıcı")]
    [KeyValue("Name", "Kullanıcı")]
    public class User : IEntityBase
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public RoleType Role { get; set; }
        public string Password { get; set; }
        public string PasswordHashCode { get; set; }

        [NotMapped]
        public string FullName => string.IsNullOrEmpty(Name) ? string.Empty : string.Concat(Name, " ", Surname);

        public bool IsActive { get; set; }

        public User Shallowcopy()
        {
            return (User)this.MemberwiseClone();
        }
    }
}