using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserAward> UserAwards
        {
            get
            {
                return Db.UserAwards;
            }
        }

        public bool CreateUserAward(UserAward instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.UserAwards.InsertOnSubmit(instance);
                Db.UserAwards.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserAward(UserAward instance)
        {
            var cache = Db.UserAwards.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.AwardID = instance.AwardID;
				cache.UserID = instance.UserID;
				cache.AddedDate = instance.AddedDate;
                Db.UserAwards.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserAward(int idUserAward)
        {
            UserAward instance = Db.UserAwards.FirstOrDefault(p => p.ID == idUserAward);
            if (instance != null)
            {
                Db.UserAwards.DeleteOnSubmit(instance);
                Db.UserAwards.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}