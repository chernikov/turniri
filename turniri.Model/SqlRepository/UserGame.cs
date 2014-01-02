using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserGame> UserGames
        {
            get
            {
                return Db.UserGames;
            }
        }

        public bool CreateUserGame(UserGame instance)
        {
            if (instance.ID == 0)
            {
                var exist = Db.UserGames.FirstOrDefault(p => p.GameID == instance.GameID && p.UserID == instance.UserID);
                if (exist != null)
                {
                    Db.UserGames.DeleteOnSubmit(exist);
                    Db.UserGames.Context.SubmitChanges();
                }
                Db.UserGames.InsertOnSubmit(instance);
                Db.UserGames.Context.SubmitChanges();

                var rating = Db.Ratings.FirstOrDefault(p => p.UserID == instance.UserID && p.GameID == instance.GameID);
                if (rating == null)
                {
                    var newRating = new Rating
                                        {
                                            GameID = instance.GameID,
                                            UserID = instance.UserID,
                                            TotalScore = 0,
                                            Level = 1,
                                            IsActive = true
                                        };
                    CreateRating(newRating);
                } else
                {
                    RecoverRating(rating.ID);
                }
                return true;
            }

            return false;
        }
 
        public bool RemoveUserGame(int idUserGame)
        {
            var instance = Db.UserGames.FirstOrDefault(p => p.ID == idUserGame);
            if (instance != null)
            {
               
                var rating = Db.Ratings.FirstOrDefault(p => p.UserID == instance.UserID && p.GameID == instance.GameID);
                if (rating != null)
                {
                    RemoveRating(rating.ID);
                }
                Db.UserGames.DeleteOnSubmit(instance);
                Db.UserGames.Context.SubmitChanges();

                return true;
            }
            return false;
        }
    }
}