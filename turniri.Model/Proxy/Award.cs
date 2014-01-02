using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Award
    {
        public string FullIconPath
        {
            get { return !string.IsNullOrWhiteSpace(IconPath) ? IconPath : "/Media/images/default_award.png"; }
        }

        public string Description
        {
            get {
                if (IsSpecial)
                {
                    return Name;
                }
                return string.Format("{0}-е место в турнире {1}", Place, Tournament.Name);
            }
        }

        public Participant Player
        {
            get
            {
                return Tournament.Participants.FirstOrDefault(p => UserAwards.Any(r => r.UserID == p.UserID));
            }
        }

        public bool IsAwarded
        {
            get
            {
                return UserAwards.Any();
            }
        }

        public User User
        {
            get
            {
                if (IsAwarded)
                {
                    return UserAwards.First().User;
                }
                return null;

            }
        }
	}
}