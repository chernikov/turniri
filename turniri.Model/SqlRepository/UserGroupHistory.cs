using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<UserGroupHistory> UserGroupHistories
        {
            get
            {
                return Db.UserGroupHistories;
            }
        }

        private bool CreateUserGroupHistory(UserGroupHistory instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.UserGroupHistories.InsertOnSubmit(instance);
                Db.UserGroupHistories.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        private bool ExitUserGroupHistory(UserGroupHistory instance)
        {
            var cache = Db.UserGroupHistories.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.ExitDate = DateTime.Now;
                Db.UserGroupHistories.Context.SubmitChanges();
                return true;
            }

            return false;
        }
    }
}