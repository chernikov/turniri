using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Message> Messages
        {
            get
            {
                return Db.Messages.Where(p => !p.IsDeleted);
            }
        }

        public bool CreateMessage(Message instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                var cache = new Message()
                {
                    IsSend = true,
                    SubjectID = instance.SubjectID,
                    ReceiverID = instance.ReceiverID,
                    SenderID = instance.SenderID,
                    Text = instance.Text,
                    AddedDate = instance.AddedDate,
                    ReadedDate = null,
                    GameID = instance.GameID,
                    MatchID = instance.MatchID,
                    GroupID = instance.GroupID
                };
                Db.Messages.InsertOnSubmit(instance);
                Db.Messages.InsertOnSubmit(cache);
                Db.Messages.Context.SubmitChanges();

                if (instance.MatchID.HasValue)
                {
                    var match = Db.Matches.FirstOrDefault(p => p.ID == instance.MatchID);
                    if (match != null)
                    {
                        match.MessageID = instance.ID;
                        Db.Matches.Context.SubmitChanges();
                    }
                }
                return true;
            }

            return false;
        }


        public bool ReadSubject(int idSubject, int idUser)
        {
            var subject = Db.Subjects.FirstOrDefault(p => p.ID == idSubject);
          
            if (subject != null)
            {
                foreach (var message in subject.Messages.Where(p => p.ReceiverID == idUser && !p.IsReaded).ToList())
                {
                    message.ReadedDate = DateTime.Now;
                    Db.Messages.Context.SubmitChanges();
                }
                return true;
            }
            return false;
        }

        public bool RemoveMessage(int idMessage)
        {
            var instance = Db.Messages.Where(p => p.ID == idMessage).FirstOrDefault();
            if (instance != null)
            {
                instance.IsDeleted = true;
                Db.Messages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        
    }
}