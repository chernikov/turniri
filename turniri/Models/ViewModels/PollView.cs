using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class PollView
    {
        public int ID { get; set; }

        public int UserID { get; set; }

		public string Name {get; set; }

		public int PollType {get; set; }

        public IEnumerable<SelectListItem> PollTypeSelectList
        {
            get
            {
                yield return new SelectListItem() { Value = "1", Text = "Множественный выбор", Selected = PollType == (int)Poll.PollTypeEnum.CheckBox };
                yield return new SelectListItem() { Value = "2", Text = "Единичный выбор", Selected = PollType == (int)Poll.PollTypeEnum.Radio };
            }
        }

        

		public bool IsClosed {get; set; }

		public List<PollItemView> PollItems {get; set; }

        public bool CanUpdateItems { get; set; }
    }
}