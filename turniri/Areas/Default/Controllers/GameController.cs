using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;

namespace turniri.Areas.Default.Controllers
{
    public class GameController : DefaultController
    {
        public ActionResult Menu(int id)
        {
            var game = Repository.Games.FirstOrDefault(p => p.ID == id);

            if (game != null)
            {
                return View(game);
            }
            return null;
        }

        public ActionResult Index(string platformUrl, string url)
        {
            var game = Repository.Games.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0 && string.Compare(p.Platform.Url, platformUrl) == 0);

            if (game != null)
            {
                return View(game);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult UserRating(int id)
        {
            var game = Repository.Games.FirstOrDefault(p => p.ID == id);

            if (game != null)
            {
                var list = game.Ratings.Where(p => p.IsActive).OrderByDescending(p => p.TotalScore).Take(10).ToList();
                return View(list);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult TournamentsList(int id, int type = 1, int page = 1)
        {
            var list = GetTournamentsByType(id, type);
            var data = new PageableData<Tournament>();
            data.Init(list, page, "TournamentsList");
            return View(data);
        }

        public ActionResult PartialTournamentsList(int id, int type = 1, int page = 1)
        {
            var list = GetTournamentsByType(id, type);
            var data = new PageableData<Tournament>();
            data.Init(list, page, "TournamentsList");
            return View(data);
        }

        private IQueryable<Tournament> GetTournamentsByType(int id, int type)
        {
            ViewBag.Type = type;
            ViewBag.GameID = id;
            var list = Repository.Tournaments.Where(p => p.GameID == id);
            switch (type)
            {
                case 1:
                    list = list.Where(p => p.Status == (int)Tournament.StatusEnum.Created || p.Status == (int)Tournament.StatusEnum.Allocated).OrderByDescending(p => p.ID);
                    break;
                case 2:
                    list = list.Where(p => p.Status == (int)Tournament.StatusEnum.InGame).OrderByDescending(p => p.ID);
                    break;
                case 3:
                    list = list.Where(p => p.Status == (int)Tournament.StatusEnum.PlayedOut).OrderByDescending(p => p.ID);
                    break;
            }
            return list;
        }

        public ActionResult Achieve(string platformUrl, string url, int page = 1)
        {
            var game = Repository.Games.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0 && string.Compare(p.Platform.Url, platformUrl) == 0);
            if (game != null)
            {
               ViewBag.Page = page;
               return View(game);
            }
            return RedirectToNotFoundPage;
        }


        public ActionResult League(string platformUrl, string url)
        {
            var game = Repository.Games.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0 && string.Compare(p.Platform.Url, platformUrl) == 0);
            if (game != null)
            {
                var league = Repository.Leagues.FirstOrDefault(p => p.GameID == game.ID);
                if (league != null)
                {
                    return RedirectToAction("Index", "League", new { url = league.Url });
                }
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Tournaments(string platformUrl, string url)
        {
            var game = Repository.Games.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0 && string.Compare(p.Platform.Url, platformUrl) == 0);
            if (game != null)
            {
                return View(game);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Group(string platformUrl, string url, int page = 1)
        {
            var game = Repository.Games.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0 && string.Compare(p.Platform.Url, platformUrl) == 0);
            if (game != null)
            {
                ViewBag.Page = page;
                return View(game);
            }
            return RedirectToLoginPage;
        }

        public ActionResult Gamers(string platformUrl, string url, int page = 1, string searchString = null)
        {
            var game = Repository.Games.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0 && string.Compare(p.Platform.Url, platformUrl) == 0);
            if (game != null)
            {
                ViewBag.Game = game;
                var list = Repository.Ratings.Where(p => p.GameID == game.ID && p.IsActive).OrderByDescending(p => p.TotalScore).Select(p => p.User);
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    var searchList = SearchEngine.Search(searchString, list);
                    return View("SearchGamers", searchList);
                }
                var data = new PageableData<User>();
                data.Init(list, page, "Gamers");
                return View(data);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult HowToPlay(string platformUrl, string url)
        {
            var game = Repository.Games.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0 && string.Compare(p.Platform.Url, platformUrl) == 0);
            if (game != null)
            {
                return View(game);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult FutureMatches(int id)
        {
            var matches = Repository.Matches.Where(p => p.GameID == id && p.TournamentID != null && p.Status == (int)Match.MatchStatusEnum.DefinedPlayers).OrderByDescending(p => p.ID).Take(5).ToList();
            return View(matches);
        }

        public ActionResult CurrentMatches(int id)
        {
            var matches = Repository.Matches.Where(p => p.GameID == id && p.TournamentID != null && p.Status == (int)Match.MatchStatusEnum.Submit).OrderByDescending(p => p.ID).Take(5).ToList();
            return View(matches);
        }
    }
}
