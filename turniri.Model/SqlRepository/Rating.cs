using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Rating> Ratings
        {
            get
            {
                return Db.Ratings;
            }
        }

        private bool CreateRating(Rating instance)
        {
            if (instance.ID == 0)
            {
                Db.Ratings.InsertOnSubmit(instance);
                Db.Ratings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        private bool UpdateRating(int idRating)
        {
            var rating = Db.Ratings.FirstOrDefault(p => p.ID == idRating);
            if (rating != null)
            {
                var ratingsAny = Db.RatingDetails.Any(p => p.RatingID == rating.ID);
                if (ratingsAny)
                {
                    rating.TotalScore = Db.RatingDetails.Where(p => p.RatingID == rating.ID).Sum(p => p.Score);
                    rating.Level = User.LevelByPoints(rating.TotalScore);
                }
                else
                {
                    rating.TotalScore = 0;
                    rating.Level = User.LevelByPoints(0);
                }
                Db.Ratings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        private bool RemoveRating(int idRating)
        {
            var cache = Db.Ratings.FirstOrDefault(p => p.ID == idRating);
            if (cache != null)
            {
                cache.IsActive = false;
                Db.Ratings.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        private bool RecoverRating(int idRating)
        {
            var cache = Db.Ratings.FirstOrDefault(p => p.ID == idRating);
            if (cache != null)
            {
                cache.IsActive = true;
                Db.Ratings.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}