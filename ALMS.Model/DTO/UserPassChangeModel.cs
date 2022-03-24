using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class UserPassChangeModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}