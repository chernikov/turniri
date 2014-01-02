using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class League
    {
        public IEnumerable<LeagueLevel> SubLevels
        {
            get
            {
                return LeagueLevels.OrderBy(p => p.Level).ToList();
            }
        }

        public IEnumerable<LeagueLevel> SubLevelsWithPlayers
        {
            get
            {
                return LeagueLevels.Where(p => p.LeagueParticipants.Any()).OrderBy(p => p.Level).ToList();
            }
        }

        public LeagueSeason LastSeason
        {
            get
            {
                return LeagueSeasons.OrderByDescending(p => p.EndDate).FirstOrDefault();
            }
        }

        public LeagueSeason LastStartedSeason
        {
            get
            {
                return LeagueSeasons.OrderBy(p => p.Status == (int)LeagueSeason.StatusEnum.Created ? 1 : 0).ThenByDescending(p => p.EndDate).FirstOrDefault();
            }
        }

        public LeagueSeason BeforeSeason
        {
            get
            {
                return LeagueSeasons.OrderByDescending(p => p.EndDate).Skip(1).FirstOrDefault();
            }
        }

        public bool CanChangeTournamentData
        {
            get
            {
                return !LeagueSeasons.Any() || (!LeagueSeasons.SelectMany(p => p.Tournaments).Any() && !LeagueSeasons.SelectMany(p => p.LeagueParticipants).Any());
            }
        }

        public bool AnyLevel
        {
            get
            {
                return LeagueLevels.Any();
            }
        }

        public bool AnyPlayer
        {
            get
            {
                return  LeagueLevels.Any(p => p.LeagueParticipants.Any());
            }
        }
	}
}