using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using turniri.Social.Facebook;
using turniri.Social.Google;

namespace turniri.Global.Config.OAuth
{
    public class GoogleAppConfig : ConfigurationSection, IGoogleAppConfig
    {
        [ConfigurationProperty("ClientId", IsRequired = true)]
        public string ClientId
        {
            get
            {
                return this["ClientId"] as string;
            }

            set
            {
                this["ClientId"] = value;
            }
        }

        [ConfigurationProperty("ClientSecret", IsRequired = true)]
        public string ClientSecret
        {
            get
            {
                return this["ClientSecret"] as string;
            }

            set
            {
                this["ClientSecret"] = value;
            }
        }
    }
}