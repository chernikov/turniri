using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class GameController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.Games.OrderBy(p => p.ID);
            var data = new PageableData<Game>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var gameView = new GameView();
            return View("Edit", gameView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var game = Repository.Games.FirstOrDefault(p => p.ID == id);

            if (game != null)
            {
                var gameView = (GameView)ModelMapper.Map(game, typeof(Game), typeof(GameView));
                return View(gameView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(GameView gameView)
        {
            var forum = Repository.Forums.FirstOrDefault(p => p.ID == gameView.ForumID);
            if (forum == null)
            {
                ModelState.AddModelError("Forum", "Выберите топик форума");
            }
            if (ModelState.IsValid)
            {
                var game = (Game)ModelMapper.Map(gameView, typeof(GameView), typeof(Game));
                if (game.ID == 0)
                {
                    Repository.CreateGame(game);
                }
                else
                {
                    Repository.UpdateGame(game);
                }

                Repository.UpdateGameAdmins(game.ID, gameView.Admins);
                Repository.UpdateGameModerators(game.ID, gameView.Moderators);

                return RedirectToAction("Index");
            }
            return View(gameView);
        }

        public ActionResult Delete(int id)
        {
            var game = Repository.Games.FirstOrDefault(p => p.ID == id);
            if (game != null)
            {
                Repository.RemoveGame(game.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetForum(int? id)
        {
            var forumList = GameView.CreateForumsSelectList(id, Repository);
            ViewBag.ForumID = id;
            return View(forumList);
        }

        public ActionResult SelectGame(string term)
        {
            var list = Repository.Games;

            var searchList = SearchEngine.Search(term, list);

            return Json(new
            {
                result = "ok",
                data = searchList.Select(p => new
                {
                    id = p.ID,
                    login = string.Format("{0} ({1})", p.Name, p.Platform.Name)
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateNotice(int id)
        {
            var noticeDistributionView = new NoticeDistributionView()
            {
                ForeignID = id
            };

            return View(noticeDistributionView);
        }

        [HttpPost]
        public ActionResult CreateNotice(NoticeDistributionView noticeDistributionView)
        {
            if (ModelState.IsValid)
            {
                var game = Repository.Games.FirstOrDefault(p => p.ID == noticeDistributionView.ForeignID);

                if (game != null)
                {
                    noticeDistributionView._ID = 0;
                    var noticeDistribution = (NoticeDistribution)ModelMapper.Map(noticeDistributionView, typeof(NoticeDistributionView), typeof(NoticeDistribution));

                    noticeDistribution.UserID = CurrentUser.ID;
                    Repository.CreateNoticeDistribution(noticeDistribution);

                    IEnumerable<User> list = game.UserGames.Select(p => p.User);

                    foreach (var user in list)
                    {
                        var notice = new Notice()
                        {
                            NoticeDistributionID = noticeDistribution.ID,
                            GameID = noticeDistributionView.ForeignID,
                            SenderID = noticeDistribution.UserID,
                            ReceiverID = user.ID,
                            Caption = noticeDistribution.Caption.Replace("%username%", user.Login),
                            Text = noticeDistribution.Text.Replace("%username%", user.Login),
                            Type = (int)Notice.TypeEnum.Game,
                            IsCloseForRead = noticeDistribution.IsCloseForRead
                        };
                        Repository.CreateNotice(notice);
                    }
                    return RedirectToAction("Index");
                }

            }
            return View(noticeDistributionView);
        }

    }
}
