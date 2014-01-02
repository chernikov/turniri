using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<MoneyFee> MoneyFees
        {
            get
            {
                return Db.MoneyFees;
            }
        }

        public bool CreateMoneyFee(MoneyFee instance)
        {
            if (instance.ID == 0)
            {
                Db.MoneyFees.InsertOnSubmit(instance);
                Db.MoneyFees.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMoneyFee(MoneyFee instance)
        {
            var cache = Db.MoneyFees.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Type = instance.Type;
				cache.PercentFee = instance.PercentFee;
                Db.MoneyFees.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveMoneyFee(int idMoneyFee)
        {
            MoneyFee instance = Db.MoneyFees.FirstOrDefault(p => p.ID == idMoneyFee);
            if (instance != null)
            {
                Db.MoneyFees.DeleteOnSubmit(instance);
                Db.MoneyFees.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}