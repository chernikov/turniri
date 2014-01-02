using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<NoticeDistribution> NoticeDistributions
        {
            get
            {
                return Db.NoticeDistributions;
            }
        }

        public bool CreateNoticeDistribution(NoticeDistribution instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.NoticeDistributions.InsertOnSubmit(instance);
                Db.NoticeDistributions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateNoticeDistribution(NoticeDistribution instance)
        {
            var cache = Db.NoticeDistributions.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Caption = instance.Caption;
				cache.Text = instance.Text;
                cache.IsCloseForRead = instance.IsCloseForRead;
                Db.NoticeDistributions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveNoticeDistribution(int idNoticeDistribution)
        {
            var instance = Db.NoticeDistributions.FirstOrDefault(p => p.ID == idNoticeDistribution);
            if (instance != null)
            {
                Db.NoticeDistributions.DeleteOnSubmit(instance);
                Db.NoticeDistributions.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ClearNoticeDistribution(int idNoticeDistribution)
        {
            var instance = Db.NoticeDistributions.FirstOrDefault(p => p.ID == idNoticeDistribution);
            if (instance != null)
            {
                var notices = instance.Notices;
                Db.Notices.DeleteAllOnSubmit(notices);
                Db.NoticeDistributions.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}