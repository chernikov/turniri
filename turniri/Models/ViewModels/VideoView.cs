using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class VideoView
    {
        public int ID { get; set; }

		public string Header {get; set; }

		public string VideoUrl {get; set; }

        public string VideoThumb { get; set; }

        public string VideoCode { get; set; }

	    public string Text {get; set; }
    }
}