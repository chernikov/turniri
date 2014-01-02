using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<NewComment> NewComments
        {
            get
            {
                return Db.NewComments;
            }
        }

        public bool CreateNewComment(NewComment instance)
        {
            if (instance.ID == 0)
            {
                Db.NewComments.InsertOnSubmit(instance);
                Db.NewComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateNewComment(NewComment instance)
        {
            var cache = Db.NewComments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.NewID = instance.NewID;
				cache.CommentID = instance.CommentID;
                Db.NewComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveNewComment(int idNewComment)
        {
            NewComment instance = Db.NewComments.FirstOrDefault(p => p.ID == idNewComment);
            if (instance != null)
            {
                Db.NewComments.DeleteOnSubmit(instance);
                Db.NewComments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}