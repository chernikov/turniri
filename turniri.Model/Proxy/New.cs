using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class New
    {
        public int CommentCount
        {
            get { return NewComments.Count; }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return NewComments.Select(p => p.Comment).Where(p => p.ParentID == null).OrderBy(p => p.AddedDate);
            }
        }

        public bool IsLikedBy(User user) 
        {
            return NewLikes.Any(p => p.UserID == user.ID);
        }

        public IEnumerable<SocialPost> SubSocialPosts
        {
            get
            {
                return NewSocialPosts.Select(p => p.SocialPost);
            }
        }
	}
}