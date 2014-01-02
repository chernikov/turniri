using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<SocialPostImage> SocialPostImages
        {
            get
            {
                return Db.SocialPostImages;
            }
        }

        public bool CreateSocialPostImage(SocialPostImage instance)
        {
            if (instance.ID == 0)
            {
                Db.SocialPostImages.InsertOnSubmit(instance);
                Db.SocialPostImages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSocialPostImage(SocialPostImage instance)
        {
            var cache = Db.SocialPostImages.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.SocialPostID = instance.SocialPostID;
				cache.PhotoUrl = instance.PhotoUrl;
                Db.SocialPostImages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveSocialPostImage(int idSocialPostImage)
        {
            SocialPostImage instance = Db.SocialPostImages.FirstOrDefault(p => p.ID == idSocialPostImage);
            if (instance != null)
            {
                Db.SocialPostImages.DeleteOnSubmit(instance);
                Db.SocialPostImages.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}