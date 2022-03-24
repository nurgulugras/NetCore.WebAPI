using System.ComponentModel.DataAnnotations;
using Elsa.NNF.Common.Library;

namespace ALMS.Model
{
    public class MailProvider : EntityBase
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool UseBasicAuthentication { get; set; }
        public bool EnableSSL { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [KeyValue("CompareIgnore", true)]
        public string Password { get; set; }

        [Required]
        public string SenderMail { get; set; }
        public string SenderDisplayName { get; set; }
    }
}