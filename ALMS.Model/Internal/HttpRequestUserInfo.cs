namespace ALMS.Model
{
    public class HttpRequestUserInfo
    {
        public HttpRequestUserInfo() { }
        public HttpRequestUserInfo(string ip, string userAgent, string mechineName)
        {
            IP = ip;
            UserAgent = userAgent;
            MechineName = mechineName;
        }

        public string IP { get; set; }
        public string UserAgent { get; set; }
        public string MechineName { get; set; }
    }
}