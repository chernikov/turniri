using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Models.ViewModels.User;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin,game_moderator,tournament_moderator,editor")]
    public class UserController : AdminController
    {
        [Authorize(Roles = "admin")]
        public ActionResult Index(int page = 1, string search = null, bool all = false)
        {
            var list = Repository.Users;

            if (search != null)
            {
                list = SearchEngine.Search(search, list, all).AsQueryable();
            }

            var data = new PageableData<User>();
            data.Init(list, page, "Index");
            return View(data);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Activate(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                if (!user.ActivatedDate.HasValue)
                {
                    Repository.ActivateUser(user);
                    var moneyDetail = new MoneyDetail()
                    {
                        SumWood = Config.FreeCharge * 5,
                        UserID = user.ID,
                        Description = "Подарок при активации",
                        Submited = true
                    };
                    Repository.CreateMoneyDetail(moneyDetail, Guid.NewGuid());
                }
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        [Authorize(Roles = "admin")]
        public ActionResult BanUser(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Repository.BanUser(user, true);
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        [Authorize(Roles = "admin")]
        public ActionResult UnBanUser(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Repository.BanUser(user, false);
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Login(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Auth.Login(user.Login);
            }
            return RedirectToAction("Index", "Home", new { area = "Default" });
        }

        public ActionResult SelectUser(string term)
        {
            var list = Repository.RegularUsers;

            var searchList = SearchEngine.Search(term, list);

            return Json(new
            {
                result = "ok",
                data = searchList.Select(p => new
                {
                    id = p.ID,
                    login = p.Login
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectPlayer(int GameID, int GroupTeam, string term)
        {
            var list = Repository.RegularUsers;//.Where(p => p.UserGames.Any(r => r.GameID == GameID));
            switch (GroupTeam)
            {
                case 1:
                    list = list.Where(p => p.UserGroups.Any(r => r.Status == (int)UserGroup.StatusEnum.Granded && r.Group.GameID == GameID));
                    break;
                case 3:
                    list = list.Where(p => p.UserGroups.Any(r => r.Status == (int)UserGroup.StatusEnum.Granded && r.Group.GameID == GameID) &&
                        p.UserRoles.Any(r => string.Compare(r.Role.Code, "group_captain", true) == 0
                            && r.UserRoleGroups.Any(s => s.Group.GameID == GameID)));
                    break;
            }

            IEnumerable<User> searchList = null;
            if (GroupTeam > 1 /*2, 3*/)
            {
                searchList = SearchEngine.SearchWithGroup(term, list, GameID);
            }
            else
            {
                searchList = SearchEngine.Search(term, list);
            }

            if (GroupTeam % 2 == 1)
            {
                return Json(new
                {
                    result = "ok",
                    data = searchList.Select(p => new
                    {
                        id = p.ID,
                        login = string.Format("{0} ({1})", p.Login, p.GroupByGame(GameID) != null ? p.GroupByGame(GameID).Name : "")
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = "ok",
                    data = searchList.Select(p => new
                    {
                        id = p.ID,
                        login = p.Login
                    })
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult ChangeMoney(int id)
        {
            var moneyDetailView = new AdminMoneyDetailView()
            {
                UserID = id
            };
            return View(moneyDetailView);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
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
                if (moneyDetail.UserID != CurrentUser.ID)
                {
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
                }
                else
                {
                    feeMoneyDetail = new MoneyDetail()
                    {
                      
                        IsFee = true,
                        Description = string.Format("Выдача себе: {0}", moneyDetail.Description),
                        SumGold = -moneyDetail.SumGold,
                    };
                    var guid = Repository.CreateTripleMoneyDetail(feeMoneyDetail, moneyDetail);
                    Repository.SubmitMoney(guid);
                }
             
                return View("Ok");
            }
            return View(moneyDetailView);
        }
    }
}
