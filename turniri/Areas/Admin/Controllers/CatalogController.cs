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
    [Authorize(Roles = "admin,seller")]
    public class CatalogController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Catalogs.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var catalogView = new CatalogView();
            return View("Edit", catalogView);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var catalog = Repository.Catalogs.FirstOrDefault(p => p.ID == id);

            if (catalog != null)
            {
                var catalogView = (CatalogView)ModelMapper.Map(catalog, typeof(Catalog), typeof(CatalogView));
                return View(catalogView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(CatalogView catalogView)
        {
            if (ModelState.IsValid)
            {
                var catalog = (Catalog)ModelMapper.Map(catalogView, typeof(CatalogView), typeof(Catalog));
                if (catalog.ID == 0)
                {
                    Repository.CreateCatalog(catalog);
                }
                else
                {
                    Repository.UpdateCatalog(catalog);
                }
                return RedirectToAction("Index");
            }
            return View(catalogView);
        }

        public ActionResult Delete(int id)
        {
            var catalog = Repository.Catalogs.FirstOrDefault(p => p.ID == id);
            if (catalog != null)
            {
                Repository.RemoveCatalog(catalog.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ClearCatalog()
        {
            var list = Repository.Catalogs.ToList();

            foreach (var item in list)
            {
                item.Name = item.Name.Trim();
                Repository.UpdateCatalog(item);
            }
            return null;
        }
    }
}
