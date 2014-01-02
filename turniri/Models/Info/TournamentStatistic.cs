using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;

namespace turniri.Models.Info
{
    public class TournamentStatistic
    {
        public Participant Participant { get; set; }

        public int Exp { get; set; }

        public int TotalGamed { get; set; }

        public int WinCount { get; set; }

        public int LoseCount { get; set; }

        public int DrawnCount { get; set; }

        public int PointWinCount { get; set; }

        public int GuestPoint { get; set; }

        public int PointLoseCount { get; set; }

        public int Diff
        {
            get
            {
                return PointWinCount - PointLoseCount;
            }
        }

        public int TournamentPoints
        {
            get
            {
                return WinCount * 3 + DrawnCount;
            }
        }
    }
}