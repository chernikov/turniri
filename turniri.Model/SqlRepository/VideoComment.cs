using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<VideoComment> VideoComments
        {
            get
            {
                return Db.VideoComments;
            }
        }

        public bool CreateVideoComment(VideoComment instance)
        {
            if (instance.ID == 0)
            {
                Db.VideoComments.InsertOnSubmit(instance);
                Db.VideoComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateVideoComment(VideoComment instance)
        {
            var cache = Db.VideoComments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.VideoID = instance.VideoID;
				cache.CommentID = instance.CommentID;
                Db.VideoComments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveVideoComment(int idVideoComment)
        {
            VideoComment instance = Db.VideoComments.FirstOrDefault(p => p.ID == idVideoComment);
            if (instance != null)
            {
                Db.VideoComments.DeleteOnSubmit(instance);
                Db.VideoComments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}