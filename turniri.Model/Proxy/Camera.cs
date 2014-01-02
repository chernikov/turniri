using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Camera
    {
        public string Desc
        {
            get
            {

                return string.Format("{0} {1} {2}", Name, Tournament.Name,  MatchID.HasValue ? (" матч: " + Match.Desc) : "");
            }
        }
	}
}