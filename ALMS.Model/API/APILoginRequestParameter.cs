using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class APILoginRequestParameter
    {
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public string ApiSecretKey { get; set; }
    }
}