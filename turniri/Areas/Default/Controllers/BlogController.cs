using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Default.Controllers
{
    public class BlogController : DefaultController
    {
        private static string PreviewFolder = "/Media/files/blog/";
        private static string BlogPreviewSize = "BlogPreviewSize";

        public ActionResult Index(string login = null, int page = 1)
        {
            ViewBag.Page = page;
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

        public ActionResult LastComments(int id = 0)
        {
            if (id == 0 && CurrentUser != null)
            {
                return View(CurrentUser.LastBlogComments);
            }

            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                return View(user.LastBlogComments);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Group(string url, int page = 1)
        {
            var group = Repository.Groups.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            ViewBag.Page = page;
            if (group != null)
            {
                return View(group);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult LastCommentsGroup(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                return View("LastComments", group.LastBlogComments);
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult Create(int? groupID)
        {
            var blogView = new BlogView
            {
                UserID = CurrentUser.ID,
                GroupID = groupID
            };
            return View("Edit", blogView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var blog = Repository.Blogs.FirstOrDefault(p => p.ID == id);
            if (blog != null && blog.UserID == CurrentUser.ID)
            {
                var blogView = (BlogView)ModelMapper.Map(blog, typeof(Blog), typeof(BlogView));
                return View(blogView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(BlogView blogView)
        {
            if (ModelState.IsValid)
            {
                var blog = (Blog)ModelMapper.Map(blogView, typeof (BlogView), typeof (Blog));
                if (blog.ID == 0)
                {
                    Repository.CreateBlog(blog);
                } else
                {
                    Repository.UpdateBlog(blog);
                }
                return RedirectToAction("Index");
            }
            return View(blogView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var blog = Repository.Blogs.FirstOrDefault(p => p.ID == id);
            if (blog != null && blog.UserID == CurrentUser.ID)
            {
                Repository.RemoveBlog(blog.ID);
                return RedirectToAction("Index");
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Item(string url)
        {
            var blog = Repository.Blogs.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (blog != null)
            {
                Repository.UpdateVisitBlog(blog.ID);
                return View(blog);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public JsonResult UploadPreview(string qqfile)
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
                        var previewUrl = MakePreview(ms, BlogPreviewSize);
                      
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                PreviewUrl = previewUrl
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

        private string MakePreview(MemoryStream ms, string avatarSize)
        {
            var avatarUrl = string.Format("{0}{1}.jpg", PreviewFolder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSavePreview(ms, previewSize, Server.MapPath(avatarUrl));
            }
            return avatarUrl;
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
                    var comment = (Comment)ModelMapper.Map(commentView, typeof(CommentView), typeof(Comment));
                    comment.UserID = CurrentUser.ID;
                    comment.ID = 0;
                    Repository.CreateComment(comment);
                    var blogComment = new BlogComment
                    {
                        BlogID = commentView.OwnerID,
                        CommentID = comment.ID
                    };
                    Repository.CreateBlogComment(blogComment);
                    return View("Ok");
                }
                return View(commentView);
            }
            return null;
        }

        [Authorize]
        public ActionResult ToggleLike(int id)
        {
            var count = Repository.ToggleBlogLike(id, CurrentUser.ID);
            return Json(new
            {
                result = "ok",
                count
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
