using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class AdminMoneyDetailView
    {
        public enum MoneyType : int
        {
            Gold = 0x01,
            Wood = 0x02,
            Crystal = 0x03
        }

        public int ID { get; set; }

        public int? UserID { get; set; }

        public int? GroupID { get; set; }

        public double PercentFee { get; set; }

        [Required(ErrorMessage="Введите сумму")]
        public double Sum { get; set; }

        [Required(ErrorMessage = "Добавьте комментарий")]
        public string Description { get; set; }

        public int Type { get; set; }

        public IEnumerable<SelectListItem> SelectListMoneyType
        {
            get
            {
                yield return new SelectListItem()
                {
                     Value = ((int)MoneyType.Gold).ToString(),
                     Text = "Золотой Ти",
                     Selected = Type == (int)MoneyType.Gold
                };

                yield return new SelectListItem()
                {
                    Value = ((int)MoneyType.Wood).ToString(),
                    Text = "Белый Ти",
                    Selected = Type == (int)MoneyType.Wood
                };

                yield return new SelectListItem()
                {
                    Value = ((int)MoneyType.Crystal).ToString(),
                    Text = "Красный Ти",
                    Selected = Type == (int)MoneyType.Crystal
                };
            }
        }
    }
}