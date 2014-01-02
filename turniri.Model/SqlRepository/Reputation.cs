using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public bool SetReputation(int senderID, int receiverID, int type, int mark)
        {
            var exist =
                Db.Reputations.FirstOrDefault(
                    p => p.SenderID == senderID && p.ReceiverID == receiverID && p.ReputationType == type);

            if (exist != null)
            {
                exist.Mark = mark;
                Db.Reputations.Context.SubmitChanges();
                RecalculateReputation(receiverID);
                return true;
            }
            var newReputation = new Reputation
                                    {
                                        SenderID = senderID,
                                        ReceiverID = receiverID,
                                        ReputationType = type,
                                        Mark = mark
                                    };
            Db.Reputations.InsertOnSubmit(newReputation);
            Db.Reputations.Context.SubmitChanges();
            RecalculateReputation(receiverID);
            return true;
        }

        private void RecalculateReputation(int userID)
        {
            var user = Db.Users.FirstOrDefault(p => p.ID == userID);
            if (user != null)
            {
                var reputationsHonest =
                    user.MyReputations.Where(p => p.ReputationType == (int) Reputation.ReputationTypeEnum.Honest);

                if (reputationsHonest.Any())
                {
                    user.ReputationHonest = ((double)reputationsHonest.Sum(p => p.Mark))/reputationsHonest.Count();
                }

                var reputationsResponsibility =
                    user.MyReputations.Where(p => p.ReputationType == (int)Reputation.ReputationTypeEnum.Responsibility);

                if (reputationsResponsibility.Any())
                {
                    user.ReputationResponsibility = ((double)reputationsResponsibility.Sum(p => p.Mark)) / reputationsResponsibility.Count();
                }

                var reputationsConnection =
                   user.MyReputations.Where(p => p.ReputationType == (int)Reputation.ReputationTypeEnum.Connection);

                if (reputationsConnection.Any())
                {
                    user.ReputationConnection = ((double)reputationsConnection.Sum(p => p.Mark)) / reputationsConnection.Count();
                }

                var reputation = user.MyReputations;

                if (reputation.Any())
                {
                    user.Reputation = ((double)reputation.Sum(p => p.Mark)) / reputation.Count();
                }
                Db.Users.Context.SubmitChanges();
            }
        }
    }
}