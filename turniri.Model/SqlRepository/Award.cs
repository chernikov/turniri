using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Award> Awards
        {
            get
            {
                return Db.Awards;
            }
        }

        public bool CreateAward(Award instance)
        {
            if (instance.ID == 0)
            {
                Db.Awards.InsertOnSubmit(instance);
                Db.Awards.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateAward(Award instance)
        {
            var cache = Db.Awards.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TournamentID = instance.TournamentID;
				cache.Name = instance.Name;
				cache.IconPath = instance.IconPath;
				cache.IsSpecial = instance.IsSpecial;
                cache.Point = instance.Point;
                cache.MoneyGoldPercent = instance.MoneyGoldPercent;
                cache.MoneyWood = instance.MoneyWood;
                cache.MoneyCrystal = instance.MoneyCrystal;
                cache.AwardedDate = instance.AwardedDate;
                Db.Awards.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveAward(int idAward)
        {
            Award instance = Db.Awards.FirstOrDefault(p => p.ID == idAward);
            if (instance != null)
            {
                Db.Awards.DeleteOnSubmit(instance);
                Db.Awards.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}