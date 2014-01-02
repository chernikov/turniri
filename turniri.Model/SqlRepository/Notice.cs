using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Notice> Notices
        {
            get
            {
                return Db.Notices;
            }
        }

        public bool CreateNotice(Notice instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Notices.InsertOnSubmit(instance);
                Db.Notices.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ReadNotice(int idNotice)
        {
            var instance = Db.Notices.FirstOrDefault(p => p.ID == idNotice);
            if (instance != null)
            {
                instance.ReadedDate = DateTime.Now;
                if (instance.IsCloseForRead)
                {
                    Db.Notices.DeleteOnSubmit(instance);
                }
                Db.Notices.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveNotice(int idNotice)
        {
            Notice instance = Db.Notices.FirstOrDefault(p => p.ID == idNotice);
            if (instance != null)
            {
                Db.Notices.DeleteOnSubmit(instance);
                Db.Notices.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}