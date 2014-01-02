using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,editor")]
    public class NewController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.News.OrderByDescending(p => p.AddedDate);

            var data = new PageableData<New>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var newView = new NewView();
            return View("Edit", newView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var @new = Repository.News.FirstOrDefault(p => p.ID == id);
            if (@new != null && (CurrentUser.InRoles("admin") || @new.UserID == CurrentUser.ID))
            {
                var newView = (NewView)ModelMapper.Map(@new, typeof(New), typeof(NewView));
                return View(newView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(NewView newView)
        {
            if (newView.ID != 0) 
            {
                var oldNew = Repository.News.FirstOrDefault(p => p.ID == newView.ID);
                if (oldNew != null)
                {
                    if (!CurrentUser.InRoles("admin") && oldNew.UserID != CurrentUser.ID)
                    {
                        return RedirectToLoginPage;
                    }
                }
                else
                {
                    return RedirectToNotFoundPage;
                }
            }
            if (ModelState.IsValid)
            {
                var @new = (New)ModelMapper.Map(newView, typeof(NewView), typeof(New));
                if (@new.ID == 0)
                {
                    @new.UserID = CurrentUser.ID;
                    Repository.CreateNew(@new);
                }
                else
                {
                    Repository.UpdateNew(@new);
                }
                return RedirectToAction("Index");
            }
            return View(newView);
        }

        public ActionResult Delete(int id)
        {
            var news = Repository.News.FirstOrDefault(p => p.ID == id);
            if (news != null && (CurrentUser.InRoles("admin") || news.UserID == CurrentUser.ID))
            {
                Repository.RemoveNew(news.ID);
            }
            return RedirectToAction("Index");
        }

    }
}
