using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Attributes.Validation;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class MessageView
    {
        public int ID { get; set; }

		public int SenderID {get; set; }

        public int? GameID { get; set; }

        public int? GroupID { get; set; }

        public int? MatchID { get; set; }

        public Match Match { get; set; }

        public Group Group { get; set; }

		public int ReceiverID {get; set; }

        public int SubjectID { get; set; }

        [Required(ErrorMessage="Введите тему")]
        public string SubjectName { get; set; }

        public Subject Subject { get; set; }

        public string ReceiverLogin { get; set; }

        [Required(ErrorMessage = "Введите текст")]
        [FilterBadWord(ErrorMessage="Текст содержит слова, которые мы запретили (а мы это можем) к общению")]
		public string Text {get; set; }
    }
}