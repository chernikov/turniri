using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PhotoAlbumSocialPost> PhotoAlbumSocialPosts
        {
            get
            {
                return Db.PhotoAlbumSocialPosts;
            }
        }

        public bool CreatePhotoAlbumSocialPost(PhotoAlbumSocialPost instance)
        {
            if (instance.ID == 0)
            {
                Db.PhotoAlbumSocialPosts.InsertOnSubmit(instance);
                Db.PhotoAlbumSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePhotoAlbumSocialPost(PhotoAlbumSocialPost instance)
        {
            var cache = Db.PhotoAlbumSocialPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.PhotoAlbumID = instance.PhotoAlbumID;
				cache.SocialPostID = instance.SocialPostID;
                Db.PhotoAlbumSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePhotoAlbumSocialPost(int idPhotoAlbumSocialPost)
        {
            PhotoAlbumSocialPost instance = Db.PhotoAlbumSocialPosts.FirstOrDefault(p => p.ID == idPhotoAlbumSocialPost);
            if (instance != null)
            {
                Db.PhotoAlbumSocialPosts.DeleteOnSubmit(instance);
                Db.PhotoAlbumSocialPosts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}