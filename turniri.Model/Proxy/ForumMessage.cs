using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;

namespace turniri.Model
{
    public partial class ForumMessage
    {
        public ForumMessage Parent
        {
            get { return ForumMessage1; }
        }

        public User Author
        {
            get { return User1; }
        }

        public User Moderator
        {
            get { return User; }
        }

        public string Quote
        {
            get
            {
                return @"[quote=" + Author.Login + "]" + Message + @"[/quote]";
            }
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

            if (user.InRoles("tournament_admin"))
            {
                var forums = user.AdminTournaments.Select(p => p.Forum).Where(p => p != null).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ForumID))
                {
                    return true;
                }
            }

            if (user.InRoles("game_admin"))
            {
                var forums = user.AdminGames.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ForumID))
                {
                    return true;
                }
            }
            
            if (user.InRoles("tournament_moderator"))
            {
                var forums = user.ModeratorTournaments.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ForumID))
                {
                    return true;
                }
            }

            if (user.InRoles("game_moderator"))
            {
                var forums = user.ModeratorGames.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ForumID))
                {
                    return true;
                }
            }
            
            if (user.ID == UserID && IsLastMessage && !ForumMessages.Any())
            {
                return true;
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
            if (user.InRoles("tournament_admin"))
            {
                var forums = user.AdminTournaments.Select(p => p.Forum).ToList();
                var topics = forums.Where(p => p != null).SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ForumID))
                {
                    return true;
                }
            }

            if (user.InRoles("game_admin"))
            {
                var forums = user.AdminGames.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ForumID))
                {
                    return true;
                }
            }

            if (user.InRoles("tournament_moderator"))
            {
                var forums = user.ModeratorTournaments.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ForumID))
                {
                    return true;
                }
            }

            if (user.InRoles("game_moderator"))
            {
                var forums = user.ModeratorGames.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ForumID))
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool IsLastMessage
        {
            get
            {
                return Forum.LastForumMessage.ID == ID;
            }
        }

        public string StatusColor
        {
            get
            {
                var user = Author;

                if (user.InRoles("admin"))
                {
                    return "superadmin";
                }
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

                return "";
            }

        }
    }
}