using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class UserGroupHistory
    {
        public Rating Rating
        {
            get
            {
                return User.Rating(Group.GameID);
            }
        }

        public int Level
        {
            get
            {
                return User.Level(Group.GameID);
            }
        }
	}
}