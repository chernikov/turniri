using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Mail> Mails
        {
            get
            {
                return Db.Mails;
            }
        }

        public bool SaveMail(Mail instance)
        {
            instance.ID = Guid.NewGuid();
            instance.AddedDate = DateTime.Now;
            instance.Delivered = false;
            instance.Subject = "";
            instance.Body = "";
            Db.Mails.InsertOnSubmit(instance);
            Db.Mails.Context.SubmitChanges();
            return true;
        }

        public bool PushMail(Mail instance)
        {
            var cache = Db.Mails.FirstOrDefault(p => p.ID == instance.ID);
            if (instance != null)
            {
                cache.Subject = instance.Subject;
                cache.Body = instance.Body;
                cache.Delivered = false;
                Db.Mails.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public Mail PopMail()
        {
            var instance = Db.Mails.FirstOrDefault(p => !p.Delivered && p.Distribution.IsStart);
            if (instance != null)
            {
                instance.Delivered = true;
                Db.Mails.Context.SubmitChanges();
                return instance;
            }
            return null;
        }

        public Mail PopMail(Guid id)
        {
            var instance = Db.Mails.FirstOrDefault(p => p.ID == id);
            if (instance != null)
            {
                instance.Delivered = true;
                Db.Mails.Context.SubmitChanges();
                return instance;
            }
            return null;
        }

        public bool RemoveMail(Guid id)
        {
            var instance = Db.Mails.FirstOrDefault(p => p.ID == id);
            if (instance != null)
            {
                Db.Mails.DeleteOnSubmit(instance);
                Db.Mails.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public void ClearMailBody(Guid id)
        {
            var instance = Db.Mails.FirstOrDefault(p => p.ID == id);
            if (instance != null)
            {
                instance.Body = string.Empty;
                Db.Mails.Context.SubmitChanges();
            }
        }
    }
}