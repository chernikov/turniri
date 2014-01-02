using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{
    public partial class Notice
    {
        public enum TypeEnum : int
        {
            Simple = 0x01,
            Tournament = 0x02,
            Game = 0x03,
            Group = 0x04,
            Match = 0x05,
            Forum = 0x06,
            Message = 0x07,
            Friendship = 0x08,
            Chat = 0x09,
            Activate = 0x0A,
            VerifiedEmail = 0x0B,
            PublishVk = 0x0C,
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

        public bool AnyNoticeAction
        {
            get
            {
                return NoticeActions.Any();
            }
        }

        public IEnumerable<NoticeAction> SubNoticeActions
        {
            get
            {
                return NoticeActions.ToList();
            }
        }

        private int? _unreadCount;

        public int UnreadCount
        {
            get
            {
                if (!_unreadCount.HasValue)
                {
                    if (Forum != null)
                    {
                        _unreadCount = Forum.UnreadMessageCount(ReceiverID);
                    }
                    else if (ChatRoom != null) 
                    {
                        _unreadCount = ChatRoom.UnreadMessageCount(ReceiverID);
                    } 
                    else {

                        _unreadCount = 0;
                    }
                }
                return _unreadCount.Value;
            }
        }

        public bool IsShow
        {
            get
            {
                if (Type == (int)Notice.TypeEnum.Forum)
                {
                    return UnreadCount > 0;
                }
                if (Type == (int)Notice.TypeEnum.Chat)
                {
                    return UnreadCount > 0;
                }
                return true;
            }
        }
    }
}