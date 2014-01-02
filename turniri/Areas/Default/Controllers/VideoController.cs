using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Default.Controllers
{
    public class VideoController : DefaultController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.Videos.OrderByDescending(p => p.AddedDate);
            var data = new PageableData<Video>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult VideoCode(int id)
        {
            var video = Repository.Videos.FirstOrDefault(p => p.ID == id);
            if (video != null)
            {

                Repository.UpdateVisitVideo(video.ID);
                return Content(video.VideoCode);
            }
            return null;
        }

        public ActionResult Item(string url)
        {
            var video = Repository.Videos.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (video != null)
            {
                Repository.UpdateVisitVideo(video.ID);
                return View(video);
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
                        var videoComment = new VideoComment
                        {
                            VideoID = commentView.OwnerID,
                            CommentID = comment.ID
                        };
                        Repository.CreateVideoComment(videoComment);
                    }
                    return View("Ok");
                }
                return View(commentView);
            }
            return null;
        } 
    }
}
