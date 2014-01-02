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
    public class CommentController : DefaultController
    {
        private static string CommentFolder = "/Media/files/comment/";

        public ActionResult UploadImage(string qqfile)
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
                        var filePath = string.Format("{0}{1}{2}", CommentFolder, StringExtension.GenerateNewFile(), extension);
                        using(var fileStream = new FileStream(Server.MapPath(filePath), FileMode.Create)) 
                        {
                             inputStream.CopyTo(fileStream);
                        }
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                ImagePath = filePath
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

        private string MakeImage(MemoryStream ms, string folder, string avatarSize)
        {
            var avatarUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSaveImage(ms, previewSize, Server.MapPath(avatarUrl));
            }
            return avatarUrl;
        }

        [Authorize(Roles = "admin,game_admin,tournament_admin")]
        public ActionResult Delete(int id)
        {
            var comment = Repository.Comments.FirstOrDefault(p => p.ID == id);
            if (comment != null)
            {
                Repository.RemoveComment(comment.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}
