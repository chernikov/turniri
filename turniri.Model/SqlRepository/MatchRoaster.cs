using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<MatchRoaster> MatchRoasters
        {
            get
            {
                return Db.MatchRoasters;
            }
        }

        public bool CreateMatchRoaster(MatchRoaster instance)
        {
            if (instance.ID == 0)
            {
                Db.MatchRoasters.InsertOnSubmit(instance);
                Db.MatchRoasters.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMatchRoaster(MatchRoaster instance)
        {
            var cache = Db.MatchRoasters.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.MatchID = instance.MatchID;
				cache.TeamID = instance.TeamID;
				cache.UserID = instance.UserID;
                Db.MatchRoasters.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveMatchRoaster(int idMatchRoaster)
        {
            MatchRoaster instance = Db.MatchRoasters.FirstOrDefault(p => p.ID == idMatchRoaster);
            if (instance != null)
            {
                Db.MatchRoasters.DeleteOnSubmit(instance);
                Db.MatchRoasters.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}