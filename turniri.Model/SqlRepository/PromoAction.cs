using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PromoAction> PromoActions
        {
            get
            {
                return Db.PromoActions;
            }
        }

        public bool CreatePromoAction(PromoAction instance)
        {
            if (instance.ID == 0)
            {
                Db.PromoActions.InsertOnSubmit(instance);
                Db.PromoActions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePromoAction(PromoAction instance)
        {
            var cache = Db.PromoActions.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ProductID = instance.ProductID;
				cache.Name = instance.Name;
				cache.OnlyManager = instance.OnlyManager;
				cache.Type = instance.Type;
				cache.Amount = instance.Amount;
				cache.ValidDate = instance.ValidDate;
				cache.Closed = instance.Closed;
				cache.Reusable = instance.Reusable;
                Db.PromoActions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePromoAction(int idPromoAction)
        {
            PromoAction instance = Db.PromoActions.FirstOrDefault(p => p.ID == idPromoAction);
            if (instance != null)
            {
                Db.PromoActions.DeleteOnSubmit(instance);
                Db.PromoActions.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ChangeStatePromoAction(int idPromoAction, bool open)
        {
            var instance = Db.PromoActions.FirstOrDefault(p => p.ID == idPromoAction);
            if (instance != null)
            {
                instance.Closed = !open;
                Db.PromoActions.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}