using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Model
{
    public partial class UserRole
    {
        public bool AnyUserRoleGames
        {
            get
            {
                return UserRoleGames.Any();
            }
        }

        public IEnumerable<UserRoleGame> SubUserRoleGames
        {
            get
            {
                return UserRoleGames.AsEnumerable();
            }
        }

        public bool AnyUserRoleTournaments
        {
            get
            {
                return UserRoleTournaments.Any();
            }
        }

        public IEnumerable<UserRoleTournament> SubUserRoleTournaments
        {
            get
            {
                return UserRoleTournaments.AsEnumerable();
            }
        }
    }
}