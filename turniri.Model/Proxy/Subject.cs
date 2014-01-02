using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace turniri.Model
{
    public partial class Subject
    {
        public Message LastSentMessage(int idUser)
        {
            return Messages.Where(p => p.IsSend && p.MatchID == null && p.GroupID == null && p.SenderID == idUser).OrderByDescending(p => p.ID).FirstOrDefault();
        }

        public IQueryable<Message> RealMessages
        {
            get
            {
                return Messages.Where(p => !p.IsDeleted).AsQueryable();
            }
        }

        public Message FirstMessage
        {
            get
            {
                return RealMessages.OrderBy(p => p.ID).FirstOrDefault();
            }
        }


        public Message LastMessage
        {
            get
            {
                return RealMessages.OrderByDescending(p => p.ID).FirstOrDefault();
            }
        }

        public Message LastReceiveMessage(int idUser)
        {
            return RealMessages.Where(p => !p.IsSend && p.MatchID == null && p.GroupID == null && p.ReceiverID == idUser).OrderByDescending(p => p.ID).FirstOrDefault();
        }

        public Message LastSentFightMessage(int idUser)
        {
            return RealMessages.Where(p => p.IsSend && p.MatchID != null && p.SenderID == idUser).OrderByDescending(p => p.ID).FirstOrDefault();
        }

        public Message LastReceiveFightMessage(int idUser)
        {
            return RealMessages.Where(p => !p.IsSend && p.MatchID != null && p.ReceiverID == idUser).OrderByDescending(p => p.ID).FirstOrDefault();
        }

        public IEnumerable<Message> MessagesByUser(int idUser, int page = 1, int itemPerPage = 20)
        {
            return RealMessages.Where(p => (p.SenderID == idUser && p.IsSend) || (p.ReceiverID == idUser && !p.IsSend)).OrderByDescending(p => p.AddedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int CountMessagesPage(int idUser, int itemPerPage = 20)
        {
            return RealMessages.Count(p => (p.SenderID == idUser && p.IsSend) || (p.ReceiverID == idUser && !p.IsSend)) / itemPerPage + (Messages.Count(p => (p.SenderID == idUser && p.IsSend) || (p.ReceiverID == idUser && !p.IsSend)) % itemPerPage != 0 ? 1 : 0);
        }
    }
}