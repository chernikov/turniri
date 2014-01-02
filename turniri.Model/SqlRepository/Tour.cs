using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Tour> Tours
        {
            get
            {
                return Db.Tours;
            }
        }

        public bool CreateTour(Tour instance)
        {
            if (instance.ID == 0)
            {
                Db.Tours.InsertOnSubmit(instance);
                Db.Tours.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTour(Tour instance)
        {
            var cache = Db.Tours.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TournamentID = instance.TournamentID;
				cache.TourType = instance.TourType;
				cache.RecommendDate = instance.RecommendDate;
				cache.Name = instance.Name;
                Db.Tours.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTour(int idTour)
        {
            Tour instance = Db.Tours.FirstOrDefault(p => p.ID == idTour);
            if (instance != null)
            {
                Db.Tours.DeleteOnSubmit(instance);
                Db.Tours.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}