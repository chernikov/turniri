using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Platform
    {

        public IEnumerable<Game> MainGames
        {
            get { return Games.Where(p => p.IsMain).ToList(); }
        }
		
	}
}