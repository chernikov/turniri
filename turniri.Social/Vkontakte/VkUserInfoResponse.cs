using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Vkontakte
{
    [JsonObject]
    public class VkUserInfoResponse
    {
        [JsonProperty("response")]
        public List<VkUserInfo> Response { get; set; }
    }

    public class VkUserInfo
    {
        [JsonProperty("uid")]
        public int Uid { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("has_mobile")]
        public bool HasMobile { get; set; }

        [JsonProperty("home_phone")]
        public string HomePhone { get; set; }

        [JsonProperty("mobile_phone")]
        public string MobilePhone { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("bdate")]
        public string BirthDate { get; set; }

        public DateTime? RealBirthDate
        {
            get
            {
                DateTime birthDayCandidate;
                if (DateTime.TryParse(BirthDate, new CultureInfo("ru-RU"), DateTimeStyles.None, out birthDayCandidate))
                {
                    return birthDayCandidate;
                }
                return null;
            }
        }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("photo_big")]
        public string Photo { get; set; }

        public string Identifier
        {
            get
            {
                return string.Format("vk-{0}", Uid);
            }
        }
    }

}
