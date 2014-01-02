using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ChatRoom> ChatRooms
        {
            get
            {
                return Db.ChatRooms;
            }
        }

        public bool CreateChatRoom(ChatRoom instance)
        {
            if (instance.ID == 0)
            {
                Db.ChatRooms.InsertOnSubmit(instance);
                Db.ChatRooms.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateChatRoom(ChatRoom instance)
        {
            var cache = Db.ChatRooms.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TournamentID = instance.TournamentID;
				cache.Name = instance.Name;
                Db.ChatRooms.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveChatRoom(int idChatRoom)
        {
            ChatRoom instance = Db.ChatRooms.FirstOrDefault(p => p.ID == idChatRoom);
            if (instance != null)
            {
                Db.ChatRooms.DeleteOnSubmit(instance);
                Db.ChatRooms.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateVisitChatRoomNotice(int idChatRoom, int idUser)
        {
            var instance = Db.Notices.FirstOrDefault(p => p.ChatRoomID == idChatRoom && p.ReceiverID == idUser);
            if (instance != null)
            {
                instance.ReadedDate = DateTime.Now;
                Db.Forums.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ToggleTranslateChatRoom(int idChatRoom)
        {
            var instance = Db.ChatRooms.FirstOrDefault(p => p.ID == idChatRoom);
            if (instance != null)
            {
                instance.TranslateInForum = !instance.TranslateInForum;
                Db.ChatRooms.Context.SubmitChanges();
                return true;
            }
            return false;
        }
        
    }
}