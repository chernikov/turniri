using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<MoneyWithdraw> MoneyWithdraws
        {
            get
            {
                return Db.MoneyWithdraws;
            }
        }

        public bool CreateMoneyWithdraw(MoneyWithdraw instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.MoneyWithdraws.InsertOnSubmit(instance);
                Db.MoneyWithdraws.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveMoneyWithdraw(int idMoneyWithdraw)
        {
            var instance = Db.MoneyWithdraws.FirstOrDefault(p => p.ID == idMoneyWithdraw);
            if (instance != null)
            {
                Db.MoneyWithdraws.DeleteOnSubmit(instance);
                Db.MoneyWithdraws.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool SubmitMoneyWithdraw(int idMoneyWithdraw)
        {
            var instance = Db.MoneyWithdraws.FirstOrDefault(p => p.ID == idMoneyWithdraw);
            if (instance != null)
            {
                instance.Submitted = true;

                var recharge = new Recharge()
                {
                    UserID = instance.UserID,
                    MoneyDetailID = instance.MoneyDetailID,
                    Sum = -instance.Sum,
                    Description = "Вывод денег",
                    IsSubmitted = true
                };
                CreateRecharge(recharge);
                Db.MoneyWithdraws.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}