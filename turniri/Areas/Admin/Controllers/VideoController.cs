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

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,editor")]
    public class VideoController : AdminController
    {
        

        public ActionResult Index(int page = 1)
        {
            var list = Repository.Videos.OrderByDescending(p => p.AddedDate);

            var data = new PageableData<Video>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var newView = new VideoView();
            return View("Edit", newView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var video = Repository.Videos.FirstOrDefault(p => p.ID == id);
            if (video != null && (CurrentUser.InRoles("admin") || video.UserID == CurrentUser.ID))
            {
                var newView = (VideoView) ModelMapper.Map(video, typeof (Video), typeof (VideoView));
                return View(newView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(VideoView videoView)
        {
            if (videoView.ID != 0)
            {
                var oldVideo = Repository.Videos.FirstOrDefault(p => p.ID == videoView.ID);
                if (oldVideo != null)
                {
                    if (!CurrentUser.InRoles("admin") && oldVideo.UserID != CurrentUser.ID)
                    {
                        return RedirectToLoginPage;
                    }
                }
                else
                {
                    return RedirectToNotFoundPage;
                }
            }
            if (ModelState.IsValid)
            {
                var video = (Video) ModelMapper.Map(videoView, typeof (VideoView), typeof (Video));
                video.VideoCode = VideoHelper.GetVideoByUrl(video.VideoUrl);
                var thumb = VideoHelper.GetVideoThumbByUrl(video.VideoUrl);
                if (thumb.StartsWith("http"))
                {
                    video.VideoThumb = SaveImageFromUrl(thumb);
                }
                if (video.ID == 0)
                {
                    video.UserID = CurrentUser.ID;
                    Repository.CreateVideo(video);
                }
                else
                {
                    Repository.UpdateVideo(video);
                }
                return RedirectToAction("Index");
            }
            return View(videoView);
        }

        public ActionResult Delete(int id)
        {
            var news = Repository.Videos.FirstOrDefault(p => p.ID == id);
            if (news != null)
            {
                Repository.RemoveVideo(news.ID);
            }
            return RedirectToAction("Index");
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

      
    }
}
