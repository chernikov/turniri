using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Tools;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,editor")]
    public class UrlController : AdminController
    {
        public ActionResult Index()
        {
            foreach (var blog in Repository.Blogs.ToList())
            {
                Repository.UpdateBlog(blog);
            }
            
            foreach (var forum in Repository.Forums.ToList())
            {
                forum.Url = Translit.WithPredicateTranslate(forum.Name);
                Repository.UpdateForum(forum);
            }

            foreach (var game in Repository.Games.ToList())
            {
                game.Url = Translit.Translate(game.Name);
                Repository.UpdateGame(game);
            }

            foreach (var @new in Repository.News.ToList())
            {
                Repository.UpdateNew(@new);
            }

            foreach (var photoAlbum in Repository.PhotoAlbums.ToList())
            {
                Repository.UpdatePhotoAlbum(photoAlbum);
            }

            foreach (var tournament in Repository.Tournaments.ToList())
            {
                var baseUrl = Translit.Translate(tournament.Name);
                var url = baseUrl;
                var num = 1;
                bool exist = false;
                do
                {
                    exist = Repository.Tournaments.Any(p => string.Compare(p.Url, url, true) == 0);
                    if (exist)
                    {
                        url = string.Format("{0}-{1}", baseUrl, num);
                        num++;
                    }
                } while (exist);
                tournament.Url = url;

                Repository.UpdateTournament(tournament);
            }

            foreach (var userVideo in Repository.UserVideos.ToList())
            {
                Repository.UpdateUserVideo(userVideo);
            }

            foreach (var video in Repository.Videos.ToList())
            {
                Repository.UpdateVideo(video);
            }

            return Content("OK");
        }

        public ActionResult Seo()
        {
            foreach (var game in Repository.Games.ToList())
            {
                game.Keywords = game.Name;
                game.MetaDescription = game.Description.StripBBCode();

                Repository.UpdateGame(game);
            }

            foreach (var tournament in Repository.Tournaments.ToList())
            {
                tournament.Keywords = tournament.Name;
                tournament.MetaDescription = tournament.Description.StripBBCode();

                Repository.UpdateTournament(tournament);
            }

            return Content("OK");
        }
    }
}
