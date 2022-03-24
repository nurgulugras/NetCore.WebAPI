namespace ALMS.Model
{
    public class MailInfo
    {
        public MailInfo() { }
        public MailInfo(string to, string subject, string body)
        {
            To = new string[] { to };
            Subject = subject;
            Body = body;
        }
        public string[] To { get; set; }
        public string[] Cc { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}