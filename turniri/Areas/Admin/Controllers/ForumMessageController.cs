using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class ForumMessageController : AdminController
    {
        public ActionResult Index(int id, int page = 1)
        {
            var forum = Repository.Forums.FirstOrDefault(p => p.ID == id);
            ViewBag.Page = page;
            return View(forum);
        }

        [HttpGet]
        public ActionResult Create(int id, int? parentId = null)
        {
            var forum = Repository.Forums.FirstOrDefault(p => p.ID == id);

            if (forum != null)
            {
                var forumMessage = new ForumMessageView
                                       {
                                           ForumID = forum.ID,
                                           UserID = CurrentUser.ID,
                                       };
                if (parentId != null)
                {
                    var parent = Repository.ForumMessages.FirstOrDefault(p => p.ID == parentId);
                    if (parent != null)
                    {
                        forumMessage.ParentID = parent.ID;
                        forumMessage.SetQuote(parent);
                    }
                }
                return View("Edit", forumMessage);
            }
            return null;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var forumMessage = Repository.ForumMessages.FirstOrDefault(p => p.ID == id);

            if (forumMessage != null && forumMessage.CanEdit(CurrentUser))
            {
                var forumMessageView = (ForumMessageView)ModelMapper.Map(forumMessage, typeof(ForumMessage), typeof(ForumMessageView));
                return View(forumMessageView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ForumMessageView forumMessageView)
        {
            var forumMessage = Repository.ForumMessages.FirstOrDefault(p => p.ID == forumMessageView.ID);
            if (forumMessage == null || forumMessage.CanEdit(CurrentUser))
            {
                if (ModelState.IsValid)
                {
                    forumMessageView.RemoveQuote();
                    forumMessage =
                        (ForumMessage)ModelMapper.Map(forumMessageView, typeof(ForumMessageView), typeof(ForumMessage));

                    if (forumMessage.ID == 0)
                    {
                        Repository.CreateForumMessage(forumMessage);
                    }
                    else
                    {
                        Repository.UpdateForumMessage(forumMessage);
                    }
                    return View("Ok");
                }
                return View(forumMessageView);
            }
            return RedirectToLoginPage;
        }

        public ActionResult Delete(int id)
        {
            var forumMessage = Repository.ForumMessages.FirstOrDefault(p => p.ID == id);

            if (forumMessage != null && forumMessage.CanDelete(CurrentUser))
            {
                Repository.RemoveForumMessage(id, CurrentUser.ID);
                return Json(new
                                {
                                    result = "ok"
                                });
            }
            return Json(new
            {
                result = "error"
            });
        }
    }
}
