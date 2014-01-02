using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class RatingDetailView
    {
        public int ID { get; set; }

		public int RatingID {get; set; }

		public int? MatchID {get; set; }

		public int Score {get; set; }

		public string Description {get; set; }
    }
}