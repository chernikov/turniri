using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class ChatBannedUserView
    {
        public enum DateTillTo : int
        {
            _20min = 0x01,
            one_hour = 0x02,
            one_day = 0x03, 
            forever = 0x04
        }
        public int ID { get; set; }

		public int ChatRoomID {get; set; }

		public int UserID {get; set; }

		public DateTime? DateTill {get; set; }

        public string Reason { get; set; }

        public int ChoisePeriod { get; set; }

        public ChatBannedUserView()
        {
            ChoisePeriod = 0x01;
        }
    }
}