using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ChatRoomLog> ChatRoomLogs
        {
            get
            {
                return Db.ChatRoomLogs;
            }
        }


        public bool CreateChatRoomLog(ChatRoomLog instance)
        {
            try
            {
                if (instance.ID == 0)
                {
                    var exist = Db.ChatRoomLogs.FirstOrDefault(p => p.ChatRoomID == instance.ChatRoomID && p.UserID == instance.UserID);

                    if (exist == null)
                    {
                        instance.ReadDate = DateTime.Now;
                        Db.ChatRoomLogs.InsertOnSubmit(instance);
                    }
                    else
                    {
                        exist.ReadDate = DateTime.Now;
                    }
                    Db.ChatRoomLogs.Context.SubmitChanges();
                    return true;
                }
            }
            catch { }
            return false;
        }
    }
}