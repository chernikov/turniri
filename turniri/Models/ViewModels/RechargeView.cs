using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class RechargeView
    {
        public double Sum { get; set; }

        public int Provider { get; set; }

        public IEnumerable<SelectListItem> SelectListProvider
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)Recharge.ProviderType.Yandex).ToString(), Text = "Яндекс.деньги", Selected = Provider == (int)Recharge.ProviderType.Yandex };
                yield return new SelectListItem() { Value = ((int)Recharge.ProviderType.Qiwi).ToString(), Text = "QIWI Кошелек", Selected = Provider == (int)Recharge.ProviderType.Qiwi };
            }
        }

        public RechargeView()
        {
            Provider = (int)Recharge.ProviderType.Yandex;
        }
    }
}