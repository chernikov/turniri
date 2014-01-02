using System.Web.Mvc;


namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin,editor,seller")]
    public class HomeController : AdminController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminMenu()
        {
            return View();
        }

    }
}
