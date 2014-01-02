using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<LeagueSeason> LeagueSeasons
        {
            get
            {
                return Db.LeagueSeasons;
            }
        }

        public bool CreateLeagueSeason(LeagueSeason instance)
        {
            if (instance.ID == 0)
            {
                instance.Status = (int)LeagueSeason.StatusEnum.Created;
                Db.LeagueSeasons.InsertOnSubmit(instance);
                Db.LeagueSeasons.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateLeagueSeason(LeagueSeason instance)
        {
            var cache = Db.LeagueSeasons.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.LeagueID = instance.LeagueID;
                cache.Name = instance.Name;
                cache.Image = instance.Image;
				cache.StartDate = instance.StartDate;
				cache.EndMainTourDate = instance.EndMainTourDate;
				cache.EndDate = instance.EndDate;
                Db.LeagueSeasons.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateLeagueSeasonStatus(LeagueSeason instance, LeagueSeason.StatusEnum status)
        {
            var cache = Db.LeagueSeasons.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Status = (int)status;
                Db.LeagueSeasons.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveLeagueSeason(int idLeagueSeason)
        {
            LeagueSeason instance = Db.LeagueSeasons.FirstOrDefault(p => p.ID == idLeagueSeason);
            if (instance != null)
            {
                Db.LeagueSeasons.DeleteOnSubmit(instance);
                Db.LeagueSeasons.Context.SubmitChanges();
                return true;
            }
            return false;
        }
       public bool LeagueSeasonRecalculatePlaces(int idLeagueSeason) 
       {
           var leagueSeason = Db.LeagueSeasons.FirstOrDefault(p => p.ID == idLeagueSeason);
           if (leagueSeason != null)
           {
               var list = new List<Tournament.LeagueStatisticGroup>();
               foreach (var tournament in leagueSeason.Tournaments.ToList())
               {
                   foreach (var participant in tournament.SubPlayers)
                   {
                       var statistic = new Tournament.LeagueStatisticGroup()
                       {
                           LeagueParticipant = participant.LeagueParticipants.FirstOrDefault(p => p.LeagueLevelID == tournament.LeagueLevelID
                               && p.LeagueSeasonID == tournament.LeagueSeasonID),
                           Participant = participant,
                           Points = tournament.TotalWinGame(participant.ID, null) * 3 + tournament.TotalDrawnGame(participant.ID, null),
                           LevelPoints = participant.TotalRating(leagueSeason.League.GameID),
                           Level = tournament.LeagueLevel
                       };
                       list.Add(statistic);
                   }
               }

               foreach (var level in leagueSeason.League.LeagueLevels)
               {
                   var subStatistic = list.Where(p => p.Level.ID == level.ID);

                   var place = 1;
                   foreach (var subStatisticItem in subStatistic.OrderByDescending(p => p.Points).ThenByDescending(p => p.LevelPoints))
                   {
                       subStatisticItem.LeagueParticipant.Place = place;
                       place++;
                       UpdateLeagueParticipant(subStatisticItem.LeagueParticipant);
                   }
               }
               return true;
           }
           return false;
       }
    }
}