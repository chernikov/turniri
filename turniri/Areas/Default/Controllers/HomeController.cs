using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.Info;
using turniri.Tools;

namespace turniri.Areas.Default.Controllers
{
    public class HomeController : DefaultController
    {
        public ActionResult Index(int? matchID = null)
        {
            ViewBag.MatchID = matchID;
            return View();
        }

        public ActionResult UnreadMessages()
        {
            return View(CurrentUser);
        }

        public ActionResult Online() 
        {
            if (CurrentUser != null)
            {
                Repository.VisitUser(CurrentUser.ID, Config.FreeCharge);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Menu(string currentController = null)
        {
            var platforms = Repository.Platforms.ToList();
            ViewBag.Leagues = Repository.Leagues.ToList();
            return View(platforms);
        }

        public ActionResult UserLogin()
        {
            return View(CurrentUser);
        }

        public ActionResult MainNewsGallery()
        {
            var list = Repository.News.OrderByDescending(p => p.AddedDate).Where(p => p.IsMain).Take(4).ToList();
            return View(list);
        }

        public ActionResult MainCameraAct()
        {
            return View(base.MainCamera);
        }

        public ActionResult MainNewsList(int id = 1)
        {
            var list = Repository.NewTypes.ToList();
            ViewBag.SelectedId = id;
            return View(list);
        }

        public ActionResult NewsList(int id, int page = 1)
        {
            var newType = Repository.NewTypes.FirstOrDefault(p => p.ID == id);
            if (newType != null)
            {
                ViewBag.Page = page;
                return View(newType);
            }
            return null;
        }

        public ActionResult BlogsList(int page = 1)
        {
            var blogs = Repository.Blogs.Where(p => !p.IsBanned && p.ShowInMain == true).OrderByDescending(p => p.ID);
            var data = new PageableData<Blog>();
            data.Init(blogs, page, "BlogsList", itemPerPage: 4);
            return View(data);
        }

        public ActionResult FutureMatches()
        {
            var matches = Repository.Matches.Where(p => p.TournamentID == null && p.Status == (int)Match.MatchStatusEnum.DefinedPlayers).OrderByDescending(p => p.ID).ToList();
            return View(matches);
        }

        public ActionResult CurrentMatches()
        {
            var matches = Repository.Matches.Where(p => p.TournamentID == null && p.Status == (int)Match.MatchStatusEnum.Submit).OrderByDescending(p => p.ID).Take(5).ToList();
            return View(matches);
        }

        public ActionResult FutureTournaments()
        {
            var tournaments = Repository.Tournaments.Where(p => p.Status == (int)Tournament.StatusEnum.Created).OrderByDescending(p => p.ID).Take(5).ToList();
            return View(tournaments);
        }

        public ActionResult CurrentTournaments()
        {
            var tournaments = Repository.Tournaments.Where(p => p.Status == (int)Tournament.StatusEnum.InGame).OrderByDescending(p => p.ID).Take(5).ToList();
            return View(tournaments);
        }

        public ActionResult PlayedOutTournaments()
        {
            var tournaments = Repository.Tournaments.Where(p => p.Status == (int)Tournament.StatusEnum.PlayedOut).OrderByDescending(p => p.ID).Take(5).ToList();
            return View(tournaments);
        }

        public ActionResult Newbie()
        {
            var users = Repository.Users.OrderByDescending(p => p.AddedDate).Take(15);
            return View(users);
        }

        public ActionResult ForumLastSubject()
        {
            var subjects = Repository.Forums.Where(p => p.IsEnd).OrderByDescending(p => p.LastUpdate).Take(15);
            return View(subjects);
        }

        public ActionResult GotMail(int type)
        {
            if (CurrentUser != null)
            {
                return CurrentUser.HasUnreadMail(type) ? Content("") : Content("-empty");
            }
            return Content("");
        }

        public ActionResult GotFriend()
        {
            if (CurrentUser != null)
            {
                return CurrentUser.AskedForNewFriend ? Content("") : Content("-empty");
            }
            return Content("");
        }

        public ActionResult TestEditor()
        {
            return View();
        }

        public ActionResult AllTexts()
        {
            var list = new List<BothTexts>();
            //blog
            foreach (var blog in Repository.Blogs)
            {
                list.Add(new BothTexts()
                {
                    ID = blog.ID,
                    TextType = BothTexts.TextTypeEnum.Blog,
                    HtmlText = blog.Text,
                    BbCodeText = blog.Text.HtmlToBbCode()
                });
            }

            //forumMessage
            foreach (var forumMessage in Repository.ForumMessages)
            {
                list.Add(new BothTexts()
                {
                    ID = forumMessage.ID,
                    TextType = BothTexts.TextTypeEnum.ForumMessage,
                    HtmlText = forumMessage.Message,
                    BbCodeText = forumMessage.Message.HtmlToBbCode()
                });
            }

            //game
            foreach (var game in Repository.Games)
            {
                list.Add(new BothTexts()
                {
                    ID = game.ID,
                    TextType = BothTexts.TextTypeEnum.Game,
                    HtmlText = game.Description,
                    BbCodeText = game.Description.HtmlToBbCode()
                });
            }

            //new
            foreach (var @new in Repository.News)
            {
                list.Add(new BothTexts()
                {
                    ID = @new.ID,
                    TextType = BothTexts.TextTypeEnum.New,
                    HtmlText = @new.Text,
                    BbCodeText = @new.Text.HtmlToBbCode()
                });
            }
            //page
            foreach (var page in Repository.Pages)
            {
                list.Add(new BothTexts()
                {
                    ID = page.ID,
                    TextType = BothTexts.TextTypeEnum.Page,
                    HtmlText = page.Description,
                    BbCodeText = page.Description.HtmlToBbCode()
                });
            }

            return View(list);
        }

        public ActionResult OneText(int id, BothTexts.TextTypeEnum type)
        {
            switch (type)
            {
                case BothTexts.TextTypeEnum.Blog : 
                    var blog = Repository.Blogs.First(p => p.ID == id);
                    return View(new BothTexts()
                    {
                        HtmlText = blog.Text,
                        BbCodeText = blog.Text.HtmlToBbCode()
                    });
                case BothTexts.TextTypeEnum.ForumMessage:
                    var forumMessage = Repository.ForumMessages.First(p => p.ID == id);
                    return View(new BothTexts()
                    {
                        HtmlText = forumMessage.Message,
                        BbCodeText = forumMessage.Message.HtmlToBbCode()
                    });
                case BothTexts.TextTypeEnum.Game:
                    var game = Repository.Games.First(p => p.ID == id);
                    return View(new BothTexts()
                    {
                        HtmlText = game.Description,
                        BbCodeText = game.Description.HtmlToBbCode()
                    });
                case BothTexts.TextTypeEnum.New:
                    var @new = Repository.News.First(p => p.ID == id);
                    return View(new BothTexts()
                    {
                        HtmlText = @new.Text,
                        BbCodeText = @new.Text.HtmlToBbCode()
                    });
                case BothTexts.TextTypeEnum.Page:
                    var page = Repository.Pages.First(p => p.ID == id);
                    return View(new BothTexts()
                    {
                        HtmlText = page.Description,
                        BbCodeText = page.Description.HtmlToBbCode()
                    });
            }
            return null;
        }

        public ActionResult ShowError(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult Banner(int type)
        {
            var url = Request.Url.AbsolutePath;

            var banner = Repository.Banners.OrderByDescending(p => p.Path).ToList().FirstOrDefault(p => url.StartsWith(p.Path, System.StringComparison.InvariantCultureIgnoreCase) && p.Type == type && p.IsOn);
            if (banner != null) 
            {
                if (!Request.Browser.Crawler)
                {
                    Repository.ViewBanner(banner.ID);
                }
                return View(banner);
            }
            return null;
        }

        public ActionResult ClickBanner(int id)
        {
            Repository.ClickBanner(id);
            return Json(new {result="ok"}, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult Background()
        {
            var url = Request.Url.AbsolutePath;
            if (url.StartsWith("/product/"))
            {
                var path = url.Substring("/product/".Length);
                var product = Repository.Products.FirstOrDefault(p => string.Compare(p.Url, path, true) == 0);

                if (product != null && !string.IsNullOrWhiteSpace(product.Background))
                {
                    return Content(product.Background);
                }
            }

            
            var background = Repository.Backgrounds.OrderByDescending(p => p.Path).ToList().FirstOrDefault(p => url.StartsWith(p.Path, System.StringComparison.InvariantCultureIgnoreCase) && p.IsOn);

            if (background != null)
            {
                return Content(background.ImagePath);
            }

            return null;
        }

        public ActionResult Sitemap()
        {
            var xmlDoc = Tools.SiteMap.CreateSiteMap(HostName, Repository);
            xmlDoc.Save(Response.OutputStream);
            Response.ContentType = "text/xml";
            return null;
        }
    }
}
