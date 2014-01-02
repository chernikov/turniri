using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Poll
    {
        public enum PollTypeEnum
        {
            CheckBox = 0x01,
            Radio = 0x02
        }

        public bool CanVote(User user)
        {
            if (IsClosed)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            return !PollVotes.Any(p => p.UserID == user.ID);
        }

        public IEnumerable<PollItem> SubPollItems
        {
            get
            {
                return PollItems.AsEnumerable();
            }
        }

        public int Total
        {
            get
            {
                return PollVotes.Count(p => p.PollItemID != null);
            }
        }

        public int Max
        {
            get
            {
                return PollItems.Max(p => p.CountVotes);
            }
        }

        public Forum FirstForum
        {
            get
            {
                return Forums.FirstOrDefault();
            }
        }
	}
}