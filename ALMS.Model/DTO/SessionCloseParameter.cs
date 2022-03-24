using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class SessionCloseParameter : APILicenseRequestBase
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public Guid SessionUID { get; set; }
    }
}