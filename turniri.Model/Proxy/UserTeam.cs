using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{
    public partial class UserTeam
    {
        public Rating Rating
        {
            get
            {
                var rating = User.Ratings.Where(p => p.GameID == Team.Tournament.GameID).FirstOrDefault();
                if (rating != null)
                {
                    return rating;
                }
                return new Rating();
            }
        }
    }
}