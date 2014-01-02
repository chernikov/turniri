using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Poll> Polls
        {
            get
            {
                return Db.Polls;
            }
        }

        public bool CreatePoll(Poll instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Polls.InsertOnSubmit(instance);
                Db.Polls.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePoll(Poll instance)
        {
            var cache = Db.Polls.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.PollType = instance.PollType;
				cache.IsClosed = instance.IsClosed;
                if (!cache.PollVotes.Any())
                {
                    cache.PollItems = instance.PollItems;
                }
                Db.Polls.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePoll(int idPoll)
        {
            Poll instance = Db.Polls.FirstOrDefault(p => p.ID == idPoll);
            if (instance != null)
            {
                Db.Polls.DeleteOnSubmit(instance);
                Db.Polls.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}