using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Tools.Video;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ChatController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.ChatRooms.OrderByDescending(p => p.LastIdUpdate).ToList();
            return View(list);
        }

        public ActionResult UpdateAllComment()
        {
            foreach (var item in Repository.Comments.ToList().Where(p => !string.IsNullOrWhiteSpace(p.VideoUrl)))
            {
                item.VideoCode = VideoHelper.GetVideoByUrl(item.VideoUrl);
                Repository.UpdateComment(item);
            }

            return null;
        }

        public ActionResult ToggleChatTranslate(int id)
        {
            var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.ID == id); 
            if (chatRoom != null) 
            {
                Repository.ToggleTranslateChatRoom(chatRoom.ID);

                chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.ID == id);

                return Json(new { result = "ok", data = chatRoom.TranslateInForum }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
    }
}
