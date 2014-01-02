using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Models.ViewModels
{
    public class SocialView
    {
        public Model.Social.ProviderType Provider { get; set; }

        public string Identifier { get; set; }

        public string UserInfo { get; set; }

        public string JsonResource { get; set; }
    }
}
