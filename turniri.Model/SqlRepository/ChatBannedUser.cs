using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ChatBannedUser> ChatBannedUsers
        {
            get
            {
                return Db.ChatBannedUsers;
            }
        }

        public bool CreateChatBannedUser(ChatBannedUser instance)
        {
            if (instance.ID == 0)
            {
                Db.ChatBannedUsers.InsertOnSubmit(instance);
                Db.ChatBannedUsers.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateChatBannedUser(ChatBannedUser instance)
        {
            var cache = Db.ChatBannedUsers.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ChatRoomID = instance.ChatRoomID;
				cache.UserID = instance.UserID;
				cache.DateTill = instance.DateTill;
                Db.ChatBannedUsers.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveChatBannedUser(int idChatBannedUser)
        {
            ChatBannedUser instance = Db.ChatBannedUsers.FirstOrDefault(p => p.ID == idChatBannedUser);
            if (instance != null)
            {
                Db.ChatBannedUsers.DeleteOnSubmit(instance);
                Db.ChatBannedUsers.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}