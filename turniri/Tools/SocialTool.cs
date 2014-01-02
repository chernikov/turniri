using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Principal;
using System.Text;

using System.Web.Profile;
using System.Web.Routing;
using System.Web.Mvc;

using turniri.Model;
using turniri.Social.Facebook;
using turniri.Global.Config;
using Newtonsoft.Json;
using turniri.Social.Vkontakte;
using turniri.Social.Twitter;

namespace turniri.Tools
{
    public class SocialTool
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void ProcessSocialPost(SocialPost socialPost, User user, IConfig config, out int groupID)
        {
            groupID = 0;
            var social = user.Socials.FirstOrDefault(p => p.Provider == socialPost.Provider);
            switch ((Model.Social.ProviderType)socialPost.Provider)
            {
                case Model.Social.ProviderType.facebook:
                    var fbProvider = new FbProvider();
                    fbProvider.Config = config.FacebookAppConfig;
                    fbProvider.AccessToken = social.JsonResource;
                    var fbUserInfo = JsonConvert.DeserializeObject<FbUserInfo>(social.UserInfo);
                    if (socialPost.SocialGroupID != null)
                    {
                        socialPost.Responce = fbProvider.Publish(socialPost.SocialGroup.Number, socialPost);
                    }
                    else
                    {
                        socialPost.Responce = fbProvider.Publish(fbUserInfo.Id, socialPost);
                    }
                    break;
                case Model.Social.ProviderType.vk:
                    var vkProvider = new VkProvider();
                    vkProvider.Config = config.VkAppConfig;
                    vkProvider.AccessToken = JsonConvert.DeserializeObject<VkAccessToken>(social.JsonResource);
                    var vkUserInfo = JsonConvert.DeserializeObject<VkUserInfo>(social.UserInfo);
                    var attached = vkProvider.PreparePublishOnGroupAttachedWall(socialPost, out groupID, socialPost.SocialGroup != null ? socialPost.SocialGroup.Name : "");
                    socialPost.VkPrepared = attached;
                    break;
                case Model.Social.ProviderType.twitter:
                    var twProvider = new TwitterProvider();
                    twProvider.Config = config.TwitterAppConfig;
                    twProvider.AccessToken = JsonConvert.DeserializeObject<TwitterAccessToken>(social.JsonResource);
                    twProvider.Publish(socialPost);
                    break;
            }
        }

        public static void ProcessSocialWinner(IRepository repository, int matchId, string host, UrlHelper Url, HttpServerUtilityBase Server, IConfig config)
        {
            logger.Debug("ProcessSocialWinner");

            var match = repository.Matches.FirstOrDefault(p => p.ID == matchId);
            if (match != null && match.Status == (int)Match.MatchStatusEnum.Submit)
            {
                if (match.WinnerID.HasValue && match.Score1.HasValue)
                {
                    var participant = repository.Participants.FirstOrDefault(p => p.ID == match.WinnerID);
                    if (participant != null)
                    {
                        if (participant.IsTeam)
                        {
                            foreach (var userTeam in participant.Team.UserTeams)
                            {
                                PublishWinForUser(repository, host, Url, config, match, userTeam.User, Server);
                            }
                        }
                        else
                        {
                            var user = repository.Users.FirstOrDefault(p => p.ID == participant.UserID);
                            PublishWinForUser(repository, host, Url, config, match, user, Server);
                        }
                    }
                }
            }
        }

        private static void PublishWinForUser(IRepository repository, string host, UrlHelper Url, IConfig config, Match match, User user, HttpServerUtilityBase Server)
        {
            if (user != null)
            {
                SocialPost socialPost = null;
                if (match.TournamentID.HasValue)
                {
                    socialPost = new SocialPost()
                    {
                        UserID = user.ID,
                        Title = "Победа в матче",
                        Link = "http://" + host + Url.Action("Index", "Tournament", new { platformUrl = match.Tournament.Platform.Url, gameUrl = match.Tournament.Game.Url, url = match.Tournament.Url, matchID = match.ID }),
                        Teaser = string.Format("Я выиграл в матче турнира {0} против {1}", match.Tournament.Name, match.Rival(match.WinnerID.Value).ActualName),
                        Preview = "http://" + host + match.Tournament.ImagePath
                    };

                    socialPost.SocialPostImages.Add(new SocialPostImage()
                    {
                        PhotoUrl = Server.MapPath(match.Tournament.ImagePath)
                    });
                }
                else
                {
                    socialPost = new SocialPost()
                    {
                        UserID = user.ID,
                        Title = "Победа в матче",
                        Link = "http://" + host + Url.Action("Index", "Home", new { matchID = match.ID }),
                        Teaser = string.Format("Я выиграл в матче против {0}", match.Rival(match.WinnerID.Value).ActualName),
                        Preview = "http://" + host + "/Media/images/social-logo.jpg"
                    };

                    socialPost.SocialPostImages.Add(new SocialPostImage()
                    {
                        PhotoUrl = Server.MapPath("/Media/images/social-logo.jpg")
                    });
                }

                foreach (var social in user.Socials.Where(p => p.IsAdvansed && p.TranslateWin))
                {
                    switch ((Model.Social.ProviderType)social.Provider)
                    {
                        case Model.Social.ProviderType.facebook:
                            var fbSocialPost = socialPost.Copy();
                            fbSocialPost.Provider = (int)Model.Social.ProviderType.facebook;
                            var fbProvider = new FbProvider();
                            fbProvider.Config = config.FacebookAppConfig;
                            fbProvider.AccessToken = social.JsonResource;
                            var fbUserInfo = JsonConvert.DeserializeObject<FbUserInfo>(social.UserInfo);
                            var fbResponse = fbProvider.Publish(fbUserInfo.Id, fbSocialPost);
                            fbSocialPost.Responce = fbResponse;
                            repository.CreateSocialPost(fbSocialPost);
                            break;
                        case Model.Social.ProviderType.vk:
                            var vkSocialPost = socialPost.Copy();
                            vkSocialPost.Provider = (int)Model.Social.ProviderType.vk;
                            var vkProvider = new VkProvider();
                            vkProvider.Config = config.VkAppConfig;
                            vkProvider.AccessToken = JsonConvert.DeserializeObject<VkAccessToken>(social.JsonResource);
                            var vkUserInfo = JsonConvert.DeserializeObject<VkUserInfo>(social.UserInfo);
                            var attached = vkProvider.PreparePublishAttachedWall(vkSocialPost);
                            vkSocialPost.VkPrepared = attached;
                            repository.CreateSocialPost(vkSocialPost);
                            /* create notice */
                            var vkNotice = new Notice()
                            {
                                ReceiverID = user.ID,
                                Text = socialPost.Teaser,
                                Caption = socialPost.Title,
                                Type = (int)Notice.TypeEnum.PublishVk,
                                IsCloseForRead = false,
                            };
                            repository.CreateNotice(vkNotice);
                            var actionNoticeAction = new NoticeAction()
                            {
                                ActionUrl = "/Connect/VkConfirm/" + vkSocialPost.ID,
                                Direct = false,
                                IsRunNotice = true,
                                IsResolveNotice = false,
                                NoticeID = vkNotice.ID,
                                Name = "Опубликовать в vk.com"
                            };
                            repository.CreateNoticeAction(actionNoticeAction);
                            break;
                        case Model.Social.ProviderType.twitter:
                            var twSocialPost = socialPost.Copy();
                            twSocialPost.Provider = (int)Model.Social.ProviderType.twitter;
                            var twProvider = new TwitterProvider();
                            twProvider.Config = config.TwitterAppConfig;
                            twProvider.AccessToken = JsonConvert.DeserializeObject<TwitterAccessToken>(social.JsonResource);
                            twProvider.Publish(socialPost);
                            repository.CreateSocialPost(twSocialPost);
                            break;
                    }
                }
            }
        }

    }
}