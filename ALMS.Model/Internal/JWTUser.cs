
using Newtonsoft.Json;

namespace ALMS.Model
{
    public class JWTUser
    {
        [JsonProperty("nu")]
        public string Username { get; set; }

        [JsonProperty("tu")]
        public RoleType UserType { get; set; }

        [JsonProperty("co")]
        public string Hash { get; set; }
    }
}