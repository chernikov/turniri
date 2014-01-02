using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<TournamentCondition> TournamentConditions
        {
            get
            {
                return Db.TournamentConditions;
            }
        }

        public bool CreateTournamentCondition(TournamentCondition instance)
        {
            if (instance.ID == 0)
            {
                Db.TournamentConditions.InsertOnSubmit(instance);
                Db.TournamentConditions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTournamentCondition(TournamentCondition instance)
        {
            var cache = Db.TournamentConditions.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.FirstName = instance.FirstName;
				cache.LastName = instance.LastName;
				cache.PlaystationID = instance.PlaystationID;
				cache.XboxGametag = instance.XboxGametag;
				cache.EAAccount = instance.EAAccount;
				cache.SteamAccount = instance.SteamAccount;
				cache.GarenaAccount = instance.GarenaAccount;
				cache.ICQ = instance.ICQ;
				cache.Skype = instance.Skype;
                cache.Vk = instance.Vk;
                Db.TournamentConditions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTournamentCondition(int idTournamentCondition)
        {
            TournamentCondition instance = Db.TournamentConditions.FirstOrDefault(p => p.ID == idTournamentCondition);
            if (instance != null)
            {
                Db.TournamentConditions.DeleteOnSubmit(instance);
                Db.TournamentConditions.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}