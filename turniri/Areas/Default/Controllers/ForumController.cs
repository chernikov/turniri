using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Default.Controllers
{
    public class ForumController : DefaultController
    {
        public ActionResult Index(string url, int page = 1, bool? lastMessage = null)
        {
            ViewBag.Page = page;
            if (string.IsNullOrWhiteSpace(url))
            {
                var list = Repository.Forums.Where(p => p.ParentID == null).OrderBy(p => p.OrderBy).ToList();
                return View("Main", list);
            }

            var item = Repository.Forums.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (item != null)
            {
                if (item.IsEnd)
                {
                    if (lastMessage == true)
                    {
                        ViewBag.Page = item.MessagesCountPage();
                    }

                    if (CurrentUser != null)
                    {
                        var forumLog = new ForumLog()
                        {
                            ForumID = item.ID,
                            UserID = CurrentUser.ID,
                        };
                        Repository.CreateForumLog(forumLog);
                        Repository.UpdateVisitForumNotice(item.ID, CurrentUser.ID);
                    }
                    Repository.UpdateVisitForum(item.ID);

                    return View("Topic", item);
                }
                return View(item);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult UserOnline()
        {
            return View(Repository.OnlineUsers.ToList());
        }

        public ActionResult VisitersOnline()
        {
            var countUsers = Repository.OnlineUsers.Count();
            var countIdentity = Repository.GlobalUniques.Count(p => p.LastDate.AddMinutes(5) > DateTime.Now && !p.UserAgent.ToLower().Contains("bot"));
            return View("VisitersOnline", countIdentity - countUsers);
        }

        public ActionResult TotalCount()
        {
            var count = Repository.GlobalUniques.Count(p => p.LastDate.AddMinutes(5) > DateTime.Now && !p.UserAgent.ToLower().Contains("bot"));
            return Content(count.ToString());
        }

        [Authorize]
        public ActionResult Create(int id)
        {
            var forum = Repository.Forums.FirstOrDefault(p => p.ID == id);
            if (forum != null)
            {
                var forumView = new ForumView
                {
                    ParentID = forum.ID,
                    UserID = CurrentUser.ID,
                    Message = new ForumMessageView()
                };
                return View(forumView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ForumView forumView)
        {
            if (ModelState.IsValid)
            {
                var forum = (Forum)ModelMapper.Map(forumView, typeof(ForumView), typeof(Forum));
                var forumMessage = (ForumMessage)ModelMapper.Map(forumView.Message, typeof(ForumMessageView), typeof(ForumMessage));
                forum.ID = 0;
                Repository.CreateForum(forum);

                forumMessage.UserID = CurrentUser.ID;
                forumMessage.ForumID = forum.ID;
                Repository.CreateForumMessage(forumMessage);
                return RedirectToAction("Index", new { id = forum.ID });
            }
            return View(forumView);
        }

        [HttpGet]
        public ActionResult CreateMessage(int id, int? parentId = null)
        {
            if (CurrentUser != null)
            {
                var forum = Repository.Forums.FirstOrDefault(p => p.ID == id);

                if (forum != null)
                {
                    var forumMessage = new ForumMessageView
                                           {
                                               ForumID = forum.ID,
                                               UserID = CurrentUser.ID,
                                           };
                    if (parentId != null)
                    {
                        var parent = Repository.ForumMessages.FirstOrDefault(p => p.ID == parentId);
                        if (parent != null)
                        {
                            forumMessage.ParentID = parent.ID;
                            forumMessage.SetQuote(parent);
                        }
                    }
                    return View(forumMessage);
                }
                return null;
            }
            else
            {
                return View("PleaseLogin");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateMessage(ForumMessageView forumMessageView)
        {
            if (ModelState.IsValid)
            {
                forumMessageView.RemoveQuote();
                var forumMessage =
                    (ForumMessage)ModelMapper.Map(forumMessageView, typeof(ForumMessageView), typeof(ForumMessage));

                Repository.CreateForumMessage(forumMessage);


                return View("Ok");
            }
            return View(forumMessageView);
        }

        public ActionResult RemoveMessage(int id)
        {
            var forumMessage = Repository.ForumMessages.FirstOrDefault(p => p.ID == id);

            if (forumMessage != null)
            {
                if (forumMessage.CanDelete(CurrentUser))
                {
                    Repository.RemoveForumMessage(forumMessage.ID, CurrentUser.ID);
                    return Json(new { result = "ok" });
                }
            }
            return Json(new { result = "error" });
        }

        [Authorize]
        public ActionResult EditMessage(int id)
        {
            var forumMessage = Repository.ForumMessages.FirstOrDefault(p => p.ID == id);
            if (forumMessage != null)
            {
                var forumMessageView = (ForumMessageView)ModelMapper.Map(forumMessage, typeof(ForumMessage), typeof(ForumMessageView));
                return View(forumMessageView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditMessage(ForumMessageView forumMessageView)
        {
            if (ModelState.IsValid)
            {
                var forumMessage = Repository.ForumMessages.FirstOrDefault(p => p.ID == forumMessageView.ID);
                if (forumMessage.CanEdit(CurrentUser))
                {
                    forumMessage = (ForumMessage)ModelMapper.Map(forumMessageView, typeof(ForumMessageView), typeof(ForumMessage));
                    if (forumMessage.UserID != CurrentUser.ID)
                    {
                        forumMessage.ModeratedByID = CurrentUser.ID;
                        Repository.ModerateForumMessage(forumMessage);
                    }
                    else
                    {
                        Repository.UpdateForumMessage(forumMessage);
                    }
                    return Json(new { result = "ok" });
                }
            }
            return Json(new { result = "error" });
        }

        public ActionResult Item(int id)
        {
            var forumMessage = Repository.ForumMessages.FirstOrDefault(p => p.ID == id);
            if (forumMessage != null)
            {
                return View(forumMessage);
            }
            return View("_OK");
        }

        public ActionResult ToggleForumNotice(int id)
        {
            var existNotice = Repository.Notices.FirstOrDefault(p => p.ForumID == id && p.ReceiverID == CurrentUser.ID);
            if (existNotice == null)
            {
                var notice = new Notice()
                {
                    ReceiverID = CurrentUser.ID,
                    IsCloseForRead = false,
                    Text = "Подписка на форум",
                    Type = (int)Notice.TypeEnum.Forum,
                    ForumID = id,
                    ReadedDate = DateTime.Now
                };
                Repository.CreateNotice(notice);

                var urlNoticeAction = new NoticeAction()
                {
                    NoticeID = notice.ID,
                    Direct = true,
                    ActionUrl = Url.Action("Index", "Forum", new { url = notice.Forum.Url, lastMessage = true }),
                    Name = "На форум"
                };

                var unsubscribeNoticeAction = new NoticeAction()
                {
                    NoticeID = notice.ID,
                    Direct = false,
                    ActionUrl = Url.Action("Remove", "Notice", new { id = notice.ID }),
                    Name = "Отписаться",
                    IsResolveNotice = true
                };
                Repository.CreateNoticeAction(urlNoticeAction);
                Repository.CreateNoticeAction(unsubscribeNoticeAction);

                return Json(new { result = "ok", data = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Repository.RemoveNotice(existNotice.ID);
                return Json(new { result = "ok", data = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IsNoticed(int id)
        {
            var isNoticed = Repository.Notices.Any(p => p.ForumID == id && p.ReceiverID == CurrentUser.ID);

            return View("IsNoticed", isNoticed);
        }
    }
}
