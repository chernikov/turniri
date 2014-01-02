using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using turniri.Model;
using turniri.Tools;


namespace turniri.Models.ViewModels
{ 
	public class ForumMessageView
	{
        private const string RegexTemplate = @"\[quote\=([^\]]+)\](.+)\[/quote\]";
	    public int _ID
	    {
	        get { return ID; }
            set { ID = value; }
	    }

        public int ID { get; set; }

		public int UserID {get; set; }

        public int ForumID { get; set; }

		public int? ParentID {get; set; }

        [Required(ErrorMessage = "¬ведите текст")]
		public string Message {get; set; }

	    public void SetQuote(ForumMessage parent)
	    {
	        Message = parent.Quote;
	    }

        public void RemoveQuote()
        {
            if (ParentID != null)
            {
                
                var match = Regex.Match(Message, RegexTemplate, RegexOptions.Singleline);
                if (match.Success)
                {
                    Message = Message.Substring(match.Length);
                }
            }
        }
        
    }
}