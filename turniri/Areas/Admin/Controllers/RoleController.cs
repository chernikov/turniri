using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles="admin")]
    public class RoleController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Users.Where(p => p.UserRoles.Any());

            return View(list);
        }

        public ActionResult RemoveRole(int id)
        {
            var role = Repository.UserRoles.FirstOrDefault(p => p.ID == id);
            if (role != null && role.Role.Code != "admin")
            {
                Repository.RemoveUserRole(role.ID);
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult RemoveGameRole(int id)
        {
            var roleGame = Repository.UserRoleGames.FirstOrDefault(p => p.ID == id);
            if (roleGame != null)
            {
                Repository.RemoveUserRoleGame(roleGame.ID);
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult RemoveTournamentRole(int id)
        {
            var roleTournament = Repository.UserRoleTournaments.FirstOrDefault(p => p.ID == id);
            if (roleTournament != null)
            {
                Repository.RemoveUserRoleTournament(roleTournament.ID);
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.DenyGet);
        }
    }
}
