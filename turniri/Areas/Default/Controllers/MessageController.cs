using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools.Mail;

namespace turniri.Areas.Default.Controllers
{
    [Authorize]
    public class MessageController : DefaultController
    {
        public ActionResult Index(int? messageID = null)
        {
            ViewBag.Tab = 1;
            ViewBag.MessageID = messageID;
            return View(CurrentUser);
        }

        public ActionResult Fight(int? messageID = null)
        {
            ViewBag.Tab = 2;
            ViewBag.MessageID = messageID;
            return View("Index", CurrentUser);
        }

        public ActionResult Invoice(int? messageID = null)
        {
            ViewBag.Tab = 3;
            ViewBag.MessageID = messageID;
            return View("Index", CurrentUser);
        }

        public ActionResult Outcome(int? messageID = null)
        {
            ViewBag.Tab = 4;
            ViewBag.MessageID = messageID;
            return View("Index", CurrentUser);
        }

        public ActionResult IncomeSubjects()
        {
            var subjects = CurrentUser.IncomeMessages.OrderByDescending(p => p.AddedDate).Select(p => p.Subject).Distinct();
            return View(subjects);
        }

        public ActionResult OutcomeSubjects()
        {
            var subjects = CurrentUser.OutcomeMessages.OrderByDescending(p => p.AddedDate).Select(p => p.Subject).Distinct();
            return View(subjects);
        }

        public ActionResult FightSubjects()
        {
            var subjects = CurrentUser.FightMessages.OrderByDescending(p => p.AddedDate).Select(p => p.Subject).Distinct();
            return View(subjects);
        }

        public ActionResult InvoiceSubjects()
        {
            var subjects = CurrentUser.InvoiceMessages.OrderByDescending(p => p.AddedDate).Select(p => p.Subject).Distinct();
            return View(subjects);
        }

        public ActionResult Messages(int id, int page = 1)
        {
            var subject = Repository.Subjects.FirstOrDefault(p => p.ID == id);
            if (subject != null)
            {
                Repository.ReadSubject(subject.ID, CurrentUser.ID);

                var notices = subject.Notices.Where(p => p.ReceiverID == CurrentUser.ID).ToList();
                foreach (var notice in notices)
                {
                    Repository.RemoveNotice(notice.ID);
                }

                ViewBag.Page = page;
                return View(subject);
            }
            return null;
        }

        [HttpGet]
        public ActionResult WriteMessage(int? id = null)
        {
            var messageView = new MessageView
            {
                SenderID = CurrentUser.ID
            };

            if (id.HasValue)
            {
                var receiver = Repository.Users.FirstOrDefault(p => p.ID == id.Value);
                if (receiver != null)
                {
                    messageView.ReceiverID = receiver.ID;
                    messageView.ReceiverLogin = receiver.Login;
                }
            }
            return View(messageView);
        }

        [HttpPost]
        public ActionResult WriteMessage(MessageView messageView)
        {
            if (messageView.ReceiverID == 0)
            {
                ModelState.AddModelError("ReceiverLogin", "Введите логин получателя");
            }
            if (ModelState.IsValid)
            {
                var message = (Message)ModelMapper.Map(messageView, typeof(MessageView), typeof(Message));

                var subject = new Subject
                {
                    Name = messageView.SubjectName
                };
                Repository.CreateSubject(subject);
                message.SubjectID = subject.ID;
                message.SenderID = CurrentUser.ID;

                SendNewPrivateMessage(subject, message);

                CreateNoticeForMessage(message, subject);

                return View("_OK");

            }
            return View(messageView);
        }

        [HttpGet]
        public ActionResult WriteFightMessage(int? id = null, int? idGame = null)
        {
            var messageView = new FightMessageView
            {
                SenderID = CurrentUser.ID,
            };

            if (id.HasValue)
            {
                var receiver = Repository.Users.FirstOrDefault(p => p.ID == id.Value);
                if (receiver != null)
                {
                    messageView.ReceiverID = receiver.ID;
                    messageView.ReceiverLogin = receiver.Login;
                    messageView.InitedReceiver = true;
                }
            }
            if (idGame.HasValue)
            {
                var game = Repository.Games.FirstOrDefault(p => p.ID == idGame.Value);
                if (game != null)
                {
                    messageView.GameID = game.ID;
                    messageView.PlatformID = game.PlatformID;
                    messageView.InitedGame = true;
                }
            }
            return View(messageView);
        }

        [HttpPost]
        public ActionResult WriteFightMessage(FightMessageView fightMessageView)
        {
            if (fightMessageView.ReceiverID == 0)
            {
                ModelState.AddModelError("ReceiverLogin", "Введите логин получателя");
            }
            if (ModelState.IsValid)
            {
                var message = (Message)ModelMapper.Map(fightMessageView, typeof(FightMessageView), typeof(Message));
                var receiver = Repository.Users.FirstOrDefault(p => p.ID == message.ReceiverID);
                var game = Repository.Games.FirstOrDefault(p => p.ID == fightMessageView.GameID);

                var subject = new Subject
                {
                    Name = "Приглашение сыграть в " + game.Name
                };
                Repository.CreateSubject(subject);
                message.SubjectID = subject.ID;

                var participant1 = new Participant()
                {
                    UserID = CurrentUser.ID,
                };

                var participant2 = new Participant()
                {
                    UserID = message.ReceiverID
                };

                Repository.CreateParticipant(participant1);
                Repository.CreateParticipant(participant2);

                var match = new Match
                {
                    GameID = fightMessageView.GameID,
                    Participant1ID = participant1.ID,
                    Participant2ID = participant2.ID,
                    Name = "Одиночный матч между " + CurrentUser.Login + " и " + receiver.Login,
                    Status = (int)Match.MatchStatusEnum.Created,
                    Number = 1
                };
                Repository.CreateMatch(match);

                if (fightMessageView.CountRound > 1)
                {

                    var round2 = new Round
                    {
                        Number = 2,
                        Participant1ID = match.Participant1ID,
                        Participant2ID = match.Participant2ID,
                        Status = (int)Round.RoundStatusEnum.Created,
                        MatchID = match.ID,
                    };
                    Repository.CreateRound(round2);
                }
                var round = new Round
                {
                    Number = 1,
                    Participant1ID = match.Participant1ID,
                    Participant2ID = match.Participant2ID,
                    Status = (int)Round.RoundStatusEnum.Created,
                    MatchID = match.ID,
                };
                Repository.CreateRound(round);
                message.MatchID = match.ID;
                message.SenderID = CurrentUser.ID;
                SendNewPrivateMessage(subject, message);

                CreateNoticeForFightMessage(message, subject, match);

                return View("_OK");
            }
            return View(fightMessageView);
        }

        [HttpGet]
        public ActionResult WriteInvoiceMessage(int id, int? idGame = null)
        {
            var receiver = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (receiver != null)
            {
                var messageView = new InvoiceMessageView
                {
                    SenderID = CurrentUser.ID,
                    ReceiverID = receiver.ID,
                    ReceiverLogin = receiver.Login,
                };
                if (idGame.HasValue)
                {
                    var group = CurrentUser.GroupsWhereImMember.FirstOrDefault(p => p.GameID == idGame);

                    if (group != null)
                    {
                        messageView.GroupID = group.ID;
                        messageView.GroupName = group.Name;
                        messageView.InitedGroup = true;
                    }
                }
                
                return View(messageView);
            }
            return null;
        }

        [HttpPost]
        public ActionResult WriteInvoiceMessage(InvoiceMessageView invoiceMessageView)
        {

            if (ModelState.IsValid)
            {
                var message = (Message)ModelMapper.Map(invoiceMessageView, typeof(InvoiceMessageView), typeof(Message));
                var group = Repository.Groups.FirstOrDefault(p => p.ID == message.GroupID);

                var subject = new Subject
                {
                    Name = "Приглашение в команду " + group.Name
                }; 
                Repository.CreateSubject(subject);
                message.SubjectID = subject.ID;
                message.SenderID = CurrentUser.ID;
                SendNewPrivateMessage(subject, message);

                CreateNoticeForInvoice(message, group, subject);

                return View("_OK");
            }
            return View(invoiceMessageView);
        }

       
        public ActionResult AutocompleteUser(string query)
        {
            var data = Repository.RegularUsers
                .Where(r => r.Login.StartsWith(query) || r.Email.StartsWith(query))
                .Take(10)
                .ToArray();
            return Json(new
            {
                query = query,
                data = data.Select(p => new
                {
                    ID = p.ID,
                    Label = p.Login
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ReplyMessage(int id, int idUser)
        {
            var subject = Repository.Subjects.FirstOrDefault(p => p.ID == id);

            if (subject != null)
            {
                var messageLast = subject.LastMessage;

                var messageView = new MessageView
                {
                    SubjectID = subject.ID,
                    Subject = subject,
                    SubjectName = subject.Name,
                    ReceiverID = idUser,
                    SenderID = CurrentUser.ID,
                    Match = messageLast.Match,
                    MatchID = messageLast.MatchID,
                    GameID = messageLast.GameID,
                    GroupID = messageLast.GroupID,
                };
                if (messageLast.GroupID != null)
                {
                    var group = Repository.Groups.FirstOrDefault(p => p.ID == messageLast.GroupID);
                    messageView.Group = group;
                }
                return View(messageView);
            }
            return null;
        }

        [HttpPost]
        public ActionResult ReplyMessage(MessageView messageView)
        {
            Subject subject = null;
            if (ModelState.IsValid)
            {
                var message = (Message)ModelMapper.Map(messageView, typeof(MessageView), typeof(Message));
                message.SenderID = CurrentUser.ID;
                subject = Repository.Subjects.First(p => p.ID == messageView.SubjectID);
                SendNewPrivateMessage(subject, message);

                CreateNoticeForMessage(message, subject);
                return View("_OK");
            }
            else
            {
                subject = Repository.Subjects.FirstOrDefault(p => p.ID == messageView.SubjectID);
                var messageLast = subject.LastMessage;
                messageView = new MessageView
                {
                    Subject = subject,
                    SubjectName = subject.Name,
                    Match = messageLast.Match,
                };
                if (messageLast.GroupID != null)
                {
                    var group = Repository.Groups.FirstOrDefault(p => p.ID == messageLast.GroupID);
                    messageView.Group = group;
                }

                return View(messageView);
            }
        }

        public ActionResult DeleteMessages(int id)
        {
            var subject = Repository.Subjects.FirstOrDefault(p => p.ID == id);
            if (subject != null)
            {
                var messages = subject.Messages.Where(p => (p.SenderID == CurrentUser.ID && p.IsSend) || (p.ReceiverID == CurrentUser.ID && !p.IsSend)).ToList();
                if (messages.Count > 0 && messages[0].MatchID != null
                    && (messages[0].Match.Status == (int)Match.MatchStatusEnum.Created))
                {
                    var matchID = messages[0].MatchID.Value;
                    Repository.DropSingleMatchWithMessages(matchID);
                }
                else
                {
                    foreach (var message in messages)
                    {
                        Repository.RemoveMessage(message.ID);
                    }
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateGamesSelect(int idPlatform, int? idGame = null)
        {
            var platform = Repository.Platforms.FirstOrDefault(p => p.ID == idPlatform);
            if (platform != null)
            {
                var games = platform.Games.Select(p => new SelectListItem
                {
                    Selected = p.ID == idGame,
                    Text = p.Name,
                    Value = p.ID.ToString()
                });
                return View(games);
            }
            return null;
        }

        public ActionResult CancelMatch(int id)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == id);
            if (match != null)
            {
                var message = match.Messages.FirstOrDefault();
                if (message != null)
                {
                    var subject = message.Subject;
                    var messages = subject.Messages.ToList();
                    foreach (var inMessage in messages)
                    {
                        Repository.RemoveMessage(inMessage.ID);
                    }
                    var notices = subject.Notices.ToList();
                    foreach (var notice in notices)
                    {
                        Repository.RemoveNotice(notice.ID);
                    }
                }
                Repository.RemoveMatch(match.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitMatch(int id)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == id);
            if (match != null)
            {
                Repository.PlayMatch(match.ID);
                var message = match.Messages.FirstOrDefault();
                if (message != null)
                {
                    var subject = message.Subject;
                    var notices = subject.Notices.ToList();
                    foreach (var notice in notices)
                    {
                        Repository.RemoveNotice(notice.ID);
                    }

                    Repository.ReadSubject(subject.ID, CurrentUser.ID);
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult CancelInvoice(int id)
        {
            var subject = Repository.Subjects.FirstOrDefault(p => p.ID == id);
            if (subject != null && subject.FirstMessage.ReceiverID == CurrentUser.ID)
            {
                Repository.RemoveSubject(subject.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult SubmitInvoice(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (group != null)
            {
                var userGroup = new UserGroup
                {
                    GroupID = group.ID,
                    UserID = CurrentUser.ID,
                    Status = (int)UserGroup.StatusEnum.Granded
                };
                Repository.CreateUserGroup(userGroup);

                var message = group.Messages.FirstOrDefault();
                if (message != null) 
                {
                    var subject = message.Subject;
                    var messages = subject.Messages;
                    foreach (var inMessage in messages)
                    {
                        Repository.RemoveMessage(inMessage.ID);
                    }
                    var notices = subject.Notices.ToList();
                    foreach (var notice in notices)
                    {
                        Repository.RemoveNotice(notice.ID);
                    }
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        private void CreateNoticeForMessage(Message message, Subject subject)
        {
            var notice = new Notice()
            {
                ReceiverID = message.ReceiverID,
                SenderID = CurrentUser.ID,
                SubjectID = subject.ID,
                Caption = "Личное сообщение",
                Text = string.Format("Пользователь {0} прислал вам личное сообщение: \r\n {1} \r\n {2}", CurrentUser.Login, subject.Name, message.Text),
                Type = (int)Notice.TypeEnum.Message,
                IsCloseForRead = false
            };
            Repository.CreateNotice(notice);

            var messageNoticeAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("Index", "Message", new { messageID = subject.ID }),
                Name = "К сообщению",
                Direct = true
            };
            Repository.CreateNoticeAction(messageNoticeAction);
        }

        private void CreateNoticeForFightMessage(Message message, Subject subject, Match match)
        {
            var notice = new Notice()
            {
                ReceiverID = message.ReceiverID,
                SenderID = CurrentUser.ID,
                SubjectID = subject.ID,
                Caption = "Боевое сообщение",
                Text = string.Format("Пользователь {0} прислал вам боевое сообщение.\r\n Предлагает сыграть в {1} {2} : \r\n {3}", CurrentUser.Login, match.Game.Name, match.CountRounds > 1 ? "матч до двух побед" : "одиночный матч", message.Text),
                Type = (int)Notice.TypeEnum.Message,
                IsCloseForRead = false
            };
            Repository.CreateNotice(notice);

            var messageNoticeAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("Fight", "Message", new { messageID = subject.ID }),
                Name = "К сообщению",
                Direct = true
            };

            var acceptNoticeAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("SubmitMatch", "Message", new { id = match.ID }),
                Name = "Принять бой",
                Direct = false,
                IsResolveNotice = true,
            };

            var declineNoticeAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("CancelMatch", "Message", new { id = match.ID }),
                Name = "Отказаться",
                Direct = false,
                IsResolveNotice = true,
            };
            Repository.CreateNoticeAction(acceptNoticeAction);
            Repository.CreateNoticeAction(declineNoticeAction);
            Repository.CreateNoticeAction(messageNoticeAction);
        }

        private void CreateNoticeForInvoice(Message message, Group group, Subject subject)
        {
            var notice = new Notice()
            {
                ReceiverID = message.ReceiverID,
                SenderID = CurrentUser.ID,
                SubjectID = subject.ID,
                Caption = "Приглашение",
                Text = string.Format("Пользователь {0} прислал вам приглашение: \r\n {1} \r\n {2}", CurrentUser.Login, subject.Name, message.Text),
                Type = (int)Notice.TypeEnum.Message,
                IsCloseForRead = false
            };

            Repository.CreateNotice(notice);

            var acceptNoticeAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("SubmitInvoice", "Message", new { id = group.ID }),
                Name = "Вступить",
                Direct = false,
                IsResolveNotice = true,
            };

            var declineNoticeAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("CancelInvoice", "Message", new { id = subject.ID }),
                Name = "Отказаться",
                Direct = false,
                IsResolveNotice = true,
            };

            var messageNoticeAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("Invoice", "Message", new { messageID = subject.ID }),
                Name = "К приглашению",
                Direct = true
            };

            Repository.CreateNoticeAction(acceptNoticeAction);
            Repository.CreateNoticeAction(declineNoticeAction);
            Repository.CreateNoticeAction(messageNoticeAction);
        }


    }
}
