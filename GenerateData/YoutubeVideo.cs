using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData
{
    public class YoutubeVideo
    {
        private static string Template = "http://youtube.com?v={0}";

        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 10000;//3 sec
                return w;
            }
        }

        public static string GetRandom()
        {
            var client = new MyWebClient();
            client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            try
            {
                var rand = client.DownloadData("http://www.youtuberandomvideo.com/get_video.php");

                var str = Encoding.UTF8.GetString(rand);
                return string.Format(Template, str);
            }
            catch (Exception ex)
            {
                return string.Format(Template, "U8HVQXkeU8U");
            }
        }
    }
}
