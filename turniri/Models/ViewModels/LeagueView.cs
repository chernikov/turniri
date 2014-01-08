using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using turniri.Model;
using Ninject;
using System.ComponentModel.DataAnnotations;
using turniri.Attributes;


namespace turniri.Models.ViewModels
{
    public class LeagueView
    {
        private Model.User GameAdmin { get; set; }

        public int ID { get; set; }

        public int GameID { get; set; }

        [Required(ErrorMessage="������� ������������ ����")]
        public string Name { get; set; }

        [Required(ErrorMessage="������� url")]
        public string Url { get; set; }
        
        [Required(ErrorMessage="�������� �����������")]
        public string Image { get; set; }

        public string FullImage
        {
            get { return string.IsNullOrWhiteSpace(Image) ? "/Media/images/default_game.jpg" : Image; }
        }

        private List<Game> Games
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                if (GameAdmin == null)
                {
                    return repository.Games.ToList();
                }
                else
                {
                    return GameAdmin.AdminGames.ToList();
                }
            }
        }

        public IEnumerable<SelectListItem> SelectListGameID
        {
            get
            {
                return Games.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Text = string.Format("{0} ({1})", p.Name, p.Platform.Name),
                    Selected = p.ID == GameID
                });
            }
        }

        public void InitGameAdmin(Model.User user)
        {
            this.GameAdmin = user;
        }

        public int CountRound { get; set; }

        public IEnumerable<SelectListItem> CountRoundSelectList
        {
            get
            {
                yield return new SelectListItem { Value = "1", Text = "1", Selected = CountRound == 1 };
                yield return new SelectListItem { Value = "3", Text = "3 (�� ���� �����)", Selected = CountRound == 3 };
            }
        }

        public bool IsGroup { get; set; }

        public IEnumerable<SelectListItem> SelectListIsGroup
        {
            get
            {
                yield return new SelectListItem { Value = "false", Text = "��� ��������� ��������", Selected = !IsGroup };
                yield return new SelectListItem { Value = "true", Text = "��� ��������� ��������", Selected = IsGroup };
            }
        }

        /// <summary>
        /// ���-�� ����� �� �������
        /// </summary>
        public int SingleWinPoint { get; set; }

        /// <summary>
        /// ���-�� ����� �� �����
        /// </summary>
        public int SingleDrawPoint { get; set; }

        /// <summary>
        /// ������� ���� � ������
        /// </summary>
        public bool HostGuest { get; set; }

        /// <summary>
        /// ������������ ���� � ������
        /// </summary>
        public bool DoubleGoalInGuest { get; set; }

   


        [Required(ErrorMessage = "��������� ������� �������")]
        public string Rules { get; set; }

        public string Description { get; set; }

        [MinValueIf("IsGroup", true, 2, ErrorMessage = "���������� ���������� � ������� ������ ���� ������ ������")]
        public int TeamCount { get; set; }

        public int? HotReplacement { get; set; }

        public bool IsRoundForPoints { get; set; }

        public bool CanChangeTournamentData { get; set; }

        public LeagueView()
        {
            CanChangeTournamentData = true;
        }
    }
}