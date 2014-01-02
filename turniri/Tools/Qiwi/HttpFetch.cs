using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace turniri.Tools.Qiwi
{
    public class HttpFetch
    {
        private HttpWebRequest _Request;
        private HttpWebResponse _Response;
        private string _ResponseContent;
        private string _Url;

        public HttpFetch(string url)
        {
            this._Url = url;
        }

        public string PostData { get; set; }

        public string Process()
        {
            this.CreateRequest();
            this.ProcessRequest();

            return this._ResponseContent;
        }

        private void CreateRequest()
        {
            this._Request = (HttpWebRequest)WebRequest.Create(this._Url);
            this._Request.Timeout = 30000;
            this._Request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; ru; rv:1.9.1.2) Gecko/20090729 Firefox/3.5.2 (.NET CLR 3.5.30729)";
            this._Request.CookieContainer = new CookieContainer();

            if (!String.IsNullOrEmpty(this.PostData))
            {
                byte[] post_bytes = Encoding.UTF8.GetBytes(this.PostData);

                this._Request.ContentType = "application/x-www-form-urlencoded";
                this._Request.ContentLength = post_bytes.Length;
                this._Request.Method = "POST";

                Stream post_stream = this._Request.GetRequestStream();
                post_stream.Write(post_bytes, 0, post_bytes.Length);
                post_stream.Close();
            }
        }

        private void ProcessRequest()
        {
            this._Response = (HttpWebResponse)this._Request.GetResponse();
            StreamReader sr = new StreamReader(this._Response.GetResponseStream());
            this._ResponseContent = sr.ReadToEnd();
        }
    }
}