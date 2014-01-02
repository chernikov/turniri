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
    public class BannerController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.Banners.OrderBy(p => p.Path);
            var data = new PageableData<Banner>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Details(int id, int page = 1)
        {
            var banner = Repository.Banners.FirstOrDefault(p => p.ID == id);
            if (banner != null)
            {
                ViewBag.Banner = banner;
                var list = banner.BannerStatistics.OrderByDescending(p => p.AddedDate).AsQueryable();
                var data = new PageableData<BannerStatistic>();
                data.Init(list, page, "Details");
                return View(data);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Create()
        {
            var bannerView = new BannerView()
            {
                IsOn = true
            };
            return View("Edit", bannerView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var banner = Repository.Banners.FirstOrDefault(p => p.ID == id);

            if (banner != null)
            {
                var bannerView = (BannerView)ModelMapper.Map(banner, typeof(Banner), typeof(BannerView));
                return View(bannerView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(BannerView bannerView)
        {
            if (ModelState.IsValid)
            {
                var banner = (Banner)ModelMapper.Map(bannerView, typeof(BannerView), typeof(Banner));
                if (banner.ID == 0)
                {
                    Repository.CreateBanner(banner);
                }
                else
                {
                    Repository.UpdateBanner(banner);
                }
                return RedirectToAction("Index");
            }
            return View(bannerView);
        }

        public ActionResult Delete(int id)
        {
            var banner = Repository.Banners.FirstOrDefault(p => p.ID == id);
            if (banner != null)
            {
                Repository.RemoveBanner(banner.ID);
            }
            return RedirectToAction("Index");
        }

    }
}
