using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<BlogComment> BlogComments
        {
            get
            {
                return Db.BlogComments;
            }
        }

        public bool CreateBlogComment(BlogComment instance)
        {
            if (instance.ID == 0)
            {
                Db.BlogComments.InsertOnSubmit(instance);
                Db.BlogComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateBlogComment(BlogComment instance)
        {
            var cache = Db.BlogComments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.BlogID = instance.BlogID;
				cache.CommentID = instance.CommentID;
                Db.BlogComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveBlogComment(int idBlogComment)
        {
            BlogComment instance = Db.BlogComments.FirstOrDefault(p => p.ID == idBlogComment);
            if (instance != null)
            {
                Db.BlogComments.DeleteOnSubmit(instance);
                Db.BlogComments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}