using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class ChatRoom
    {
        public ChatMessage LastChatMessage
        {
            get
            {
                return ChatMessages.OrderBy(p => p.ID).LastOrDefault();
            }
        }

        public bool CanAddMessage(User user)
        {
            if (user == null)
            {
                return false;
            }

            return !ChatBannedUsers.Any(p => (!p.DateTill.HasValue || p.DateTill.Value > DateTime.Now) && p.UserID == user.ID);
        }

        public IEnumerable<ChatMessage> LastMessages
        {
            get
            {
                return ChatMessages.OrderByDescending(p => p.ID).Take(30);
            }
        }

        public bool More
        {
            get
            {
                return ChatMessages.Count() > 30;
            }
        }

        public IEnumerable<ChatMessage> LastMessagesReverse
        {
            get
            {
                return ChatMessages.OrderByDescending(p => p.ID).Take(30).OrderBy(p => p.ID);
            }
        }

        public bool CanBan(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (user.InRoles("admin"))
            {
                return true;
            }

            if (user.InRoles("tournament_admin,game_admin"))
            {
                var chats = user.AdminTournaments.SelectMany(p => p.ChatRooms).ToList();
                if (chats.Any(p => p.ID == ID))
                {
                    return true;
                }
            }

            if (user.InRoles("tournament_moderator,game_moderator"))
            {
                var chats = user.ModeratorTournaments.SelectMany(p => p.ChatRooms).ToList();
                if (chats.Any(p => p.ID == ID))
                {
                    return true;
                }
            }

            return false;
        }

        public int UnreadMessageCount(int userID)
        {
            var chatRoomLog = ChatRoomLogs.LastOrDefault(p => p.UserID == userID);
            if (chatRoomLog != null)
            {
                return ChatMessages.Count(p => p.AddedDate > chatRoomLog.ReadDate);
            }
            return 0;
        }

        public bool IsNoticed(User user)
        {
            if (user != null) 
            {
                return Notices.Any(p => p.ReceiverID == user.ID);
            }
            return false;
        }
	}
}