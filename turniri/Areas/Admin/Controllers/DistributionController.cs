using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Tools;
using turniri.Models.ViewModels;
using turniri.Helpers;
using System.IO;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class DistributionController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            IQueryable<Distribution> list = null;
            if (CurrentUser.InRoles("admin"))
            {
                list = Repository.Distributions.OrderByDescending(p => p.ID);
            }
            else
            {
                list = CurrentUser.Distributions.OrderByDescending(p => p.ID).AsQueryable();
            }
            var data = new PageableData<Distribution>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var distributionView = new DistributionView();
            return View("Edit", distributionView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var distribution = Repository.Distributions.FirstOrDefault(p => p.ID == id);

            if (distribution != null)
            {
                if (distribution.CanEdit(CurrentUser))
                {
                    var distributionView = (DistributionView)ModelMapper.Map(distribution, typeof(Distribution), typeof(DistributionView));

                    return View(distributionView);
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(DistributionView distributionView)
        {
            if (ModelState.IsValid)
            {
                var distribution = Repository.Distributions.FirstOrDefault(p => p.ID == distributionView.ID);
                if ((distribution != null && distribution.CanEdit(CurrentUser)) || distribution == null)
                {
                    distribution = (Distribution)ModelMapper.Map(distributionView, typeof(DistributionView), typeof(Distribution));

                    if (distribution.ID == 0)
                    {
                        distribution.UserID = CurrentUser.ID;
                        Repository.CreateDistribution(distribution);
                    }
                    else
                    {
                        Repository.UpdateDistribution(distribution);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(distributionView);
        }

        public ActionResult Delete(int id)
        {
            var distribution = Repository.Distributions.FirstOrDefault(p => p.ID == id);
            if (distribution != null)
            {
                if (distribution.CanDelete(CurrentUser))
                {
                    Repository.RemoveDistribution(distribution.ID);
                    return RedirectToAction("Index");
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Start(int id)
        {
            var distribution = Repository.Distributions.FirstOrDefault(p => p.ID == id);
            if (distribution != null && !distribution.IsStart)
            {

                if (distribution.CanEdit(CurrentUser))
                {
                    var mailController = new MailController();

                    if (distribution.AnyMails)
                    {
                        foreach (var mail in distribution.Mails.Where(p => !p.Delivered))
                        {
                            mail.Subject = distribution.Subject;
                            mail.Subject = mail.Subject.Replace("%username%", mail.User.Login);
                            mail.Body = distribution.Body.BBCodeToHTML().ReplaceSmiles((index, name) => CustomHelpers.SetSmiles(index, name));
                            mail.Body = mail.Body.Replace("%username%", mail.User.Login);
                            mail.Body = mail.Body.Replace("<img src=\"/Media", "<img src=\"" + Config.Host + "/Media");
                            mail.Body = mail.Body.Replace("<a href=\"/", "<a href=\"" + Config.Host + "/");
                            var email = mailController.Subscription(mail, Config.Host);
                            using (var reader = new StreamReader(email.Mail.AlternateViews[0].ContentStream))
                            {
                                mail.Body = reader.ReadToEnd();
                            }
                            Repository.PushMail(mail);
                        }
                        Repository.StartDistribution(distribution.ID);
                    }
                    return RedirectToAction("Index");
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }
    }
}
