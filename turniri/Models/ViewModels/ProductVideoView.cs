using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class ProductVideoView
    {
        public int ID { get; set; }

        public string VideoUrl { get; set; }

        public string VideoThumb { get; set; }

        public string FullVideoThumb
        {
            get
            {
                return VideoThumb ?? "/Media/images/default.png";
            }
        }

        [AllowHtml]
        public string VideoCode { get; set; }

    }
}