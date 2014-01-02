using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<MoneyDetail> MoneyDetails
        {
            get
            {
                return Db.MoneyDetails;
            }
        }

        public Guid CreateTripleMoneyDetail(MoneyDetail from, MoneyDetail to, MoneyDetail fee = null)
        {
            var guid = Guid.NewGuid();

            if (fee != null)
            {
                if (from.SumGold < 0)
                {
                    from.SumCrystal = fee.SumGold / 10;
                }
                if (to.SumGold < 0)
                {
                    to.SumCrystal = fee.SumGold / 10;
                }
            }
            CreateMoneyDetail(from, guid);
            CreateMoneyDetail(to, guid);
            if (fee != null)
            {
                CreateMoneyDetail(fee, guid);
            }
            return guid;
        }

        public bool CreateMoneyDetail(MoneyDetail instance, Guid uniqueGuid, int? moneyFeeID = null)
        {
            instance.MoneyFeeID = moneyFeeID;
            instance.Global = uniqueGuid;
            instance.AddedDate = DateTime.Now;
            instance.Description = instance.Description ?? string.Empty;
            Db.MoneyDetails.InsertOnSubmit(instance);
            Db.MoneyDetails.Context.SubmitChanges();

            if (instance.UserID.HasValue)
            {
                RecalculateUserMoney(instance.UserID.Value);
            }
            if (instance.GroupID.HasValue)
            {
                RecalculateGroupMoney(instance.GroupID.Value);
            }
            if (instance.IsFee)
            {
                RecalculateBalanceSite();
            }
            return true;
        }

        public bool SubmitMoney(Guid guid)
        {
            var moneyDetails = Db.MoneyDetails.Where(p => p.Global == guid);
            if (moneyDetails.Any())
            {
                foreach (var moneyDetail in moneyDetails.ToList())
                {
                    var cache = Db.MoneyDetails.FirstOrDefault(p => p.ID == moneyDetail.ID);
                    cache.Submited = true;
                    Db.MoneyDetails.Context.SubmitChanges();
                    if (cache.UserID.HasValue)
                    {
                        RecalculateUserMoney(cache.UserID.Value);
                    }
                    if (cache.GroupID.HasValue)
                    {
                        RecalculateGroupMoney(cache.GroupID.Value);
                    };
                }

                RecalculateBalanceSite();
                return true;
            }
            return false;
        }

        public bool DiscardMoney(Guid guid)
        {
            var moneyDetails = Db.MoneyDetails.Where(p => p.Global == guid);
            if (moneyDetails.Any())
            {
                Db.MoneyDetails.DeleteAllOnSubmit(moneyDetails.ToList());
                Db.MoneyDetails.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        private void RecalculateUserMoney(int userID)
        {
            double totalSumGold = 0;
            double totalSumWood = 0;
            double totalSumCrystal = 0;
            if (MoneyDetails.Any(p => p.UserID == userID && p.Submited))
            {
                totalSumGold = MoneyDetails.Where(p => p.UserID == userID && p.Submited).Sum(p => p.SumGold);
                totalSumWood = MoneyDetails.Where(p => p.UserID == userID && p.Submited).Sum(p => p.SumWood);
                totalSumCrystal = MoneyDetails.Where(p => p.UserID == userID && p.Submited).Sum(p => p.SumCrystal);
            }
            var user = Users.FirstOrDefault(p => p.ID == userID);
            if (user != null)
            {
                user.MoneyGold = totalSumGold;
                user.MoneyWood = totalSumWood;
                user.MoneyCrystal = totalSumCrystal;
                Db.Users.Context.SubmitChanges();
            }
        }

        private void RecalculateGroupMoney(int groupID)
        {
            double totalSumGold = 0;
            double totalSumWood = 0;
            double totalSumCrystal = 0;
            if (MoneyDetails.Any(p => p.GroupID == groupID && p.Submited))
            {
                totalSumGold = MoneyDetails.Where(p => p.GroupID == groupID && p.Submited).Sum(p => p.SumGold);
                totalSumWood = MoneyDetails.Where(p => p.GroupID == groupID && p.Submited).Sum(p => p.SumWood);
                totalSumCrystal = MoneyDetails.Where(p => p.GroupID == groupID && p.Submited).Sum(p => p.SumCrystal);
            }
            var group = Groups.FirstOrDefault(p => p.ID == groupID);
            if (group != null)
            {
                group.MoneyGold = totalSumGold;
                group.MoneyWood = totalSumWood;
                group.MoneyCrystal = totalSumCrystal;
                Db.Groups.Context.SubmitChanges();
            }
        }

        private void RecalculateBalanceSite()
        {
            double totalSumGold = 0;
            if (MoneyDetails.Any(p => p.IsFee && p.Submited))
            {
                totalSumGold = MoneyDetails.Where(p => p.IsFee && p.Submited).Sum(p => p.SumGold);
            }
            var balanceSiteMoney = new BalanceSiteMoney()
            {
                MoneyGold = totalSumGold
            };
            SaveBalanceSiteMoney(balanceSiteMoney);

        }

        public void RecalculateAll()
        {
            RecalculateBalanceSite();

            foreach (var userID in Users.Select(p => p.ID).ToList())
            {
                RecalculateUserMoney(userID);
            }

            foreach (var groupID in Groups.Select(p => p.ID).ToList())
            {
                RecalculateGroupMoney(groupID);
            }
        }

        public void RemoveUnsubmitted()
        {
            var moneyDetails = Db.MoneyDetails.Where(p => !p.Submited).ToList();
            Db.MoneyDetails.DeleteAllOnSubmit(moneyDetails);
            Db.MoneyDetails.Context.SubmitChanges();
        }

        public void RemoveMoneyTransaction(Guid guid)
        {
            var moneyDetails = Db.MoneyDetails.Where(p => p.Global == guid).ToList();
            Db.MoneyDetails.DeleteAllOnSubmit(moneyDetails);
            Db.MoneyDetails.Context.SubmitChanges();
        }

    }
}