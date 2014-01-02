using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace turniri.Areas.Default.Controllers
{
    public class AwardController : DefaultController
    {
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

    }
}
