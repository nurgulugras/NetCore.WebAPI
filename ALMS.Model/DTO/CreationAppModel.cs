using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class CreationAppModel : IDtoEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        // public bool IsApprovalActive { get; set; }
        public bool IsActive { get; set; }
    }
}