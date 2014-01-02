using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PollItem> PollItems
        {
            get
            {
                return Db.PollItems;
            }
        }

        public bool CreatePollItem(PollItem instance)
        {
            if (instance.ID == 0)
            {
                Db.PollItems.InsertOnSubmit(instance);
                Db.PollItems.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePollItem(PollItem instance)
        {
            var cache = Db.PollItems.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.PollID = instance.PollID;
				cache.Description = instance.Description;
                Db.PollItems.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePollItem(int idPollItem)
        {
            PollItem instance = Db.PollItems.FirstOrDefault(p => p.ID == idPollItem);
            if (instance != null)
            {
                Db.PollItems.DeleteOnSubmit(instance);
                Db.PollItems.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        private bool UpdateCountVotes(int idPollItem)
        {
            var instance = Db.PollItems.FirstOrDefault(p => p.ID == idPollItem);
            if (instance != null)
            {
                var count = instance.PollVotes.Count();
                instance.CountVotes = count;
                Db.PollItems.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}