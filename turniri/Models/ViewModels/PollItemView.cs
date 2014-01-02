using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class PollItemView
    {
        public int ID { get; set; }

		public int PollID {get; set; }

		public string Description {get; set; }
    }
}