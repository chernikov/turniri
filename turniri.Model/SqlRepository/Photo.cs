using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Photo> Photos
        {
            get
            {
                return Db.Photos;
            }
        }

        public bool CreatePhoto(Photo instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Photos.InsertOnSubmit(instance);
                Db.Photos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool BindPhoto(Photo instance)
        {
            var cache = Db.Photos.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.PhotoAlbumID = instance.PhotoAlbumID;
				Db.Photos.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateVisitPhoto(int idPhoto)
        {
            try
            {
                var cache = Db.Photos.FirstOrDefault(p => p.ID == idPhoto);
                if (cache != null)
                {
                    cache.VisitCount++;
                    Db.Photos.Context.SubmitChanges();
                    return true;
                }
            }
            catch { }
            return false;
        }


        public bool RemovePhoto(int idPhoto)
        {
            Photo instance = Db.Photos.FirstOrDefault(p => p.ID == idPhoto);
            if (instance != null)
            {
                Db.Photos.DeleteOnSubmit(instance);
                Db.Photos.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public Photo ChangePhoto(int idCurrentPhoto, bool next)
        {
            var currentPhoto = Db.Photos.FirstOrDefault(p => p.ID == idCurrentPhoto);

            if (currentPhoto != null)
            {
                var photoAlbumID = currentPhoto.PhotoAlbumID;

                Photo photo = null;
                if (next)
                {
                    //next 
                    photo = Db.Photos.Where(p => p.PhotoAlbumID == photoAlbumID && p.AddedDate > currentPhoto.AddedDate).OrderBy(p => p.AddedDate).FirstOrDefault();
                    //if last - get first
                    if (photo == null)
                    {
                        photo = Db.Photos.Where(p => p.PhotoAlbumID == photoAlbumID).OrderBy(p => p.AddedDate).FirstOrDefault();
                    }
                }
                else
                {
                    //next 
                    photo = Db.Photos.Where(p => p.PhotoAlbumID == photoAlbumID && p.AddedDate < currentPhoto.AddedDate).OrderByDescending(p => p.AddedDate).FirstOrDefault();
                    //if first - get last
                    if (photo == null)
                    {
                        photo = Db.Photos.Where(p => p.PhotoAlbumID == photoAlbumID).OrderByDescending(p => p.AddedDate).FirstOrDefault();
                    }
                }
                return photo;
            }
            return null;
        }

        public int TogglePhotoLike(int idPhoto, int idUser)
        {
            var photo = Db.Photos.FirstOrDefault(p => p.ID == idPhoto);

            if (photo != null)
            {
                var photoLike = Db.PhotoLikes.FirstOrDefault(p => p.PhotoID == idPhoto && p.UserID == idUser);

                if (photoLike != null)
                {
                    Db.PhotoLikes.DeleteOnSubmit(photoLike);

                }
                else
                {
                    var newPhotoLike = new PhotoLike
                    {
                        PhotoID = photo.ID,
                        UserID = idUser
                    };

                    Db.PhotoLikes.InsertOnSubmit(newPhotoLike);
                }
                Db.PhotoLikes.Context.SubmitChanges();
                photo.Likes = photo.PhotoLikes.Count;

                Db.Photos.Context.SubmitChanges();
                return photo.Likes;
            }

            return 0;
        }
    }
}