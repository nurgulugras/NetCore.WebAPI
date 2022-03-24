using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class UserLoginRequestParameter
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}