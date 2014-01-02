using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<LeagueLevel> LeagueLevels
        {
            get
            {
                return Db.LeagueLevels;
            }
        }

        public bool CreateLeagueLevel(LeagueLevel instance)
        {
            if (instance.ID == 0)
            {
                Db.LeagueLevels.InsertOnSubmit(instance);
                Db.LeagueLevels.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateLeagueLevel(LeagueLevel instance)
        {
            var cache = Db.LeagueLevels.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.LeagueID = instance.LeagueID;
                cache.Image = instance.Image;
				cache.Name = instance.Name;
				cache.Level = instance.Level;
				cache.Quantity = instance.Quantity;
                Db.LeagueLevels.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveLeagueLevel(int idLeagueLevel)
        {
            LeagueLevel instance = Db.LeagueLevels.FirstOrDefault(p => p.ID == idLeagueLevel);
            if (instance != null)
            {
                Db.LeagueLevels.DeleteOnSubmit(instance);
                Db.LeagueLevels.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}