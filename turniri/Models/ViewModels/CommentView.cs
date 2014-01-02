using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class CommentView
    {
        public int ID { get; set; }

        public int OwnerID { get; set; }

	    public int? ParentID { get; set; }

		public int UserID { get; set; }

        public string ImagePath { get; set; }
        
        public string VideoUrl { get; set; }

        public string Text {get; set; }
    }
}