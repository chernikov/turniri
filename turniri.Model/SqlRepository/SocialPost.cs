using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<SocialPost> SocialPosts
        {
            get
            {
                return Db.SocialPosts;
            }
        }

        public bool CreateSocialPost(SocialPost instance)
        {
            if (instance.ID == 0)
            {
                if (string.IsNullOrEmpty(instance.Identifier))
                {
                    instance.Identifier = string.Empty;
                }
                instance.AddedDate = DateTime.Now;
                Db.SocialPosts.InsertOnSubmit(instance);
                Db.SocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSocialPost(SocialPost instance)
        {
            var cache = Db.SocialPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Provider = instance.Provider;
				cache.AddedDate = instance.AddedDate;
				cache.Title = instance.Title;
				cache.Preview = instance.Preview;
				cache.Teaser = instance.Teaser;
				cache.Link = instance.Link;
                Db.SocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveSocialPost(int idSocialPost)
        {
            SocialPost instance = Db.SocialPosts.FirstOrDefault(p => p.ID == idSocialPost);
            if (instance != null)
            {
                Db.SocialPosts.DeleteOnSubmit(instance);
                Db.SocialPosts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}