using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class PhotoAlbum
    {
        public IEnumerable<Photo> SubPhotos
        {
            get 
            {
                return Photos.ToList(); 
            }

        }
        public int PhotosCount
        {
            get { return Photos.Count; }
        }

        public IEnumerable<Photo> PhotosPage(int page = 1, int itemPerPage = 36)
        {
            return Photos.OrderBy(p => p.AddedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int CountPage(int itemPerPage = 36)
        {
            return PhotosCount / itemPerPage + (PhotosCount % itemPerPage != 0 ? 1 : 0);
        }

        public Photo AlbumPhoto
        {
            get
            {
                return Photos.FirstOrDefault();
            }
        }

        public IEnumerable<Photo> OtherAlbumPhotos
        {
            get
            {
                return Photos.Count > 0 ? Photos.Skip(1).Take(6).AsEnumerable() : null;
            }
        }

        public IEnumerable<SocialPost> SubSocialPosts
        {
            get
            {
                return PhotoAlbumSocialPosts.Select(p => p.SocialPost);
            }
        }
	}
}