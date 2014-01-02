using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ChatMessage> ChatMessages
        {
            get
            {
                return Db.ChatMessages;
            }
        }

        public bool CreateChatMessage(ChatMessage instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.ChatMessages.InsertOnSubmit(instance);
                Db.ChatMessages.Context.SubmitChanges();

                var room = Db.ChatRooms.FirstOrDefault(p => p.ID == instance.ChatRoomID);
                room.LastIdUpdate = instance.ID;
                room.LastUpdate = DateTime.Now;
                Db.ChatRooms.Context.SubmitChanges();

                ResetChatRoomNotice(instance.ChatRoomID);

                return true;
            }

            return false;
        }

        public bool RemoveChatMessage(int idChatMessage)
        {
            var instance = Db.ChatMessages.FirstOrDefault(p => p.ID == idChatMessage);
            if (instance != null)
            {
                var room = Db.ChatRooms.FirstOrDefault(p => p.ID == instance.ChatRoomID);
                room.LastIdUpdate = instance.ID;
                room.LastUpdate = DateTime.Now;
                Db.ChatRooms.Context.SubmitChanges();

                Db.ChatMessages.DeleteOnSubmit(instance);
                Db.ChatMessages.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        private void ResetChatRoomNotice(int chatRoomID)
        {
            var chatRoom = Db.ChatRooms.FirstOrDefault(p => p.ID == chatRoomID);

            if (chatRoom != null)
            {
                var notices = chatRoom.Notices.ToList();

                foreach (var notice in notices)
                {
                    notice.ReadedDate = null;
                    Db.Notices.Context.SubmitChanges();
                }
            }

        }
     
    }
}