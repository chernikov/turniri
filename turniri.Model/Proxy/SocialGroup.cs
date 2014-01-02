using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class SocialGroup
    {
        public string ProviderName
        {
            get
            {
                return ((Social.ProviderType)Provider).ToString();
            }
        }
	}
}