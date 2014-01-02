using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ForumMessage> ForumMessages
        {
            get
            {
                return Db.ForumMessages;
            }
        }

        public bool CreateForumMessage(ForumMessage instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.ForumMessages.InsertOnSubmit(instance);
                Db.ForumMessages.Context.SubmitChanges();
                UpdateLastVisitForum(instance.ForumID.Value);
                if (!instance.Forum.IsEnd)
                {
                    SealForum(instance.ForumID.Value);
                }
                if (instance.Forum != null)
                {
                    ResetForumNotice(instance.Forum.ID);
                }
                return true;
            }

            return false;
        }

        public bool UpdateForumMessage(ForumMessage instance)
        {
            var cache = Db.ForumMessages.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Message = instance.Message;
				Db.ForumMessages.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ModerateForumMessage(ForumMessage instance)
        {
            var cache = Db.ForumMessages.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.ModeratedByID = instance.ModeratedByID;
                cache.ModeratedDate = DateTime.Now;
                cache.Message = instance.Message;
                Db.ForumMessages.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveForumMessage(int idForumMessage, int idUser)
        {
            var cache = Db.ForumMessages.FirstOrDefault(p => p.ID == idForumMessage);
            if (cache != null)
            {
                if (cache.ForumMessages.Any())
                {
                    cache.IsDeleted = true;
                    cache.ModeratedDate = DateTime.Now;
                    cache.ModeratedByID = idUser;
                }
                else
                {
                    Db.ForumMessages.DeleteOnSubmit(cache);
                }
                Db.ForumMessages.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        private void ResetForumNotice(int forumID)
        {
            var forum = Db.Forums.FirstOrDefault(p => p.ID == forumID);

            if (forum != null)
            {
                var notices = forum.Notices.ToList();

                foreach (var notice in notices)
                {
                    notice.ReadedDate = null;
                    Db.Notices.Context.SubmitChanges();
                }
            }

        }
     
    }
}