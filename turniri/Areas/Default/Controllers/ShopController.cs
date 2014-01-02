using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.Info;

namespace turniri.Areas.Default.Controllers
{
    public class ShopController : DefaultController
    {
        public ActionResult Index(ProductFilter productFilter)
        {
            return View(productFilter);
        }

        public ActionResult ShopFilter(ProductFilter productFilter)
        {
            return View(productFilter);
        }


        public ActionResult Content(ProductFilter productFilter)
        {
            productFilter.Process(Repository.Products.Where(p => !p.IsDeleted).ToList().AsQueryable(), "Index");
            return View(productFilter);
        }
    }
}
