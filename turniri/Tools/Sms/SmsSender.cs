using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.IO;
using turniri.Global.Config;

namespace turniri.Tools.Sms
{
    public static class SmsSender
    {
        public static ISmsConfig config;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string SendSms(string phone, string text)
        {
            if (!string.IsNullOrWhiteSpace(config.SmsAPIKey))
            {
                return GetRequest(phone, config.SmsSender, text);
            }
            else
            {
                logger.Debug("Sms \t Phone: {0} Body: {1}", phone, text);
                return "Success";
            }
        }

        private static string GetRequest(string phone, string sender, string text)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(config.SmsTemplateUri);
                //important, otherwise the service can't desirialse your request properly
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                webRequest.KeepAlive = false;
                webRequest.PreAuthenticate = false;

                string postData = "format=json&api_key=" + config.SmsAPIKey + "&phone=" + phone
                    + "&sender=" + sender + "&text=" + HttpUtility.UrlEncode(text);
                var ascii = new ASCIIEncoding();
                byte[] byteArray = ascii.GetBytes(postData);
                webRequest.ContentLength = byteArray.Length;
                Stream dataStream = webRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse webResponse = webRequest.GetResponse();
                StreamReader loResponseStream = new
                        StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);

                string Response = loResponseStream.ReadToEnd();
                return Response;
            }
            catch (Exception ex)
            {
                logger.ErrorException("Ошибка при отправке SMS", ex);
                return "Ошибка при отправке SMS";
            }
        }
    }
}