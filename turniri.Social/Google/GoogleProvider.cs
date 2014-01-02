using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Google
{
    public class GoogleProvider
    {
        private static string AuthorizeUri = "https://accounts.google.com/o/oauth2/auth?client_id={0}&response_type=code&scope=openid%20email%20https://www.googleapis.com/auth/userinfo.profile&redirect_uri={1}&state={2}";

        private static string GetAccessTokenUri = "https://accounts.google.com/o/oauth2/token";

        private static string GetProfileUri = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={0}";

        public IGoogleAppConfig Config { get; set; }

        public GoogleAccessTokenInfo AccessToken { get; set; }

        public string Authorize(string redirectTo, string secretState)
        {
            return string.Format(AuthorizeUri, Config.ClientId, redirectTo, secretState);
        }

        public bool GetAccessToken(string code, string redirectTo)
        {
            try
            {
                var postData = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code", code, Config.ClientId, Config.ClientSecret, redirectTo);
                var encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postData);
                var myRequest = (HttpWebRequest)WebRequest.Create(GetAccessTokenUri);
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
                var jObj = JObject.Parse(result);
                AccessToken = JsonConvert.DeserializeObject<GoogleAccessTokenInfo>(jObj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public JObject GetUserInfo()
        {
            var request = string.Format(GetProfileUri, AccessToken.AccessToken);
            WebClient webClient = new WebClient();

            string response = webClient.DownloadString(request);
            return JObject.Parse(response);
        }
    }
}
