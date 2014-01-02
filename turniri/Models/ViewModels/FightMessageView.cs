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
    public class FightMessageView
    {
        public bool InitedReceiver { get; set; }

        public bool InitedGame { get; set; }

        public int ID { get; set; }

        public int SenderID { get; set; }

        public int ReceiverID { get; set; }

        public string ReceiverLogin { get; set; }

        [Required(ErrorMessage = "Введите текст")]
        public string Text { get; set; }

        public int PlatformID { get; set; }

        private List<Platform> Platforms
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.Platforms.ToList();
            }
        }

        public IEnumerable<SelectListItem> PlatformSelectList
        {
            get
            {
                return Platforms.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Selected = p.ID == PlatformID,
                    Text = p.Name
                });
            }
        }

        public int GameID { get; set; }

        private List<Game> Games
        {
            get
            {
                if (PlatformID != 0)
                {
                    var kernel = DependencyResolver.Current.GetService<IKernel>();
                    var repository = kernel.Get<IRepository>("RequestScoped");
                    return repository.Games.Where(p => p.PlatformID == PlatformID).ToList();
                }
                return new List<Game>();
            }
        }

        public IEnumerable<SelectListItem> GamesSelectList
        {
            get
            {
                return Games.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Selected = p.ID == GameID,
                    Text = p.Name
                });
            }
        }

        /// <summary>
        /// Количество игр в матче 1, 3 
        /// </summary>
        public int CountRound { get; set; }

        public IEnumerable<SelectListItem> CountRoundSelectList
        {
            get
            {
                yield return new SelectListItem { Value = "1", Text = "1", Selected = CountRound == 1 };
                yield return new SelectListItem { Value = "3", Text = "3 (до двух побед)", Selected = CountRound == 3 };
            }
        }

        public FightMessageView()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            PlatformID = repository.Platforms.First().ID;
        }
    }
}