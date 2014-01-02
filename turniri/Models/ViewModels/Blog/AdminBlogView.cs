using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Models.ViewModels
{
    public class AdminBlogView : BlogView
    {
        public bool IsBanned { get; set; }

        public bool ShowInMain { get; set; }

        public string BanDescription { get; set; }

        public string UserLogin { get; set; }

        public string Url { get; set; }
    }
}