using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class AwardView
    {
        public int _ID
        {
            get { return ID; }
            set { ID = value; }
        }

        public int ID { get; set; }

        public int TournamentID { get; set; }

        [Required(ErrorMessage = "Введите наименование")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Добавьте изображение")]
        public string IconPath { get; set; }

        public int? Place { get; set; }

        [Required(ErrorMessage = "Введите кол-во очков")]
        public int Point { get; set; }

        public string FullIconPath
        {
            get
            {
                return IconPath ?? "/Media/images/default_award.png";
            }
        }

        public bool IsSpecial { get; set; }

        public double? MoneyGoldPercent { get; set; }

        public double? MoneyWood { get; set; }

        public double? MoneyCrystal { get; set; }

        public int? UserID { get; set; }

        public IEnumerable<turniri.Model.User> Users
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.Participants.Where(p => p.TournamentID == TournamentID).Select(p => p.User);
            }
        }

        public IEnumerable<SelectListItem> UsersSelectList
        {
            get
            {
                return Users.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Selected = p.ID == UserID,
                    Text = p.Login
                });
            }
        }
    }
}