using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<NewSocialPost> NewSocialPosts
        {
            get
            {
                return Db.NewSocialPosts;
            }
        }

        public bool CreateNewSocialPost(NewSocialPost instance)
        {
            if (instance.ID == 0)
            {
                Db.NewSocialPosts.InsertOnSubmit(instance);
                Db.NewSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateNewSocialPost(NewSocialPost instance)
        {
            var cache = Db.NewSocialPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.NewID = instance.NewID;
				cache.SocialPostID = instance.SocialPostID;
                Db.NewSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveNewSocialPost(int idNewSocialPost)
        {
            NewSocialPost instance = Db.NewSocialPosts.FirstOrDefault(p => p.ID == idNewSocialPost);
            if (instance != null)
            {
                Db.NewSocialPosts.DeleteOnSubmit(instance);
                Db.NewSocialPosts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}