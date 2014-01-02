using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace turniri.Areas.Default.Controllers
{
    public class PageController : DefaultController
    {
        public ActionResult Index(string url)
        {
            var page = Repository.Pages.FirstOrDefault(p => p.Url == url);
            if (page != null)
            {
                return View(page);
            }
            return RedirectToNotFoundPage;
        }

    }
}
