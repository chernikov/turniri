using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class BackgroundView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "������� ����")]
		public string Path {get; set; }

        [Required(ErrorMessage = "�������� �����������")]
		public string ImagePath {get; set; }

		public bool IsOn {get; set; }
    }
}