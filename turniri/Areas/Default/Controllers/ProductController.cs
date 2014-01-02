using ImageResizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.Info;

namespace turniri.Areas.Default.Controllers
{
    public class ProductController : DefaultController
    {
        public ActionResult Index(string path)
        {
            var product = Repository.Products.FirstOrDefault(p => p.Url == path);

            if (product != null)
            {
                return View(product);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult AlsoProduct(int id)
        {
            var product = Repository.Products.FirstOrDefault(p => p.ID == id);

            if (product != null)
            {
                var list = product.SimilarProducts.Select(p => p.OtherProduct);
                list = list.Union(product.BackSimilarProducts.Select(p => p.Product).AsEnumerable()).Where(p => !p.IsDeleted);

                return View(list.ToList());
            }
            return null;
        }

        public ActionResult ViewScreenshot(int id)
        {
            var photo = Repository.ProductImages.FirstOrDefault(p => p.ID == id);
            if (photo != null)
            {
                return View(photo);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult ChangeScreenshot(int id, bool next = true)
        {
            var photo = Repository.ChangeProductScreenshot(id, next);
            if (photo != null)
            {
                return View("ViewScreenshot", photo);
            }
            return RedirectToNotFoundPage;
        }


        public ActionResult ShowVideo(int id)
        {
            var video = Repository.ProductVideos.FirstOrDefault(p => p.ID == id);
            if (video != null)
            {
                return Content(video.VideoCode);
            }
            return null;
        }
    }
}
