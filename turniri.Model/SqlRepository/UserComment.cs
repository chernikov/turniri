using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserComment> UserComments
        {
            get
            {
                return Db.UserComments;
            }
        }

        public bool CreateUserComment(UserComment instance)
        {
            if (instance.ID == 0)
            {
                Db.UserComments.InsertOnSubmit(instance);
                Db.UserComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserComment(UserComment instance)
        {
            var cache = Db.UserComments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.CommentID = instance.CommentID;
                Db.UserComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserComment(int idUserComment)
        {
            UserComment instance = Db.UserComments.FirstOrDefault(p => p.ID == idUserComment);
            if (instance != null)
            {
                Db.UserComments.DeleteOnSubmit(instance);
                Db.UserComments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}