using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Distribution
    {
        public int CountMails
        {
            get
            {
                return Mails.Count();
            }
        }

        public bool AnyMails
        {
            get
            {
                return Mails.Any();
            }
        }

        public int CountDeliveredMails
        {
            get
            {
                return Mails.Count(p => p.Delivered);
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

        public IEnumerable<Mail> SubMails
        {
            get
            {
                return Mails.AsEnumerable();
            }
        }
	}
}