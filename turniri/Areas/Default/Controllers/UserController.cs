using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Helpers;
using turniri.Model;
using turniri.Models.Info;
using turniri.Models.ViewModels;
using turniri.Models.ViewModels.User;
using turniri.Tools;
using turniri.Tools.Mail;

namespace turniri.Areas.Default.Controllers
{
    public class UserController : DefaultController
    {
        private static string AvatarFolder = "/Media/files/avatars/";

        private static string Avatar173Size = "Avatar173Size";
        private static string Avatar96Size = "Avatar96Size";
        private static string Avatar84Size = "Avatar84Size";
        private static string Avatar57Size = "Avatar57Size";
        private static string Avatar30Size = "Avatar30Size";
        private static string Avatar26Size = "Avatar26Size";
        private static string Avatar18Size = "Avatar18Size";

        public ActionResult Index(string login = null, int? matchID = null)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                if (CurrentUser != null)
                {
                    Repository.UserVisitCount(CurrentUser.ID);
                    ViewBag.MatchID = matchID;
                    return View(CurrentUser);
                }
                return RedirectToLoginPage;
            }
            var user = Repository.Users.FirstOrDefault(p => string.Compare(p.Login, login, true) == 0);
            if (user != null)
            {
                Repository.UserVisitCount(user.ID);
                return View(user);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            if (CurrentUser != null)
            {
                var userView = (UserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(UserView));
                return View(userView);
            }
            return RedirectToLoginPage;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(UserView userView)
        {
            if (CurrentUser.ID == userView.ID)
            {
                if (ModelState.IsValid)
                {
                    var user = (User)ModelMapper.Map(userView, typeof(UserView), typeof(User));
                    Repository.UpdateUser(user);

                    return RedirectToAction("Index");
                }
                return View(userView);
            }
            return RedirectToLoginPage;
        }

        [HttpGet]
        public ActionResult Register()
        {
            var registerUserView = new RegisterUserView();
            return View(registerUserView);
        }

        [HttpPost]
        public ActionResult Register(RegisterUserView registerUserView)
        {
            if (Session[CaptchaImage.CaptchaValueKey] as string != registerUserView.Captcha)
            {
                ModelState.AddModelError("Captcha", "Код введен неверно");
            }
            if (!registerUserView.Agreement)
            {
                ModelState.AddModelError("Agreement", "Подтвердите согласие с правилами сайта");
            }
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(registerUserView, typeof(RegisterUserView), typeof(User));
                Repository.CreateUser(user);

                NotifyMail.SendNotify(Config, "Register", user.Email,
                                      (u, format) => string.Format(format, HostName),
                                      (u, format) => string.Format(format, u.ActivatedLink, HostName),
                                      user);
                return RedirectToAction("RegisterSuccess");
            }
            return View(registerUserView);
        }

        public ActionResult ResendActivation()
        {
            NotifyMail.SendNotify(Config, "Register", CurrentUser.Email,
                                  (u, format) => string.Format(format, HostName),
                                  (u, format) => string.Format(format, u.ActivatedLink, HostName),
                                  CurrentUser);
            return RedirectToAction("RegisterSuccess");
        }

        [HttpGet]
        public ActionResult RegisterSuccess()
        {
            var userCode = new UserCode();
            return View(userCode);
        }

        [HttpPost]
        public ActionResult RegisterSuccess(UserCode userCode)
        {
            var user = Repository.Users.FirstOrDefault(p => string.Compare(p.ActivatedLink, userCode.Code, true) == 0);
            if (user == null)
            {
                ModelState.AddModelError("Code", "Код введен неверно");
            }

            if (ModelState.IsValid)
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
                return View("ActivateSuccess");
            }
            return View(userCode);
        }


        public ActionResult Captcha()
        {
            Session[CaptchaImage.CaptchaValueKey] = new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString();
            // Create a CAPTCHA image using the text stored in the Session object.
            var ci = new CaptchaImage(Session[CaptchaImage.CaptchaValueKey].ToString(), 211, 50, "Arial");

            // Change the response headers to output a JPEG image.
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.
            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            ci.Dispose();
            return null;
        }

        public ActionResult Activate(string id)
        {
            var user = Repository.Users.FirstOrDefault(p => string.Compare(p.ActivatedLink, id, true) == 0);
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
                Auth.Login(user.Email);
                return View("ActivateSuccess");
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult ResendVerifyEmail()
        {
            NotifyMail.SendNotify(Config, "VerifyEmail", CurrentUser.Email,
                                      (u, format) => string.Format(format, HostName),
                                      (u, format) => string.Format(format, u.ActivatedLink, HostName),
                                      CurrentUser);
            return View();
        }

        public ActionResult VerifyEmail(string id)
        {
            var user = Repository.Users.FirstOrDefault(p => string.Compare(p.ActivatedLink, id, true) == 0);
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
                return View("VerifyEmail");
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public JsonResult UploadAvatar(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && PreviewCreator.SupportMimeType(mimeType.Name))
                    {
                        var ms = GetMemoryStream(inputStream);
                        var avatar173Url = MakeAvatar(ms, Avatar173Size);
                        var avatar96Url = MakeAvatar(ms, Avatar96Size);
                        var avatar84Url = MakeAvatar(ms, Avatar84Size);
                        var avatar57Url = MakeAvatar(ms, Avatar57Size);
                        var avatar30Url = MakeAvatar(ms, Avatar30Size);
                        var avatar26Url = MakeAvatar(ms, Avatar26Size);
                        var avatar18Url = MakeAvatar(ms, Avatar18Size);
                        return Json(new
                                        {
                                            success = true,
                                            result = "ok",
                                            data = new
                                                       {
                                                           Avatar173Url = avatar173Url,
                                                           Avatar96Url = avatar96Url,
                                                           Avatar84Url = avatar84Url,
                                                           Avatar57Url = avatar57Url,
                                                           Avatar30Url = avatar30Url,
                                                           Avatar26Url = avatar26Url,
                                                           Avatar18Url = avatar18Url
                                                       }
                                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        private static MemoryStream GetMemoryStream(Stream inputStream)
        {
            var buffer = new byte[inputStream.Length];
            var ms = new MemoryStream(buffer);
            inputStream.CopyTo(ms);
            return ms;
        }

        private string MakeAvatar(MemoryStream ms, string avatarSize)
        {
            var avatarUrl = string.Format("{0}{1}.jpg", AvatarFolder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSavePreview(ms, previewSize, Server.MapPath(avatarUrl));
            }
            return avatarUrl;
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            var changePasswordView = new ChangePasswordView
                                         {
                                             ID = CurrentUser.ID
                                         };
            return View(changePasswordView);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePasswordAjax(ChangePasswordView changePasswordView)
        {
            if (ModelState.IsValid)
            {
                CurrentUser.Password = changePasswordView.NewPassword;
                Repository.ChangePassword(CurrentUser);
                TempData["message"] = "Сохранено!";
                changePasswordView = new ChangePasswordView();
            }
            return View("ChangePassword", changePasswordView);
        }

        public ActionResult Games(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                return View(user);
            }
            return null;
        }

        public ActionResult PlatformGames(int id)
        {
            var platform = Repository.Platforms.FirstOrDefault(p => p.ID == id);
            if (platform != null)
            {
                return View(platform.Games.ToList());
            }
            return null;
        }

        public ActionResult PlayGame(int id)
        {
            var userGame = new UserGame
                               {
                                   GameID = id,
                                   UserID = CurrentUser.ID
                               };
            Repository.CreateUserGame(userGame);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StopGame(int id)
        {
            var userGame = Repository.UserGames.FirstOrDefault(p => p.GameID == id && p.UserID == CurrentUser.ID);
            if (userGame != null)
            {
                if (!userGame.User.AllMatches.Any(p => p.GameID == id && p.TournamentID == null && (p.Status != (int)Match.MatchStatusEnum.Submit || p.Status != (int)Match.MatchStatusEnum.Closed))
                    && !userGame.User.Participants.Any(p => (p.TournamentID != null && p.Tournament.GameID == id && (
                        p.Tournament.Status != (int)Tournament.StatusEnum.Closed ||
                        p.Tournament.Status != (int)Tournament.StatusEnum.PlayedOut))
                        || p.Matches1.Any(m => m.GameID == id) || p.Matches.Any(m => m.GameID == id))
            && !userGame.User.GroupsWhereImMember.Any(p => p.GameID == id))
                {
                    Repository.RemoveUserGame(userGame.ID);
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Reputation(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                return View(user);
            }
            return null;
        }

        public ActionResult TournamentReputation(int id, int userID)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            var senderReputation = Repository.Users.FirstOrDefault(p => p.ID == userID);
            if (user != null)
            {
                ViewBag.User = senderReputation;
                return View(user);
            }
            return null;
        }

        [Authorize]
        public ActionResult VoteReputation(int id, int type, int mark)
        {
            Repository.SetReputation(CurrentUser.ID, id, type, mark);
            return Json(new
            {
                result = "ok"
            });
        }

        [Authorize]
        public ActionResult VoteGrade(int id, int grade)
        {
            Repository.SetGrade(CurrentUser.ID, id, grade);
            return Json(new
            {
                result = "ok"
            });
        }

        [Authorize]
        public ActionResult Matches()
        {
            return View(CurrentUser);
        }

        [Authorize]
        public ActionResult Tournaments()
        {
            return View(CurrentUser);
        }

        public ActionResult Group(string login = null)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                if (CurrentUser != null)
                {

                    return View(CurrentUser);
                }
                return RedirectToLoginPage;
            }
            var user = Repository.Users.FirstOrDefault(p => string.Compare(p.Login, login, true) == 0);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToLoginPage;
        }

        public ActionResult Money()
        {
            return View(CurrentUser);
        }

        public ActionResult MoneyList(int page = 1)
        {
            var list = CurrentUser.MoneyDetails.OrderByDescending(p => p.ID).AsQueryable();
            var data = new PageableData<MoneyDetail>();
            data.Init(list, page, "MoneyList");
            return View(data);
        }

        [HttpGet]
        public ActionResult MoneyRecharge()
        {
            var rechargeView = new RechargeView();
            return View(rechargeView);
        }

        [HttpPost]
        public ActionResult MoneyRecharge(RechargeView rechargeView)
        {
            if (ModelState.IsValid)
            {
                var recharge = (Recharge)ModelMapper.Map(rechargeView, typeof(RechargeView), typeof(Recharge));

                recharge.UserID = CurrentUser.ID;
                recharge.Description = string.Format("Пополнение золотых ТИ ({0})", (Recharge.ProviderType)recharge.Provider);
                Repository.CreateRecharge(recharge);

                switch ((Recharge.ProviderType)recharge.Provider)
                {
                    case Recharge.ProviderType.Yandex:
                        var yandexRechargeInfo = new YandexRechargeInfo()
                        {
                            Label = recharge.ID,
                            ShortDest = "Пополнение золотых ТИ",
                            WritableTargets = false,
                            CommentNeeded = false,
                            QuickpayForm = "shop",
                            Targets = "Покупка золотых Ти на turniri.ru",
                            Comment = "",
                            Sum = recharge.Sum,
                            Receiver = Config.YandexWallet
                        };
                        var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeYandex);

                        if (moneyFee != null)
                        {
                            yandexRechargeInfo.ChargeSum = yandexRechargeInfo.Sum * (1 - moneyFee.PercentFee / 100);
                        }
                        else
                        {
                            yandexRechargeInfo.ChargeSum = yandexRechargeInfo.Sum;
                        }
                        return View("Yandex", yandexRechargeInfo);
                    case Recharge.ProviderType.Qiwi:
                        var qiwiRechargeInfo = new QiwiRechargeInfo(Config)
                        {
                            Label = recharge.ID,
                            Description = "Покупка золотых Ти на turniri.ru",
                            To = !string.IsNullOrWhiteSpace(CurrentUser.Phone) ? CurrentUser.Phone.ClearPhone().Substring(1) : "",
                            Sum = recharge.Sum,
                        };
                        var qiwiMoneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.ChargeQiwi);

                        if (qiwiMoneyFee != null)
                        {
                            qiwiRechargeInfo.ChargeSum = qiwiRechargeInfo.Sum * (1 - qiwiMoneyFee.PercentFee / 100);
                        }
                        else
                        {
                            qiwiRechargeInfo.ChargeSum = qiwiRechargeInfo.Sum;
                        }
                        return View("Qiwi", qiwiRechargeInfo);
                }
            }
            return View(rechargeView);
        }

        [HttpGet]
        public ActionResult MoneyWithdraw()
        {
            var moneyWithdrawView = new MoneyWithdrawView()
            {
                Sum = Config.MinWithdraw
            };
            return View(moneyWithdrawView);
        }

        [HttpPost]
        public ActionResult MoneyWithdraw(MoneyWithdrawView moneyWithdrawView)
        {
            if (moneyWithdrawView.Sum > CurrentUser.MoneyGold)
            {
                ModelState.AddModelError("Common", "У вас нет столько золотых ТИ");
            }
            else if (moneyWithdrawView.Sum < Config.MinWithdraw)
            {
                ModelState.AddModelError("Common", string.Format("Минимальное количество для вывода {0} золотых ТИ", Config.MinWithdraw));
            }
            if (ModelState.IsValid)
            {
                var moneyWithdraw = (MoneyWithdraw)ModelMapper.Map(moneyWithdrawView, typeof(MoneyWithdrawView), typeof(MoneyWithdraw));

                var moneyDetail = new MoneyDetail()
                {
                    UserID = CurrentUser.ID,
                    SumGold = -moneyWithdrawView.Sum,
                    Description = "Вывод золотых ТИ",
                };
                moneyWithdraw.UserID = CurrentUser.ID;

                MoneyFee moneyFee = null;

                switch ((MoneyWithdraw.ProviderType)moneyWithdraw.Provider)
                {
                    case Model.MoneyWithdraw.ProviderType.Yandex:
                        moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.WithdrawYandex);
                        break;
                    case Model.MoneyWithdraw.ProviderType.Qiwi:
                        moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.WithdrawQiwi);
                        break;
                    case Model.MoneyWithdraw.ProviderType.Webmoney:
                        moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.WithdrawWebMoney);
                        break;
                    case Model.MoneyWithdraw.ProviderType.Robokassa:
                        moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.WithdrawRobokassa);
                        break;
                }
                moneyDetail.MoneyFeeID = moneyFee.ID;
                var guid = Guid.NewGuid();
                Repository.CreateMoneyDetail(moneyDetail, guid);

                moneyWithdraw.MoneyDetailID = moneyDetail.ID;
                Repository.CreateMoneyWithdraw(moneyWithdraw);
                Repository.SubmitMoney(guid);

                return View("WithdrawOk");
            }
            return View(moneyWithdrawView);
        }
    }
}
