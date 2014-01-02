using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using turniri.Model;

namespace turniri.Models.ViewModels
{
    public class GroupView : BaseGroupView
    {
        public IEnumerable<Game> AllGames
        {
            get
            {
                if (UserID != 0)
                {
                    var kernel = DependencyResolver.Current.GetService<IKernel>();
                    var repository = kernel.Get<IRepository>("RequestScoped");
                    var user = repository.Users.FirstOrDefault(p => p.ID == UserID);

                    var usedGames = user.GroupsWhereImMember.Select(p => p.GameID);
                    return user.Games.Where(p => !usedGames.Contains(p.ID) || p.ID == GameID);
                } return null;
            }
        }

        public IEnumerable<SelectListItem> SelectListGameID
        {
            get
            {
                if (AllGames != null)
                {
                    foreach (var game in AllGames)
                    {
                        yield return  new SelectListItem
                        {
                            Value = game.ID.ToString(),
                            Text = string.Format("{0} ({1})", game.Name, game.Platform.Name),
                            Selected = GameID == game.ID
                        };
                    }
                }
            }
        }

    }
}