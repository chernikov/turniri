using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Default.Controllers
{
    public class PhotoController : DefaultController
    {
        private static string PhotoFolder = "/Media/files/photos/";

        private static string PhotoSize = "PhotoSize";
        private static string PhotoAlbumPreviewSize = "PhotoAlbumPreviewSize";
        private static string PhotoAvatarSize = "PhotoAvatarSize";
        private static string SmallSize = "SmallSize";

        public ActionResult Index(string login = null)
        {
            if (string.IsNullOrWhiteSpace(login) && CurrentUser != null)
            {
                return View(CurrentUser);
            }
            var user = Repository.Users.FirstOrDefault(p => string.Compare(p.Login, login, true) == 0);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Group(string url)
        {
            var group = Repository.Groups.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (group != null)
            {
                return View(group);
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult Create(int? groupID)
        {
            var photoAlbumView = new PhotoAlbumView
            {
                UserID = CurrentUser.ID,
                GroupID = groupID
            };
            return View("Edit", photoAlbumView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var photoAlbum = Repository.PhotoAlbums.FirstOrDefault(p => p.ID == id);
            if (photoAlbum != null && photoAlbum.UserID == CurrentUser.ID)
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
                if (photoAlbum.ID == 0)
                {
                    Repository.CreatePhotoAlbum(photoAlbum);
                }
                else
                {
                    Repository.UpdatePhotoAlbum(photoAlbum);
                }
                return RedirectToAction("Item", new {url = photoAlbum.Url});
            }
            return View(photoAlbumView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var photoAlbum = Repository.PhotoAlbums.FirstOrDefault(p => p.ID == id);
            if (photoAlbum != null && photoAlbum.UserID == CurrentUser.ID)
            {
                Repository.RemovePhotoAlbum(photoAlbum.ID);
                return RedirectToAction("Index");
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Item(string url, int page = 1)
        {
            var photoAlbum = Repository.PhotoAlbums.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (photoAlbum != null)
            {
                ViewBag.Page = page;
                return View(photoAlbum);    
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public JsonResult UploadPhoto(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && PreviewCreator.SupportMimeType(mimeType.Name))
                    {
                        var ms = GetMemoryStream(inputStream);
                        var photo = new Photo();
                        Size outSize;
                        photo.FilePath = MakeImage(ms, PhotoFolder, PhotoSize, out outSize);
                        photo.Width = outSize.Width;
                        photo.Height = outSize.Height;
                        photo.AlbumPreviewPath = MakePreview(ms, PhotoAlbumPreviewSize);
                        photo.AvatarPath = MakePreview(ms, PhotoAvatarSize, true);
                        photo.SmallPath = MakePreview(ms, SmallSize, false);
                        photo.Name = fileName;
                        Repository.CreatePhoto(photo);

                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                photo.ID,
                                photo.SmallPath
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        private static Stream GetMemoryStream(Stream inputStream)
        {
            var buffer = new byte[inputStream.Length];
            var ms = new MemoryStream(buffer);
            inputStream.CopyTo(ms);
            return ms;
        }

        private string MakeImage(Stream ms, string folder, string imageSize)
        {
            Size outSize;
            return MakeImage(ms, folder, imageSize, out outSize);
        }

        private string MakeImage(Stream ms, string folder, string imageSize, out Size outSize)
        {
            outSize = new Size();
            var avatarUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == imageSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSaveImage(ms, previewSize, Server.MapPath(avatarUrl), out outSize);
            }
            return avatarUrl;
        }

        private string MakePreview(Stream ms, string avatarSize, bool grayscale = false)
        {
            var avatarUrl = string.Format("{0}{1}.jpg", PhotoFolder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSavePreview(ms, previewSize, Server.MapPath(avatarUrl), grayscale);
            }
            return avatarUrl;
        }

        public ActionResult BindPhoto(int id, int idPhotoAlbum)
        {
            var photo = Repository.Photos.FirstOrDefault(p => p.ID == id);
            if (photo != null)
            {
                var photoAlbum = Repository.PhotoAlbums.FirstOrDefault(p => p.ID == idPhotoAlbum);
                
                if (photoAlbum != null)
                {
                    photo.PhotoAlbumID = photoAlbum.ID;
                    photo.UserID = photoAlbum.UserID;
                    Repository.BindPhoto(photo);
                    return Json(new {result = "ok"}, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        public ActionResult View(int id)
        {
            var photo = Repository.Photos.FirstOrDefault(p => p.ID == id);
            if (photo != null)
            {
                Repository.UpdateVisitPhoto(photo.ID);
                return View(photo);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult ChangeView(int id, bool next = true)
        {
            var photo = Repository.ChangePhoto(id, next);
            if (photo != null)
            {
                Repository.UpdateVisitPhoto(photo.ID);
                return View("View", photo);
            }
            return RedirectToNotFoundPage;
        }


        [HttpGet]
        public ActionResult CreateComment(int id)
        {
            if (CurrentUser != null)
            {
                var commentView = new CommentView
                {
                    OwnerID = id
                };
                return View(commentView);
            }
            return null;
        }

        [HttpPost]
        public ActionResult CreateComment(CommentView commentView)
        {
            if (CurrentUser != null)
            {
                if (ModelState.IsValid)
                {
                    var comment = CreateBasicComment(commentView);
                    if (comment.ID != 0)
                    {
                        var photoComment = new PhotoComment
                        {
                            PhotoID = commentView.OwnerID,
                            CommentID = comment.ID
                        };
                        Repository.CreatePhotoComment(photoComment);
                    }
                    return View("Ok");
                }
                return View(commentView);
            }
            return null;
        }

        [Authorize]
        public ActionResult RemovePhoto(int id)
        {
            var photo = Repository.Photos.FirstOrDefault(p => p.ID == id);
            if (photo != null)
            {
                if (photo.UserID == CurrentUser.ID)
                {
                    Repository.RemovePhoto(photo.ID);
                    return Json(new { result = "ok" });
                }
            }
            return Json(new { result = "error" });
        }


        [Authorize]
        public ActionResult ToggleLike(int id)
        {
            var count = Repository.TogglePhotoLike(id, CurrentUser.ID);
            return Json(new
            {
                result = "ok",
                count
            }, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult UpdatePhoto()
        {
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == SmallSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                foreach (var photo in Repository.Photos)
                {
                    try
                    {
                        using (var fs = new FileStream(Server.MapPath(photo.FilePath), FileMode.Open))
                        {
                            PreviewCreator.CreateAndSavePreview(fs, previewSize, Server.MapPath(photo.SmallPath), false);
                        }
                    }
                    catch
                    {
                    }
                }
               
            }
            return null;
        }
    }
}
