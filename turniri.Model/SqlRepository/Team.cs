using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Team> Teams
        {
            get
            {
                return Db.Teams;
            }
        }

        public bool CreateTeam(Team instance, int? hotReplacement)
        {
            if (instance.ID == 0)
            {
                instance.HotReplacement = hotReplacement;
                instance.AddedDate = DateTime.Now;
                Db.Teams.InsertOnSubmit(instance);
                Db.Teams.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTeam(Team instance)
        {
            var cache = Db.Teams.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.ImagePath18 = instance.ImagePath18;
				cache.ImagePath26 = instance.ImagePath26;
				cache.ImagePath30 = instance.ImagePath30;
                Db.Teams.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool CloseTeam(int idTeam)
        {
            var cache = Db.Teams.FirstOrDefault(p => p.ID == idTeam);
            if (cache != null)
            {
                foreach (var userTeam in cache.UserTeams.Where(p => !p.Accepted).ToList())
                {
                    RemoveUserTeam(userTeam.ID);
                }
                var perticipant = Db.Participants.FirstOrDefault(p => p.TeamID == idTeam);
                var tournamentID = perticipant.TournamentID;
                foreach (var userTeam in cache.UserTeams.Where(p => p.Accepted).ToList())
                {
                    var list = UserTeams.Where(p => p.UserID == userTeam.UserID
                        && p.Team.Participants.Any(r => r.TournamentID == tournamentID)
                        && p.TeamID != cache.ID).ToList();

                    Db.UserTeams.DeleteAllOnSubmit(list);
                    Db.UserTeams.Context.SubmitChanges();
                }

                cache.IsClosed = true;
                Db.Teams.Context.SubmitChanges();

                return true;
            }

            return false;
        }

        public bool RemoveTeam(int idTeam)
        {
            Team instance = Db.Teams.FirstOrDefault(p => p.ID == idTeam);
            if (instance != null)
            {
                
                Db.Teams.DeleteOnSubmit(instance);
                Db.Teams.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}