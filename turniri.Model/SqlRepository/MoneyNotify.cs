using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<MoneyNotify> MoneyNotifies
        {
            get
            {
                return Db.MoneyNotifies;
            }
        }

        public bool CreateMoneyNotify(MoneyNotify instance)
        {
            if (instance.ID == 0)
            {
                Db.MoneyNotifies.InsertOnSubmit(instance);
                Db.MoneyNotifies.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMoneyNotify(MoneyNotify instance)
        {
            var cache = Db.MoneyNotifies.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Type = instance.Type;
                cache.Data = instance.Data;
                cache.IsSuccess = instance.IsSuccess;
                Db.MoneyNotifies.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveMoneyNotify(int idMoneyNotify)
        {
            var instance = Db.MoneyNotifies.FirstOrDefault(p => p.ID == idMoneyNotify);
            if (instance != null)
            {
                Db.MoneyNotifies.DeleteOnSubmit(instance);
                Db.MoneyNotifies.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        public void ClearMoneyNotices()
        {
            Db.MoneyNotifies.DeleteAllOnSubmit(Db.MoneyNotifies.ToList());
            Db.MoneyNotifies.Context.SubmitChanges();
        }
    }
}