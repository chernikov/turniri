using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;

namespace turniri.Model
{
    public partial class Forum
    {
        public Forum ParentForum
        {
            get
            {
                return Forum1;
            }
        }

        public string FullImagePath
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath) ? ImagePath : "/Media/images/forum_default_preview.png";
            }
        }

        public string FullImagePathRead(User user)
        {
            return IsReaded(user) ?
                FullImagePath.GetPreviewPath("_g") : FullImagePath;
        }

        public IEnumerable<Forum> SubForums
        {
            get
            {
                return Forums.OrderBy(p => p.OrderBy).AsEnumerable();
            }
        }

        public IQueryable<ForumMessage> SubMessages
        {
            get { return ForumMessages.OrderBy(p => p.AddedDate).AsQueryable(); }
        }

        public int SubForumsCount
        {
            get
            {
                return Forums.Count(p => p.IsEnd) + Forums.Where(p => !p.IsEnd).ToList().Sum(p => p.SubForumsCount);
            }
        }

        public int MessagesCount
        {
            get
            {
                return ForumMessages.Count() + Forums.ToList().Sum(p => p.MessagesCount);
            }
        }

        public IQueryable<Forum> Topics
        {
            get
            {
                return Forums.Where(p => p.IsEnd).AsQueryable();
            }
        }

        public IQueryable<Forum> AllTopics
        {
            get
            {
                IQueryable<Forum> allTopics = Forums.Where(p => p.IsEnd).AsQueryable();
                foreach (var forum in Forums.Where(p => !p.IsEnd))
                {
                    allTopics = allTopics.Union(forum.AllTopics);
                }
                return allTopics;
            }
        }

        public Forum LastForumTopic
        {
            get
            {
                if (Forums.Any())
                {
                    if (Topics.Any())
                    {
                        return Topics.OrderByDescending(p => p.LastUpdate).FirstOrDefault();
                    }
                    else
                    {
                        return AllTopics.OrderByDescending(p => p.LastUpdate).FirstOrDefault();
                    }
                }
                return null;
            }
        }

        public ForumMessage LastForumMessage
        {
            get { return SubMessages.LastOrDefault(); }
        }

        public ForumMessage FirstForumMessage
        {
            get { return SubMessages.FirstOrDefault(); }
        }

        public ForumMessage TotalLastForumMessage
        {
            get
            {
                return SubMessages.Union(AllTopics.SelectMany(p => p.ForumMessages)).OrderBy(p => p.AddedDate).LastOrDefault();
            }
        }

        public ForumMessage TotalFirstForumMessage
        {
            get
            {
                return SubMessages.Union(AllTopics.SelectMany(p => p.ForumMessages)).OrderBy(p => p.AddedDate).FirstOrDefault();
            }
        }

        public int PlainMessagesCount
        {
            get
            {
                return ForumMessages.Count();
            }
        }

        public IEnumerable<ForumMessage> ForumMessagesPage(int page = 1, int itemPerPage = 25)
        {
            return ForumMessages.OrderBy(p => p.AddedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int MessagesCountPage(int itemPerPage = 25)
        {
            return PlainMessagesCount / itemPerPage + (PlainMessagesCount % itemPerPage != 0 ? 1 : 0);
        }

        public int SubjectCount
        {
            get { return Forums.Count; }
        }

        public IEnumerable<Forum> ForumPage(int page = 1, int itemPerPage = 5)
        {
            return Forums.OrderBy(p => p.OrderBy).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int ForumsCountPage(int itemPerPage = 5)
        {
            return SubjectCount / itemPerPage + (SubjectCount % itemPerPage != 0 ? 1 : 0);
        }

        public bool IsFather
        {
            get
            {
                return !IsEnd && Forums.Count(p => p.IsEnd) == Forums.Count();
            }
        }

        public bool IsReaded(User user)
        {
            if (user != null)
            {
                if (IsEnd)
                {
                    return ForumLogs.Any(r => r.UserID == user.ID) && LastUpdate < ForumLogs.Where(r => r.UserID == user.ID).OrderBy(r => r.ID).LastOrDefault().ReadDate;
                }
                return !AllTopics.Any(p => !p.ForumLogs.Any(r => r.UserID == user.ID) || p.LastUpdate > p.ForumLogs.Where(r => r.UserID == user.ID).OrderBy(r => r.ID).LastOrDefault().ReadDate);
            }
            return false;
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
                var forums = user.AdminTournaments.Where(p => p.Forum != null).Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ID))
                {
                    return true;
                }
            }
            if (user.InRoles("game_admin"))
            {
                var forums = user.AdminGames.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ID))
                {
                    return true;
                }
            }

            if (user.InRoles("tournament_moderator"))
            {
                var forums = user.ModeratorTournaments.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ID))
                {
                    return true;
                }
            }

            if (user.InRoles("game_moderator"))
            {
                var forums = user.ModeratorGames.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ID))
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

            if (user.InRoles("tournament_admin"))
            {
                var forums = user.AdminTournaments.Where(p => p.Forum != null).Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ID))
                {
                    return true;
                }
            }

            if (user.InRoles("game_admin"))
            {
                var forums = user.AdminGames.Select(p => p.Forum).ToList();
                var topics = forums.SelectMany(p => p.AllTopics);
                if (topics.Any(p => p.ID == ID))
                {
                    return true;
                }
            }
            return false;
        }

        public int TotalVisitCount
        {
            get
            {
                if (Forums.Any())
                {
                    return Forums.ToList().Sum(p => p.TotalVisitCount);
                }
                else
                {
                    return VisitCount;
                }
            }
        }

        public int UnreadMessageCount(int userID)
        {
            var forumLog = ForumLogs.LastOrDefault(p => p.UserID == userID);
            if (forumLog != null)
            {
                return ForumMessages.Count(p => !p.IsDeleted && p.AddedDate > forumLog.ReadDate);
            }
            return 0;
        }
    }
}