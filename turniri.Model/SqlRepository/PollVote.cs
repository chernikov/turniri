using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PollVote> PollVotes
        {
            get
            {
                return Db.PollVotes;
            }
        }

        public bool CreatePollVote(PollVote instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.PollVotes.InsertOnSubmit(instance);
                Db.PollVotes.Context.SubmitChanges();
                if (instance.PollItemID.HasValue)
                {
                    UpdateCountVotes(instance.PollItemID.Value);
                }
                return true;
            }

            return false;
        }
    }
}