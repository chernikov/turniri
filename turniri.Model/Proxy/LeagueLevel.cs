using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class LeagueLevel
    {
        public IEnumerable<Tournament> SubTournaments(LeagueSeason leagueSeason)
        {
            return Tournaments.Where(p => p.LeagueSeasonID == leagueSeason.ID).AsEnumerable();
        }

        public IEnumerable<LeagueParticipant> SubParticipants(LeagueSeason leagueSeason)
        {
            return LeagueParticipants.Where(p => p.LeagueSeasonID == leagueSeason.ID).AsEnumerable();
        }

        public int CountParticipants(LeagueSeason leagueSeason)
        {
            return LeagueParticipants.Count(p => p.LeagueSeasonID == leagueSeason.ID);
        }

        public int CountTournaments(LeagueSeason leagueSeason)
        {
            return Tournaments.Count(p => p.LeagueSeasonID == leagueSeason.ID);
        }
	}
}