using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace turniri.Global.Config
{
    public class IconSizesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("iconSizes")]
        public IconSizesCollection iconSizes
        {
            get
            {
                return this["iconSizes"] as IconSizesCollection;
            }
        }
    }

    public class IconSizesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IconSize();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IconSize)element).Name;
        }
    }

    public class IconSize : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("width", IsRequired = true, DefaultValue = "48")]
        public int Width
        {
            get
            {
                return (int)this["width"];
            }
        }

        [ConfigurationProperty("height", IsRequired = true, DefaultValue = "48")]
        public int Height
        {
            get
            {
                return (int)this["height"];
            }
        }

        public override string ToString()
        {
            return string.Format("{0}x{1}", Width, Height);
        }
    }
}