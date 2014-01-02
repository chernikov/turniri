using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class PromoActionView
    {
        public int ID { get; set; }

        public int? ProductID { get; set; }

        public string ProductName { get; set; }

        [Required(ErrorMessage = "Введите наименование")]
        public string Name { get; set; }

        public bool OnlyManager { get; set; }

        public int Type { get; set; }

        public IEnumerable<SelectListItem> SelectListType
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = ((int)PromoAction.TypeEnum.Absolute).ToString(),
                    Text = "руб.",
                    Selected = Type == (int)PromoAction.TypeEnum.Absolute
                };
                yield return new SelectListItem()
                {
                    Value = ((int)PromoAction.TypeEnum.Percentage).ToString(),
                    Text = "%",
                    Selected = Type == (int)PromoAction.TypeEnum.Percentage
                };
            }
        }

        public double Amount { get; set; }

        public DateTime? ValidDate { get; set; }

        public bool Closed { get; set; }

        public bool Reusable { get; set; }

        public int Quantity { get; set; }

        public string Code { get; set; }

        public IEnumerable<SelectListItem> SelectListReusable
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "false",
                    Text = "Нет",
                    Selected = !Reusable
                };

                yield return new SelectListItem()
                {
                    Value = "true",
                    Text = "Да",
                    Selected = Reusable
                };
            }
        }

        public bool CanChangeReusable { get; set; }
    }
}