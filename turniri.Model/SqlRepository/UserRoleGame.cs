using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserRoleGame> UserRoleGames
        {
            get
            {
                return Db.UserRoleGames;
            }
        }

        public bool CreateUserRoleGame(UserRoleGame instance)
        {
            if (instance.ID == 0)
            {
                Db.UserRoleGames.InsertOnSubmit(instance);
                Db.UserRoleGames.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserRoleGame(UserRoleGame instance)
        {
            var cache = Db.UserRoleGames.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserRoleID = instance.UserRoleID;
				cache.GameID = instance.GameID;
                Db.UserRoleGames.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserRoleGame(int idUserRoleGame)
        {
            UserRoleGame instance = Db.UserRoleGames.FirstOrDefault(p => p.ID == idUserRoleGame);
            if (instance != null)
            {
                var role = instance.UserRole;
                Db.UserRoleGames.DeleteOnSubmit(instance);
                Db.UserRoleGames.Context.SubmitChanges();
                if (!role.AnyUserRoleGames)
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