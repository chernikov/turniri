using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.Areas.Default.Controllers
{
    public class PublicPhotoController : DefaultController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.PhotoAlbums.Where(p => p.ShowOnMain).OrderByDescending(p => p.AddedDate);
            var data = new PageableData<PhotoAlbum>();
            data.Init(list, page, "Index", 5);
            return View(data);
        }

    }
}
