using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<League> Leagues
        {
            get
            {
                return Db.Leagues;
            }
        }

        public bool CreateLeague(League instance)
        {
            if (instance.ID == 0)
            {
                Db.Leagues.InsertOnSubmit(instance);
                Db.Leagues.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateLeague(League instance)
        {
            var cache = Db.Leagues.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.GameID = instance.GameID;
                if (cache.CanChangeTournamentData)
                {
                    cache.IsGroup = instance.IsGroup;
                    cache.HostGuest = instance.HostGuest;
                    cache.CountRound = instance.CountRound;
                    cache.TeamCount = instance.TeamCount;
                    cache.HotReplacement = instance.HotReplacement;
                    cache.SingleWinPoint = instance.SingleWinPoint;
                    cache.SingleDrawPoint = instance.SingleDrawPoint;
                    cache.DoubleGoalInGuest = instance.DoubleGoalInGuest;
                }
				cache.Name = instance.Name;
                cache.Url = instance.Url;
				cache.Image = instance.Image;
                cache.Rules = instance.Rules;
                cache.Description = instance.Description;
                Db.Leagues.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveLeague(int idLeague)
        {
            League instance = Db.Leagues.FirstOrDefault(p => p.ID == idLeague);
            if (instance != null)
            {
                Db.Leagues.DeleteOnSubmit(instance);
                Db.Leagues.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}