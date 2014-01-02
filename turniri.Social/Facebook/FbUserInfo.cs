using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Facebook
{
    [JsonObject]
    public class FbUserInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("timezone")]
        public double? Timezone { get; set; }

        [JsonProperty("verified")]
        public bool? Verified { get; set; }

        [JsonProperty("updated_time")]
        public DateTime? updatedTime { get; set; }

        [JsonProperty("birthday")]
        public string BirthDay { get; set; }

        public DateTime? RealBirthDate
        {
            get
            {
                DateTime birthDayCandidate;
                if (DateTime.TryParse(BirthDay, new CultureInfo("en-US"), DateTimeStyles.None, out birthDayCandidate))
                {
                    return birthDayCandidate;
                }
                return null;
            }
        }

        public string Identifier
        {
            get
            {
                return string.Format("facebook-{0}", Id);
            }
        }

    }
}
