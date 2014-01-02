using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class UserVideo
    {
        public int CommentCount
        {
            get { return UserVideoComments.Count; }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return UserVideoComments.Select(p => p.Comment).Where(p => p.ParentID == null).OrderBy(p => p.AddedDate);
            }
        }
	}
}