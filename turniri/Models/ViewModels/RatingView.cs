using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class RatingView
    {
        public int ID { get; set; }

		public int UserID {get; set; }

		public int GameID {get; set; }

		public int Level {get; set; }

		public int TotalScore {get; set; }
    }
}