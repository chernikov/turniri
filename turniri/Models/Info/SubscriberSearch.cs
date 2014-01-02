using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using turniri.Model;

namespace turniri.Models.Info
{
    public class SubscriberSearch
    {
        public int ID { get; set; }

        public int? RoleID { get; set; }

        public User CurrentUser { get; set; }

        private IEnumerable<Role> Roles
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.Roles;
            }
        }

        public IEnumerable<SelectListItem> RolesSelectList
        {
            get
            {
                if (CurrentUser == null || CurrentUser.InRoles("admin"))
                {
                    yield return new SelectListItem() { Value = "", Text = "Все", Selected = !RoleID.HasValue };
                    foreach (var role in Roles)
                    {
                        yield return new SelectListItem() { Value = role.ID.ToString(), Text = role.Name, Selected = RoleID == role.ID };
                    }
                    yield return new SelectListItem() { Value = "101", Text = "Игроки", Selected = RoleID == 101 };
                    yield return new SelectListItem() { Value = "102", Text = "Участники турнира", Selected = RoleID == 102 };
                }
                else if (CurrentUser.InRoles("game_admin"))
                {
                    yield return new SelectListItem() { Value = "101", Text = "Игроки", Selected = RoleID == 101 };
                    yield return new SelectListItem() { Value = "102", Text = "Участники турнира", Selected = RoleID == 102 };
                } else {
                    yield return new SelectListItem() { Value = "102", Text = "Участники турнира", Selected = RoleID == 102 };
                }
                
            }
        }

        private IEnumerable<Tournament> Tournaments
        {
            get
            {
                if (CurrentUser == null || CurrentUser.InRoles("admin"))
                {
                    var kernel = DependencyResolver.Current.GetService<IKernel>();
                    var repository = kernel.Get<IRepository>("RequestScoped");
                    return repository.OpenTournaments;
                }
                else 
                {
                    return CurrentUser.AdminTournaments;
                }
            }
        }

        public List<int> SelectedTournaments { get; set; }

        public IEnumerable<SelectListItem> SelectListTournaments
        {
            get
            {
                foreach (var tournament in Tournaments)
                {
                    yield return new SelectListItem()
                    {
                        Value = tournament.ID.ToString(),
                        Text = tournament.Name,
                        Selected = SelectedTournaments.Contains(tournament.ID),
                    };
                }
            }
        }

        private IEnumerable<Game> Games
        {
            get
            {
                if (CurrentUser == null || CurrentUser.InRoles("admin"))
                {
                    var kernel = DependencyResolver.Current.GetService<IKernel>();
                    var repository = kernel.Get<IRepository>("RequestScoped");
                    return repository.Games;
                }
                else 
                {
                    return CurrentUser.AdminGames;
                }
            }
        }

        public List<int> SelectedGames { get; set; }

        public IEnumerable<SelectListItem> SelectListGames
        {
            get
            {
                foreach (var game in Games)
                {
                    yield return new SelectListItem()
                    {
                        Value = game.ID.ToString(),
                        Text = game.Platform.Name + " /  " + game.Name,
                        Selected = SelectedGames.Contains(game.ID),
                    };
                }
            }
        }

        public SubscriberSearch()
        {
            SelectedTournaments = new List<int>();
            SelectedGames = new List<int>();
        }

        public SubscriberSearch(User currentUser) : this()
        {
            CurrentUser = currentUser;
        }
    }
}