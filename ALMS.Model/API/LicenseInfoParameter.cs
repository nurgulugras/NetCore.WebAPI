using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class LicenseInfoParameter
    {
        [Required]
        public string LicenseNo { get; set; }
    }
}