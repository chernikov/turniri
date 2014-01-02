using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using turniri.Attributes;
using turniri.Model;
using turniri.Models.Info;


namespace turniri.Models.ViewModels
{
    public class TournamentView
    {
        public int ID { get; set; }

        public int? LeagueID { get; set; }

        public int PlatformID { get; set; }

        private Model.User GameAdmin { get; set; }

        public bool DisablePlatformAndGame { get; set; }

        private List<Platform> Platforms
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                if (GameAdmin == null)
                {
                    return repository.Platforms.ToList();
                }
                else
                {
                    return GameAdmin.AdminGames.Select(p => p.Platform).Distinct().ToList();
                }
            }
        }

        public string PlatformName { get; set; }

        public string GameName { get; set; }

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
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                if (GameAdmin == null)
                {
                    if (PlatformID != 0)
                    {
                        return repository.Games.Where(p => p.PlatformID == PlatformID).ToList();
                    }
                }
                else
                {
                    if (PlatformID != 0)
                    {
                        return GameAdmin.AdminGames.Where(p => p.PlatformID == PlatformID).ToList();
                    }
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

        [Required(ErrorMessage = "Введите наименование турнира")]
        public string Name { get; set; }

        public int TournamentType { get; set; }

        public IEnumerable<SelectListItem> TournamentTypeSelectList
        {
            get
            {
                yield return
                   new SelectListItem
                    {
                        Value = ((int)Tournament.TournamentTypeEnum.SingleElimination).ToString(),
                        Text = "Сингл-элиминейшн",
                        Selected = TournamentType == ((int)Tournament.TournamentTypeEnum.SingleElimination)
                    };
                yield return
                    new SelectListItem
                    {
                        Value = ((int)Tournament.TournamentTypeEnum.DoubleElimination).ToString(),
                        Text = "Дабл-элиминейшн",
                        Selected = TournamentType == ((int)Tournament.TournamentTypeEnum.DoubleElimination)
                    };
                yield return
                   new SelectListItem
                   {
                       Value = ((int)Tournament.TournamentTypeEnum.RoundRobin).ToString(),
                       Text = "Каждый с каждым",
                       Selected = TournamentType == ((int)Tournament.TournamentTypeEnum.RoundRobin)
                   };
                yield return
                 new SelectListItem
                 {
                     Value = ((int)Tournament.TournamentTypeEnum.GroupTournament).ToString(),
                     Text = "Групповой турнир + плейофф",
                     Selected = TournamentType == ((int)Tournament.TournamentTypeEnum.GroupTournament)
                 };
            }
        }

        public int? PlayersCount { get; set; }

        public int? MinLevel { get; set; }

        public int? MaxLevel { get; set; }

        public DateTime OpenRegistrationDate { get; set; }

        [IsDateAfter("OpenRegistrationDate", true, ErrorMessage = "Дата должна быть позже или равна даты открытия регистрации")]
        public DateTime CloseRegistrationDate { get; set; }

        [IsDateAfter("CloseRegistrationDate", true, ErrorMessage = "Дата должна быть позже или равна даты закрытия регистрации")]
        public DateTime BeginDate { get; set; }

        [IsDateAfter("BeginDate", true, ErrorMessage = "Дата должна быть позже  или равна даты начала игры")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Загрузите картинку турнира")]
        public string ImagePath { get; set; }

        public string FullImagePath
        {
            get { return (string.IsNullOrWhiteSpace(ImagePath) ? "/Media/images/default_game103.jpg" : ImagePath) + "?width=103&height=103&mode=crop"; }
        }

        [Required(ErrorMessage = "Заполните правила турнира")]
        public string Rules { get; set; }

        public string Description { get; set; }

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

        /// <summary>
        /// Кол-во очков за выигрыш
        /// </summary>
        public int SingleWinPoint { get; set; }

        /// <summary>
        /// Кол-во очков за ничью
        /// </summary>
        public int SingleDrawPoint { get; set; }

        /// <summary>
        /// Правило игры в гостях
        /// </summary>
        public bool HostGuest { get; set; }

        /// <summary>
        /// Преимущество гола в гостях
        /// </summary>
        public bool DoubleGoalInGuest { get; set; }

        /// <summary>
        /// Кол-во групп
        /// </summary>
        public int GroupCount { get; set; }

        /// <summary>
        /// Игроков в группе
        /// </summary>
        public int PlayersInGroup { get; set; }

        /// <summary>
        /// Кол-во вышедших из группы
        /// </summary>
        public int ExitFromGroup { get; set; }

        /// <summary>
        /// Жеребьевка
        /// </summary>
        public bool Toss { get; set; }

        public List<int> Admins { get; set; }

        public List<int> Moderators { get; set; }

        public IQueryable<Model.User> Users
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.RegularUsers;
            }
        }

        public IQueryable<Participant> Participants
        {
            get
            {
                if (ID != 0)
                {
                    var kernel = DependencyResolver.Current.GetService<IKernel>();
                    var repository = kernel.Get<IRepository>("RequestScoped");
                    return repository.Participants.Where(p => p.TournamentID == ID);
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<SelectListItem> SelectListTournamentAdmins
        {
            get
            {
                var list = new List<SelectListItem>();
                if (Admins != null)
                {
                    foreach (var admin in Admins)
                    {
                        var user = Users.FirstOrDefault(p => p.ID == admin);
                        if (user != null)
                        {
                            list.Add(new SelectListItem
                                         {
                                             Selected = true,
                                             Text = user.Login,
                                             Value = user.ID.ToString()
                                         });
                        }
                    }
                }
                return list;
            }
        }

        public IEnumerable<SelectListItem> SelectListTournamentModerators
        {
            get
            {
                var list = new List<SelectListItem>();
                if (Moderators != null)
                {
                    foreach (var moderator in Moderators)
                    {
                        var user = Users.FirstOrDefault(p => p.ID == moderator);
                        if (user != null)
                        {
                            list.Add(new SelectListItem
                            {
                                Selected = true,
                                Text = user.Login,
                                Value = user.ID.ToString()
                            });
                        }
                    }
                }
                return list;
            }
        }

        public List<int> Players { get; set; }

        public IEnumerable<SelectListItem> SelectListPlayers
        {
            get
            {
                var list = new List<SelectListItem>();
                if (Players != null && Participants != null)
                {
                    foreach (var player in Players)
                    {
                        var participant = Participants.FirstOrDefault(p => p.UserID == player);
                        if (participant != null)
                        {
                            var selectListItem = new SelectListItem
                            {
                                Selected = true,
                                Value = participant.UserID.ToString()
                            };
                            if (participant.TeamID != null)
                            {
                                selectListItem.Text = string.Format("{0} ({1})", participant.Team.Name, participant.User.Login);
                            }
                            else
                            {
                                selectListItem.Text = participant.User.Login;
                            }
                            list.Add(selectListItem);
                        }
                    }
                }
                return list;
            }
        }

        public int MatchesCount { get; set; }

        public bool IsLive { get; set; }

        [RequiredIf("IsLive", true, ErrorMessage = "Введите наименование турнира")]
        public string Place { get; set; }

        public int Status { get; set; }

        public bool IsRoundForPoints { get; set; }

        public bool IsTeam { get; set; }

        public bool IsGroup { get; set; }


        public int GroupTeam
        {
            get
            {
                return (IsTeam ? 1 : 0) * 0x02 + (IsGroup ? 1 : 0);
            }
            set
            {
                IsGroup = (value % 2) > 0;
                IsTeam = (value >> 1) % 2 > 0;
            }
        }

        public IEnumerable<SelectListItem> SelectListGroupTeam
        {
            get
            {
                yield return new SelectListItem { Value = "0", Text = "Одиночный турнир", Selected = GroupTeam == 0 };
                yield return new SelectListItem { Value = "1", Text = "Одиночный турнир только для участников команд", Selected = GroupTeam == 1 };
                yield return new SelectListItem { Value = "2", Text = "Шляпный турнир", Selected = GroupTeam == 2 };
                yield return new SelectListItem { Value = "3", Text = "Командный турнир", Selected = GroupTeam == 3 };
            }
        }

        [MinValueIf("IsTeam", true, 2, ErrorMessage = "Количество участников в команде должно быть больше одного")]
        public int TeamCount { get; set; }

        public int? HotReplacement { get; set; }

        public int? MaxPlayersCount
        {
            get
            {
                if (TournamentType == (int)Tournament.TournamentTypeEnum.GroupTournament)
                {
                    return GroupCount * PlayersInGroup;
                }
                return PlayersCount;
            }
        }

        public int ForumID { get; set; }

        public string ForumName { get; set; }

        public TournamentConditionView TournamentCondition { get; set; }

        public List<Forum> Forum(int? parentID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            if (parentID.HasValue)
            {
                return repository.Forums.Where(p => p.ParentID == parentID).OrderBy(p => p.Name).ToList();
            }
            return repository.Forums.Where(p => p.ParentID == null).OrderBy(p => p.Name).ToList();
        }

        public IEnumerable<EnumerableOfSelectListItem> ForumsSelectList
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return CreateForumsSelectList(ForumID, repository);
            }
        }

        public static IEnumerable<EnumerableOfSelectListItem> CreateForumsSelectList(int? forumID, IRepository repository)
        {
            var parentList = GetParentWay(forumID, repository);
            if (parentList != null)
            {
                foreach (var i in parentList)
                {
                    var list = ForumSelectListByName(i, repository);
                    if (list.Any())
                    {
                        yield return new EnumerableOfSelectListItem
                        {
                            EnumerableOfSelectListItems = list
                        };
                    }
                }
            }
            var downList = ForumSelectListChildByName(parentList.LastOrDefault(), repository);
            if (downList.Any())
            {
                yield return new EnumerableOfSelectListItem
                {
                    EnumerableOfSelectListItems = downList
                };
            }
        }

        private static IEnumerable<SelectListItem> ForumSelectListByName(int? id, IRepository repository)
        {
            if (id != null)
            {
                var forum = repository.Forums.FirstOrDefault(p => p.ID == id);
                var parentID = forum.ParentID;
                List<Forum> forums = forum.ParentID.HasValue
                                         ? repository.Forums.Where(p => p.ParentID == forum.ParentID).ToList()
                                         : repository.Forums.Where(p => p.ParentID == null).ToList();
                yield return new SelectListItem
                {
                    Value = parentID.HasValue ? parentID.ToString() : "",
                    Text = "Выберите форум",
                    Selected = false
                };
                foreach (var item in forums)
                {

                    yield return new SelectListItem
                    {
                        Value = item.ID.ToString(CultureInfo.InvariantCulture),
                        Text = item.Name,
                        Selected = item.ID == id
                    };
                }
            }
        }

        private static IEnumerable<SelectListItem> ForumSelectListChildByName(int? id, IRepository repository)
        {
            List<Forum> forums = id.HasValue ? repository.Forums.Where(p => p.ParentID == id).ToList()
                : repository.Forums.Where(p => p.ParentID == null).ToList();
            if (forums.Any())
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "Выберите форум",
                    Selected = false
                };
                foreach (var item in forums)
                {
                    yield return new SelectListItem
                    {
                        Value = item.ID.ToString(CultureInfo.InvariantCulture),
                        Text = item.Name,
                        Selected = false
                    };
                }
            }
        }

        private static List<int?> GetParentWay(int? forumID, IRepository repository)
        {
            var list = new List<int?>();
            list.Add(null);
            if (forumID.HasValue)
            {
                var currentID = forumID;
                while (currentID.HasValue)
                {
                    var item = repository.Forums.FirstOrDefault(p => p.ID == currentID);
                    if (item == null)
                    {
                        break;
                    }
                    list.Insert(1, item.ID);
                    currentID = item.ParentID;
                }
            }
            return list;
        }

        public string Keywords { get; set; }

        public string MetaDescription { get; set; }

        public int? MoneyType { get; set; }

        public IEnumerable<SelectListItem> SelectListMoneyType
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)Tournament.MoneyTypeEnum.Free).ToString(), Text = "Без взноса", Selected = !MoneyType.HasValue || MoneyType.Value == (int)Tournament.MoneyTypeEnum.Free };
                yield return new SelectListItem() { Value = ((int)Tournament.MoneyTypeEnum.Wood).ToString(), Text = "Взнос белый ТИ", Selected = MoneyType == (int)Tournament.MoneyTypeEnum.Wood };
                yield return new SelectListItem() { Value = ((int)Tournament.MoneyTypeEnum.Gold).ToString(), Text = "Взнос золотой ТИ", Selected = MoneyType == (int)Tournament.MoneyTypeEnum.Gold };
            }
        }

        public double? Fee { get; set; }

        public TournamentView()
        {
            OpenRegistrationDate = DateTime.Now;
            CloseRegistrationDate = DateTime.Now.AddDays(1);
            BeginDate = DateTime.Now.AddDays(2);
            EndDate = DateTime.Now.AddDays(3);
            Admins = new List<int>();
            Players = new List<int>();
            IsRoundForPoints = true;
            TournamentCondition = new TournamentConditionView();

            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var platform = repository.Platforms.FirstOrDefault();
            if (platform != null)
            {
                PlatformID = platform.ID;
            }
        }

        public void InitGameAdmin(Model.User user)
        {
            this.GameAdmin = user;
            var listGame = GameAdmin.AdminGames.ToList();

            if (!listGame.Any(p => p.ID == GameID) && ID != 0)
            {
                DisablePlatformAndGame = true;
            }
        }

        public string MoneyTypeName
        {
            get
            {
                if (MoneyType.HasValue)
                {
                    switch ((Tournament.MoneyTypeEnum)MoneyType.Value)
                    {
                        case Tournament.MoneyTypeEnum.Free:
                            return "без взноса";
                        case Tournament.MoneyTypeEnum.Wood:
                            return "белый ТИ";
                        case Tournament.MoneyTypeEnum.Gold:
                            return "золотой ТИ";
                    }
                }
                return "без взноса";
            }
        }
    }
}