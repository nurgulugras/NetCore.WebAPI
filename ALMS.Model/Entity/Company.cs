using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Model
{
    [Logger("Şirket")]
    public class Company : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [ForeignKey("Organization")]
        [Required]
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public bool IsActive { get; set; }
    }
}