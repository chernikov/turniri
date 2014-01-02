using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class EditorController : AdminController
    {
        public ActionResult Index()
        {
            var role = Repository.Roles.FirstOrDefault(p => string.Compare(p.Code, "editor", true) == 0);
            if (role != null) 
            {
                return View(role.UserRoles);
            }
            return RedirectToNotFoundPage;
        }


        public ActionResult Delete(int id)
        {
            Repository.RemoveUserRole(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(int id)
        {
            var role = Repository.Roles.FirstOrDefault(p => string.Compare(p.Code, "editor", true) == 0);
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (role != null && user != null)
            {
                var userRole = new UserRole()
                {
                    UserID = user.ID,
                    RoleID = role.ID
                };
                Repository.CreateUserRole(userRole);
            }
            return Json(new { result = "ok" });
        }
    }
}
