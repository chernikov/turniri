using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<GroupRating> GroupRatings
        {
            get
            {
                return Db.GroupRatings;
            }
        }

        public bool CreateGroupRating(GroupRating instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.GroupRatings.InsertOnSubmit(instance);
                Db.GroupRatings.Context.SubmitChanges();
                UpdateGroupRating(instance.GroupID);

                return true;
            }

            return false;
        }

     

        public bool UpdateGroupRating(GroupRating instance)
        {
            var cache = Db.GroupRatings.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.GroupID = instance.GroupID;
				cache.MatchID = instance.MatchID;
				cache.Score = instance.Score;
				cache.AddedDate = instance.AddedDate;
				cache.Description = instance.Description;
                Db.GroupRatings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveGroupRating(int idGroupRating)
        {
            GroupRating instance = Db.GroupRatings.FirstOrDefault(p => p.ID == idGroupRating);
            if (instance != null)
            {
                Db.GroupRatings.DeleteOnSubmit(instance);
                Db.GroupRatings.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}