using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Global.Config;
using turniri.Model;
using turniri.Tools.Mail;

namespace turniri.Tools
{
    public class MailProcessor
    {
        public static bool SendNextMail(IRepository repository, IConfig config)
        {
            var mail = repository.PopMail();

            if (mail != null)
            {
                MailSender.SendMail(mail.Email, mail.Subject, mail.Body);
                repository.ClearMailBody(mail.ID);
                return true;
            }
            return false;
        }
    }
}