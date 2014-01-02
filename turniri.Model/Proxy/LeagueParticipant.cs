using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class LeagueParticipant
    {
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", LeagueLevel.Name, Place, Participant.ActualName);
        }
	}
}