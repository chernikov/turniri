using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class NoticeDistributionView
    {
        public int _ID { 
            get 
            {
                return ID;
            }
            set 
            {
                ID = value;
            }
        }

        public int ID { get; set; }

        public int UserID { get; set; }

		public string Caption {get; set; }

        public int ForeignID { get; set; }

        public string ForeignUrl { get; set; }

        public bool IsCloseForRead { get; set; }

        [Required(ErrorMessage = "¬ведите содержание письма")]
		public string Text {get; set; }
    }
}