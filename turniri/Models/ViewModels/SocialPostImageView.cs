using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class SocialPostImageView
    {
        public int ID { get; set; }

		public int SocialPostID {get; set; }

		public string PhotoUrl {get; set; }


		public SocialPostView SocialPost {get; set; }
    }
}