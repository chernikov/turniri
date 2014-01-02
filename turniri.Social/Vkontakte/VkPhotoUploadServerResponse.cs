using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Vkontakte
{
    [JsonObject]
    public class VkPhotoUploadServerResponse
    {
        [JsonProperty("response")]
        public PhotoUploadServerItem Response { get; set; }
    }

    public class PhotoUploadServerItem
    {
        [JsonProperty("upload_url")]
        public string UploadUrl { get; set; }

    }
}
