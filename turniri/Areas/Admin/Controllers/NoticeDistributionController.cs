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
    public class NoticeDistributionController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            IQueryable<NoticeDistribution> list = null;
            if (CurrentUser.InRoles("admin"))
            {
                list = Repository.NoticeDistributions.OrderByDescending(p => p.ID);
            }
            else
            {
                list = CurrentUser.NoticeDistributions.OrderByDescending(p => p.ID).AsQueryable();
            }
            var data = new PageableData<NoticeDistribution>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var noticeDistributionView = new NoticeDistributionView();
            return View("Edit", noticeDistributionView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var noticeDistribution = Repository.NoticeDistributions.FirstOrDefault(p => p.ID == id);

            if (noticeDistribution != null)
            {
                if (noticeDistribution.CanEdit(CurrentUser))
                {
                    var noticeDistributionView = (DistributionView)ModelMapper.Map(noticeDistribution, typeof(Distribution), typeof(DistributionView));

                    return View(noticeDistributionView);
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(NoticeDistributionView noticeDistributionView)
        {
            if (ModelState.IsValid)
            {
                var noticeDistribution = Repository.NoticeDistributions.FirstOrDefault(p => p.ID == noticeDistributionView.ID);
                if ((noticeDistribution != null && noticeDistribution.CanEdit(CurrentUser)) || noticeDistribution == null)
                {
                    noticeDistribution = (NoticeDistribution)ModelMapper.Map(noticeDistributionView, typeof(NoticeDistributionView), typeof(NoticeDistribution));

                    if (noticeDistribution.ID == 0)
                    {
                        noticeDistribution.UserID = CurrentUser.ID;
                        Repository.CreateNoticeDistribution(noticeDistribution);
                    }
                    else
                    {
                        Repository.UpdateNoticeDistribution(noticeDistribution);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(noticeDistributionView);
        }

        public ActionResult Delete(int id)
        {
            var noticeDistribution = Repository.NoticeDistributions.FirstOrDefault(p => p.ID == id);
            if (noticeDistribution != null)
            {
                if (noticeDistribution.CanDelete(CurrentUser))
                {
                    Repository.RemoveNoticeDistribution(noticeDistribution.ID);
                    return RedirectToAction("Index");
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }
    }
}
