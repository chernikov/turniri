using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<BlogSocialPost> BlogSocialPosts
        {
            get
            {
                return Db.BlogSocialPosts;
            }
        }

        public bool CreateBlogSocialPost(BlogSocialPost instance)
        {
            if (instance.ID == 0)
            {
                Db.BlogSocialPosts.InsertOnSubmit(instance);
                Db.BlogSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateBlogSocialPost(BlogSocialPost instance)
        {
            var cache = Db.BlogSocialPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.BlogID = instance.BlogID;
				cache.SocialPostID = instance.SocialPostID;
                Db.BlogSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveBlogSocialPost(int idBlogSocialPost)
        {
            BlogSocialPost instance = Db.BlogSocialPosts.FirstOrDefault(p => p.ID == idBlogSocialPost);
            if (instance != null)
            {
                Db.BlogSocialPosts.DeleteOnSubmit(instance);
                Db.BlogSocialPosts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}