using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace turniri.Tools.Video
{
    [JsonObject]
    public class VimeoInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("upload_date")]
        public DateTime UploadDate { get; set; }

        [JsonProperty("mobile_url")]
        public string MobileUrl { get; set; }

        [JsonProperty("thumbnail_small")]
        public string ThumbnailSmall { get; set; }

        [JsonProperty("thumbnail_medium")]
        public string ThumbnailMedium { get; set; }

        [JsonProperty("thumbnail_large")]
        public string ThumbnailLarge { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_url")]
        public string UserUrl { get; set; }

        [JsonProperty("user_portrait_small")]
        public string UserPortraitSmall { get; set; }

        [JsonProperty("user_portrait_medium")]
        public string UserPortraitMedium { get; set; }

        [JsonProperty("user_portrait_large")]
        public string UserPortraitLarge { get; set; }

        [JsonProperty("user_portrait_huge")]
        public string UserPortraitHuge { get; set; }

        [JsonProperty("stats_number_of_likes")]
        public int StatsNumberOfLikes { get; set; }

        [JsonProperty("stats_number_of_plays")]
        public int StatsNumberOfPlays { get; set; }

        [JsonProperty("stats_number_of_comments")]
        public int StatsNumberOfComments { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("embed_privacy")]
        public string EmbedPrivacy { get; set; }
    }

}
