using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<RatingDetail> RatingDetails
        {
            get
            {
                return Db.RatingDetails;
            }
        }

        private bool CreateRatingDetail(RatingDetail instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.RatingDetails.InsertOnSubmit(instance);
                Db.RatingDetails.Context.SubmitChanges();

                UpdateRating(instance.RatingID);
                return true;
            }
            return false;
        }

        public void UpdateRatingDetailTournament(int idRatingDetail)
        {
            var instance = Db.RatingDetails.FirstOrDefault(p => p.ID == idRatingDetail);
            
            if (instance != null) {
                if (instance.MatchID != null && instance.Match.TournamentID != null)
                {
                    instance.TournamentID = instance.Match.TournamentID;
                    Db.RatingDetails.Context.SubmitChanges();
                }
            }
        }
    }
}