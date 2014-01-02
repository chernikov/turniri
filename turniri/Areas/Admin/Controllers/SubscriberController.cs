using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.Info;
using turniri.Tools.Mail;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class SubscriberController : AdminController
    {
        public ActionResult Index(int id)
        {
            var distribution = Repository.Distributions.FirstOrDefault(p => p.ID == id);
            if (distribution != null)
            {
                if (distribution.CanEdit(CurrentUser))
                {
                    return View(distribution);
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult ClearAll(int id)
        {
            Repository.ClearDistribution(id);
            
           
            return RedirectToAction("Index", new { id = id });
        }

        public ActionResult Delete(Guid id)
        {
            var mail = Repository.Mails.FirstOrDefault(p => p.ID == id);
            if (mail != null)
            {
                Repository.RemoveMail(mail.ID);
                return Json(new { result = "ok" });
            }
            return null;
        }

        [HttpGet]
        public ActionResult Add(int id)
        {
            var subscriberSearch = new SubscriberSearch(CurrentUser)
            {
                ID = id
            };
            return View(subscriberSearch);
        }

        [HttpPost]
        public ActionResult Add(SubscriberSearch subscriberSearch)
        {
            var distribution = Repository.Distributions.FirstOrDefault(p => p.ID == subscriberSearch.ID);
            subscriberSearch.CurrentUser = CurrentUser;

            if (distribution != null)
            {
                IQueryable<User> list = null;
                if (!subscriberSearch.RoleID.HasValue || subscriberSearch.RoleID > 100)
                {
                    //all users 
                    list = Repository.RegularUsers;
                    if (subscriberSearch.RoleID.HasValue)
                    {
                        switch (subscriberSearch.RoleID.Value)
                        {
                            case 101:
                                //game selector
                                if (subscriberSearch.SelectedGames.Any())
                                {
                                    list = list.Where(p => p.UserGames.Any(r => subscriberSearch.SelectedGames.Contains(r.GameID)));
                                }
                                break;
                            case 102:
                                //tournament selector
                                if (subscriberSearch.SelectedTournaments.Any())
                                {
                                    list = list.Where(p => p.Participants.Any(r => subscriberSearch.SelectedTournaments.Contains(r.TournamentID.Value)));
                                }
                                break;
                        }
                    }
                }
                else
                {
                    list = Repository.RegularUsers.Where(p => p.UserRoles.Any(r => r.Role.ID == subscriberSearch.RoleID.Value));

                    switch (subscriberSearch.RoleID.Value)
                    {
                        case 2:
                        case 4:
                            //game selector
                            if (subscriberSearch.SelectedGames.Any())
                            {
                                list = list.Where(p => p.UserRoles.Any(r => r.UserRoleGames.Any(g => subscriberSearch.SelectedGames.Contains(g.GameID))));
                            }
                            break;
                        case 3:
                        case 5:
                            //tournament selector
                            if (subscriberSearch.SelectedTournaments.Any())
                            {
                                list = list.Where(p => p.UserRoles.Any(r => r.UserRoleTournaments.Any(g => subscriberSearch.SelectedTournaments.Contains(g.TournamentID))));
                            }
                            break;
                    }
                }
                foreach (var user in list.Where(p => p.Subscription))
                {
                    if (!distribution.Mails.Any(p => p.UserID == user.ID))
                    {
                        if (!string.IsNullOrWhiteSpace(user.Email))
                        {
                            var newMail = new Mail()
                            {
                                UserID = user.ID,
                                Email = user.Email,
                                DistributionID = distribution.ID
                            };
                            Repository.SaveMail(newMail);
                        }
                    }
                }
            }
            return View("Ok");
        }

        public ActionResult Test()
        {
            var mailController = new MailController();

            var mail = new Mail() {
                Subject = "Тест",
                Body = "Тест",
                User = CurrentUser,
                Email ="rollinx@yandex.ru"
            };
            var email = mailController.Subscription(mail, Config.Host);
            mail.Subject = email.Mail.Subject;
            using (var reader = new StreamReader(email.Mail.AlternateViews[0].ContentStream))
            {
                var content = reader.ReadToEnd();
                mail.Body = content;
            }
            MailSender.SendMail(mail.Email, mail.Subject, mail.Body);
            return null;
        }
    }
}
