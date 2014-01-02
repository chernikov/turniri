using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin,editor")]
    public class SocialPostController : AdminController
    {
        public ActionResult MakeFromBlog(int id)
        {
            var blog = Repository.Blogs.FirstOrDefault(p => p.ID == id);

            if (blog != null)
            {
                var socialPostView = new SocialPostView()
                {
                    ForeignID = id,
                    ForeignType = (int)SocialPostView.Foreign.blog,
                    Link = "http://" + HostName + Url.Action("Item", "Blog", new { area = "Default", url = blog.Url }),
                    Title = blog.Header,
                    Teaser = blog.Text.StripBBCode()
                };
                socialPostView.InitUser(CurrentUser);

                if (!string.IsNullOrEmpty(blog.PreviewUrl))
                {
                    socialPostView.Preview = blog.PreviewUrl;
                }
                else
                {
                    socialPostView.Preview = "/Media/images/social-logo.jpg";
                }
                return View("SocialPost", socialPostView);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult MakeFromNew(int id)
        {
            var @new = Repository.News.FirstOrDefault(p => p.ID == id);

            if (@new != null)
            {
                var socialPostView = new SocialPostView()
                {
                    ForeignID = id,
                    ForeignType = (int)SocialPostView.Foreign.@new,
                    Link = "http://" + HostName + Url.Action("Index", "New", new { area = "Default", url = @new.Url }),
                    Title = @new.Header,
                    Teaser = @new.Text.StripBBCode()
                };
                socialPostView.InitUser(CurrentUser);

                if (!string.IsNullOrEmpty(@new.PreviewPath))
                {
                    socialPostView.Preview = @new.PreviewPath;
                }
                else
                {
                    socialPostView.Preview = "/Media/images/social-logo.jpg";
                }
                return View("SocialPost", socialPostView);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult MakeFromVideo(int id)
        {
            var video = Repository.Videos.FirstOrDefault(p => p.ID == id);

            if (video != null)
            {
                var socialPostView = new SocialPostView()
                {
                    ForeignID = id,
                    ForeignType = (int)SocialPostView.Foreign.video,
                    Link = "http://" + HostName + Url.Action("Index", "Video", new { area = "Default", url = video.Url }),
                    Title = video.Header,
                    Teaser = video.Text.StripBBCode()
                };
                socialPostView.InitUser(CurrentUser);

                if (!string.IsNullOrEmpty(video.VideoThumb))
                {
                    socialPostView.Preview = video.VideoThumb;
                }
                else
                {
                    socialPostView.Preview = "/Media/images/social-logo.jpg";
                }
                return View("SocialPost", socialPostView);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult MakeFromPhotoAlbum(int id)
        {
            var photoAlbum = Repository.PhotoAlbums.FirstOrDefault(p => p.ID == id);

            if (photoAlbum != null)
            {
                var socialPostView = new SocialPostView()
                {
                    ForeignID = id,
                    ForeignType = (int)SocialPostView.Foreign.photoAlbum,
                    Link = "http://" + HostName + Url.Action("Item", "Photo", new { area = "Default", url = photoAlbum.Url }),
                    Title = photoAlbum.Name,
                    Teaser = string.Empty
                };
                socialPostView.InitUser(CurrentUser);


                if (photoAlbum.AlbumPhoto != null)
                {
                    socialPostView.Preview = photoAlbum.AlbumPhoto.AlbumPreviewPath;
                }
                else
                {
                    socialPostView.Preview = "/Media/images/social-logo.jpg";
                }
                return View("SocialPost", socialPostView);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult MakeFromTournament(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null)
            {
                var socialPostView = new SocialPostView()
                {
                    ForeignID = id,
                    ForeignType = (int)SocialPostView.Foreign.tournament,
                    Link = "http://" + HostName + Url.Action("Index", "Tournament", new { area = "Default", url = tournament.Url, gameUrl = tournament.Game.Url, platformUrl = tournament.Platform.Url }),
                    Title = tournament.Name,
                    Teaser = tournament.Description.StripBBCode()
                };
                socialPostView.InitUser(CurrentUser);

                if (!string.IsNullOrEmpty(tournament.ImagePath))
                {
                    socialPostView.Preview = tournament.ImagePath;
                }
                else
                {
                    socialPostView.Preview = "/Media/images/social-logo.jpg";
                }
                return View("SocialPost", socialPostView);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult UpdateGroupsSelect(int id)
        {
            List<SelectListItem> socialGroups = new List<SelectListItem>();
            socialGroups.Add(new SelectListItem
            {
                Selected = true,
                Text = "На стену",
                Value = ""
            });
            socialGroups.AddRange(Repository.SocialGroups.Where(p => p.Provider == id).Select(p => new SelectListItem
            {
                Selected = false,
                Text = p.Name,
                Value = p.ID.ToString()
            }).ToList());

            return View(socialGroups);
        }

        public ActionResult VkConnect(int id)
        {
            var socialPost = Repository.SocialPosts.FirstOrDefault(p => p.ID == id);
            if (socialPost != null)
            {
                ViewBag.GroupID = 0;
                return View("VkConnect", socialPost);
            }
            return null;
        }

        public ActionResult SocialPost(SocialPostView socialPostView)
        {
            if (!CurrentUser.HasAdvansedSocial((Model.Social.ProviderType)socialPostView.Provider))
            {
                ModelState.AddModelError("Provider", "Вы не дали прав на публикацию в этой соц.сети");
            };
            if (ModelState.IsValid) 
            {
                var socialPost = (SocialPost)ModelMapper.Map(socialPostView, typeof(SocialPostView), typeof(SocialPost));
                socialPost.UserID = CurrentUser.ID;
                socialPost.ID = 0;
                socialPost.SocialPostImages.Add(new SocialPostImage()
                {
                    PhotoUrl = Server.MapPath(socialPost.Preview)
                });

                socialPost.Preview = "http://" + HostName + socialPost.Preview;
                Repository.CreateSocialPost(socialPost);

                switch ((SocialPostView.Foreign)socialPostView.ForeignType)
                {
                    case SocialPostView.Foreign.blog :
                        var blogSocialPost = new BlogSocialPost()
                        {
                            BlogID = socialPostView.ForeignID,
                            SocialPostID = socialPost.ID
                        };
                        Repository.CreateBlogSocialPost(blogSocialPost);
                        break;
                    case SocialPostView.Foreign.@new:
                        var newSocialPost = new NewSocialPost()
                        {
                            NewID = socialPostView.ForeignID,
                            SocialPostID = socialPost.ID
                        };
                        Repository.CreateNewSocialPost(newSocialPost);
                        break;
                    case SocialPostView.Foreign.photoAlbum:
                        var photoAlbumSocialPost = new PhotoAlbumSocialPost()
                        {
                            PhotoAlbumID = socialPostView.ForeignID,
                            SocialPostID = socialPost.ID
                        };
                        Repository.CreatePhotoAlbumSocialPost(photoAlbumSocialPost);
                        break;
                    case SocialPostView.Foreign.tournament:
                        var tournamentSocialPost = new TournamentSocialPost()
                        {
                            TournamentID = socialPostView.ForeignID,
                            SocialPostID = socialPost.ID
                        };
                        Repository.CreateTournamentSocialPost(tournamentSocialPost);
                        break;
                    case SocialPostView.Foreign.video:
                        var videoSocialPost = new VideoSocialPost()
                        {
                            VideoID = socialPostView.ForeignID,
                            SocialPostID = socialPost.ID
                        };
                        Repository.CreateVideoSocialPost(videoSocialPost);
                        break;
                }
                int groupID = 0;
                SocialTool.ProcessSocialPost(socialPost, CurrentUser, Config, out groupID);

                if (socialPost.Provider == (int)Model.Social.ProviderType.vk)
                {
                    ViewBag.GroupID = groupID;
                    return View("VkConnect", socialPost);
                }
                return View("Ok");
            }

            socialPostView.InitUser(CurrentUser);
            return View(socialPostView);
        }
    }
}
