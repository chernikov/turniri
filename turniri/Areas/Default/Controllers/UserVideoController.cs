using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;
using turniri.Tools.Video;

namespace turniri.Areas.Default.Controllers
{
    public class UserVideoController : DefaultController
    {
        private static string VideoFolder = "/Media/files/video/";
        private static string VideoThumbSize = "VideoThumbSize";

        public ActionResult Index(string login = null, int page = 1)
        {
            ViewBag.Page = page;
            if (string.IsNullOrWhiteSpace(login) && CurrentUser != null)
            {
                return View(CurrentUser);
            }
            var user = Repository.Users.FirstOrDefault(p => string.Compare(p.Login, login, true) == 0);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToNotFoundPage;
        }


        public ActionResult Group(string url, int page = 1)
        {
            ViewBag.Page = page;
            var group = Repository.Groups.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (group != null)
            {
                return View(group);
            }
            return RedirectToNotFoundPage;
        }


        public ActionResult VideoCode(int id)
        {
            var video = Repository.UserVideos.FirstOrDefault(p => p.ID == id);
            if (video != null)
            {

                Repository.UpdateVisitUserVideo(video.ID);
                return Content(video.VideoCode);
            }
            return null;
        }

        public ActionResult Item(int id)
        {
            var video = Repository.UserVideos.FirstOrDefault(p => p.ID == id);
            if (video != null)
            {
                Repository.UpdateVisitUserVideo(video.ID);
                return View(video);
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult Create(int? groupID)
        {
            var userVideoView = new UserVideoView
            {
                UserID = CurrentUser.ID,
                GroupID = groupID
            };
            return View("Edit", userVideoView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userVideo = Repository.UserVideos.FirstOrDefault(p => p.ID == id);
            if (userVideo != null && userVideo.UserID == CurrentUser.ID)
            {
                var userVideoView = (UserVideoView)ModelMapper.Map(userVideo, typeof(UserVideo), typeof(UserVideoView));
                return View(userVideoView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(UserVideoView userVideoView)
        {
            if (ModelState.IsValid)
            {
                var userVideo = (UserVideo)ModelMapper.Map(userVideoView, typeof(UserVideoView), typeof(UserVideo));
                userVideo.VideoCode = VideoHelper.GetVideoByUrl(userVideo.VideoUrl);
                var thumb = VideoHelper.GetVideoThumbByUrl(userVideo.VideoUrl);
                if (thumb.StartsWith("http"))
                {
                    userVideo.VideoThumb = SaveImageFromUrl(thumb);
                }
                if (userVideo.ID == 0)
                {
                    Repository.CreateUserVideo(userVideo);
                }
                else
                {
                    Repository.UpdateUserVideo(userVideo);
                }
                return RedirectToAction("Index");
            }
            return View(userVideoView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var userVideo = Repository.UserVideos.FirstOrDefault(p => p.ID == id);
            if (userVideo != null && userVideo.UserID == CurrentUser.ID)
            {
                Repository.RemoveUserVideo(userVideo.ID);
                return RedirectToAction("Index");
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult ProcessUrl(string url)
        {
            var code = VideoHelper.GetVideoByUrl(url);
            var thumbUrl = VideoHelper.GetVideoThumbByUrl(url);

            return Json(new
            {
                result = "ok",
                VideoCode = code,
                VideoThumb = thumbUrl
            }, JsonRequestBehavior.AllowGet);
        }

        public string SaveImageFromUrl(string url)
        {
            var webClient = new WebClient();

            var bytes = webClient.DownloadData(url);
            var ms = new MemoryStream(bytes);

            var thumbUrl = string.Format("{0}{1}.jpg", VideoFolder, StringExtension.GenerateNewFile());
            var thumbSizes = Config.IconSizes.FirstOrDefault(c => c.Name == VideoThumbSize);
            if (thumbSizes != null)
            {
                var thumbSize = new Size(thumbSizes.Width, thumbSizes.Height);
                PreviewCreator.CreateAndSaveFitToSize(ms, thumbSize, Server.MapPath(thumbUrl));
            }
            return thumbUrl;
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
                        var userVideoComment = new UserVideoComment
                        {
                            UserVideoID = commentView.OwnerID,
                            CommentID = comment.ID
                        };
                        Repository.CreateUserVideoComment(userVideoComment);
                    }
                    return View("Ok");
                }
                return View(commentView);
            }
            return null;
        }
    }
}
