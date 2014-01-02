using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class ReputationView
    {
        public int ID { get; set; }

		public int SenderID {get; set; }

		public int ReceiverID {get; set; }

		public int ReputationType {get; set; }

		public int Mark {get; set; }
    }
}