using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class MoneyWithdrawView
    {
        public int ID { get; set; }

		public double Sum {get; set; }

		public int Provider {get; set; }

        public IEnumerable<SelectListItem> SelectListProvider
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)MoneyWithdraw.ProviderType.Yandex).ToString(), Text = "яндекс.деньги", Selected = Provider == (int)Recharge.ProviderType.Yandex };
            }
        }

        public string Account { get; set; }

        public MoneyWithdrawView()
        {
            Provider = (int)MoneyWithdraw.ProviderType.Yandex;
        }
    }
}