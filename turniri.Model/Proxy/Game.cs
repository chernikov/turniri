using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Game
    {
        public enum GameCategoryEnum
        {
            Common = 0x01,
            Soccer = 0x02
        }

		public enum GameTypeEnum
		{
		    Points = 0x01,
            WinLose = 0x02
		}

        public IEnumerable<Tournament> SubTournaments
        {
            get
            {
                return Tournaments.ToList();
            }
        }

        public IEnumerable<Tournament> SubFutureTournaments
        {
            get 
            {
                return Tournaments.Where(p => p.Status == (int)Tournament.StatusEnum.Created || p.Status == (int)Tournament.StatusEnum.Allocated).ToList();
            }
        }

        public IEnumerable<Tournament> SubPlaingTournaments
        {
            get
            {
                return Tournaments.Where(p => p.Status == (int)Tournament.StatusEnum.InGame).ToList();
            }
        }

        public IEnumerable<Tournament> SubPlayedOutTournaments
        {
            get
            {
                return Tournaments.Where(p => p.Status == (int)Tournament.StatusEnum.PlayedOut).ToList();
            }
        }

        public IEnumerable<Award> AwardsPage(int page = 1, int itemPerPage = 20)
        {
            return Tournaments.SelectMany(p => p.Awards).Where(p => p.UserAwards.Any()).OrderByDescending(p => p.ID).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int AwardsCountPage(int itemPerPage = 20)
        {
            var totalCount = Tournaments.SelectMany(p => p.Awards).Count();
            return totalCount / itemPerPage + (totalCount % itemPerPage != 0 ? 1 : 0);
        }	

        public bool TournamentsAnyCreated
        {
            get 
            {
                return Tournaments.Any(p => p.Status == (int)turniri.Model.Tournament.StatusEnum.Created || p.Status == (int)turniri.Model.Tournament.StatusEnum.Allocated);
            }
        }
           
        public bool TournamentsAnyPlaying
        {
            get 
            {
                return Tournaments.Any(p => p.Status == (int)turniri.Model.Tournament.StatusEnum.InGame);
            }
        }
        public bool TournamentsAnyPlayed
        {
            get
            {
                return Tournaments.Any(p => p.Status == (int)turniri.Model.Tournament.StatusEnum.PlayedOut);
            }
        }

        public IQueryable<User> Admins
        {
            get
            {
                return UserRoleGames.Where(p => p.UserRole.Role.Code == "game_admin").Select(p => p.UserRole.User).AsQueryable();
            }
        }

        public IQueryable<User> Moderators
        {
            get
            {
                return UserRoleGames.Where(p => p.UserRole.Role.Code == "game_moderator").Select(p => p.UserRole.User).AsQueryable();
            }
        }

        public IEnumerable<Group> GroupPage(int page = 1, int itemPerPage = 10)
        {
            return Groups.Where(p => p.State == (int)Group.StateType.Live).OrderByDescending(p => p.TotalRating).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public bool AnyGroups
        {
            get
            {
                return Groups.Any(p => p.State == (int)Group.StateType.Live);
            }
        }

        public int GroupCount
        {
            get
            {
                return Groups.Count(p => p.State == (int)Group.StateType.Live);
            }
        }

        public int GroupCountPage(int itemPerPage = 10)
        {
            return GroupCount / itemPerPage + (GroupCount % itemPerPage != 0 ? 1 : 0);
        }

        public bool AnyLeague
        {
            get
            {
               return Leagues.Any();
            }
        }
	}
}