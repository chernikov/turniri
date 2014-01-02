using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Video
    {
        public int CommentCount
        {
            get { return VideoComments.Count; }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return VideoComments.Select(p => p.Comment).Where(p => p.ParentID == null).OrderBy(p => p.AddedDate);
            }
        }

        public IEnumerable<SocialPost> SubSocialPosts
        {
            get
            {
                return VideoSocialPosts.Select(p => p.SocialPost);
            }
        }
	}
}