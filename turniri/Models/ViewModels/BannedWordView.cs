using ManageAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class BannedWordView
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Слово введите")]
        public string Word {get; set; }

		public bool IsCanBeSubWord {get; set; }
    }
}