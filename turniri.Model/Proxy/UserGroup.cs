using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class UserGroup
    {
        public enum StatusEnum
        {
            Asked = 0x01, 
            Granded = 0x02
        }

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

        public bool InRole(string role)
        {
            return User.UserRoles.Any(p => string.Compare(p.Role.Code, role, true) == 0
                && p.UserRoleGroups.Any(r => r.GroupID == GroupID));
        }
	}
}