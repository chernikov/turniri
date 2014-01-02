using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<TournamentGroup> TournamentGroups
        {
            get
            {
                return Db.TournamentGroups;
            }
        }

        public bool CreateTournamentGroup(TournamentGroup instance)
        {
            if (instance.ID == 0)
            {
                Db.TournamentGroups.InsertOnSubmit(instance);
                Db.TournamentGroups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTournamentGroup(TournamentGroup instance)
        {
            var cache = Db.TournamentGroups.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TournamentID = instance.TournamentID;
				cache.Name = instance.Name;
                Db.TournamentGroups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTournamentGroup(int idTournamentGroup)
        {
            TournamentGroup instance = Db.TournamentGroups.FirstOrDefault(p => p.ID == idTournamentGroup);
            if (instance != null)
            {
                Db.TournamentGroups.DeleteOnSubmit(instance);
                Db.TournamentGroups.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}