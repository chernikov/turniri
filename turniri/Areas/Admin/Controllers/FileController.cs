using ImageResizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Tools;
using turniri.Tools.FineUploader;

namespace turniri.Areas.Admin.Controllers
{
    public class FileController : AdminController
    {
        private static string NewsFolder = "/Media/files/news/";
        private static string ForumFolder = "/Media/files/forum/";
        private static string GameFolder = "/Media/files/game/";
        private static string TournamentFolder = "/Media/files/tournament/";
        private static string AwardFolder = "/Media/files/award/";
        private static string AvatarFolder = "/Media/files/avatars/";
        private static string PhotoFolder = "/Media/files/photos/";
        private static string BannerFolder = "/Media/files/banners/";
        private static string BackgroundFolder = "/Media/files/backgrounds/";
        private static string SocialFolder = "/Media/files/socials/";

        private static string CatalogFolder = "/Media/files/catalog/";
        private static string ProductFolder = "/Media/files/product/";
        private static string CodeFolder = "/Media/files/code/";

        private static string Avatar30Size = "Avatar30Size";
        private static string Avatar26Size = "Avatar26Size";
        private static string Avatar18Size = "Avatar18Size";


        private static string NewPreviewSize = "NewPreviewSize";
        private static string NewAvatarPreviewSize = "NewAvatarPreviewSize";
        private static string NewTitleSize = "NewTitleSize";
        private static string NewAvatarTitleSize = "NewAvatarTitleSize";

        private static string ForumTopicSize = "ForumTopicSize";

        private static string GameImage189 = "GameImage189Size";
        private static string GameImage103 = "GameImage103Size";
        private static string GameImage144v = "GameImage144vSize";
        private static string GameImage47 = "GameImage47Size";
        private static string GameImage22 = "GameImage22Size";

        private static string TournamentImageSize = "TournamentImageSize";
        
        private static string AwardIconSize = "AwardIconSize";

        private static string PhotoSize = "PhotoSize";
        private static string PhotoAlbumPreviewSize = "PhotoAlbumPreviewSize";
        private static string PhotoAvatarSize = "PhotoAvatarSize";
        private static string SmallSize = "SmallSize";

        private static string BigBannerSize = "BigBannerSize";
        private static string SmallBannerSize = "SmallBannerSize";
        private static string PreviewBannerSize = "PreviewBannerSize";

        private static string PreviewBackgroundSize = "PreviewBackgroundSize";
        private static string SmallBackgroundSize = "SmallBackgroundSize";

        private static string SocialPreviewSize = "SocialPreviewSize";

        private static string CatalogPreviewSize = "CatalogImageSize";

        private static string CodeImage = "CodeImageSize";

        [HttpPost]
        public JsonResult UploadNewPreview(string qqfile)
        {
            var fileName = string.Empty;
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
                        var previewPath = MakePreview(ms, NewsFolder, NewPreviewSize);
                        var avatarPreviewPath = MakePreview(ms, NewsFolder, NewAvatarPreviewSize);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                PreviewPath = previewPath,
                                AvatarPreviewPath = avatarPreviewPath
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        [HttpPost]
        public JsonResult UploadNewTitle(string qqfile)
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
                        var titlePath = MakePreview(ms, NewsFolder, NewTitleSize);
                        var avatarTitlePath = MakePreview(ms, NewsFolder, NewAvatarTitleSize);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                TitlePath = titlePath,
                                AvatarTitlePath = avatarTitlePath
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        [HttpPost]
        public JsonResult UploadGameImage(string qqfile)
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
                        var imagePath189 = MakePreview(ms, GameFolder, GameImage189);
                        var imagePath103 = MakePreview(ms, GameFolder, GameImage103);
                        var imagePath144v = MakePreview(ms, GameFolder, GameImage144v);
                        var imagePath47 = MakePreview(ms, GameFolder, GameImage47);
                        var imagePath22 = MakePreview(ms, GameFolder, GameImage22);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                ImagePath189 = imagePath189,
                                ImagePath103 = imagePath103,
                                ImagePath144v = imagePath144v,
                                ImagePath47 = imagePath47,
                                ImagePath22 = imagePath22,
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        [HttpPost]
        public JsonResult UploadForumPreview(string qqfile)
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

                    if (mimeType != null && (mimeType.Name == "image/png" || mimeType.Name == "image/gif"))
                    {
                        var ms = GetMemoryStream(inputStream);
                        
                        var topicSizes = Config.IconSizes.FirstOrDefault(c => c.Name == ForumTopicSize);
                        if (topicSizes != null)
                        {
                            var topicSize = new Size(topicSizes.Width, topicSizes.Height);
                            if (PreviewCreator.CheckSize(ms, topicSize))
                            {
                                string imagePath = null;
                                if (mimeType.Name == "image/png")
                                {
                                    imagePath = string.Format("{0}{1}.png", ForumFolder, StringExtension.GenerateNewFile());
                                }
                                if (mimeType.Name == "image/gif")
                                {
                                    imagePath = string.Format("{0}{1}.gif", ForumFolder, StringExtension.GenerateNewFile());
                                }

                                var imagePathGray = imagePath.GetPreviewPath("_g");
                                PreviewCreator.CreateAndSaveImagePng(ms, Server.MapPath(imagePath));
                                PreviewCreator.CreateAndSaveImagePng(ms, Server.MapPath(imagePathGray), true);
                                return Json(new
                                                {
                                                    result = "ok",
                                                    previewPath = imagePath
                                                }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new
                            {
                                result = "wrong-size",
                                error = "Размер должен быть " + topicSizes
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json(new
            {
                result = "error"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadTournamentImage(string qqfile)
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
                        var imagePath = MakePreview(ms, TournamentFolder, TournamentImageSize);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                ImagePath = imagePath,
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        [HttpPost]
        public JsonResult UploadCatalogImage(string qqfile)
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
                        var photoPath = MakePreview(ms, CatalogFolder, CatalogPreviewSize);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                Photo = photoPath,
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        [HttpPost]
        public JsonResult UploadSocialPreview(string qqfile)
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
                        var imagePath = MakePreview(ms, SocialFolder, SocialPreviewSize);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                ImagePath = imagePath,
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        [HttpPost]
        public JsonResult UploadAwardIcon(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                 var awardSizes = Config.IconSizes.FirstOrDefault(c => c.Name == AwardIconSize);
                 if (awardSizes != null)
                 {
                     var awardSize = new Size(awardSizes.Width, awardSizes.Height);
                     var extension = Path.GetExtension(fileName);
                     if (extension != null)
                     {
                         extension = extension.ToLower();
                         var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                         if (mimeType != null && (mimeType.Name == "image/png" || mimeType.Name == "image/gif"))
                         {
                             var ms = GetMemoryStream(inputStream);
                             if (PreviewCreator.CheckSize(ms, awardSize))
                             {
                                 string imagePath = null;
                                 if (mimeType.Name == "image/png")
                                 {
                                     imagePath = string.Format("{0}{1}.png", AwardFolder, StringExtension.GenerateNewFile());
                                 }
                                 if (mimeType.Name == "image/gif")
                                 {
                                     imagePath = string.Format("{0}{1}.gif", AwardFolder, StringExtension.GenerateNewFile());
                                 }
                                 using (var file = new FileStream(Server.MapPath(imagePath), FileMode.Create))
                                 {
                                     ms.Position = 0;
                                     ms.CopyTo(file);
                                 }
                                 return Json(new
                                 {
                                     result = "ok",
                                     iconPath = imagePath
                                 }, JsonRequestBehavior.AllowGet);
                             }
                             return Json(new
                             {
                                 result = "error",
                                 error = string.Format("Размер должен быть {0} x {1}", awardSize.Width, awardSize.Height)
                             }, JsonRequestBehavior.AllowGet);
                         }
                         else
                         {
                             return Json(new
                             {
                                 result = "error",
                                 error = string.Format("Файл должен быть .png, .gif размером {0} x {1}", awardSize.Width, awardSize.Height)
                             }, JsonRequestBehavior.AllowGet);
                         }
                     }
                 }
            }
            return Json(new
            {
                result = "error",
                error = "Не удалось загрузить файл"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadAvatar(string qqfile)
        {
            var fileName = string.Empty;
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
                        var avatar30Url = MakePreview(ms, AvatarFolder, Avatar30Size);
                        var avatar26Url = MakePreview(ms, AvatarFolder, Avatar26Size);
                        var avatar18Url = MakePreview(ms, AvatarFolder, Avatar18Size);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                ImagePath30 = avatar30Url,
                                ImagePath26 = avatar26Url,
                                ImagePath18 = avatar18Url
                            }
                        }, "text/html");
                    }
                    else
                    {
                        return Json(new
                        {
                            success = true,
                            result = "error",
                            error = "Файл не является изображением"
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error", error = "Не удалось загрузить файл" });
        }

        public ActionResult UploadBanner(string qqfile)
        {
            var fileName = string.Empty;
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
                        var filePath = string.Format("{0}{1}{2}", BannerFolder, StringExtension.GenerateNewFile(), extension);
                        var previewPath = filePath.GetPreviewPath("_prev");
                        using (var fileStream = new FileStream(Server.MapPath(filePath), FileMode.Create))
                        {
                            inputStream.CopyTo(fileStream);
                            MakePreview(fileStream, previewPath, BannerFolder, PreviewBannerSize);
                        };
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                filePath = filePath
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        public ActionResult UploadBackground(string qqfile)
        {
            var fileName = string.Empty;
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
                        var filePath = string.Format("{0}{1}{2}", BackgroundFolder, StringExtension.GenerateNewFile(), extension);
                        var previewPath = filePath.GetPreviewPath("_prev");
                        var smallPreviewPath = filePath.GetPreviewPath("_small");
                        using (var fileStream = new FileStream(Server.MapPath(filePath), FileMode.Create))
                        {
                            inputStream.CopyTo(fileStream);
                            MakePreview(fileStream, previewPath, BackgroundFolder, PreviewBackgroundSize);
                            MakePreview(fileStream, smallPreviewPath, BackgroundFolder, SmallBackgroundSize);
                        };

                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                filePath = filePath,
                                preview = previewPath,
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        private static MemoryStream GetMemoryStream(Stream inputStream)
        {
            var buffer = new byte[inputStream.Length];
            var ms = new MemoryStream(buffer);
            inputStream.CopyTo(ms);
            return ms;
        }

        private string MakeAvatar(MemoryStream ms, string folder, string avatarSize)
        {
            var avatarUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSaveAvatar(ms, previewSize, Server.MapPath(avatarUrl));
            }
            return avatarUrl;
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
                        Size size;
                        photo.FilePath = MakeImage(ms, PhotoFolder, PhotoSize, out size);
                        photo.Width = size.Width;
                        photo.Height = size.Height;
                        photo.AlbumPreviewPath = MakePreview(ms, PhotoFolder, PhotoAlbumPreviewSize);
                        photo.AvatarPath = MakePreview(ms, PhotoFolder, PhotoAvatarSize, true);
                        photo.SmallPath = MakePreview(ms, PhotoFolder, SmallSize, false);
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
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        public JsonResult UpdateBannerSize(string path, int type)
        {
            using(var fs = new FileStream(Server.MapPath(path), FileMode.Open))
            {
                var newPath = path.GetPreviewPath("_banner");

                switch ((Banner.TypeEnum)type)
                {
                    case Banner.TypeEnum.BigBanner :
                        newPath = MakePreview(fs, newPath, BannerFolder, BigBannerSize);
                        return Json(new { result = "ok", path = newPath + "?" + Guid.NewGuid().ToString("N") }, JsonRequestBehavior.AllowGet);
                    case Banner.TypeEnum.SmallBanner :
                        newPath = MakePreview(fs, newPath, BannerFolder, SmallBannerSize);
                        return Json(new { result = "ok", path = newPath + "?" + Guid.NewGuid().ToString("N") }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "error" });
        }

        
        [HttpPost]
        public JsonResult UploadCodeImage(string qqfile)
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
                        var codeImage = MakePreview(ms, CodeFolder, CodeImage);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                codeImage
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        private string MakePreview(Stream ms, string path, string folder, string avatarSize, bool grayscale = false)
        {
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSavePreview(ms, previewSize, Server.MapPath(path), grayscale);
            }
            return path;
        }

        private string MakePreview(Stream ms, string folder, string avatarSize, bool grayscale = false)
        {
            var avatarUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSavePreview(ms, previewSize, Server.MapPath(avatarUrl), grayscale);
            }
            return avatarUrl;
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


        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadProductImage(FineUpload upload)
        {
            var uDir = "Media/files/product/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), uDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + uDir + uFile });
        }

        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadProductScreenshot(FineUpload upload)
        {
            var uDir = "Media/files/screenshot/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), uDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + uDir + uFile });
        }

        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadProductVariation(FineUpload upload)
        {
            var uDir = "Media/files/variation/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), uDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + uDir + uFile });
        }

        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadProductBackground(FineUpload upload)
        {
            var uDir = "Media/files/product_background/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), uDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings());
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + uDir + uFile });
        }

        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadLeagueImage(FineUpload upload)
        {
            var uDir = "Media/files/league/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), uDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + uDir + uFile });
        }

    }
}
