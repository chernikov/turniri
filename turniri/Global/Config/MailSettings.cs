using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace turniri.Global.Config
{
    public class MailSettings : ConfigurationSection
    {
        // Create a "SmtpServer" attribute.
        [ConfigurationProperty("SmtpServer", IsRequired = true)]
        public string SmtpServer
        {
            get
            {
                return this["SmtpServer"] as string;
            }
            set
            {
                this["SmtpServer"] = value;
            }
        }

        // Create a "SmtpPort" attribute.
        [ConfigurationProperty("SmtpPort", IsRequired = true)]
        public int SmtpPort
        {
            get
            {
                return (int)this["SmtpPort"];
            }
            set
            {
                this["SmtpPort"] = value;
            }
        }

        // Create a "SmtpUserName" attribute.
        [ConfigurationProperty("SmtpUserName", IsRequired = true)]
        public string SmtpUserName
        {
            get
            {
                return this["SmtpUserName"] as string;
            }
            set
            {
                this["SmtpUserName"] = value;
            }
        }


        // Create a "SmtpPassword" attribute.
        [ConfigurationProperty("SmtpPassword", IsRequired = true)]
        public string SmtpPassword
        {
            get
            {
                return this["SmtpPassword"] as string;
            }
            set
            {
                this["SmtpPassword"] = value;
            }
        }

        // Create a "SmtpReply" attribute.
        [ConfigurationProperty("SmtpReply", IsRequired = true)]
        public string SmtpReply
        {
            get
            {
                return this["SmtpReply"] as string;
            }
            set
            {
                this["SmtpReply"] = value;
            }
        }

        // Create a "SmtpUser" attribute.
        [ConfigurationProperty("SmtpUser", IsRequired = true)]
        public string SmtpUser
        {
            get
            {
                return this["SmtpUser"] as string;
            }
            set
            {
                this["SmtpUser"] = value;
            }
        }


        // Create a "enable ssl" attribute.
        [ConfigurationProperty("EnableSsl", IsRequired = true)]
        public bool EnableSsl
        {
            get
            {
                return (bool)this["EnableSsl"];
            }
            set
            {
                this["EnableSsl"] = value;
            }
        }
    }
}