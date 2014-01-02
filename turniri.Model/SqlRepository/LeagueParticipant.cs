using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<LeagueParticipant> LeagueParticipants
        {
            get
            {
                return Db.LeagueParticipants;
            }
        }

        public bool CreateLeagueParticipant(LeagueParticipant instance)
        {
            if (instance.ID == 0)
            {
                Db.LeagueParticipants.InsertOnSubmit(instance);
                Db.LeagueParticipants.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateLeagueParticipant(LeagueParticipant instance)
        {
            var cache = Db.LeagueParticipants.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.LeagueSeasonID = instance.LeagueSeasonID;
				cache.LeagueLevelID = instance.LeagueLevelID;
				cache.ParticipantID = instance.ParticipantID;
				cache.Place = instance.Place;
                Db.LeagueParticipants.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveLeagueParticipant(int idLeagueParticipant)
        {
            LeagueParticipant instance = Db.LeagueParticipants.FirstOrDefault(p => p.ID == idLeagueParticipant);
            if (instance != null)
            {
                Db.LeagueParticipants.DeleteOnSubmit(instance);
                Db.LeagueParticipants.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}