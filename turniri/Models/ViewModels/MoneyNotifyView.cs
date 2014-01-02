using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class MoneyNotifyView
    {
        public int ID { get; set; }

		public int Type {get; set; }

		public string Data {get; set; }

		public DateTime AddedDate {get; set; }

    }
}