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

        [RequiredIf("IsReal", true, ErrorMessage = "������� �����")]
        public string Address { get; set; }

        [Required(ErrorMessage = "������� �������")]
        [ValidPhone(ErrorMessage = "������� ���������� �������")]
        public string Phone { get; set; }

        [RequiredIf("IsCodes", true, ErrorMessage = "������� Email")]
        [ValidEmail(ErrorMessage = "������� ���������� Email")]
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
                            Text = "������� ��",
                            Selected = PaymentType == (int)Cart.PaymentTypes.GoldMoney
                        };
                    }
                }
                yield return new SelectListItem()
                {
                    Value = ((int)Cart.PaymentTypes.Yandex).ToString(),
                    Text = "������.������",
                    Selected = PaymentType == (int)Cart.PaymentTypes.Yandex
                };

                yield return new SelectListItem()
                {
                    Value = ((int)Cart.PaymentTypes.Qiwi).ToString(),
                    Text = "QIWI �������",
                    Selected = PaymentType == (int)Cart.PaymentTypes.Qiwi
                };
            }
        }
    }
}