using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ForumLog> ForumLogs
        {
            get
            {
                return Db.ForumLogs;
            }
        }

        public bool CreateForumLog(ForumLog instance)
        {
            if (instance.ID == 0)
            {
                var exist = Db.ForumLogs.FirstOrDefault(p => p.ForumID == instance.ForumID && p.UserID == instance.UserID);

                if (exist == null)
                {
                    instance.ReadDate = DateTime.Now;
                    Db.ForumLogs.InsertOnSubmit(instance);
                }
                else
                {
                    exist.ReadDate = DateTime.Now;
                }
                Db.ForumLogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}