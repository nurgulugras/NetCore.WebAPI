using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Model
{
    [Logger("Lisans Ürün")]

    public class LicenseProduct : EntityBase
    {
        [Required]
        [ForeignKey("License")]
        public int LicenseId { get; set; }
        public virtual License License { get; set; }

        [Required]
        [ForeignKey("AppProduct")]
        public int AppProductId { get; set; }
        public virtual AppProduct AppProduct { get; set; }
    }
}