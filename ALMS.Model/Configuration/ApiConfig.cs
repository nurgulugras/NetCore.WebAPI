namespace ALMS.Model
{
    public class ApiConfig
    {
        public string AppName { get; set; }
        public string Version { get; set; }
        public string APIUrl { get; set; }
        public string Environment { get; set; }
        public string UIAddress { get; set; }
        public string AllowedHosts { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }
        public MailAccount MailSettings { get; set; }
        public ApprovalFlowConfig ApprovalFlowConfig { get; set; }
        public JwtTokenConfig JwtTokenConfig { get; set; }
        public bool IsProduction => Environment?.Trim().Equals("Production") ?? false;
    }
}