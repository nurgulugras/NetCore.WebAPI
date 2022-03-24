using System.ComponentModel.DataAnnotations;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Model
{
    [Logger("Organization")]
    public class Organization : EntityBase
    {
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}