using ManageAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class SocialGroupView
    {
        [PrimaryField]
        public int ID { get; set; }

        [ShowIndex]
        [DropDownField]
		public int Provider {get; set; }

        public IEnumerable<SelectListItem> ProviderSelectList
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)Model.Social.ProviderType.facebook).ToString(), Text = "Facebook", Selected = Provider == (int)Model.Social.ProviderType.facebook };
                yield return new SelectListItem() { Value = ((int)Model.Social.ProviderType.vk).ToString(), Text = "Vk", Selected = Provider == (int)Model.Social.ProviderType.vk };
            }
        }

        [ShowIndex]
        [TextBoxField]
		public string Name {get; set; }
    }
}