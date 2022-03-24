using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class SessionCheckParameter
    {
        [Required]
        public Guid SessionUID { get; set; }
    }
}