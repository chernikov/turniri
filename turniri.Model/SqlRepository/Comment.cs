using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Comment> Comments
        {
            get
            {
                return Db.Comments;
            }
        }

        public bool CreateComment(Comment instance)
        {
            if (instance.ID == 0)
            {
                var exist = Db.Comments.Count(p => p.UserID == instance.UserID &&  p.Text == instance.Text && p.AddedDate >= DateTime.Now.AddMinutes(-1));
                if (exist == 0)
                {
                    instance.AddedDate = DateTime.Now;
                    instance.Text = instance.Text ?? string.Empty;
                    Db.Comments.InsertOnSubmit(instance);
                    Db.Comments.Context.SubmitChanges();
                    return true;
                }
            }

            return false;
        }

        public bool UpdateComment(Comment instance)
        {
            var cache = Db.Comments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Text = instance.Text;
                cache.VideoUrl = instance.VideoUrl;
                cache.ImagePath = instance.ImagePath;
                cache.VideoCode = instance.VideoCode;
				cache.IsBanned = instance.IsBanned;
				cache.BanDescription = instance.BanDescription;
                Db.Comments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveComment(int idComment)
        {
            Comment instance = Db.Comments.FirstOrDefault(p => p.ID == idComment);
            if (instance != null)
            {
                Db.Comments.DeleteOnSubmit(instance);
                Db.Comments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}