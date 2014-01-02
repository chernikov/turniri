using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PhotoAlbum> PhotoAlbums
        {
            get
            {
                return Db.PhotoAlbums;
            }
        }

        public bool CreatePhotoAlbum(PhotoAlbum instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.Url = Translit.WithPredicateTranslate(instance.Name);
                Db.PhotoAlbums.InsertOnSubmit(instance);
                Db.PhotoAlbums.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePhotoAlbum(PhotoAlbum instance)
        {
            var cache = Db.PhotoAlbums.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                cache.GroupID = instance.GroupID;
				Db.PhotoAlbums.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePhotoAlbum(int idPhotoAlbum)
        {
            PhotoAlbum instance = Db.PhotoAlbums.FirstOrDefault(p => p.ID == idPhotoAlbum);
            if (instance != null)
            {
                Db.PhotoAlbums.DeleteOnSubmit(instance);
                Db.PhotoAlbums.Context.SubmitChanges();
                return true;
            }
            return false;
        }

    
    }
}