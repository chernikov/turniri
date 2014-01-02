using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Social
    {
        public enum ProviderType
        {
            facebook = 0x01,
            google = 0x02,
            vk = 0x03,
            twitter = 0x04
        }
	}
}