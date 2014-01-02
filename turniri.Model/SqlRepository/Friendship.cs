using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Friendship> Friendships
        {
            get
            {
                return Db.Friendships;
            }
        }

        public bool CreateFriendship(Friendship instance)
        {
            if (instance.ID == 0)
            {
                Db.Friendships.InsertOnSubmit(instance);
                Db.Friendships.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ConfirmFriendship(int idFriendship)
        {
            var instance = Db.Friendships.Where(p => p.ID == idFriendship).FirstOrDefault();
            instance.Approved = true;
            var backFriendship = new Friendship
            {
                Approved = instance.Approved,
                ReceiverID = instance.SenderID,
                SenderID = instance.ReceiverID
            };
            Db.Friendships.InsertOnSubmit(backFriendship);
            Db.Friendships.Context.SubmitChanges();
            return true;
        }

        public bool DeclineFriendship(int idFriendship)
        {
            Friendship instance = Db.Friendships.Where(p => p.ID == idFriendship).FirstOrDefault();
            if (instance != null)
            {
                Db.Friendships.DeleteOnSubmit(instance);
                Db.Friendships.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveFriend(int idUser, int idFriend)
        {
            var friendship = Db.Friendships.Where(p => p.SenderID == idUser && p.ReceiverID == idFriend).FirstOrDefault();
            var backFriendship = Db.Friendships.Where(p => p.ReceiverID == idUser && p.SenderID == idFriend).FirstOrDefault();
            if (friendship != null)
            {
                Db.Friendships.DeleteOnSubmit(friendship);
            }
            if (backFriendship != null)
            {
                Db.Friendships.DeleteOnSubmit(backFriendship);
            }
            Db.Friendships.Context.SubmitChanges();
            return true;
        }

    }
}