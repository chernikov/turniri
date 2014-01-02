using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Google
{
    [JsonObject]
    public class GoogleUserInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("verified_email")]
        public bool Verified { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("given_name")]
        public string FirstName { get; set; }

        [JsonProperty("family_name")]
        public string LastName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

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

        [JsonProperty("locale")]
        public string Locale { get; set; }

        public string Identifier
        {
            get
            {
                return string.Format("google-{0}", ID);
            }
        }
    }
}
