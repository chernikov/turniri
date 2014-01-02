using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class NewTypeView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Введите наименование")]
		public string Name {get; set; }
    }
}