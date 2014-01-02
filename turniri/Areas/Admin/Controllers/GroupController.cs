using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;


namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin")]
    public class GroupController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            IQueryable<Group> list = null;
            if (CurrentUser.InRoles("admin"))
            {
                list = Repository.Groups.OrderByDescending(p => p.TotalRating).ThenByDescending(p => p.ID);
            }
            else if (CurrentUser.HasAdminTournament)
            {
                list = CurrentUser.AdminGroups.OrderByDescending(p => p.TotalRating).ThenByDescending(p => p.ID); 
            }
            var data = new PageableData<Group>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Count()
        {
            var count = Repository.Groups.Count(p => p.State == (int)Group.StateType.Registered);
            return View(count);
        }

        public ActionResult Create()
        {
            var groupView = new AdminGroupView();

            return View("Edit", groupView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (group != null)
            {
                var groupView = (AdminGroupView)ModelMapper.Map(group, typeof(Group), typeof(AdminGroupView));
                return View(groupView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(AdminGroupView groupView)
        {
            if (ModelState.IsValid)
            {
                var group = (Group)ModelMapper.Map(groupView, typeof(AdminGroupView), typeof(Group));
                if (group.ID == 0)
                {
                    Repository.CreateGroup(group);
                    return RedirectToAction("Edit", new { id = group.ID });
                }
                else
                {
                    Repository.UpdateGroup(group);
                    return RedirectToAction("Index");
                }
            }
            return View(groupView);
        }

        public ActionResult Delete(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                Repository.RemoveGroup(group.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Purge(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                try
                {
                    Repository.PurgeGroup(group.ID);
                }
                catch
                {
                    TempData["message"] = "Нет возможности уничтожить эту команду, оставьте для истории";
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Restore(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                Repository.AcceptGroup(group.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Active(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                Repository.AcceptGroup(group.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult UserList(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                var groupView = (AdminGroupView)ModelMapper.Map(group, typeof(Group), typeof(AdminGroupView));
                return View(groupView);
            }
            return null;
        }

        public ActionResult AddPlayer(UserGroupView userGroupView)
        {
            if (ModelState.IsValid) 
            {
                var userGroup = (UserGroup)ModelMapper.Map(userGroupView, typeof(UserGroupView), typeof(UserGroup));
                userGroup.Status = (int)UserGroup.StatusEnum.Granded;
                Repository.CreateUserGroup(userGroup);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "fail" });
        }


        public ActionResult DeletePlayer(int id)
        {
            var userGroup = Repository.UserGroups.FirstOrDefault(p => p.ID == id);
            if (userGroup != null)
            {
                Repository.RemoveUserGroup(userGroup.ID);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "fail" });
        }

        public ActionResult AcceptPlayer(int id)
        {
            var userGroup = Repository.UserGroups.FirstOrDefault(p => p.ID == id);
            if (userGroup != null)
            {
                Repository.AcceptUserGroup(userGroup.ID);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "fail" });
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

        [HttpGet]
        public ActionResult ChangeMoney(int id)
        {
            var moneyDetailView = new AdminMoneyDetailView()
            {
                GroupID = id
            };
            return View(moneyDetailView);
        }

        [HttpPost]
        public ActionResult ChangeMoney(AdminMoneyDetailView moneyDetailView)
        {
            if (ModelState.IsValid)
            {
                var moneyDetail = (MoneyDetail)ModelMapper.Map(moneyDetailView, typeof(AdminMoneyDetailView), typeof(MoneyDetail));

                switch ((AdminMoneyDetailView.MoneyType)moneyDetailView.Type)
                {
                    case AdminMoneyDetailView.MoneyType.Gold:
                        moneyDetail.SumGold = moneyDetailView.Sum;
                        break;
                    case AdminMoneyDetailView.MoneyType.Wood:
                        moneyDetail.SumWood = moneyDetailView.Sum;
                        break;
                    case AdminMoneyDetailView.MoneyType.Crystal:
                        moneyDetail.SumCrystal = moneyDetailView.Sum;
                        break;
                }
                MoneyDetail feeMoneyDetail = null;
                MoneyFee moneyFee = null;
                if (moneyDetail.SumGold > 0)
                {
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeAdmin);
                }
                var adminMoney = new MoneyDetail()
                {
                    UserID = CurrentUser.ID,
                    Description = string.Format("Примечание: {0}", moneyDetail.Description),
                    SumGold = -moneyDetail.SumGold,
                    SumWood = -moneyDetail.SumWood,
                    SumCrystal = -moneyDetail.SumCrystal,
                    Submited = true
                };
                if (moneyFee != null)
                {
                    adminMoney.SumGold = -moneyDetail.SumGold * (1 + moneyFee.PercentFee / 100);
                    adminMoney.MoneyFeeID = moneyFee.ID;
                }
                if (moneyFee != null)
                {
                    feeMoneyDetail = new MoneyDetail()
                    {
                        IsFee = true,
                        SumGold = moneyFee.PercentFee / 100 * moneyDetail.SumGold
                    };
                }
                var guid = Repository.CreateTripleMoneyDetail(adminMoney, moneyDetail, feeMoneyDetail);
                Repository.SubmitMoney(guid);
                return View("Ok");
            }
            return View(moneyDetailView);
        }
    }
}