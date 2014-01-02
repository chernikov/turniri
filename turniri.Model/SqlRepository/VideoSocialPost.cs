using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<VideoSocialPost> VideoSocialPosts
        {
            get
            {
                return Db.VideoSocialPosts;
            }
        }

        public bool CreateVideoSocialPost(VideoSocialPost instance)
        {
            if (instance.ID == 0)
            {
                Db.VideoSocialPosts.InsertOnSubmit(instance);
                Db.VideoSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateVideoSocialPost(VideoSocialPost instance)
        {
            var cache = Db.VideoSocialPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.VideoID = instance.VideoID;
				cache.SocialPostID = instance.SocialPostID;
                Db.VideoSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveVideoSocialPost(int idVideoSocialPost)
        {
            VideoSocialPost instance = Db.VideoSocialPosts.FirstOrDefault(p => p.ID == idVideoSocialPost);
            if (instance != null)
            {
                Db.VideoSocialPosts.DeleteOnSubmit(instance);
                Db.VideoSocialPosts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}