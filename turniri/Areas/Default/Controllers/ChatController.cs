using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Default.Controllers
{
    public class ChatController : DefaultController
    {
        public ActionResult Index(int? id = null)
        {
            if (id == null)
            {
                var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.TournamentID == null);
                if (chatRoom != null)
                {
                    if (CurrentUser != null)
                    {
                        try
                        {
                            var chatRoomLog = new ChatRoomLog()
                            {
                                ChatRoomID = chatRoom.ID,
                                UserID = CurrentUser.ID,
                            };
                            Repository.CreateChatRoomLog(chatRoomLog);
                        }
                        catch { }
                     
                    }
                    return View("CommonChat", chatRoom);
                }
            }
            else
            {
                var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.TournamentID == id);
                if (chatRoom != null)
                {
                    if (CurrentUser != null)
                    {
                        try
                        {
                            var chatRoomLog = new ChatRoomLog()
                            {
                                ChatRoomID = chatRoom.ID,
                                UserID = CurrentUser.ID,
                            };
                            Repository.CreateChatRoomLog(chatRoomLog);
                            Repository.UpdateVisitChatRoomNotice(chatRoom.ID, CurrentUser.ID);
                        }
                        catch{ }
                    }
                    return View(chatRoom);
                }
            }
            return null;
        }

        public ActionResult Forum()
        {
            var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.TournamentID == null);
            if (chatRoom != null)
            {
                return View("Index", chatRoom);
            }
            return null;
        }

        [HttpPost]
        public ActionResult Write(int id, string message)
        {
            var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.ID == id);

            if (chatRoom != null && chatRoom.CanAddMessage(CurrentUser))
            {
                var chatMessage = new ChatMessage()
                {
                    ChatRoomID = chatRoom.ID,
                    Type = (int)ChatMessage.MessageType.Simple,
                    Message = message,
                    UserID = CurrentUser.ID
                };
                Repository.CreateChatMessage(chatMessage);

                if (chatRoom.TranslateInForum)
                {
                    if (chatRoom.Tournament != null && chatRoom.Tournament.Forum != null)
                    {
                        var forum = chatRoom.Tournament.Forum;

                        var forumMessage = new ForumMessage()
                        {
                            UserID = CurrentUser.ID,
                            ChatRoomID = chatRoom.ID,
                            ForumID = forum.ID,
                            Message = message,
                        };
                        Repository.CreateForumMessage(forumMessage);
                    }

                }
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public ActionResult Remove(int id)
        {
            var chatMessage = Repository.ChatMessages.FirstOrDefault(p => p.ID == id);
            if (chatMessage != null && chatMessage.CanDelete(CurrentUser))
            {
                Repository.RemoveChatMessage(chatMessage.ID);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public ActionResult Update(int id, int chatMessageId)
        {
            var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.ID == id);
            if (chatRoom != null)
            {
                ViewBag.CanAdd = chatRoom.CanAddMessage(CurrentUser);
                if (chatMessageId != chatRoom.LastIdUpdate)
                {
                    if (chatRoom.LastIdUpdate.HasValue)
                    {
                        var min = Math.Min(chatMessageId, chatRoom.LastIdUpdate.Value);
                        var list = Repository.ChatMessages.Where(p => p.ChatRoomID == id && p.ID >= min).OrderByDescending(p => p.ID).ToList();
                        ViewBag.LastUpdateID = chatRoom.LastIdUpdate;
                        return View(list);
                    }
                }
                return View("Update", null);
            };
            return null;
        }

        public ActionResult LoadOld(int id)
        {
            var lastChat = Repository.ChatMessages.FirstOrDefault(p => p.ID == id);
            if (lastChat != null)
            {
                var list = Repository.ChatMessages.Where(p => p.ChatRoomID == lastChat.ChatRoomID && p.ID < id).OrderByDescending(p => p.ID).ToList();

                ViewBag.ShowMore = list.Count > 50;

                return View(list.Take(50));
            };
            return null;
        }

        [HttpGet]
        public ActionResult BanUser(int id)
        {
            var chatBanUserView = new ChatBannedUserView()
            {
                UserID = id
            };
            return View(chatBanUserView);
        }

        [HttpPost]
        public ActionResult BanUser(ChatBannedUserView chatBannedUserView)
        {
            var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.ID == chatBannedUserView.ChatRoomID);
            if (ModelState.IsValid)
            {
                if (chatRoom != null && chatRoom.CanBan(CurrentUser))
                {
                    var chatBannedUser = (ChatBannedUser)ModelMapper.Map(chatBannedUserView, typeof(ChatBannedUserView), typeof(ChatBannedUser));

                    var dateTill = "";
                    switch ((ChatBannedUserView.DateTillTo)chatBannedUserView.ChoisePeriod)
                    {
                        case ChatBannedUserView.DateTillTo._20min:
                            chatBannedUser.DateTill = DateTime.Now.AddMinutes(20);
                            dateTill = "на 20 минут";
                            break;
                        case ChatBannedUserView.DateTillTo.one_hour:
                            chatBannedUser.DateTill = DateTime.Now.AddHours(1);
                            dateTill = "на один час";
                            break;
                        case ChatBannedUserView.DateTillTo.one_day:
                            chatBannedUser.DateTill = DateTime.Now.AddDays(1);
                            dateTill = "на одни сутки";
                            break;
                        case ChatBannedUserView.DateTillTo.forever:
                            chatBannedUser.DateTill = null;
                            dateTill = "навсегда";
                            break;
                    }
                    Repository.CreateChatBannedUser(chatBannedUser);

                    var chatMessage = new ChatMessage()
                    {
                        ChatRoomID = chatRoom.ID,
                        Message = string.Format("{0} забанил {1} по причине \"{2}\"  {3}", CurrentUser.Login, chatBannedUser.User.Login, chatBannedUser.Reason, dateTill),
                        UserID = CurrentUser.ID,
                        Type = (int)ChatMessage.MessageType.System
                    };
                    Repository.CreateChatMessage(chatMessage);

                    return View("_OK");
                }
            }
            return View(chatBannedUserView);
        }

        [HttpGet]
        public ActionResult CommonBanUser(int id)
        {
            var chatBanUserView = new ChatBannedUserView()
            {
                UserID = id
            };
            return View(chatBanUserView);
        }

        [HttpPost]
        public ActionResult CommonBanUser(ChatBannedUserView chatBannedUserView)
        {
            var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.ID == chatBannedUserView.ChatRoomID);
            if (ModelState.IsValid)
            {
                if (chatRoom != null && chatRoom.CanBan(CurrentUser))
                {
                    var chatBannedUser = (ChatBannedUser)ModelMapper.Map(chatBannedUserView, typeof(ChatBannedUserView), typeof(ChatBannedUser));

                    var dateTill = "";
                    switch ((ChatBannedUserView.DateTillTo)chatBannedUserView.ChoisePeriod)
                    {
                        case ChatBannedUserView.DateTillTo._20min:
                            chatBannedUser.DateTill = DateTime.Now.AddMinutes(20);
                            dateTill = "на 20 минут";
                            break;
                        case ChatBannedUserView.DateTillTo.one_hour:
                            chatBannedUser.DateTill = DateTime.Now.AddHours(1);
                            dateTill = "на один час";
                            break;
                        case ChatBannedUserView.DateTillTo.one_day:
                            chatBannedUser.DateTill = DateTime.Now.AddDays(1);
                            dateTill = "на одни сутки";
                            break;
                        case ChatBannedUserView.DateTillTo.forever:
                            chatBannedUser.DateTill = null;
                            dateTill = "навсегда";
                            break;
                    }
                    Repository.CreateChatBannedUser(chatBannedUser);

                    var chatMessage = new ChatMessage()
                    {
                        ChatRoomID = chatRoom.ID,
                        Message = string.Format("{0} забанил {1} по причине \"{2}\"  {3}", CurrentUser.Login, chatBannedUser.User.Login, chatBannedUser.Reason, dateTill),
                        UserID = CurrentUser.ID,
                        Type = (int)ChatMessage.MessageType.System
                    };
                    Repository.CreateChatMessage(chatMessage);

                    return View("_OK");
                }
            }
            return View(chatBannedUserView);
        }

        public ActionResult ToggleChatNotice(int id)
        {
            var chatRoom = Repository.ChatRooms.FirstOrDefault(p => p.ID == id);
            if (chatRoom != null)
            {
                var existNotice = Repository.Notices.FirstOrDefault(p => p.ChatRoomID == id && p.ReceiverID == CurrentUser.ID);
                if (existNotice == null)
                {
                    var notice = new Notice()
                    {
                        ReceiverID = CurrentUser.ID,
                        IsCloseForRead = false,
                        Text = "Подписка на чат",
                        Type = (int)Notice.TypeEnum.Chat,
                        ChatRoomID = id,
                        ReadedDate = DateTime.Now
                    };
                    Repository.CreateNotice(notice);
                    if (chatRoom.TournamentID != null)
                    {
                        var urlNoticeAction = new NoticeAction()
                        {
                            NoticeID = notice.ID,
                            Direct = true,
                            ActionUrl = Url.Action("Index", "Tournament", new { platformUrl = chatRoom.Tournament.Game.Platform.Url, gameUrl = chatRoom.Tournament.Game.Url, url = chatRoom.Tournament.Url }),
                            Name = "К турниру"
                        };
                        Repository.CreateNoticeAction(urlNoticeAction);
                    }
                    var unsubscribeNoticeAction = new NoticeAction()
                    {
                        NoticeID = notice.ID,
                        Direct = false,
                        ActionUrl = Url.Action("Remove", "Notice", new { id = notice.ID }),
                        Name = "Отписаться",
                        IsResolveNotice = true
                    };
                   
                    Repository.CreateNoticeAction(unsubscribeNoticeAction);

                    return Json(new { result = "ok", data = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Repository.RemoveNotice(existNotice.ID);
                    return Json(new { result = "ok", data = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
    }
}
