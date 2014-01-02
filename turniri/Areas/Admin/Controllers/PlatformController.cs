using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PlatformController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Platforms.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var platformView = new PlatformView();

            return View("Edit", platformView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var platform = Repository.Platforms.FirstOrDefault(p => p.ID == id);

            if (platform != null)
            {
                var platformView = (PlatformView)ModelMapper.Map(platform, typeof(Platform), typeof(PlatformView));
                return View(platformView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(PlatformView platformView)
        {
            if (ModelState.IsValid)
            {
                var platform = (Platform)ModelMapper.Map(platformView, typeof(PlatformView), typeof(Platform));
                if (platform.ID == 0)
                {
                    Repository.CreatePlatform(platform);
                }
                else
                {
                    Repository.UpdatePlatform(platform);
                }
                return RedirectToAction("Index");
            }
            return View(platformView);
        }

        public ActionResult Delete(int id)
        {
            var platform = Repository.Platforms.FirstOrDefault(p => p.ID == id);
            if (platform != null)
            {
                Repository.RemovePlatform(platform.ID);
            }
            return RedirectToAction("Index");
        }
    }
}
