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
    public class BackgroundController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.Backgrounds.OrderBy(p => p.Path);
            var data = new PageableData<Background>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var bannerView = new BackgroundView()
            {
                IsOn = true
            };
            return View("Edit", bannerView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var banner = Repository.Backgrounds.FirstOrDefault(p => p.ID == id);

            if (banner != null)
            {
                var bannerView = (BackgroundView)ModelMapper.Map(banner, typeof(Background), typeof(BackgroundView));
                return View(bannerView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(BackgroundView bannerView)
        {
            if (ModelState.IsValid)
            {
                var banner = (Background)ModelMapper.Map(bannerView, typeof(BackgroundView), typeof(Background));
                if (banner.ID == 0)
                {
                    Repository.CreateBackground(banner);
                }
                else
                {
                    Repository.UpdateBackground(banner);
                }
                return RedirectToAction("Index");
            }
            return View(bannerView);
        }

        public ActionResult Delete(int id)
        {
            var banner = Repository.Backgrounds.FirstOrDefault(p => p.ID == id);
            if (banner != null)
            {
                Repository.RemoveBackground(banner.ID);
            }
            return RedirectToAction("Index");
        }
    }
}
