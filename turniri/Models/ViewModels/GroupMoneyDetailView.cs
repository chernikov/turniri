using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class GroupMoneyDetailView
    {
        public enum MoneyType : int
        {
            Gold = 0x01,
            Wood = 0x02,
            Crystal = 0x03
        }

        public int ID { get; set; }

        public int GroupID { get; set; }

        public int UserID { get; set; }

        public double MaxGold { get; set; }

        public double MaxWood { get; set; }

        public double MaxCrystal { get; set; }

        private IEnumerable<Model.User> Users
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");

                var group = repository.Groups.FirstOrDefault(p => p.ID == GroupID);
                if (group != null)
                {
                    var users = group.GrantedUserGroups.Select(p => p.User);
                    return users;
                }
                return null;
            }
        }

        public IEnumerable<SelectListItem> SelectListUserID
        {
            get
            {
                if (Users != null)
                {
                    foreach (var user in Users)
                    {
                        yield return new SelectListItem()
                        {
                            Value = user.ID.ToString(),
                            Text = user.Login,
                            Selected = user.ID == UserID
                        };
                    }
                }
            }
        }

        public double PercentFee { get; set; }

        [Required(ErrorMessage="Введите сумму")]
        public double Sum { get; set; }

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