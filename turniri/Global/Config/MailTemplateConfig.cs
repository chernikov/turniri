using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace turniri.Global.Config
{
    public class MailTemplateConfig : ConfigurationSection
    {
        [ConfigurationProperty("mailTemplates")]
        public MailTemplatesCollection mailTemplates
        {
            get
            {
                return this["mailTemplates"] as MailTemplatesCollection;
            }
        }
    }

    public class MailTemplatesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MailTemplate();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MailTemplate)element).Name;
        }
    }

    public class MailTemplate : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("subject", IsRequired = true)]
        public string Subject
        {
            get
            {
                return this["subject"] as string;
            }
        }

        [ConfigurationProperty("template", IsRequired = true)]
        public string Template
        {
            get
            {
                return this["template"] as string;
            }
        }
    }
}