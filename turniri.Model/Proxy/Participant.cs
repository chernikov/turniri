using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;

namespace turniri.Model
{ 
    public partial class Participant
    {
        public Rating Rating
        {
            get
            {
                var rating = User.Ratings.Where(p => p.GameID == Tournament.GameID).FirstOrDefault();
                if (rating != null)
                {
                    return rating;
                }
                return new Rating();
            }
        }

        public string FullImagePath18
        {
            get
            {
                return ImagePath18 ?? User.FullAvatarPath18;
            }
        }
        
        public string FullImagePath26
        {
            get
            {
                return ImagePath26 ?? User.FullAvatarPath26;
            }
        }

        public string FullImagePath30
        {
            get
            {
                return ImagePath30 ?? User.FullAvatarPath30;
            }
        }

        public bool IsTeam
        {
            get
            {
                if (Tournament != null)
                {
                    return Tournament.IsTeam;
                }
                return false;
            }
        }

        public string ActualName
        {
            get
            {
                if (IsTeam)
                {
                    if (TeamID.HasValue)
                    {
                        if (!string.IsNullOrWhiteSpace(Name))
                        {
                            return Name + " (" + Team.Name +")";
                        }
                        return Team.Name;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(Name))
                        {
                            return Name + " (" +User.Login + " (ê))";
                        }
                        return User.Login + " (ê)";
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Name))
                    {
                        return Name + " (" + User.Login + ")";
                    }
                }
                return User.Login;
            }
        }

        public Group Group
        {
            get
            {
                return User.GroupByGame(Tournament.GameID);
            }
        }

        public int TotalRating(int gameId)
        {
            if (TeamID != null) 
            {
                var group = User.Groups.FirstOrDefault(p => p.GameID == gameId) ?? User.GroupByGame(gameId);

                if (group != null) 
                {
                    return group.TotalRating;
                }
                return 0;
            }
            var rating = User.Rating(gameId);
            if (rating != null)
            {
                return rating.TotalScore;
            }
            return 0;
        }

        public int RatingLevel(int gameId)
        {
            if (TeamID != null)
            {
                var group = User.Groups.FirstOrDefault(p => p.GameID == gameId) ?? User.GroupByGame(gameId);

                if (group != null)
                {
                    return group.TotalRating;
                }
                return 0;
            }
            var rating = User.Rating(gameId);
            if (rating != null)
            {
                return rating.Level;
            }
            return 0;
        }
	}
}