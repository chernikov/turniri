using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using turniri.Model;
using turniri.Models.Info;


namespace turniri.Models.ViewModels
{ 
	public class GameView
    {
        public int ID { get; set; }

		public int PlatformID {get; set; }

        private List<Platform> Platforms
	    {
	        get 
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.Platforms.ToList();
            }
	    }

        public IEnumerable<SelectListItem> PlatformsSelectList
        {
            get
            {
                return Platforms.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Text = p.Name,
                    Selected = p.ID == PlatformID
                });
            }
        }

        [Required(ErrorMessage = "Введите наименование")]
	    public string Name {get; set; }

        [Required(ErrorMessage = "Введите url")]
        public string Url { get; set; }

        public int GameType { get; set; }

	    public IEnumerable<SelectListItem> GameTypeSelectList
	    {
	        get
	        {
	            yield return
	                new SelectListItem
	                    {
	                        Value = ((int) Game.GameTypeEnum.Points).ToString(),
	                        Text = "Счет",
	                        Selected = GameType == (int) Game.GameTypeEnum.Points
	                    };
                yield return
                    new SelectListItem
                        {
                            Value = ((int)Game.GameTypeEnum.WinLose).ToString(),
                            Text = "Победа/поражение",
                            Selected = GameType == (int)Game.GameTypeEnum.WinLose
                        };
	        }
	    }
        
        [Required(ErrorMessage = "Введите описание")]
		public string Description {get; set; }

        [Required(ErrorMessage = "Добавьте картинку игры")]
		public string ImagePath189 {get; set; }

		public string ImagePath103 {get; set; }

		public string ImagePath144v {get; set; }

        public string ImagePath47 { get; set; }

		public string ImagePath22 {get; set; }

		public string HowToPlay {get; set; }

		public bool IsMain {get; set; }

	    public string FullImagePath189
	    {
	        get { return string.IsNullOrWhiteSpace(ImagePath189) ? "/Media/images/default_game.jpg" : ImagePath189; }
	    }

        public string FullImagePath144v
        {
            get { return string.IsNullOrWhiteSpace(ImagePath144v) ? "/Media/images/default_game144.jpg" : ImagePath144v; }
        }

        public string FullImagePath103
        {
            get { return string.IsNullOrWhiteSpace(ImagePath103) ? "/Media/images/default_game103.jpg" : ImagePath103; }
        }

        public string FullImagePath47
        {
            get { return string.IsNullOrWhiteSpace(ImagePath47) ? "/Media/images/default_game47.jpg" : ImagePath47; }
        }

        public string FullImagePath22
        {
            get { return string.IsNullOrWhiteSpace(ImagePath22) ? "/Media/images/default_game22.jpg" : ImagePath22; }
        }

        public int ForumID { get; set; }

        public string ForumName { get; set; }

        public int GameCategory { get; set; }

        public IEnumerable<SelectListItem> GameCategorySelectList
        {
            get
            {
                yield return new SelectListItem() { Value = "1", Text = "Общий", Selected = GameCategory == (int)Game.GameCategoryEnum.Common };
                yield return new SelectListItem() { Value = "2", Text = "Футбол", Selected = GameCategory == (int)Game.GameCategoryEnum.Soccer };
            }
        }

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
                //не берем топики
                if (forums.Count() == forums.Count(p => p.IsEnd))
                {
                    yield break;
                }
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

        public int? MaxCountPlayer { get; set; }

        public string Keywords { get; set; }

        public string MetaDescription { get; set; }

        public GameView()
        {
            GameCategory = 1;
        }
    }
}