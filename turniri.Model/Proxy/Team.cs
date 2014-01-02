using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Team
    {
        public string FullImagePath30
        {
            get { return ImagePath30 ?? "/Media/images/default_avatar_30.png"; }
        }

        public string FullImagePath26
        {
            get { return ImagePath26 ?? "/Media/images/default_avatar_26.png"; }
        }

        public string FullImagePath18
        {
            get { return ImagePath18 ?? "/Media/images/default_avatar_18.png"; }
        }


        public bool CanTakePart(User user)
        {
            if (user != null && !IsClosed && !IsGroup)
            {
                if (!UserTeams.Any(p => p.UserID == user.ID))
                {
                    if (Participants.Any())
                    {
                        //если где-то уже не зарегистрирован
                        return !Participants.First().Tournament.Participants.
                            Where(p => p.TeamID != null).
                            Select(p => p.Team).
                            SelectMany(p => p.UserTeams).
                            Any(p => p.UserID == user.ID && (p.IsCaptain || p.Accepted))
                            &&
                            !Participants.First().Tournament.Participants.Any(p => p.UserID == user.ID);
                    }
                }
            }
            return false;
        }

        public Group Group
        {
            get
            {
                if (Participants.Any())
                {
                    return Participants.First().Group;
                }
                return null;
            }
        }

        public bool IsGroup
        {
            get
            {
                if (Participants.Any())
                {
                    return Participants.First().Tournament.IsGroup;
                }
                return false;
            }
        }

        public bool IsRegisteredUser(User user)
        {
            if (user != null )
            {
                if (UserTeams.Any(p => p.UserID == user.ID && p.Accepted))
                {
                    return true;
                };
            }
            return false;
        }

        public UserTeam GetByUserID(int userID)
        {
            return UserTeams.FirstOrDefault(p => p.UserID == userID);
        }

        public bool CanTakeOffPart(User user)
        {
            if (user != null && !IsClosed && !IsGroup)
            {
                if (UserTeams.Any(p => p.UserID == user.ID && !p.IsCaptain))
                {
                    return true;
                };
            }
            return false;
        }

        public User Captain
        {
            get
            {
                return Participants.First().User;
            }
        }

        public Tournament Tournament
        {
            get
            {
                return Participants.First().Tournament;
            }
        }

        public IEnumerable<UserTeam> SubUsers
        {
            get
            {
                return UserTeams.AsEnumerable();
            }
        }

        public int PlayerCount
        {
            get
            {
                return UserTeams.Count();
            }
        }

        public int NotAccepted
        {
            get
            {
                return UserTeams.Count(p => !p.Accepted);
            }
        }

        public int AcceptedCount
        {
            get
            {
                return UserTeams.Count(p => p.Accepted);
            }
        }

        public bool AnyTeamReplacements
        {
            get
            {
                return TeamReplacements.Any();
            }
        }

        public IEnumerable<TeamReplacement> SubTeamReplacements
        {
            get
            {
                return TeamReplacements.AsEnumerable();
            }
        }

        public IEnumerable<User> Roaster(int matchID)
        {
            return MatchRoasters.Where(p => p.MatchID == matchID).Select(p => p.User).ToList();
            
        }
	}
}