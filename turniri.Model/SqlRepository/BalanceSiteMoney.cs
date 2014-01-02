using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public BalanceSiteMoney BalanceSiteMoney
        {
            get
            {
                return Db.BalanceSiteMoneys.FirstOrDefault();
            }
        }

        public BalanceSiteMoney BalanceSiteRecharge
        {
            get
            {
                return Db.BalanceSiteMoneys.Skip(1).FirstOrDefault();
            }
        }


        private bool SaveBalanceSiteMoney(BalanceSiteMoney instance)
        {
            var cache = Db.BalanceSiteMoneys.FirstOrDefault();
            if (cache != null)
            {
                cache.MoneyGold = instance.MoneyGold;
                cache.LastUpdateDate = DateTime.Now;
            }
            return true;
        }

        private bool SaveBalanceSiteRecharge(BalanceSiteMoney instance)
        {
            var cache = Db.BalanceSiteMoneys.Skip(1).FirstOrDefault();
            if (cache != null)
            {
                cache.MoneyGold = instance.MoneyGold;
                cache.LastUpdateDate = DateTime.Now;
            }
            return true;
        }

    }
}