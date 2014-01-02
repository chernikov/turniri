using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Video> Videos
        {
            get
            {
                return Db.Videos;
            }
        }

        public bool CreateVideo(Video instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.Url = Translit.WithPredicateTranslate(instance.Header);
                Db.Videos.InsertOnSubmit(instance);
                Db.Videos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateVideo(Video instance)
        {
            var cache = Db.Videos.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Header = instance.Header;
                cache.VideoCode = instance.VideoCode;
                cache.VideoThumb = instance.VideoThumb;
				cache.VideoUrl = instance.VideoUrl;
				cache.Text = instance.Text;
			    Db.Videos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveVideo(int idVideo)
        {
            Video instance = Db.Videos.FirstOrDefault(p => p.ID == idVideo);
            if (instance != null)
            {
                Db.Videos.DeleteOnSubmit(instance);
                Db.Videos.Context.SubmitChanges();
                return true;
            }
            return false;
        }

          public bool UpdateVisitVideo(int idVideo)
          {
              var cache = Db.Videos.FirstOrDefault(p => p.ID == idVideo);
              if (cache != null)
              {
                  cache.VisitCount++;
                  Db.Videos.Context.SubmitChanges();
                  return true;
              }
              return false;
          }
    }
}