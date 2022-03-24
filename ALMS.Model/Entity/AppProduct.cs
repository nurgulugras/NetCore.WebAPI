using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elsa.NNF.Data.ORM.Abilities;

namespace ALMS.Model
{
    [Logger("Uygulama Ürün")]
    public class AppProduct : EntityBase
    {
        [Required]
        [ForeignKey("App")]
        public int AppId { get; set; }
        public virtual App App { get; set; }

        [Required]
        public string Product { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}