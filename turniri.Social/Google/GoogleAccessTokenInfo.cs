using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Google
{
    [JsonObject]
    public class GoogleAccessTokenInfo
    {
        [JsonProperty("id_token")]
        public string ID { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string Type { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiredIn { get; set; }
    }
}
