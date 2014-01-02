using System;
using System.Linq;
using turniri.Global.Config;

namespace turniri.Tools.Mail
{
    public static class NotifyMail
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void SendNotify<T>(IConfig config, string templateName, string email, Func<T, string, string> subject, Func<T, string, string> body, T obj)
        {
            var template = config.MailTemlates.FirstOrDefault(p => string.Compare(p.Name, templateName, true) == 0);
            if (template == null)
            {
                logger.Error("Can't find template (" + templateName + ")");
            }
            else
            {
                MailSender.SendMail(email,
                    subject.Invoke(obj, template.Subject),
                    body.Invoke(obj, template.Template));
            }
        }
    }
}