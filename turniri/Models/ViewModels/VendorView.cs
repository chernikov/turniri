using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;
using ManageAttribute;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace turniri.Models.ViewModels
{ 
	public class VendorView
    {
        public int ID { get; set; }

        [Required(ErrorMessage="¬ведите наименование")]
		public string Name {get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "¬ведите текст")]
        public string Text { get; set; }
    }
}