using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class TeamReplacement
    {
        public User UserIn
        {
            get {
                return User;
            }
            set
            {
                User = value;
            }
        }

        public User UserOut 
        {
            get
            {
                return User1;
            }
            set
            {
                User1 = value;
            }
        }
	}
}