﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Social.Facebook;
using turniri.Social.Google;
using turniri.Social.Twitter;
using turniri.Social.Vkontakte;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin,editor")]
    public class ConnectController : AdminController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private FbProvider fbProvider;

        private VkProvider vkProvider;

        private TwitterProvider twProvider;

        private GoogleProvider googleProvider;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            fbProvider = new FbProvider();
            fbProvider.Config = Config.FacebookAppConfig;

            vkProvider = new VkProvider();
            vkProvider.Config = Config.VkAppConfig;

            twProvider = new TwitterProvider();
            twProvider.Config = Config.TwitterAppConfig;

            googleProvider = new GoogleProvider();
            googleProvider.Config = Config.GoogleAppConfig;

            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            return View(CurrentUser);
        }

        #region Connect

        #region Facebook

        public ActionResult FacebookConnect()
        {
            if (CurrentUser.HasAdvansedSocial(Model.Social.ProviderType.facebook))
            {
                var social = CurrentUser.Socials.FirstOrDefault(p => p.IsAdvansed && p.Provider == (int)Model.Social.ProviderType.facebook);
                if (social != null)
                {
                    Repository.RemoveSocial(social.ID);
                    return RedirectToAction("Index");
                }
            }
            return Redirect(fbProvider.SuperAuthorize("http://" + HostName + "/admin/Connect/SaveFbCode"));
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
            if (fbProvider.GetAccessToken(code, "http://" + HostName + "/admin/Connect/SaveFbCode"))
            {
                var jObj = fbProvider.GetUserInfo();
                var fbUserInfo = JsonConvert.DeserializeObject<FbUserInfo>(jObj.ToString());
                var identifier = fbUserInfo.Identifier;

                var social = new Model.Social()
                {
                    UserID = CurrentUser.ID,
                    IsAdvansed = true,
                    Provider = (int)Model.Social.ProviderType.facebook,
                    Identified = fbUserInfo.Identifier,
                    JsonResource = fbProvider.AccessToken,
                    UserInfo = jObj.ToString()
                };

                return RegisterSocial(social);
            }
            return View("CantInitialize");
        }
        #endregion

        #region Vkontakte

        public ActionResult VkontakteConnect()
        {
            if (CurrentUser.HasAdvansedSocial(Model.Social.ProviderType.vk))
            {
                var social = CurrentUser.Socials.FirstOrDefault(p => p.IsAdvansed && p.Provider == (int)Model.Social.ProviderType.vk);
                if (social != null)
                {
                    Repository.RemoveSocial(social.ID);
                    return RedirectToAction("Index");
                }
            }
            return Redirect(vkProvider.SuperAuthorize("http://" + HostName + "/admin/Connect/SaveVkCode"));
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
            if (vkProvider.GetAccessToken(code, "http://" + HostName + "/admin/Connect/SaveVkCode"))
            {
                var jObj = vkProvider.GetUserInfo();

                var vkUserInfoResonse = JsonConvert.DeserializeObject<VkUserInfoResponse>(jObj.ToString());
                var vkUserInfo = vkUserInfoResonse.Response[0];
                var identifier = vkUserInfo.Identifier;

                var social = new Model.Social()
                {
                    UserID = CurrentUser.ID,
                    IsAdvansed = true,
                    Provider = (int)Model.Social.ProviderType.vk,
                    Identified = vkUserInfo.Identifier,
                    JsonResource = JsonConvert.SerializeObject(vkProvider.AccessToken),
                    UserInfo = jObj.ToString()
                };
                return RegisterSocial(social);
            }
            return View("CantInitialize");
        }

        #endregion

        #region Twitter

        public ActionResult TwitterConnect()
        {
            if (CurrentUser.HasAdvansedSocial(Model.Social.ProviderType.twitter))
            {
                var social = CurrentUser.Socials.FirstOrDefault(p => p.IsAdvansed && p.Provider == (int)Model.Social.ProviderType.twitter);
                if (social != null)
                {
                    Repository.RemoveSocial(social.ID);
                    return RedirectToAction("Index");
                }
            }

            return Redirect(twProvider.Authorize("http://" + HostName + "/admin/Connect/SaveTwitterCode"));
        }

        public ActionResult SaveTwitterCode()
        {
            if (Request.Params.AllKeys.Contains("oauth_token") &&
                Request.Params.AllKeys.Contains("oauth_verifier"))
            {
                var token = Request.Params["oauth_token"];
                var verifier = Request.Params["oauth_verifier"];

                var accessToken = twProvider.GetAuthToken(token, verifier);
                return ProcessTwCode(accessToken);
            }
            return View("CantInitialize");
        }

        protected ActionResult ProcessTwCode(TwitterAccessToken accessToken)
        {
            twProvider.AccessToken = accessToken;
            var jObj = twProvider.GetUserInfo(accessToken.UserId);
            var twUserInfo = JsonConvert.DeserializeObject<TwitterUserInfo>(jObj.ToString());

            var social = new Model.Social()
            {
                UserID = CurrentUser.ID,
                IsAdvansed = true,
                Provider = (int)Model.Social.ProviderType.twitter,
                Identified = twUserInfo.Identifier,
                JsonResource = JsonConvert.SerializeObject(accessToken),
                UserInfo = JsonConvert.SerializeObject(twUserInfo)
            };
            return RegisterSocial(social);
        }
        #endregion

        #endregion

        private ActionResult RegisterSocial(Model.Social social)
        {
            try
            {
                Repository.CreateSocial(social);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Debug("Can't initialize : " + social.UserInfo);
                return View("CantInitialize");
            }
        }

        public ActionResult VkConfirm(int id)
        {
            var socialPost = Repository.SocialPosts.FirstOrDefault(p => p.ID == id);
            if (socialPost != null)
            {
                return View(socialPost);
            }
            return null;
        }
    }
}
