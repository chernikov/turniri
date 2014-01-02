using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Vkontakte
{
    [JsonObject]
    public class VkUploadPhotoResponse
    {
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
