using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class MatchView
    {
        public int ID { get; set; }

		public int? MessageID {get; set; }

		public int? TournamentID {get; set; }

		public int? Participant1ID {get; set; }

		public int? Participant2ID {get; set; }

		public int Number {get; set; }

		public int Score1 {get; set; }

		public int Score2 {get; set; }

		public int? WinnerID {get; set; }
    }
}