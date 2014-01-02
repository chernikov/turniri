using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Net;
using turniri.Global.Config;

namespace turniri.Tools.Mail
{
    public static class MailSender
    {
        public static IConfig config;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void SendMail(string email, string subject, string body, MailAddress mailAddress = null)
        {
            
            try
            {
                if (config.EnableMail)
                {
                    if (mailAddress == null)
                    {
                        mailAddress = new MailAddress(config.MailSettings.SmtpReply, config.MailSettings.SmtpUser); 
                    }
                    MailMessage message = new MailMessage(
                        mailAddress,
                        new MailAddress(email))
                                              {
                                                  Subject = subject,
                                                  BodyEncoding = Encoding.UTF8,
                                                  Body = body,
                                                  IsBodyHtml = true,
                                                  SubjectEncoding = Encoding.UTF8
                                              };
                    SmtpClient client = new SmtpClient
                                            {
                                                Host = config.MailSettings.SmtpServer,
                                                Port = config.MailSettings.SmtpPort,
                                                UseDefaultCredentials = false,
                                                EnableSsl = config.MailSettings.EnableSsl,
                                                Credentials =
                                                    new NetworkCredential(config.MailSettings.SmtpUserName,
                                                                          config.MailSettings.SmtpPassword),
                                                DeliveryMethod = SmtpDeliveryMethod.Network
                                            };
                    client.Send(message);
                }
                else
                {
                    logger.Debug("Email : {0} {1} \t Subject: {2} {3} Body: {4}", email, Environment.NewLine, subject, Environment.NewLine, body);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("Mail send exception", ex);
            }
        }
    }
}