using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class DistributionView
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage="������� ������������")]
		public string Name {get; set; }

        [Required(ErrorMessage = "������� ���� ������")]
		public string Subject {get; set; }

        [Required(ErrorMessage = "������� ���������� ������")]
		public string Body {get; set; }
    }
}