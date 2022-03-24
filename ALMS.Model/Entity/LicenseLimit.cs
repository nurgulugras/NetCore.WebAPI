using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Model
{
    [Logger("Lisans Limit")]
    public class LicenseLimit : EntityBase
    {
        [Required]
        [ForeignKey("License")]
        public int LicenseId { get; set; }
        public virtual License License { get; set; }

        [Required]
        [ForeignKey("AppLimit")]
        public int AppLimitId { get; set; }
        public virtual AppLimit AppLimit { get; set; }

        [Required]
        public string Limit { get; set; }
    }
}