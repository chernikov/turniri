using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Attributes;
using turniri.Attributes.Validation;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class CartDeliverView
    {
        public int ID { get; set; }

        public Guid GlobalUniqueID { get; set; }

        public int? UserID { get; set; }

        public Model.User User { get; set; }

        public double TotalSum { get; set; }

        [RequiredIf("IsReal", true, ErrorMessage = "¬ведите адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "¬ведите телефон")]
        [ValidPhone(ErrorMessage = "¬ведите корректный телефон")]
        public string Phone { get; set; }

        [RequiredIf("IsCodes", true, ErrorMessage = "¬ведите Email")]
        [ValidEmail(ErrorMessage = "¬ведите корректный Email")]
        public string Email { get; set; }

        public string Notice { get; set; }

        public bool IsCodes { get; set; }

        public bool IsReal { get; set; }

        public bool IsGold { get; set; }

        public int PaymentType { get; set; }

        public IEnumerable<SelectListItem> PaymentTypeSelectList
        {
            get
            {
                if (User != null)
                {
                    if (User.MoneyGold > TotalSum && !IsGold)
                    {
                        yield return new SelectListItem()
                        {
                            Value = ((int)Cart.PaymentTypes.GoldMoney).ToString(),
                            Text = "«олотым “и",
                            Selected = PaymentType == (int)Cart.PaymentTypes.GoldMoney
                        };
                    }
                }
                yield return new SelectListItem()
                {
                    Value = ((int)Cart.PaymentTypes.Yandex).ToString(),
                    Text = "яндекс.деньги",
                    Selected = PaymentType == (int)Cart.PaymentTypes.Yandex
                };

                yield return new SelectListItem()
                {
                    Value = ((int)Cart.PaymentTypes.Qiwi).ToString(),
                    Text = "QIWI  ошелек",
                    Selected = PaymentType == (int)Cart.PaymentTypes.Qiwi
                };
            }
        }
    }
}