using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,editor")]
    public class PhotoAlbumController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.PhotoAlbums.Where(p => p.ShowOnMain);
            return View(list);
        }

        public ActionResult Create()
        {
            var photoAlbumView = new PhotoAlbumView();
            return View("Edit", photoAlbumView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var photoAlbum = Repository.PhotoAlbums.FirstOrDefault(p => p.ID == id);

            if (photoAlbum != null)
            {
                var photoAlbumView = (PhotoAlbumView)ModelMapper.Map(photoAlbum, typeof(PhotoAlbum), typeof(PhotoAlbumView));
                return View(photoAlbumView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PhotoAlbumView photoAlbumView)
        {
            if (ModelState.IsValid)
            {
                var photoAlbum = (PhotoAlbum)ModelMapper.Map(photoAlbumView, typeof(PhotoAlbumView), typeof(PhotoAlbum));
                photoAlbum.ShowOnMain = true;
                if (photoAlbum.ID == 0)
                {
                    photoAlbum.UserID = CurrentUser.ID;
                    Repository.CreatePhotoAlbum(photoAlbum);
                }
                else
                {
                    Repository.UpdatePhotoAlbum(photoAlbum);
                }
                return RedirectToAction("Index");
            }
            return View(photoAlbumView);
        }

        public ActionResult Delete(int id)
        {
            var photoAlbum = Repository.PhotoAlbums.FirstOrDefault(p => p.ID == id);
            if (photoAlbum != null)
            {
                Repository.RemovePhotoAlbum(photoAlbum.ID);
            }
            return RedirectToAction("Index");
        }


        public ActionResult Photos(int id)
        {
            var photoAlbum = Repository.PhotoAlbums.FirstOrDefault(p => p.ID == id);
            if (photoAlbum != null)
            {
                return View(photoAlbum);
            }
            return null;
        }

        public ActionResult RemovePhoto(int id)
        {
            var photo = Repository.Photos.FirstOrDefault(p => p.ID == id);
            if (photo != null)
            {
                Repository.RemovePhoto(photo.ID);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }
    }
}
