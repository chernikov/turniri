using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class PromoCodeView
    {

        public int ID { get; set; }

        public int PromoActionID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Code { get; set; }

        public bool Reusable { get; set; }
    }
}