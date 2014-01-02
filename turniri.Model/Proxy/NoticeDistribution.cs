using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;

namespace turniri.Model
{ 
    public partial class NoticeDistribution
    {
        public string Name
        {
            get
            {
                return Caption ?? Text.Teaser(50);
            }
        }
        public IEnumerable<Notice> SubNotices
        {
            get
            {
                return Notices.AsEnumerable();
            }
        }

        public int CountNotices
        {
            get
            {
                return Notices.Count();
            }
        }

        public bool AnyNotices
        {
            get
            {
                return Notices.Any();
            }
        }

        public int CountReaderNotices
        {
            get
            {
                return Notices.Count(p => p.ReadedDate != null);
            }
        }

        public bool CanEdit(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (user.ID == UserID)
            {
                return true;
            }

            if (user.InRoles("admin"))
            {
                return true;
            }

            return false;
        }

        public bool CanDelete(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (user.ID == UserID)
            {
                return true;
            }

            if (user.InRoles("admin"))
            {
                return true;
            }
            return false;
        }
	}
}