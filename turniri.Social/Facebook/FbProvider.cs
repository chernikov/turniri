using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace turniri.Social.Facebook
{
    public class FbProvider
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static string AuthorizeUri = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope=email";

        private static string SuperAuthorizeUri = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope=email,publish_stream,offline_access,user_groups,friends_groups";

        private static string GetAccessTokenUri = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}";

        private static string GetUserInfoUri = "https://graph.facebook.com/me?access_token={0}";

        private static string PostUri = "https://graph.facebook.com/{0}/feed";

        private static string GraphUri = "https://graph.facebook.com/{0}";

        public IFbAppConfig Config { get; set; }

        public string AccessToken { get; set; }

        public string Authorize(string redirectTo)
        {
            return string.Format(AuthorizeUri, Config.AppId, redirectTo);
        }

        public string SuperAuthorize(string redirectTo)
        {
            return string.Format(SuperAuthorizeUri, Config.AppId, redirectTo);
        }

        public bool GetAccessToken(string code, string redirectTo)
        {
            var request = string.Format(GetAccessTokenUri, Config.AppId, redirectTo, Config.AppSecret, code);
            WebClient webClient = new WebClient();
            string response = webClient.DownloadString(request);
            try
            {
                var pairResponse = response.Split('&');
                AccessToken = pairResponse[0].Split('=')[1];
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public JObject GetUserInfo()
        {
            var request = string.Format(GetUserInfoUri, AccessToken);
            WebClient webClient = new WebClient();

            string response = webClient.DownloadString(request);
            return JObject.Parse(response);
        }

        public string Publish(string facebookId, ISocialPost post)
        {
            post.Identifier = facebookId;
            var request = string.Format(PostUri, facebookId);
            var sb = new StringBuilder();
            request += "?access_token=" + AccessToken;
            sb.AppendFormat("access_token={0}&", AccessToken);

            //name
            if (!string.IsNullOrWhiteSpace(post.Title))
            {
                sb.AppendFormat("name={0}&", HttpUtility.UrlEncode(post.Title));
            }

            //message
            if (!string.IsNullOrWhiteSpace(post.Teaser))
            {
                sb.AppendFormat("message={0}&", HttpUtility.UrlEncode(post.Teaser));
            }

            //picture
            if (!string.IsNullOrWhiteSpace(post.Preview))
            {
                sb.AppendFormat("picture={0}&", post.Preview);
            }

            //link
            if (!string.IsNullOrWhiteSpace(post.Link))
            {
                sb.AppendFormat("link={0}&", post.Link);
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            
            var data = Encoding.UTF8.GetBytes(sb.ToString());

            logger.Debug(sb.ToString());
            var myRequest = (HttpWebRequest)WebRequest.Create(request);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            var newStream = myRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            var response = myRequest.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);
            var result = responseReader.ReadToEnd();
            return result;
        }
    }
}
