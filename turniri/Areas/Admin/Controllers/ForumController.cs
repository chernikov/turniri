using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class ForumController : AdminController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            if (CurrentUser.InRoles("admin"))
            {
                var list = Repository.Forums.Where(p => p.ParentID == null);
                return View(list);
            }
            if (CurrentUser.InRoles("game_admin"))
            {
                var list = CurrentUser.AdminGames.Select(p => p.Forum);
                return View("TinyIndex", list);
            }
            if (CurrentUser.InRoles("tournament_admin"))
            {
                var list = CurrentUser.AdminTournaments.Select(p => p.Forum); 
                return View("TinyIndex", list);
            }
            
            return RedirectToLoginPage;
        }

        public ActionResult SubForum(int id)
        {
            var list = Repository.Forums.Where(p => p.ParentID == id).OrderBy(p => p.OrderBy).ToList();
            return View(list);
        }

        public ActionResult TinySubForum(int id)
        {
            var list = Repository.Forums.Where(p => p.ParentID == id).OrderBy(p => p.OrderBy).ToList();
            return View(list);
        }

        public ActionResult Create(int? id = null)
        {
            var forumView = new ForumView()
            {
                UserID = CurrentUser.ID,
                ParentID = id
            };
            forumView.InitAdmin(CurrentUser);
            return View("Edit", forumView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var forum = Repository.Forums.FirstOrDefault(p => p.ID == id);

            if (forum != null && forum.CanEdit(CurrentUser))
            {
                var forumView = (ForumView)ModelMapper.Map(forum, typeof(Forum), typeof(ForumView));
                forumView.InitAdmin(CurrentUser);
                return View(forumView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(ForumView forumView)
        {
            var forum = Repository.Forums.FirstOrDefault(p => p.ID == forumView.ID);
            if (forum == null || forum.CanEdit(CurrentUser))
            {
                if (string.IsNullOrWhiteSpace(forumView.Url) && forumView.ID != 0)
                {
                    forumView.Url = Translit.WithPredicateTranslate(forumView.Name);
                }
                if (ModelState.IsValid)
                {
                    forum = (Forum)ModelMapper.Map(forumView, typeof(ForumView), typeof(Forum));
                    if (forum.ID == 0)
                    {
                        Repository.CreateForum(forum);
                    }
                    else
                    {
                        Repository.UpdateForum(forum);
                    }
                    return RedirectToAction("Index");
                }
                forumView.InitAdmin(CurrentUser);
                return View(forumView);
            }
            return RedirectToLoginPage;
        }

        public ActionResult Delete(int id)
        {
            var forum = Repository.Forums.FirstOrDefault(p => p.ID == id);
            if (forum != null && forum.CanDelete(CurrentUser))
            {
                Repository.RemoveForum(forum.ID);
            }
            return RedirectToAction("Index");
        }

        public JsonResult AjaxForumMove(int id, int moveTo)
        {
            if (Repository.ChangeParentForum(id, moveTo))
            {
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public ActionResult AjaxForumOrder(int id, int replaceTo)
        {
            if (Repository.MoveForum(id, replaceTo))
            {
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public ActionResult ReorderAll()
        {
            var order = 1;
            foreach (var forum in Repository.Forums.Where(p => p.ParentID == null))
            {
                forum.OrderBy = order;
                Repository.SetOrderForum(forum);
                ReorderByForum(forum);
                order++;
            }
            return null;
        }

        public void ReorderByForum(Forum forum)
        {
            var order = 1;
            foreach (var subForum in forum.Forums)
            {
                subForum.OrderBy = order;
                Repository.SetOrderForum(forum);
                ReorderByForum(subForum);
                order++;
            }
        }
    }
}
