using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Tools;

namespace turniri.Areas.Default.Controllers
{
    public class FileController : DefaultController
    {
        private static string ImageFolder = "/Media/files/images/";

        private static string GroupFolder = "/Media/files/groups/";

        private static string MaxImageSize = "MaxImageSize";

        private static string Group173Size = "Group173Size";
        private static string Group96Size = "Group96Size";
        private static string Group84Size = "Group84Size";
        private static string Group57Size = "Group57Size";
        private static string Group30Size = "Group30Size";
        private static string Group26Size = "Group26Size";
        private static string Group18Size = "Group18Size";

        [HttpPost]
        public JsonResult UploadImage(string qqfile)
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
                        var imageFile = MakeImage(ms, ImageFolder, MaxImageSize);

                        return Json(new
                        {
                            result = "ok",
                            data = new
                            {
                                imageFile
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { result = "error" });
        }

        [HttpPost]
        public JsonResult UploadGroupLogo(string qqfile)
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
                        var group173Url = MakePreview(ms, GroupFolder, Group173Size);
                        var group96Url = MakePreview(ms, GroupFolder, Group96Size);
                        var group84Url = MakePreview(ms, GroupFolder, Group84Size);
                        var group57Url = MakePreview(ms, GroupFolder, Group57Size);
                        var group30Url = MakePreview(ms, GroupFolder, Group30Size);
                        var group26Url = MakePreview(ms, GroupFolder, Group26Size);
                        var group18Url = MakePreview(ms, GroupFolder, Group18Size);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                LogoPath173 = group173Url,
                                LogoPath96 = group96Url,
                                LogoPath84 = group84Url,
                                LogoPath57 = group57Url,
                                LogoPath30 = group30Url,
                                LogoPath26 = group26Url,
                                LogoPath18 = group18Url
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

        private string MakePreview(MemoryStream ms, string folder, string avatarSize, bool grayscale = false)
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

        private static MemoryStream GetMemoryStream(Stream inputStream)
        {
            var buffer = new byte[inputStream.Length];
            var ms = new MemoryStream(buffer);
            inputStream.CopyTo(ms);
            return ms;
        }

        private string MakeImage(MemoryStream ms, string folder, string imageSize)
        {
            var imageUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var imageSizes = Config.IconSizes.FirstOrDefault(c => c.Name == imageSize);
            if (imageSizes != null)
            {
                var previewSize = new Size(imageSizes.Width, imageSizes.Height);
                PreviewCreator.CreateAndSaveImage(ms, previewSize, Server.MapPath(imageUrl));
            }
            return imageUrl;
        }


    }
}
