using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PageController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Pages;
            return View(list);
        }

        public ActionResult Create()
        {
            var pageView = new PageView();
            return View("Edit", pageView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var page = Repository.Pages.FirstOrDefault(p => p.ID == id);

            if (page != null)
            {
                var pageView = (PageView)ModelMapper.Map(page, typeof(Page), typeof(PageView));
                return View(pageView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PageView pageView)
        {
            if (ModelState.IsValid)
            {
                var page = (Page)ModelMapper.Map(pageView, typeof(PageView), typeof(Page));
                if (page.ID == 0)
                {
                    page.Url = Translit.Translate(page.Name);
                    Repository.CreatePage(page);
                }
                else
                {
                    Repository.UpdatePage(page);
                }
                return RedirectToAction("Index");
            }
            return View(pageView);
        }

        public ActionResult Delete(int id)
        {
            var page = Repository.Pages.FirstOrDefault(p => p.ID == id);
            if (page != null)
            {
                Repository.RemovePage(page.ID);
            }
            return RedirectToAction("Index");
        }
    }
}
