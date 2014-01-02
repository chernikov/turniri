using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Blog> Blogs
        {
            get
            {
                return Db.Blogs;
            }
        }

        public bool CreateBlog(Blog instance)
        {
            if (instance.ID == 0)
            {
                instance.ShowInMain = true;
                instance.AddedDate = DateTime.Now;
                instance.LastModificateDate = DateTime.Now;
                instance.Url = Translit.WithPredicateTranslate(instance.Header);

                Db.Blogs.InsertOnSubmit(instance);
                Db.Blogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateBlog(Blog instance)
        {
            var cache = Db.Blogs.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Header = instance.Header;
				cache.Text = instance.Text;
				cache.PreviewUrl = instance.PreviewUrl;
                cache.GroupID = instance.GroupID;
				cache.LastModificateDate = DateTime.Now;
				Db.Blogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateAdminBlog(Blog instance)
        {
            var cache = Db.Blogs.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Header = instance.Header;
                cache.Url = instance.Url;
                cache.Text = instance.Text;
                cache.PreviewUrl = instance.PreviewUrl;
                cache.LastModificateDate = DateTime.Now;
                cache.IsBanned = instance.IsBanned;
                cache.BanDescription = instance.BanDescription;
                cache.ShowInMain = instance.ShowInMain;
                Db.Blogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveBlog(int idBlog)
        {
            Blog instance = Db.Blogs.FirstOrDefault(p => p.ID == idBlog);
            if (instance != null)
            {
                Db.Blogs.DeleteOnSubmit(instance);
                Db.Blogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateVisitBlog(int idBlog)
        {
            try
            {
                var instance = Db.Blogs.FirstOrDefault(p => p.ID == idBlog);
                if (instance != null)
                {
                    instance.VisitCount++;
                    Db.Blogs.Context.SubmitChanges();
                    return true;
                }
            }
            catch {}
            return false;
        }

        public int ToggleBlogLike(int idBlog, int idUser)
        {
            var blog = Db.Blogs.FirstOrDefault(p => p.ID == idBlog);

            if (blog != null)
            {
                var blogLike = Db.BlogLikes.FirstOrDefault(p => p.BlogID == idBlog && p.UserID == idUser);

                if (blogLike != null)
                {
                    Db.BlogLikes.DeleteOnSubmit(blogLike);
                   
                }
                else
                {
                    var newBlogLike = new BlogLike
                    {
                        BlogID = blog.ID,
                        UserID = idUser
                    };

                    Db.BlogLikes.InsertOnSubmit(newBlogLike);
                }
                Db.BlogLikes.Context.SubmitChanges();
                blog.Likes = blog.BlogLikes.Count;

                Db.Blogs.Context.SubmitChanges();
                return blog.Likes;
            }

            return 0;
        }
    }
}