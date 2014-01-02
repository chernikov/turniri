using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace turniri.Models.Info
{
    public class TournamentSearch
    {
        public enum StateEnum
        {
            All = 0x01,
            Active = 0x02, 
            Archive = 0x03,
            Leagues = 0x04
        }

        public bool ShowLeague { get; set; }

        public StateEnum State { get; set; }

        public string SearchString { get; set; }

        public TournamentSearch()
        {
            State = StateEnum.All;
        }
    }
}