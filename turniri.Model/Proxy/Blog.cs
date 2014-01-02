using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Blog
    {
        public int CommentCount
        {
            get { return BlogComments.Count; }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return BlogComments.Select(p => p.Comment).Where(p => p.ParentID == null).OrderBy(p => p.AddedDate);
            }
        }

        public bool IsLikedBy(User user)
        {
            return BlogLikes.Any(p => p.UserID == user.ID);
        }

        public IEnumerable<SocialPost> SubSocialPosts
        {
            get
            {
                return BlogSocialPosts.Select(p => p.SocialPost);
            }
        }
	}
}