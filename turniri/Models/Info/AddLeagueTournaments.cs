using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Models.Info
{
    public class AddLeagueTournaments
    {
        public int LeagueSeasonID { get; set; }

        public int LeagueLevelID { get; set; }

        public string LeagueName { get; set; }

        public string LeagueSeasonName { get; set; }

        public string LeagueLevelName { get; set; }

        public int ParticipantCount { get; set; }

        public int Count { get; set; }

        public bool AutoFillParticipants { get; set; }
    }
}