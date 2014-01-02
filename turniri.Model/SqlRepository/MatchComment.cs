using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<MatchComment> MatchComments
        {
            get
            {
                return Db.MatchComments;
            }
        }

        public bool CreateMatchComment(MatchComment instance)
        {
            if (instance.ID == 0)
            {
                Db.MatchComments.InsertOnSubmit(instance);
                Db.MatchComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMatchComment(MatchComment instance)
        {
            var cache = Db.MatchComments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.MatchID = instance.MatchID;
				cache.CommentID = instance.CommentID;
                Db.MatchComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveMatchComment(int idMatchComment)
        {
            MatchComment instance = Db.MatchComments.FirstOrDefault(p => p.ID == idMatchComment);
            if (instance != null)
            {
                Db.MatchComments.DeleteOnSubmit(instance);
                Db.MatchComments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}