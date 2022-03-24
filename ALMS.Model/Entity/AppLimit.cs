using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Elsa.NNF.Common.Library;
using Elsa.NNF.Data.ORM.Abilities;
namespace ALMS.Model
{
    [Logger("Uygulama Limit")]
    [KeyValue("Name", "Uygulama Limit")]
    public class AppLimit : EntityBase
    {
        [Required]
        [ForeignKey("App")]
        public int AppId { get; set; }
        public virtual App App { get; set; }

        [Required]
        public string LimitName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}