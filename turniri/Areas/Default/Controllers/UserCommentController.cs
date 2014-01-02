using System.Linq;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Default.Controllers
{
    public class UserCommentController : DefaultController
    {

        public ActionResult Index(int id = 0, int page = 1)
        {
            ViewBag.Page = page;
            if (id == 0 && CurrentUser != null)
            {
                return View(CurrentUser);
            }
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                return View(user);
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
                        var userComment = new UserComment
                        {
                            UserID = commentView.OwnerID,
                            CommentID = comment.ID
                        };
                        Repository.CreateUserComment(userComment);
                    }
                    return View("Ok");
                }
                return View(commentView);
            }
            return null;
        } 

    }
}
