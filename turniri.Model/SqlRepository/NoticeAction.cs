using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<NoticeAction> NoticeActions
        {
            get
            {
                return Db.NoticeActions;
            }
        }

        public bool CreateNoticeAction(NoticeAction instance)
        {
            if (instance.ID == 0)
            {
                Db.NoticeActions.InsertOnSubmit(instance);
                Db.NoticeActions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveNoticeAction(int idNoticeAction)
        {
            NoticeAction instance = Db.NoticeActions.FirstOrDefault(p => p.ID == idNoticeAction);
            if (instance != null)
            {
                Db.NoticeActions.DeleteOnSubmit(instance);
                Db.NoticeActions.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}