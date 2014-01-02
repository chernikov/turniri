using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels.User;
using turniri.Social.Facebook;
using turniri.Social.Google;
using turniri.Social.Twitter;
using turniri.Social.Vkontakte;
using turniri.Tools;
using turniri.Tools.Mail;

namespace turniri.Areas.Default.Controllers
{
    public class OauthController : DefaultController
    {
        private static string AvatarFolder = "/Media/files/avatars/";

        private static string Avatar173Size = "Avatar173Size";
        private static string Avatar96Size = "Avatar96Size";
        private static string Avatar84Size = "Avatar84Size";
        private static string Avatar57Size = "Avatar57Size";
        private static string Avatar30Size = "Avatar30Size";
        private static string Avatar26Size = "Avatar26Size";
        private static string Avatar18Size = "Avatar18Size";

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private FbProvider fbProvider;

        private VkProvider vkProvider;

        private GoogleProvider googleProvider;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {

            fbProvider = new FbProvider();
            fbProvider.Config = Config.FacebookAppConfig;

            vkProvider = new VkProvider();
            vkProvider.Config = Config.VkAppConfig;

            googleProvider = new GoogleProvider();
            googleProvider.Config = Config.GoogleAppConfig;

            base.Initialize(requestContext);
        }

        #region Register
        #region facebook

        public ActionResult FacebookLogin()
        {
            return Redirect(fbProvider.Authorize("http://" + HostName + "/Oauth/SaveFbCode"));
        }

        public ActionResult SaveFbCode()
        {
            if (Request.Params.AllKeys.Contains("code"))
            {
                var code = Request.Params["code"];
                return ProcessFbCode(code);
            }
            return View("CantInitialize");
        }

        protected ActionResult ProcessFbCode(string code)
        {
            if (fbProvider.GetAccessToken(code, "http://" + HostName + "/Oauth/SaveFbCode"))
            {
                var jObj = fbProvider.GetUserInfo();
                var fbUserInfo = JsonConvert.DeserializeObject<FbUserInfo>(jObj.ToString());
                var identifier = fbUserInfo.Identifier;
                if (TryLogin(identifier, fbUserInfo.Email))
                {
                    return RedirectToAction("Index", "User");
                }

                var registerView = new SocialRegisterUserView()
                {
                    Avatar = string.Format("http://graph.facebook.com/{0}/picture", fbUserInfo.Id),
                    Birthdate = fbUserInfo.RealBirthDate,
                    City = string.Empty,
                    Country = string.Empty,
                    Email = fbUserInfo.Email,
                    VerifiedEmail = true,
                    Phone = string.Empty,
                    FirstName = fbUserInfo.FirstName,
                    LastName = fbUserInfo.LastName,
                    Login = fbUserInfo.UserName,
                    Identifier = fbUserInfo.Identifier,
                    Provider = Model.Social.ProviderType.facebook,
                    UserInfo = jObj.ToString()
                };
                return RegisterSocial(registerView);
            }
            return View("CantInitialize");
        }
        #endregion

        #region vkontakte

        public ActionResult VkontakteLogin()
        {
            return Redirect(vkProvider.Authorize("http://" + HostName + "/OAuth/SaveVkCode"));
        }

        public ActionResult SaveVkCode()
        {
            if (Request.Params.AllKeys.Contains("code"))
            {
                var code = Request.Params["code"];
                return ProcessVkCode(code);
            }
            return View("CantInitialize");
        }

        protected ActionResult ProcessVkCode(string code)
        {
            if (vkProvider.GetAccessToken(code, "http://" + HostName + "/OAuth/SaveVkCode"))
            {
                var jObj = vkProvider.GetUserInfo();
                logger.Debug("Vk Info : " + jObj.ToString());
                var vkUserInfoResonse = JsonConvert.DeserializeObject<VkUserInfoResponse>(jObj.ToString());
                var vkUserInfo = vkUserInfoResonse.Response[0];
                var identifier = vkUserInfo.Identifier;
                if (TryLogin(identifier, string.Empty))
                {
                    return RedirectToAction("Index", "User");
                }
                var registerView = new SocialRegisterUserView()
                {
                    Avatar = vkUserInfo.Photo,
                    Birthdate = vkUserInfo.RealBirthDate,
                    City = string.Empty,
                    Country = string.Empty,
                    Email = string.Empty,
                    Phone = vkUserInfo.MobilePhone ?? vkUserInfo.HomePhone ?? string.Empty,
                    VerifiedEmail = false,
                    FirstName = vkUserInfo.FirstName,
                    LastName = vkUserInfo.LastName,
                    Login = !string.IsNullOrWhiteSpace(vkUserInfo.Nickname) ? vkUserInfo.Nickname : !string.IsNullOrWhiteSpace(vkUserInfo.Domain) ? vkUserInfo.Domain : "vk_" + vkUserInfo.Domain,
                    Identifier = vkUserInfo.Identifier,
                    Provider = Model.Social.ProviderType.vk,
                    UserInfo = jObj.ToString()
                };
                return RegisterSocial(registerView);

            }
            return View("CantInitialize");
        }

        #endregion

        #region google

        public ActionResult GoogleLogin()
        {
            var sessionState = Guid.NewGuid().ToString("N");

            Session["GoogleSecretState"] = sessionState;
            return Redirect(googleProvider.Authorize("http://" + HostName + "/Oauth/SaveGoogleCode", sessionState));
        }

        public ActionResult SaveGoogleCode()
        {
            if (Request.Params.AllKeys.Contains("state"))
            {
                var state = Request.Params["state"];

                if (state != Session["GoogleSecretState"] as string)
                {
                    Response.StatusCode = 401;
                    return Content("Invalid state parameter");
                }
            }


            if (Request.Params.AllKeys.Contains("code"))
            {
                var code = Request.Params["code"];
                return ProcessGoogleCode(code);
            }
            return View("CantInitialize");
        }

        protected ActionResult ProcessGoogleCode(string code)
        {
            if (googleProvider.GetAccessToken(code, "http://" + HostName + "/Oauth/SaveGoogleCode"))
            {
                var jObj = googleProvider.GetUserInfo();
                var googleUserInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(jObj.ToString());
                var identifier = googleUserInfo.Identifier;

                if (TryLogin(identifier, googleUserInfo.Email))
                {
                    return RedirectToAction("Index", "User");
                }
                var registerView = new SocialRegisterUserView()
                {
                    Avatar = googleUserInfo.Picture,
                    Birthdate = googleUserInfo.RealBirthDate,
                    City = string.Empty,
                    Country = string.Empty,
                    Email = googleUserInfo.Email,
                    Phone = string.Empty,
                    VerifiedEmail = googleUserInfo.Verified,
                    FirstName = googleUserInfo.FirstName,
                    LastName = googleUserInfo.LastName,
                    Login = googleUserInfo.Name,
                    Identifier = googleUserInfo.Identifier,
                    Provider = Model.Social.ProviderType.google,
                    UserInfo = jObj.ToString()
                };
                return RegisterSocial(registerView);
            }
            return View("CantInitialize");
        }

        #endregion

        private bool TryLogin(string identifier, string email)
        {
            var social = Repository.Socials.FirstOrDefault(p => p.Identified == identifier);

            if (social != null)
            {
                Auth.Login(social.User.Login);
                return true;
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = Repository.Users.FirstOrDefault(p => string.Compare(p.Email, email, true) == 0);
                Auth.Login(user.Login);
                return true;
            }
            return false;
        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }

        private ActionResult RegisterSocial(SocialRegisterUserView registerView)
        {
            try
            {
                //проверить логин (при необходимости - изменить)
                registerView.Login = Translit.Translate(registerView.Login).ToLower();
                registerView.Login = registerView.Login.Replace("-", "_");
                bool exist = Repository.Users.Any(p => string.Compare(p.Login, registerView.Login, true) == 0);
                var baseLogin = registerView.Login;
                if (exist)
                {
                    registerView.Login = registerView.Login + "_" + registerView.Provider.ToString();
                }
                exist = Repository.Users.Any(p => string.Compare(p.Login, registerView.Login, true) == 0);
                if (exist)
                {
                    while (true)
                    {
                        registerView.Login = baseLogin + "_" + Translit.Predicate();
                        exist = Repository.Users.Any(p => string.Compare(p.Login, registerView.Login, true) == 0);
                        if (!exist)
                        {
                            break;
                        }
                    }
                }
                //создать аккаунт 
                var user = (User)ModelMapper.Map(registerView, typeof(SocialRegisterUserView), typeof(User));

                user.Password = StringExtension.GenerateNewFile();

                Repository.CreateUser(user);

                if (!string.IsNullOrWhiteSpace(user.Email) && !user.VerifiedEmail)
                {
                    NotifyMail.SendNotify(Config, "Register", user.Email,
                                      (u, format) => string.Format(format, HostName),
                                      (u, format) => string.Format(format, u.ActivatedLink, HostName),
                                      user);
                }
                else
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
                //создать Social

                var social = new Model.Social()
                {
                    Identified = registerView.Identifier,
                    Provider = (int)registerView.Provider,
                    UserInfo = registerView.UserInfo,
                    JsonResource = string.Empty,
                    IsAdvansed = false,
                    UserID = user.ID
                };
                Repository.CreateSocial(social);

                //скачать картинку (если есть)
                if (!string.IsNullOrWhiteSpace(registerView.Avatar))
                {
                    CreateAvatar(user, registerView.Avatar);
                }
                Auth.Login(user.Login);
                return RedirectToAction("RegisterSuccess");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Debug("Can't initialize : " + registerView.UserInfo);
                return View("CantInitialize");
            }
        }

        public void CreateAvatar(User user, string pictureUrl)
        {
            var webClient = new WebClient();
            var bytes = webClient.DownloadData(pictureUrl);
            var ms = new MemoryStream(bytes);

            user.AvatarPath173 = MakeAvatar(ms, Avatar173Size);
            user.AvatarPath96 = MakeAvatar(ms, Avatar96Size);
            user.AvatarPath84 = MakeAvatar(ms, Avatar84Size);
            user.AvatarPath57 = MakeAvatar(ms, Avatar57Size);
            user.AvatarPath30 = MakeAvatar(ms, Avatar30Size);
            user.AvatarPath26 = MakeAvatar(ms, Avatar26Size);
            user.AvatarPath18 = MakeAvatar(ms, Avatar18Size);

            Repository.UpdateUser(user);
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

        #endregion


    }
}
