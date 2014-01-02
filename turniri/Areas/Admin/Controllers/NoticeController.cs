using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.Info;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class NoticeController : AdminController
    {
        public ActionResult Item(int id)
        {
            var noticeDistribution = Repository.NoticeDistributions.FirstOrDefault(p => p.ID == id);
            if (noticeDistribution != null)
            {
                if (noticeDistribution.CanEdit(CurrentUser))
                {
                    return View(noticeDistribution);
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult ClearAll(int id)
        {
            Repository.ClearNoticeDistribution(id);

            return RedirectToAction("Item", new { id = id });
        }

        public ActionResult Delete(int id)
        {
            var notice = Repository.Notices.FirstOrDefault(p => p.ID == id);
            if (notice != null)
            {
                Repository.RemoveNotice(notice.ID);
                return Json(new { result = "ok" });
            }
            return null;
        }

        [HttpGet]
        public ActionResult Add(int id)
        {
            var noticeUserSearch = new NoticeUserSearch(CurrentUser)
            {
                ID = id
            };
            return View(noticeUserSearch);
        }

        [HttpPost]
        public ActionResult Add(NoticeUserSearch noticeUserSearch)
        {
            var noticeDistribution = Repository.NoticeDistributions.FirstOrDefault(p => p.ID == noticeUserSearch.ID);
            noticeUserSearch.CurrentUser = CurrentUser;

            if (noticeDistribution != null)
            {
                IQueryable<User> list = null;
                if (!noticeUserSearch.RoleID.HasValue || noticeUserSearch.RoleID > 100)
                {
                    //all users 
                    list = Repository.RegularUsers;
                    if (noticeUserSearch.RoleID.HasValue)
                    {
                        switch (noticeUserSearch.RoleID.Value)
                        {
                            case 101:
                                //game selector
                                if (noticeUserSearch.SelectedGames.Any())
                                {
                                    list = list.Where(p => p.UserGames.Any(r => noticeUserSearch.SelectedGames.Contains(r.GameID)));
                                }
                                break;
                            case 102:
                                //tournament selector
                                if (noticeUserSearch.SelectedTournaments.Any())
                                {
                                    list = list.Where(p => p.Participants.Any(r => noticeUserSearch.SelectedTournaments.Contains(r.TournamentID.Value)));
                                }
                                break;
                        }
                    }
                }
                else
                {
                    list = Repository.RegularUsers.Where(p => p.UserRoles.Any(r => r.Role.ID == noticeUserSearch.RoleID.Value));

                    switch (noticeUserSearch.RoleID.Value)
                    {
                        case 2:
                        case 4:
                            //game selector
                            if (noticeUserSearch.SelectedGames.Any())
                            {
                                list = list.Where(p => p.UserRoles.Any(r => r.UserRoleGames.Any(g => noticeUserSearch.SelectedGames.Contains(g.GameID))));
                            }
                            break;
                        case 3:
                        case 5:
                            //tournament selector
                            if (noticeUserSearch.SelectedTournaments.Any())
                            {
                                list = list.Where(p => p.UserRoles.Any(r => r.UserRoleTournaments.Any(g => noticeUserSearch.SelectedTournaments.Contains(g.TournamentID))));
                            }
                            break;
                    }
                }
                foreach (var user in list)
                {
                    var notice = new Notice()
                    {
                        NoticeDistributionID = noticeDistribution.ID,
                        SenderID = noticeDistribution.UserID,
                        ReceiverID = user.ID,
                        Caption = noticeDistribution.Caption.Replace("%username%", user.Login),
                        Text = noticeDistribution.Text.Replace("%username%", user.Login),
                        Type = (int)Notice.TypeEnum.Simple,
                        IsCloseForRead = noticeDistribution.IsCloseForRead
                    };
                    Repository.CreateNotice(notice);
                }
            }
            return View("Ok");
        }
    }
}
