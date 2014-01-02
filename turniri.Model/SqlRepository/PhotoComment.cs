using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PhotoComment> PhotoComments
        {
            get
            {
                return Db.PhotoComments;
            }
        }

        public bool CreatePhotoComment(PhotoComment instance)
        {
            if (instance.ID == 0)
            {
                Db.PhotoComments.InsertOnSubmit(instance);
                Db.PhotoComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePhotoComment(PhotoComment instance)
        {
            var cache = Db.PhotoComments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.PhotoID = instance.PhotoID;
				cache.CommentID = instance.CommentID;
                Db.PhotoComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePhotoComment(int idPhotoComment)
        {
            PhotoComment instance = Db.PhotoComments.FirstOrDefault(p => p.ID == idPhotoComment);
            if (instance != null)
            {
                Db.PhotoComments.DeleteOnSubmit(instance);
                Db.PhotoComments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}