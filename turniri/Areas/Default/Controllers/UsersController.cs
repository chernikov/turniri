using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;

namespace turniri.Areas.Default.Controllers
{
    public class UsersController : DefaultController
    {
        public ActionResult Index(int page = 1, string searchString = null)
        {
            var list = Repository.Users.OrderByDescending(p => p.Reputation);
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var searchList = SearchEngine.Search(searchString, list);
                return View("SearchUsers", searchList);
            }
            var data = new PageableData<User>();
            data.Init(list, page, "Index");
            return View(data);
        }

        
    }
}
