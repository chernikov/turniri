using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;


namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin,editor")]
    public class SocialGroupController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.SocialGroups.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var socialgroupView = new SocialGroupView();

			return View("Edit", socialgroupView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
            var socialgroup = Repository.SocialGroups.FirstOrDefault(p => p.ID == id); 

			if (socialgroup != null) {
				var socialgroupView = (SocialGroupView)ModelMapper.Map(socialgroup, typeof(SocialGroup), typeof(SocialGroupView));
				return View(socialgroupView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(SocialGroupView socialgroupView)
        {
            if (ModelState.IsValid)
            {
                var socialGroup = (SocialGroup)ModelMapper.Map(socialgroupView, typeof(SocialGroupView), typeof(SocialGroup));
                if (socialGroup.ID == 0)
                {
                    Repository.CreateSocialGroup(socialGroup);
                }
                else
                {
                    Repository.UpdateSocialGroup(socialGroup);
                }
                return RedirectToAction("Index");
            }
            return View(socialgroupView);
        }

        public ActionResult Delete(int id)
        {
            var socialGroup = Repository.SocialGroups.FirstOrDefault(p => p.ID == id);
            if (socialGroup != null)
            {
                Repository.RemoveSocialGroup(socialGroup.ID);
            }
			return RedirectToAction("Index");
        }

	}
}