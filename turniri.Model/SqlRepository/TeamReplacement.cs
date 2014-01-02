using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<TeamReplacement> TeamReplacements
        {
            get
            {
                return Db.TeamReplacements;
            }
        }
    }
}