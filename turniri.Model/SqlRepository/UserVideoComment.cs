using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserVideoComment> UserVideoComments
        {
            get
            {
                return Db.UserVideoComments;
            }
        }

        public bool CreateUserVideoComment(UserVideoComment instance)
        {
            if (instance.ID == 0)
            {
                Db.UserVideoComments.InsertOnSubmit(instance);
                Db.UserVideoComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserVideoComment(UserVideoComment instance)
        {
            var cache = Db.UserVideoComments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserVideoID = instance.UserVideoID;
				cache.CommentID = instance.CommentID;
                Db.UserVideoComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserVideoComment(int idUserVideoComment)
        {
            UserVideoComment instance = Db.UserVideoComments.FirstOrDefault(p => p.ID == idUserVideoComment);
            if (instance != null)
            {
                Db.UserVideoComments.DeleteOnSubmit(instance);
                Db.UserVideoComments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}