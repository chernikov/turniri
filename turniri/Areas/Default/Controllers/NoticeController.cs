using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.Areas.Default.Controllers
{
    [Authorize]
    public class NoticeController : DefaultController
    {

        public ActionResult Count()
        {
            if (CurrentUser.AnyNotice)
            {
                return View("Count", CurrentUser.Notices.Count(p => p.ReadedDate == null));
            }
            return Content("");
        }

        public ActionResult Index()
        {
            var list = CurrentUser.Notices.OrderByDescending(p => p.AddedDate).ToList();
            return View(list);
        }

        public ActionResult ReadAll()
        {
            var list = CurrentUser.Notices.Where(p => p.ReadedDate == null).ToList();
            foreach (var item in list)
            {
                Repository.ReadNotice(item.ID);
            }
            return null;
        }

        public ActionResult Remove(int id)
        {
            var notice = CurrentUser.Notices.FirstOrDefault(p => p.ID == id);

            if (notice != null)
            {
                Repository.RemoveNotice(id);

                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }
    }
}
