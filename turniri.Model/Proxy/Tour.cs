using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Tour
    {
		public enum TourTypeEnum
		{
		    RoundRobin = 0x01,
            SingleElimination = 0x02,
            DoubleEliminationMaster = 0x03, 
            DoubleEliminationSecondary = 0x04,
            Group = 0x05,
            DoubleEliminationSuperFinal = 0x06
		}

        public bool IsPlayoffType
        {
            get
            {
                return TourType == (int)TourTypeEnum.SingleElimination
                    || TourType == (int)TourTypeEnum.DoubleEliminationMaster
                    || TourType == (int)TourTypeEnum.DoubleEliminationSecondary
                    || TourType == (int)TourTypeEnum.DoubleEliminationSuperFinal;
            }
        }

        public IEnumerable<Match> SubMatches
        {
            get
            {
                return Matches.ToList();
            }
        }
	}
}