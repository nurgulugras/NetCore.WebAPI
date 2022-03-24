using Newtonsoft.Json;

namespace ALMS.Model
{
    public class JWTApp
    {
        public JWTApp(int appId, string passHass)
        {
            AppId = appId;
            Hash = passHass;
        }
        [JsonProperty("pu")]
        public int AppId { get; set; }

        [JsonProperty("co")]
        public string Hash { get; set; }
    }
}