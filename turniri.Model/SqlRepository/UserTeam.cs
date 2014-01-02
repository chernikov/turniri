using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserTeam> UserTeams
        {
            get
            {
                return Db.UserTeams;
            }
        }

        public bool CreateUserTeam(UserTeam instance)
        {
            if (instance.ID == 0)
            {
                Db.UserTeams.InsertOnSubmit(instance);
                Db.UserTeams.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserTeam(UserTeam instance)
        {
            var cache = Db.UserTeams.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TeamID = instance.TeamID;
				cache.UserID = instance.UserID;
				cache.IsCaptain = instance.IsCaptain;
				cache.Accepted = instance.Accepted;
                Db.UserTeams.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserTeam(int idUserTeam)
        {
            var instance = Db.UserTeams.FirstOrDefault(p => p.ID == idUserTeam);
            if (instance != null)
            {
                Db.UserTeams.DeleteOnSubmit(instance);
                Db.UserTeams.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ReplaceUser(int idUserTeam, int idUser)
        {
            var instance = Db.UserTeams.FirstOrDefault(p => p.ID == idUserTeam);
            var user = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null && user != null && instance.Team.HotReplacement != 0)
            {

                var teamReplacement = new TeamReplacement()
                {
                    TeamID = instance.TeamID,
                    UserOutID = instance.UserID,
                    UserInID = user.ID,
                    AddedDate = DateTime.Now
                };

                
                Db.TeamReplacements.InsertOnSubmit(teamReplacement);
                instance.User = user;
                instance.Team.HotReplacement--;
                Db.UserTeams.Context.SubmitChanges();
                return true;
            }
            return false;
        
        }
    }
}