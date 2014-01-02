using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class ChatMessage
    {
        public enum MessageType
        {
            Simple = 0x01, 
            System = 0x02
        }

        public bool CanEdit(User user)
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
                if (chats.Any(p => p.ID == ChatRoomID))
                {
                    return true;
                }
            }

            if (user.InRoles("tournament_moderator,game_moderator"))
            {
                var chats = user.ModeratorTournaments.SelectMany(p => p.ChatRooms).ToList();
                if (chats.Any(p => p.ID == ChatRoomID))
                {
                    return true;
                }
            }
           
            return false;
        }

        public bool CanDelete(User user)
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
                if (chats.Any(p => p.ID == ChatRoomID))
                {
                    return true;
                }
            }
            if (user.InRoles("tournament_moderator,game_moderator"))
            {
                var chats = user.ModeratorTournaments.SelectMany(p => p.ChatRooms).ToList();
                if (chats.Any(p => p.ID == ChatRoomID))
                {
                    return true;
                }
            }
            return false;
        }

        public string StatusColor
        {
            get
            {
                var user = User;

                if (user.InRoles("admin"))
                {
                    return "superadmin";
                }
                if (ChatRoom.TournamentID.HasValue)
                {
                    if (CanEdit(user))
                    {
                        if (user.InRoles("game_admin,tournament_admin"))
                        {
                            return "admin";
                        };
                        if (user.InRoles("tournament_moderator,game_moderator"))
                        {
                            return "moderator";
                        };
                    }
                }
                else
                {
                    if (user.InRoles("game_admin,tournament_admin"))
                    {
                        return "admin";
                    };
                    if (user.InRoles("tournament_moderator,game_moderator"))
                    {
                        return "moderator";
                    };
                }

                return "";
            }

        }
	}
}