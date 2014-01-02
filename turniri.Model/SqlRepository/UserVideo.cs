using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserVideo> UserVideos
        {
            get
            {
                return Db.UserVideos;
            }
        }

        public bool CreateUserVideo(UserVideo instance)
        {
            if (instance.ID == 0)
            {
                instance.Url = Translit.WithPredicateTranslate(instance.Header);
                instance.AddedDate = DateTime.Now;
                Db.UserVideos.InsertOnSubmit(instance);
                Db.UserVideos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserVideo(UserVideo instance)
        {
            var cache = Db.UserVideos.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.VideoThumb = instance.VideoThumb;
				cache.Header = instance.Header;
				cache.VideoUrl = instance.VideoUrl;
				cache.Text = instance.Text;
                cache.GroupID = instance.GroupID;
                Db.UserVideos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserVideo(int idUserVideo)
        {
            UserVideo instance = Db.UserVideos.FirstOrDefault(p => p.ID == idUserVideo);
            if (instance != null)
            {
                Db.UserVideos.DeleteOnSubmit(instance);
                Db.UserVideos.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateVisitUserVideo(int idUserVideo)
        {
            var cache = Db.UserVideos.FirstOrDefault(p => p.ID == idUserVideo);
            if (cache != null)
            {
                cache.VisitCount++;
                Db.UserVideos.Context.SubmitChanges();
                return true;
            }

            return false;
        }
    }
}