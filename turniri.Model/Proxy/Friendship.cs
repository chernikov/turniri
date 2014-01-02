using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Friendship
    {
        public User Sender
        {
            get
            {
                return User1;
            }
            set
            {
                User1 = value;
            }
        }

        public User Receiver
        {
            get
            {
                return User;
            }
            set
            {
                User = value;
            }
        }

	}
}