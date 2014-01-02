using ManageAttribute;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels.User;
using turniri.Attributes.Validation;


namespace turniri.Models.ViewModels
{
    public class AdminGroupView : BaseGroupView
    {
        public string UserLogin { get; set; }

        public IEnumerable<SelectListItem> SelectListUserID
        {
            get
            {
                yield return new SelectListItem { Value = UserID.ToString(), Text = UserLogin, Selected = true };
            }
        }

        public IQueryable<Game> AllGames
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.Games;
            }
        }

        public IEnumerable<SelectListItem> SelectListGameID
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "Не выбрано",
                    Selected = GameID == 0
                });
                foreach (var game in AllGames)
                {
                    list.Add(new SelectListItem
                    {
                        Value = game.ID.ToString(),
                        Text = string.Format("{0} ({1})", game.Name, game.Platform.Name),
                        Selected = GameID == game.ID
                    });
                }
                return list;
            }
        }

        public Dictionary<string, UserGroupView> UserGroups { get; set; }
        
        public AdminGroupView()
        {
            UserGroups = new Dictionary<string, UserGroupView>();
        }
    }
}