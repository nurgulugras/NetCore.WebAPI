namespace ALMS.Model
{
    public class MailAccount
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool UseBasicAuthentication { get; set; }
        public bool EnableSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderMail { get; set; }
        public string SenderDisplayName { get; set; }
    }
}