using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class PageView
    {
        public int ID { get; set; }

        [Required(ErrorMessage="������� ������������")]
		public string Name {get; set; }

		public string Url {get; set; }

        [Required(ErrorMessage = "������� �����")]
		public string Text {get; set; }

		public string Description {get; set; }

		public string Keywords {get; set; }

    }
}