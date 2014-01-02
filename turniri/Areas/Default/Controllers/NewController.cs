using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Default.Controllers
{
    public class NewController : DefaultController
    {
        public ActionResult Index(string url)
        {
            var @new = Repository.News.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (@new != null)
            {
                Repository.UpdateVisitNew(@new.ID);
                return View(@new);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        public ActionResult CreateComment(int id)
        {
            if (CurrentUser != null)
            {
                var commentView = new CommentView
                                      {
                                          OwnerID = id
                                      };
                return View(commentView);
            }
            return null;
        }

        [HttpPost]
        public ActionResult CreateComment(CommentView commentView)
        {
            if (CurrentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var comment = CreateBasicComment(commentView);
                    if (comment.ID != 0)
                    {
                        var newComment = new NewComment
                                             {
                                                 NewID = commentView.OwnerID,
                                                 CommentID = comment.ID
                                             };
                        Repository.CreateNewComment(newComment);
                    }
                    return View("Ok");
                }
                return View(commentView);
            }
            return null;
        }

        [Authorize]
        public ActionResult ToggleLike(int id)
        {
            var count = Repository.ToggleNewLike(id, CurrentUser.ID);
            return Json(new 
            {
                result = "ok",
                count
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
