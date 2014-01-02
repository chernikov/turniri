using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;
using ManageAttribute;


namespace turniri.Models.ViewModels
{ 
	public class CategoryView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [TextBoxField]
        [ShowIndex]
		public string Name {get; set; }

        [ShowIndex]
		public string Url {get; set; }
    }
}