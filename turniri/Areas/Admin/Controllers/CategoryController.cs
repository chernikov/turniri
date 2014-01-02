using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;
using turniri.Tools;


namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,seller")]
    public class CategoryController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Categories.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var categoryView = new CategoryView();
            return View("Edit", categoryView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = Repository.Categories.FirstOrDefault(p => p.ID == id);
            if (category != null)
            {
                var categoryView = (CategoryView)ModelMapper.Map(category, typeof(Category), typeof(CategoryView));
                return View(categoryView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(CategoryView categoryView)
        {
            if (ModelState.IsValid)
            {
                var category = (Category)ModelMapper.Map(categoryView, typeof(CategoryView), typeof(Category));
                category.Url = Translit.Translate(category.Name);
                if (category.ID == 0)
                {
                    
                    Repository.CreateCategory(category);
                }
                else
                {
                    Repository.UpdateCategory(category);
                }
                return RedirectToAction("Index");
            }
            return View(categoryView);
        }

        public ActionResult Delete(int id)
        {
            var category = Repository.Categories.FirstOrDefault(p => p.ID == id);
            if (category != null)
            {
                Repository.RemoveCategory(category.ID);
            }
            return RedirectToAction("Index");
        }

    }
}