using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserRoleTournament> UserRoleTournaments
        {
            get
            {
                return Db.UserRoleTournaments;
            }
        }

        public bool CreateUserRoleTournament(UserRoleTournament instance)
        {
            if (instance.ID == 0)
            {
                Db.UserRoleTournaments.InsertOnSubmit(instance);
                Db.UserRoleTournaments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserRoleTournament(UserRoleTournament instance)
        {
            var cache = Db.UserRoleTournaments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserRoleID = instance.UserRoleID;
				cache.TournamentID = instance.TournamentID;
                Db.UserRoleTournaments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserRoleTournament(int idUserRoleTournament)
        {
            UserRoleTournament instance = Db.UserRoleTournaments.FirstOrDefault(p => p.ID == idUserRoleTournament);
            if (instance != null)
            {
                var role = instance.UserRole;
                Db.UserRoleTournaments.DeleteOnSubmit(instance);
                Db.UserRoleTournaments.Context.SubmitChanges();
                if (!role.AnyUserRoleTournaments)
                {
                    Db.UserRoles.DeleteOnSubmit(role);
                    Db.UserRoles.Context.SubmitChanges();
                }
                return true;
            }
            return false;
        }
    }
}