using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Default.Controllers
{
    public class GroupController : DefaultController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.Groups.Where(p => p.State == (int)Group.StateType.Live).OrderByDescending(p => p.TotalRating).ThenByDescending(p => p.ID);
            var data = new PageableData<Group>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Item(string url)
        {
            var item = Repository.Groups.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (item != null)
            {
                Repository.VisitGroup(item.ID);
                return View(item);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult GroupControlPanel(int id)
        {
            var item = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (item != null)
            {
                return View(item);
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult Create()
        {
            var groupView = new GroupView()
            {
                UserID = CurrentUser.ID
            };

            return View("Edit", groupView);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (group != null)
            {
                if (CurrentUser.IsLeaderOfGroup(group))
                {
                    var groupView = (GroupView)ModelMapper.Map(group, typeof(Group), typeof(GroupView));

                    return View(groupView);
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(GroupView groupView)
        {
            if (groupView.ID != 0)
            {
                var existGroup = Repository.Groups.FirstOrDefault(p => p.ID == groupView.ID);
                if (existGroup == null)
                {
                    return RedirectToNotFoundPage;

                }
                if (!CurrentUser.IsLeaderOfGroup(existGroup))
                {
                    return RedirectToLoginPage;
                }
            }
            if (ModelState.IsValid)
            {
                var group = (Group)ModelMapper.Map(groupView, typeof(GroupView), typeof(Group));
                if (group.ID == 0)
                {
                    Repository.CreateGroup(group);
                    CreateLeader(group);
                }
                else
                {
                    Repository.UpdateGroup(group);
                }
                return RedirectToAction("Group", "User");
            }
            return View(groupView);
        }

        private void CreateLeader(Group group)
        {
            var userGroup = new UserGroup
            {
                UserID = CurrentUser.ID,
                GroupID = group.ID,
                Status = (int)UserGroup.StatusEnum.Granded
            };
            Repository.CreateUserGroup(userGroup);

            var role = Repository.Roles.FirstOrDefault(p => string.Compare(p.Code, "group_leader", true) == 0);

            if (role != null)
            {
                var userRole = new UserRole
                {
                    UserID = CurrentUser.ID,
                    RoleID = role.ID,
                };
                Repository.CreateUserRole(userRole);

                var userRoleGroup = new UserRoleGroup
                {
                    UserRoleID = userRole.ID,
                    GroupID = group.ID
                };
                Repository.CreateUserRoleGroup(userRoleGroup);
            }

            var captainRole = Repository.Roles.FirstOrDefault(p => string.Compare(p.Code, "group_captain", true) == 0);
            if (captainRole != null)
            {
                var userRole = new UserRole
                {
                    UserID = CurrentUser.ID,
                    RoleID = captainRole.ID,
                };
                Repository.CreateUserRole(userRole);

                var userRoleGroup = new UserRoleGroup
                {
                    UserRoleID = userRole.ID,
                    GroupID = group.ID
                };
                Repository.CreateUserRoleGroup(userRoleGroup);
            }
        }

        public ActionResult Roster(string url)
        {
            var group = Repository.Groups.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);

            if (group != null)
            {
                return View(group);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Transfers(string url)
        {
            var group = Repository.Groups.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);

            if (group != null)
            {
                return View(group);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Award(string url, int page = 1)
        {
            var group = Repository.Groups.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (group != null)
            {
                ViewBag.Page = page;
                return View(group);
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult AcceptPlayer(int id)
        {
            var userGroup = Repository.UserGroups.FirstOrDefault(p => p.ID == id);
            if (userGroup != null && CurrentUser.IsLeaderOfGroup(userGroup.Group))
            {
                Repository.AcceptUserGroup(userGroup.ID);
                return RedirectToAction("Roster", new { url = userGroup.Group.Url });
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult RemovePlayer(int id)
        {
            var userGroup = Repository.UserGroups.FirstOrDefault(p => p.ID == id);
            if (userGroup != null && CurrentUser.IsLeaderOfGroup(userGroup.Group))
            {
                var groupUrl = userGroup.Group.Url;
                Repository.RemoveUserGroup(userGroup.ID);
                return RedirectToAction("Roster", new { url = groupUrl });
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult SwitchRole(int id, int roleId)
        {
            var userGroup = Repository.UserGroups.FirstOrDefault(p => p.ID == id);
            if (userGroup != null)
            {
                if (Repository.SwitchGroupRole(userGroup.UserID, userGroup.GroupID, roleId))
                {
                    return Json(new { result = "ok" });
                }
            }
            return Json(new { result = "fail" });
        }

        public ActionResult Users(string url)
        {
            var group = Repository.Groups.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);

            if (group != null)
            {
                if (group.UserID == CurrentUser.ID)
                {
                    return View(group);
                }
            }
            return null;
        }

        [Authorize]
        public ActionResult Leave(int id)
        {
            var userGroup = Repository.UserGroups.FirstOrDefault(p => p.UserID == CurrentUser.ID && p.GroupID == id);

            if (userGroup != null)
            {
                var groupID = userGroup.GroupID;
                Repository.RemoveUserGroup(userGroup.ID);
                return Redirect(Request.UrlReferrer.AbsolutePath);
            }
            return RedirectToNotFoundPage;
        }

        [Authorize]
        public ActionResult Enter(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                var userGroup = new UserGroup()
                {
                    GroupID = group.ID,
                    UserID = CurrentUser.ID,
                    Status = (int)UserGroup.StatusEnum.Asked
                };
                Repository.CreateUserGroup(userGroup);
                return Redirect(Request.UrlReferrer.AbsolutePath);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult InvoiceControlPart(int id)
        {
            var item = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (item != null)
            {
                return View(item);
            }
            return null;
        }

        [HttpGet]
        public ActionResult Notify(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (group != null && CurrentUser.IsLeaderOfGroup(group))
            {
                var noticeDistributionView = new NoticeDistributionView()
                {
                    ForeignID = id,
                    ForeignUrl = group.Url
                };
                return View(noticeDistributionView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Notify(NoticeDistributionView noticeDistributionView)
        {
            if (ModelState.IsValid)
            {
                var group = Repository.Groups.FirstOrDefault(p => p.ID == noticeDistributionView.ForeignID);

                if (group != null && CurrentUser.IsLeaderOfGroup(group))
                {
                    noticeDistributionView._ID = 0;
                    var noticeDistribution = (NoticeDistribution)ModelMapper.Map(noticeDistributionView, typeof(NoticeDistributionView), typeof(NoticeDistribution));

                    noticeDistribution.UserID = CurrentUser.ID;
                    Repository.CreateNoticeDistribution(noticeDistribution);

                    IEnumerable<User> list = group.GrantedUserGroups.Select(p => p.User);

                    foreach (var user in list)
                    {
                        var notice = new Notice()
                        {
                            NoticeDistributionID = noticeDistribution.ID,
                            GroupID = noticeDistributionView.ForeignID,
                            SenderID = noticeDistribution.UserID,
                            ReceiverID = user.ID,
                            Caption = noticeDistribution.Caption.Replace("%username%", user.Login),
                            Text = noticeDistribution.Text.Replace("%username%", user.Login),
                            Type = (int)Notice.TypeEnum.Group,
                            IsCloseForRead = noticeDistribution.IsCloseForRead
                        };
                        Repository.CreateNotice(notice);
                    }
                    return RedirectToAction("Item", new { id = noticeDistributionView.ForeignID });
                }

            }
            return View(noticeDistributionView);
        }

        public ActionResult MoneyList(int id, int page = 1)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                var list = group.MoneyDetails.OrderByDescending(p => p.ID).AsQueryable();
                var data = new PageableData<MoneyDetail>();
                data.Init(list, page, "MoneyList");
                return View(data);
            }
            return null;
        }
    }
}
