using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Reputation
    {
		public enum ReputationTypeEnum : int  
		{
		    Connection = 0x01,
            Honest = 0x02,
            Responsibility = 0x03
		}
	}
}