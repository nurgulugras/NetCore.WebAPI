using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class APILicenseRequestBase
    {
        /// <summary>
        /// İşlem yapılacak lisans numarası
        /// </summary>
        /// <value></value>
        [Required]
        public string LicenseNo { get; set; }
    }
}