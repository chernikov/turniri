using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using turniri.Social.Vkontakte;

namespace turniri.Global.Config.OAuth
{
    public class VkontakteAppConfig : ConfigurationSection, IVkAppConfig
    {
        [ConfigurationProperty("AppKey", IsRequired = true)]
        public string AppKey
        {
            get
            {
                return this["AppKey"] as string;
            }

            set
            {
                this["AppKey"] = value;
            }
        }

        [ConfigurationProperty("AppSecret", IsRequired = true)]
        public string AppSecret
        {
            get
            {
                return this["AppSecret"] as string;
            }

            set
            {
                this["AppSecret"] = value;
            }
        }
    }
}