using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using turniri.Global;
using turniri.Model;
using turniri.Models.Info;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Default.Controllers
{
    public class MoneyController : DefaultController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        public ActionResult TournamentUserFee(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null && tournament.MoneyType != (int)Tournament.MoneyTypeEnum.Free)
            {
                var moneyDetail = new MoneyDetail()
                {
                    TournamentID = tournament.ID,
                    Description = "Взнос участия в турнире " + tournament.Name
                };

                if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold && tournament.Fee > 0)
                {
                    moneyDetail.SumGold = tournament.Fee.Value;
                }
                if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Wood && tournament.Fee > 0)
                {
                    moneyDetail.SumWood = tournament.Fee.Value;
                }

                if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold)
                {
                    var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentUserFee);
                    if (moneyFee != null)
                    {
                        moneyDetail.MoneyFee = moneyFee;
                    };
                }
                ViewBag.Action = "TournamentUserFee";
                return View("Money", moneyDetail);
            }
            return Content("OK");
        }

        [HttpPost]
        public ActionResult TournamentUserFee(MoneyDetailView moneyDetailView)
        {
            //To tournament
            var moneyDetail = (MoneyDetail)ModelMapper.Map(moneyDetailView, typeof(MoneyDetailView), typeof(MoneyDetail));
            moneyDetail.ID = 0;
            if (moneyDetail.UserID.HasValue)
            {
                moneyDetail.UserID = 0;
            }

            if (moneyDetail.SumCrystal < 0 || moneyDetail.SumGold < 0 || moneyDetail.SumWood < 0)
            {
                return null;
            }
            //fee 

            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentUserFee);

            //From user
            var userMoneyDetail = new MoneyDetail()
            {
                UserID = CurrentUser.ID,
                Description = moneyDetail.Description,
                SumGold = -moneyDetail.SumGold,
                SumWood = -moneyDetail.SumWood,
                SumCrystal = -moneyDetail.SumCrystal
            };
            moneyDetail.Description = "Взнос от " + CurrentUser.Login;

            if (moneyFee != null)
            {
                userMoneyDetail.MoneyFeeID = moneyFee.ID;
                userMoneyDetail.SumGold = -moneyDetail.SumGold * (1 + moneyFee.PercentFee / 100);
            }

            if (CurrentUser.MoneyGold < -userMoneyDetail.SumGold ||
                CurrentUser.MoneyWood < -userMoneyDetail.SumWood ||
                CurrentUser.MoneyCrystal < -userMoneyDetail.SumCrystal)
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
            MoneyDetail feeMoneyDetail = null;
            if (moneyFee != null && moneyFee.PercentFee > 0)
            {
                feeMoneyDetail = new MoneyDetail()
                {
                    IsFee = true,
                    SumGold = moneyDetail.SumGold * moneyFee.PercentFee / 100,
                };
            }
            var guid = Repository.CreateTripleMoneyDetail(userMoneyDetail, moneyDetail, feeMoneyDetail);
            return Json(new { result = "ok", guid, id = moneyDetail.TournamentID });
        }

        [HttpGet]
        public ActionResult TournamentUserReturn(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null && tournament.MoneyType != (int)Tournament.MoneyTypeEnum.Free)
            {
                var moneyDetail = new MoneyDetail()
                {
                    TournamentID = tournament.ID,
                    Description = "Возврат взноса турнира " + tournament.Name
                };

                if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold && tournament.Fee > 0)
                {
                    moneyDetail.SumGold = tournament.Fee.Value;
                }
                if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Wood && tournament.Fee > 0)
                {
                    moneyDetail.SumWood = tournament.Fee.Value;
                }
                if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold)
                {
                    var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentUserReturn);
                    if (moneyFee != null)
                    {
                        moneyDetail.MoneyFee = moneyFee;
                    };
                }
                ViewBag.Action = "TournamentUserReturn";
                return View("MoneyReturn", moneyDetail);
            }
            return Content("OK");


        }

        [HttpPost]
        public ActionResult TournamentUserReturn(MoneyDetailView moneyDetailView)
        {
            //To tournament
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == moneyDetailView.TournamentID);
            if (tournament != null)
            {
                var participant = tournament.Participants.FirstOrDefault(p => p.UserID == CurrentUser.ID);
                if (participant != null)
                {
                    var moneyDetail = new MoneyDetail()
                    {
                        TournamentID = tournament.ID,
                        Description = "Возврат взноса в турнире " + tournament.Name
                    };

                    if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold && tournament.Fee > 0)
                    {
                        moneyDetail.SumGold = -tournament.Fee.Value;
                    }
                    if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Wood && tournament.Fee > 0)
                    {
                        moneyDetail.SumWood = -tournament.Fee.Value;
                    }

                    var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentUserReturn);

                    //To User
                    var userMoneyDetail = new MoneyDetail()
                    {
                        UserID = CurrentUser.ID,
                        Description = moneyDetail.Description,
                        SumGold = -moneyDetail.SumGold,
                        SumWood = -moneyDetail.SumWood,
                        SumCrystal = -moneyDetail.SumCrystal
                    };
                    moneyDetail.Description = "Возврат взноса " + CurrentUser.Login;
                    MoneyDetail feeMoneyDetail = null;
                    if (moneyFee != null)
                    {
                        userMoneyDetail.MoneyFeeID = moneyFee.ID;
                        userMoneyDetail.SumGold = -moneyDetail.SumGold * (1 - moneyFee.PercentFee / 100);

                        feeMoneyDetail = new MoneyDetail()
                        {
                            IsFee = true,
                            SumGold = -moneyDetail.SumGold * moneyFee.PercentFee / 100,
                        };
                    };
                    var guid = Repository.CreateTripleMoneyDetail(moneyDetail, userMoneyDetail, feeMoneyDetail);
                    return Json(new { result = "ok", guid, id = moneyDetail.TournamentID });
                }
                return Json(new { result = "error", error = "Вы не участник турнира" });
            }
            return Json(new { result = "error", error = "Турнир не найден" });
        }

        [HttpGet]
        public ActionResult TournamentGroupReturn(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null && tournament.MoneyType != (int)Tournament.MoneyTypeEnum.Free)
            {
                var participant = tournament.Participants.FirstOrDefault(p => p.UserID == CurrentUser.ID);
                if (participant != null)
                {
                    var group = participant.Group;
                    if (group != null)
                    {
                        var moneyDetail = new MoneyDetail()
                        {
                            TournamentID = tournament.ID,
                            GroupID = group.ID,
                            Description = "Возврат взноса турнира " + tournament.Name
                        };

                        if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold && tournament.Fee > 0)
                        {
                            moneyDetail.SumGold = tournament.Fee.Value;
                        }
                        if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Wood && tournament.Fee > 0)
                        {
                            moneyDetail.SumWood = tournament.Fee.Value;
                        }
                        if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold)
                        {
                            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentUserReturn);
                            if (moneyFee != null)
                            {
                                moneyDetail.MoneyFee = moneyFee;
                            };
                        }
                        ViewBag.Action = "TournamentGroupReturn";
                        return View("MoneyReturn", moneyDetail);
                    }
                }
            }
            return Content("OK");
        }

        [HttpPost]
        public ActionResult TournamentGroupReturn(MoneyDetailView moneyDetailView)
        {
            //To tournament
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == moneyDetailView.TournamentID);
            if (tournament != null)
            {
                var participant = tournament.Participants.FirstOrDefault(p => p.UserID == CurrentUser.ID);
                if (participant != null)
                {
                    var group = participant.Group;
                    var moneyDetail = new MoneyDetail()
                    {
                        TournamentID = tournament.ID,
                        Description = "Возврат взноса в турнире " + tournament.Name
                    };

                    if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold && tournament.Fee > 0)
                    {
                        moneyDetail.SumGold = -tournament.Fee.Value;
                    }
                    if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Wood && tournament.Fee > 0)
                    {
                        moneyDetail.SumWood = -tournament.Fee.Value;
                    }
                    var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentUserReturn);

                    var returnMoneyDetail = new MoneyDetail()
                    {

                        Description = moneyDetail.Description,
                        SumGold = -moneyDetail.SumGold,
                        SumWood = -moneyDetail.SumWood,
                        SumCrystal = -moneyDetail.SumCrystal
                    };

                    //To Group
                    if (group != null)
                    {
                        returnMoneyDetail.GroupID = group.ID;
                        moneyDetail.Description = "Возврат в группу " + group.Name;
                    }
                    else
                    {
                        returnMoneyDetail.UserID = CurrentUser.ID;
                        moneyDetail.Description = "Возврат игроку " + CurrentUser.Login;
                    }
                    MoneyDetail feeMoneyDetail = null;
                    if (moneyFee != null)
                    {
                        returnMoneyDetail.MoneyFeeID = moneyFee.ID;
                        returnMoneyDetail.SumGold = -moneyDetail.SumGold * (1 - moneyFee.PercentFee / 100);

                        feeMoneyDetail = new MoneyDetail()
                        {
                            IsFee = true,
                            SumGold = -moneyDetail.SumGold * moneyFee.PercentFee / 100,
                        };
                    };
                    var guid = Repository.CreateTripleMoneyDetail(returnMoneyDetail, moneyDetail, feeMoneyDetail);
                    return Json(new { result = "ok", guid, id = moneyDetail.TournamentID });
                }
                return Json(new { result = "error", error = "Вы не участник турнира" });
            }
            return Json(new { result = "error", error = "Турнир не найден" });
        }

        [HttpGet]
        public ActionResult UserToUser(int id)
        {
            ViewBag.Action = "UserToUser";
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var moneyDetail = new AdminMoneyDetailView()
                {
                    UserID = user.ID,
                    Description = "Послать ТИ " + user.Login
                };
                var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.UserToUser);

                if (moneyFee != null)
                {
                    moneyDetail.PercentFee = moneyFee.PercentFee;
                }
                return View("SendMoney", moneyDetail);
            }
            return Content("OK");
        }

        [HttpPost]
        public ActionResult UserToUser(AdminMoneyDetailView moneyDetailView)
        {
            ViewBag.Action = "UserToUser";
            //To user 
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

            //From user
            var userMoneyDetail = new MoneyDetail()
            {
                UserID = CurrentUser.ID,
                Description = moneyDetail.Description,
                SumGold = -moneyDetail.SumGold,
                SumWood = -moneyDetail.SumWood,
                SumCrystal = -moneyDetail.SumCrystal
            };

            moneyDetail.Description = "Переслано от " + CurrentUser.Login;

            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.UserToUser);
            if (moneyFee != null)
            {
                userMoneyDetail.MoneyFeeID = moneyFee.ID;
                userMoneyDetail.SumGold = -moneyDetail.SumGold * (1 + moneyFee.PercentFee / 100);
            }

            if (CurrentUser.MoneyGold < -userMoneyDetail.SumGold ||
             CurrentUser.MoneyWood < -userMoneyDetail.SumWood ||
             CurrentUser.MoneyCrystal < -userMoneyDetail.SumCrystal)
            {
                return View("SendMoney", moneyDetailView);
            }
            MoneyDetail feeMoneyDetail = null;
            if (moneyFee != null && moneyFee.PercentFee > 0)
            {
                feeMoneyDetail = new MoneyDetail()
                {
                    IsFee = true,
                    SumGold = moneyDetail.SumGold * moneyFee.PercentFee / 100,
                };
            }
            var guid = Repository.CreateTripleMoneyDetail(userMoneyDetail, moneyDetail, feeMoneyDetail);
            Repository.SubmitMoney(guid);
            return View("_OK");
        }

        [HttpGet]
        public ActionResult UserToGroup(int id)
        {
            ViewBag.Action = "UserToGroup";
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                var moneyDetail = new AdminMoneyDetailView()
                {
                    GroupID = group.ID,
                    Description = "Переслать в группу " + group.Name
                };
                var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.UserToGroup);

                if (moneyFee != null)
                {
                    moneyDetail.PercentFee = moneyFee.PercentFee;
                }
                return View("SendMoney", moneyDetail);
            }
            return Content("OK");
        }

        [HttpPost]
        public ActionResult UserToGroup(AdminMoneyDetailView moneyDetailView)
        {
            ViewBag.Action = "UserToGroup";
            //To user 
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


            //From user
            var userMoneyDetail = new MoneyDetail()
            {
                UserID = CurrentUser.ID,
                Description = moneyDetail.Description,
                SumGold = -moneyDetail.SumGold,
                SumWood = -moneyDetail.SumWood,
                SumCrystal = -moneyDetail.SumCrystal
            };

            moneyDetail.Description = "Переслано от " + CurrentUser.Login;

            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.UserToGroup);
            if (moneyFee != null && moneyFee.PercentFee > 0)
            {
                userMoneyDetail.MoneyFeeID = moneyFee.ID;
                userMoneyDetail.SumGold = -moneyDetail.SumGold * (1 + moneyFee.PercentFee / 100);
            }

            if (CurrentUser.MoneyGold < -userMoneyDetail.SumGold ||
               CurrentUser.MoneyWood < -userMoneyDetail.SumWood ||
               CurrentUser.MoneyCrystal < -userMoneyDetail.SumCrystal)
            {
                return View("SendMoney", moneyDetailView);
            }
            MoneyDetail feeMoneyDetail = null;
            if (moneyFee != null && moneyFee.PercentFee > 0)
            {
                feeMoneyDetail = new MoneyDetail()
                {
                    IsFee = true,
                    SumGold = moneyDetail.SumGold * moneyFee.PercentFee / 100,
                };

            }
            var guid = Repository.CreateTripleMoneyDetail(userMoneyDetail, moneyDetail, feeMoneyDetail);
            Repository.SubmitMoney(guid);
            return View("_OK");
        }

        [HttpGet]
        public ActionResult GroupToUser(int id)
        {
            ViewBag.Action = "GroupToUser";
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                var moneyDetail = new GroupMoneyDetailView()
                {
                    GroupID = group.ID,
                    Description = "Распределено из группы " + group.Name,
                    MaxGold = group.MoneyGold,
                    MaxCrystal = group.MoneyCrystal,
                    MaxWood = group.MoneyWood,
                };
                var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.GroupToUser);

                if (moneyFee != null)
                {
                    moneyDetail.PercentFee = moneyFee.PercentFee;
                }
                return View("SendMoneyGroup", moneyDetail);
            }
            return Content("OK");
        }

        [HttpPost]
        public ActionResult GroupToUser(GroupMoneyDetailView moneyDetailView)
        {
            ViewBag.Action = "GroupToUser";
            var group = Repository.Groups.FirstOrDefault(p => p.ID == moneyDetailView.GroupID);
            var user = Repository.Users.FirstOrDefault(p => p.ID == moneyDetailView.UserID);
            if (group != null && user != null)
            {
                if (CurrentUser.IsLeaderOfGroup(group))
                {
                    //From group
                    var moneyDetail = (MoneyDetail)ModelMapper.Map(moneyDetailView, typeof(AdminMoneyDetailView), typeof(MoneyDetail));

                    switch ((GroupMoneyDetailView.MoneyType)moneyDetailView.Type)
                    {
                        case GroupMoneyDetailView.MoneyType.Gold:
                            moneyDetail.SumGold = -moneyDetailView.Sum;
                            break;
                        case GroupMoneyDetailView.MoneyType.Wood:
                            moneyDetail.SumWood = -moneyDetailView.Sum;
                            break;
                        case GroupMoneyDetailView.MoneyType.Crystal:
                            moneyDetail.SumCrystal = -moneyDetailView.Sum;
                            break;
                    }


                    //To user
                    var userMoneyDetail = new MoneyDetail()
                    {
                        UserID = moneyDetail.UserID,
                        Description = moneyDetail.Description,
                        SumGold = -moneyDetail.SumGold,
                        SumWood = -moneyDetail.SumWood,
                        SumCrystal = -moneyDetail.SumCrystal
                    };
                    moneyDetail.UserID = null;
                    moneyDetail.Description = "Переслано участнику " + user.Login;

                    var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.GroupToUser);
                    if (moneyFee != null && moneyFee.PercentFee > 0)
                    {
                        moneyDetail.MoneyFeeID = moneyFee.ID;
                        moneyDetail.SumGold = moneyDetail.SumGold * (1 + moneyFee.PercentFee / 100);
                    }

                    if (group.MoneyGold < moneyDetail.SumGold ||
                      group.MoneyWood < moneyDetail.SumWood ||
                      group.MoneyCrystal < moneyDetail.SumCrystal)
                    {
                        return View("SendMoneyGroup", moneyDetailView);
                    }
                    MoneyDetail feeMoneyDetail = null;
                    if (moneyFee != null && moneyFee.PercentFee > 0)
                    {
                        feeMoneyDetail = new MoneyDetail()
                         {
                             IsFee = true,
                             SumGold = userMoneyDetail.SumGold * moneyFee.PercentFee / 100,
                         };
                    }
                    var guid = Repository.CreateTripleMoneyDetail(moneyDetail, userMoneyDetail, feeMoneyDetail);
                    Repository.SubmitMoney(guid);
                    return View("_OK");
                }
                return RedirectToLoginPage;
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult GetChargePercent(int type)
        {
            MoneyFee moneyFee = null;
            switch ((Recharge.ProviderType)type)
            {
                case Recharge.ProviderType.Yandex:
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeYandex);
                    if (moneyFee != null)
                    {
                        return Json(new { result = "ok", percent = moneyFee.PercentFee });
                    }
                    break;
                case Recharge.ProviderType.Webmoney:
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeWebMoney);
                    if (moneyFee != null)
                    {
                        return Json(new { result = "ok", percent = moneyFee.PercentFee });
                    }
                    break;
                case Recharge.ProviderType.Robokassa:
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeRobokassa);
                    if (moneyFee != null)
                    {
                        return Json(new { result = "ok", percent = moneyFee.PercentFee });
                    }
                    break;
                case Recharge.ProviderType.Qiwi:
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeQiwi);
                    if (moneyFee != null)
                    {
                        return Json(new { result = "ok", percent = moneyFee.PercentFee });
                    }
                    break;
            }

            return Json(new { result = "ok", percent = 0 });
        }

        public ActionResult GetWithdrawPercent(int type)
        {
            MoneyFee moneyFee = null;
            switch ((MoneyWithdraw.ProviderType)type)
            {
                case MoneyWithdraw.ProviderType.Yandex:
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.WithdrawYandex);
                    if (moneyFee != null)
                    {
                        return Json(new { result = "ok", percent = moneyFee.PercentFee });
                    }
                    break;
                case MoneyWithdraw.ProviderType.Webmoney:
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.WithdrawWebMoney);
                    if (moneyFee != null)
                    {
                        return Json(new { result = "ok", percent = moneyFee.PercentFee });
                    }
                    break;
                case MoneyWithdraw.ProviderType.Robokassa:
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.WithdrawRobokassa);
                    if (moneyFee != null)
                    {
                        return Json(new { result = "ok", percent = moneyFee.PercentFee });
                    }
                    break;
                case MoneyWithdraw.ProviderType.Qiwi:
                    moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.WithdrawQiwi);
                    if (moneyFee != null)
                    {
                        return Json(new { result = "ok", percent = moneyFee.PercentFee });
                    }
                    break;
            }

            return Json(new { result = "ok", percent = 0 });
        }

        public ActionResult AcceptYandexMoney(YandexAcceptMoney money)
        {
            MoneyNotify moneyNotify = null;
            try
            {
                var info = string.Format("AcceptYandexMoney notification_type: {0}\n operation_id :{1} \n amount: {2}\n currency : {3}\n datetime: {4}\n sender: {5}\n codepro: {6}\n label: {7}\n sha1_hash: {8}", money.notification_type, money.operation_id, money.amount,
                    money.currency,
                    money.datetime,
                    money.sender,
                    money.codepro,
                    money.label,
                    money.sha1_hash);

                moneyNotify = new MoneyNotify()
                {
                    Data = info,
                    Type = (int)MoneyNotify.TypeEnum.Yandex,
                    AddedDate = DateTime.Now
                };

                Repository.CreateMoneyNotify(moneyNotify);
                var strToCode = string.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}&{8}", money.notification_type,
                    money.operation_id, money.amount, money.currency, money.datetime, money.sender, money.codepro,
                    Config.YandexSecret, money.label);
                var shaEncode = getSHA1(strToCode, Encoding.Default).ToLower();
                if (string.Compare(shaEncode, money.sha1_hash, true) == 0)
                {
                    int id = 0;
                    if (Int32.TryParse(money.label, out id))
                    {
                        var recharge = Repository.Recharges.FirstOrDefault(p => p.ID == id && !p.IsSubmitted);

                        if (recharge.CartID != null)
                        {
                            var cart = Repository.Carts.FirstOrDefault(p => p.ID == recharge.CartID);
                            if (cart != null)
                            {
                                ProcessCart(cart);
                            }
                            recharge.IsSubmitted = true;
                            recharge.AdditionalInfo = info;
                        }
                        else
                        {
                            var moneyDetail = new MoneyDetail()
                            {
                                UserID = recharge.UserID,
                                CartID = recharge.CartID,
                                Description = recharge.Description,
                                SumGold = recharge.Sum
                            };
                            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeYandex);
                            if (moneyFee != null)
                            {
                                moneyDetail.MoneyFeeID = moneyFee.ID;
                                moneyDetail.SumGold = moneyDetail.SumGold * (1 - moneyFee.PercentFee / 100);
                            }
                            var guid = Guid.NewGuid();
                            Repository.CreateMoneyDetail(moneyDetail, guid);

                            recharge.MoneyDetailID = moneyDetail.ID;
                            recharge.IsSubmitted = true;
                            recharge.AdditionalInfo = info;
                            Repository.SubmitMoney(guid);
                        }
                        Repository.UpdateRecharge(recharge);
                        moneyNotify.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);
                moneyNotify.IsSuccess = false;
                try
                {
                    var serialize = new XmlSerializer(typeof(ErrorSerialize));
                    var stringWriter = new StringWriter();
                    serialize.Serialize(stringWriter, new ErrorSerialize(ex));
                    stringWriter.Flush();
                    moneyNotify.Exception = stringWriter.ToString();
                }
                catch
                {
                    moneyNotify.Exception = "Too Short: " + ex.Message;
                }
            }
            finally
            {
                if (moneyNotify != null)
                {
                    Repository.UpdateMoneyNotify(moneyNotify);
                }
            }
            return Content("OK");
        }

        public static string getSHA1(string userPassword, Encoding encoding)
        {
            return ConvertStringToHex(new SHA1Managed().ComputeHash(encoding.GetBytes(userPassword)));
        }

        public static string ConvertStringToHex(byte[] bytes)
        {
            StringBuilder sbBytes = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        [HttpPost]
        public bool QiwiRecordInvoiceSum(int transactionId, string sum)
        {
            try
            {
                var intsum = 0;
                Int32.TryParse(sum, out intsum);
                if (intsum > 15000)
                {
                    return false;
                }
                var recharge = Repository.Recharges.FirstOrDefault(p => p.ID == transactionId);
                if (recharge == null)
                {
                    return false;
                }
                recharge.Sum = intsum;
                Repository.UpdateRecharge(recharge);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ActionResult AcceptQiwiMoney(string order)
        {
            ViewBag.Id = order;
            return View();
        }

        public ActionResult FailQiwiMoney()
        {
            return View();
        }
    }
}
