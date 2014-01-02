using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace turniri.Tools.Video
{
    public class VideoHelper
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static string rutubeRegEx = ".*meta property=\"og:image\" content=\"http://tub.rutube.ru/thumbs-wide/../../(?<code>.*?)-1-1.jpg\" />";

        public static string GetVideoByUrl(string url)
        {
            Uri uriResult;
            if (Uri.TryCreate(url, UriKind.Absolute, out uriResult))
            {
                string resultCode = string.Empty;
                switch (uriResult.Host)
                {
                    case "www.rutube.ru":
                    case "rutube.ru":
                        if (uriResult.Segments.Count() == 3)
                        {
                            var webClient = new WebClient();
                            var resultPage = webClient.DownloadString(url);

                            var regex = new Regex(rutubeRegEx);
                            var matches = regex.Match(resultPage);
                            if (matches.Success)
                            {
                                var code = matches.Groups["code"].Value;
                                resultCode = "<OBJECT width=\"640\" height=\"480\" id=\"participantID\"><PARAM name=\"movie\" value=\"http://video.rutube.ru/" + code + "\"></PARAM><PARAM name=\"wmode\" value=\"opaque\"></PARAM><PARAM name=\"allowFullScreen\" value=\"true\"></PARAM><PARAM name=\"allowscriptaccess\" value=\"always\"></PARAM><EMBED src=\"http://video.rutube.ru/" + code + "\" type=\"application/x-shockwave-flash\" wmode=\"opaque\" width=\"800\" height=\"600\" allowFullScreen=\"true\" allowscriptaccess=\"always\"></EMBED></OBJECT>";
                            }
                        }
                        break;
                    case "www.vimeo.com":
                    case "vimeo.com":
                        if (uriResult.Segments.Count() == 2)
                        {
                            var id = uriResult.Segments[1];
                            resultCode = "<iframe src='http://participant.vimeo.com/video/" + id + "?title=0&amp;byline=0&amp;portrait=0' width='640' height='480' frameborder='0'></iframe>";
                        }
                        break;
                    case "www.youtube.com":
                    case "youtube.com":
                        var resYu = HttpUtility.ParseQueryString(uriResult.Query);
                        if (resYu["v"] != null)
                        {
                            var id = resYu["v"];
                            resultCode = "<iframe width='640' height='480' src='http://www.youtube.com/embed/" + id + "' frameborder='0' allowfullscreen></iframe>";
                        }
                        break;
                    case "www.youtu.be":
                    case "youtu.be":
     
                        if (uriResult.Segments.Count() == 2)
                        {
                            var id = uriResult.Segments[1];
                            resultCode = "<iframe width='640' height='480' src='http://www.youtube.com/embed/" + id + "' frameborder='0' allowfullscreen></iframe>";
                        }
                        break;
                }
                return resultCode;
            }
            return string.Empty;
        }

        public static string GetVideoThumbByUrl(string url)
        {
            Uri uriResult;
            if (Uri.TryCreate(url, UriKind.Absolute, out uriResult))
            {
                string resultCode = string.Empty;
                WebClient webClient;
                switch (uriResult.Host)
                {
                    case "www.rutube.ru":
                    case "rutube.ru":
                        webClient = new WebClient();
                        var resultPage = webClient.DownloadString(url);

                        var regex = new Regex(rutubeRegEx);
                        var matches = regex.Match(resultPage);
                        if (matches.Success)
                        {
                            var code = matches.Groups["code"].Value;
                            var segment1 = code.Substring(0, 2);
                            var segment2 = code.Substring(2, 2);
                            resultCode = string.Format("http://tub.rutube.ru/thumbs-wide/{0}/{1}/{2}-1.jpg", segment1, segment2, code);
                        }
                        break;
                    case "www.vimeo.com":
                    case "vimeo.com":
                        if (uriResult.Segments.Count() == 2)
                        {
                            var id = uriResult.Segments[1];
                            var request = string.Format("http://vimeo.com/api/v2/video/{0}.json", id);
                            webClient = new WebClient();
                            var response = webClient.DownloadString(request);
                            response = response.Substring(1, response.Length - 2);
                            var jObj = JObject.Parse(response);
                            var vimeoInfo = JsonConvert.DeserializeObject<VimeoInfo>(jObj.ToString());
                            resultCode = vimeoInfo.ThumbnailLarge;
                        }
                        break;
                    case "www.youtube.com":
                    case "youtube.com":
                        var resYu = HttpUtility.ParseQueryString(uriResult.Query);
                        if (resYu["v"] != null)
                        {
                            var id = resYu["v"];
                            try
                            {
                                var request = string.Format("http://img.youtube.com/vi/{0}/maxresdefault.jpg", id);
                                webClient = new WebClient();
                                var response = webClient.DownloadString(request);
                                resultCode = request;
                            }
                            catch (WebException ex)
                            {
                                logger.Error("Max resolution thumb can't be downloaded (" + ex.Message + ")");
                                resultCode = string.Format("http://img.youtube.com/vi/{0}/default.jpg", id);
                            }
                        }
                        break;
                }
                return resultCode;
            }
            return string.Empty;
        }
    }
}
