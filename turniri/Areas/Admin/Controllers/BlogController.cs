using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles="admin")]
    public class BlogController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.Blogs.OrderByDescending(p => p.ID);
            var data = new PageableData<Blog>();
            data.Init(list, page, "Index");
            return View(data);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var blog = Repository.Blogs.FirstOrDefault(p => p.ID == id);

            if (blog != null)
            {
                var blogView = (AdminBlogView)ModelMapper.Map(blog, typeof(Blog), typeof(AdminBlogView));
                return View(blogView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(AdminBlogView blogView)
        {
            if (ModelState.IsValid)
            {
                var blog = (Blog)ModelMapper.Map(blogView, typeof(AdminBlogView), typeof(Blog));
                Repository.UpdateAdminBlog(blog);
                return RedirectToAction("Index");
            }
            return View(blogView);
        }

        public ActionResult Delete(int id)
        {
            var blog = Repository.Blogs.FirstOrDefault(p => p.ID == id);
            if (blog != null)
            {
                Repository.RemoveBlog(blog.ID);
            }
            return RedirectToAction("Index");
        }

    }
}
