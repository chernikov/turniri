using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Photo
    {
        public int CommentCount
        {
            get { return PhotoComments.Count; }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return PhotoComments.Select(p => p.Comment).Where(p => p.ParentID == null).OrderBy(p => p.AddedDate);
            }
        }

        public bool IsLikedBy(User user)
        {
            return PhotoLikes.Any(p => p.UserID == user.ID);
        }
	}
}