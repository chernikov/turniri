using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ActionMailer.Net.Mvc;
using turniri.Model;

namespace turniri.Areas.Admin.Controllers
{
    public class MailController : MailerBase
    {
        public EmailResult Subscription(Mail mail, string host)
        {
            To.Add(mail.Email);
            Subject = mail.Subject;
            MessageEncoding = Encoding.UTF8;
            mail.Host = host;
            return Email("Subscription", mail);
        }
    }
}
