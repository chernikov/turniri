using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Message
    {
        public enum MessageType
        {
            All = 0x00,
            Simple = 0x01,
            Fight = 0x02
        }

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

        public bool IsReaded
        {
            get
            {
                return ReadedDate.HasValue;
            }
        }

        public User Companion(int idUser)
        {
            if (ReceiverID == idUser)
            {
                return Sender;
            }
            return Receiver;
        }

	}
}