using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Tools.Mail;

namespace turniri.Areas.Default.Controllers
{
    public class FriendController : DefaultController
    {
        public ActionResult Index(string login = null)
        {
            ViewBag.TabID = 1;
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

        [Authorize]
        public ActionResult Income()
        {
            ViewBag.TabID = 2;
            return View("Index", CurrentUser);
        }

        public ActionResult Friends(int id = 0, int page = 1)
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

        public ActionResult Orders(int id = 0, int page = 1)
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

        [Authorize]
        public ActionResult AddFriend(int id)
        {
            var user = Repository.RegularUsers.Where(p => p.ID == id).FirstOrDefault();
            if (user != null)
            {
                if (user.AskForFriend(CurrentUser.ID))
                {
                    var friendship = user.Friendships1.FirstOrDefault(p => p.ReceiverID == CurrentUser.ID);
                    if (friendship != null)
                    {
                        Repository.ConfirmFriendship(friendship.ID);
                    }
                    return View("AlreadyAddedFriend", user);
                }
                else
                {
                    if (!CurrentUser.AskForFriend(user.ID) && !user.HasFriend(CurrentUser.ID))
                    {
                        var friendship = new Friendship()
                        {
                            SenderID = CurrentUser.ID,
                            ReceiverID = user.ID
                        };
                        Repository.CreateFriendship(friendship);
                        NotifyMail.SendNotify(Config, "Friend", user.Email,
                                      (u, format) => string.Format(format, CurrentUser.Login, HostName),
                                      (u, format) => string.Format(format, HostName, u.ID, u.Login),
                                      CurrentUser);

                        var notice = new Notice()
                        {
                            ReceiverID = user.ID,
                            SenderID = CurrentUser.ID,
                            Type = (int)Notice.TypeEnum.Friendship,
                            Caption = "Приглашение дружить",
                            Text = string.Format("Пользователь {0} хочет подружиться с вами", CurrentUser.Login),
                            IsCloseForRead = false,
                        };

                        Repository.CreateNotice(notice);

                        var acceptNoticeAction = new NoticeAction()
                        {
                            NoticeID = notice.ID,
                            ActionUrl = Url.Action("ConfirmFriendship", "Friend", new { id = friendship.ID }),
                            Name = "Принять",
                            Direct = false,
                            IsResolveNotice = true,
                        };

                        var declineNoticeAction = new NoticeAction()
                        {
                            NoticeID = notice.ID,
                            ActionUrl = Url.Action("DeclineFriendship", "Friend", new { id = friendship.ID }),
                            Name = "Отказаться",
                            Direct = false,
                            IsResolveNotice = true,
                        };

                        Repository.CreateNoticeAction(acceptNoticeAction);
                        Repository.CreateNoticeAction(declineNoticeAction);
                    }
                }
                return View(user);
            }
            return null;
        }

        [Authorize]
        public ActionResult RemoveFriend(int id)
        {
            var user = Repository.Users.Where(p => p.ID == id).FirstOrDefault();
            if (user != null)
            {
                Repository.RemoveFriend(CurrentUser.ID, user.ID);
                if (Request.UrlReferrer != null)
                {
                    return Redirect(Request.UrlReferrer.AbsoluteUri);
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        public ActionResult ConfirmFriendship(int id)
        {
            Repository.ConfirmFriendship(id);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        public ActionResult DeclineFriendship(int id)
        {
            Repository.DeclineFriendship(id);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}
