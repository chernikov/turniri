using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class TournamentConditionView
    {
        public int ID { get; set; }

		public int TournamentID {get; set; }

		public bool FirstName {get; set; }

		public bool LastName {get; set; }

		public bool PlaystationID {get; set; }

		public bool XboxGametag {get; set; }

		public bool EAAccount {get; set; }

		public bool SteamAccount {get; set; }

		public bool GarenaAccount {get; set; }

		public bool ICQ {get; set; }

		public bool Skype {get; set; }

        public bool Vk { get; set; }



		public TournamentView Tournament {get; set; }
    }
}