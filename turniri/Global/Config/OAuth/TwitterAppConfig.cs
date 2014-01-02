using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using turniri.Social.Twitter;

namespace turniri.Global.Config.OAuth
{
    public class TwitterAppConfig : ConfigurationSection, ITwitterAppConfig
    {
        [ConfigurationProperty("twitterConsumerKey", IsRequired = true)]
        public string twitterConsumerKey
        {
            get
            {
                return this["twitterConsumerKey"] as string;
            }

            set
            {
                this["twitterConsumerKey"] = value;
            }
        }

        [ConfigurationProperty("twitterConsumerSecret", IsRequired = true)]
        public string twitterConsumerSecret
        {
            get
            {
                return this["twitterConsumerSecret"] as string;
            }

            set
            {
                this["twitterConsumerSecret"] = value;
            }
        }
    }
}