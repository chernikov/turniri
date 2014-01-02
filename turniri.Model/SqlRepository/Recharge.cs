using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Recharge> Recharges
        {
            get
            {
                return Db.Recharges;
            }
        }

        public bool CreateRecharge(Recharge instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.AdditionalInfo = instance.AdditionalInfo ?? string.Empty;
                Db.Recharges.InsertOnSubmit(instance);
                Db.Recharges.Context.SubmitChanges();

                RecalculateBalanceRecharge();
                return true;
            }

            return false;
        }

        public bool UpdateRecharge(Recharge instance)
        {
            var cache = Db.Recharges.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.MoneyDetailID = instance.MoneyDetailID;
                cache.IsSubmitted = instance.IsSubmitted;
                cache.Description = instance.Description;
                cache.Provider = instance.Provider;
                cache.AdditionalInfo = instance.AdditionalInfo;
                Db.Recharges.Context.SubmitChanges();

                RecalculateBalanceRecharge();
                return true;
            }

            return false;
        }

        public bool RemoveRecharge(int id)
        {
            var recharge = Db.Recharges.FirstOrDefault(p => p.ID == id);
            if (recharge != null)
            {
                Db.Recharges.DeleteOnSubmit(recharge);
                Db.Recharges.Context.SubmitChanges();
            }
            return true;
        }

        public bool RemoveUnSubmittedRecharge()
        {
            //QIWI Нельзя
            var unsubmitted = Db.Recharges.Where(p => !p.IsSubmitted && p.Provider != (int)Recharge.ProviderType.Qiwi);
            Db.Recharges.DeleteAllOnSubmit(unsubmitted);
            Db.Recharges.Context.SubmitChanges();
            return true;
        }

        private void RecalculateBalanceRecharge()
        {
            double totalSum = 0;
            if (Recharges.Any(p => p.IsSubmitted))
            {
                totalSum = Recharges.Where(p => p.IsSubmitted).Sum(p => p.Sum);
            }
            var balanceSiteMoney = new BalanceSiteMoney()
            {
                MoneyGold = totalSum
            };
            SaveBalanceSiteRecharge(balanceSiteMoney);

        }

    }
}