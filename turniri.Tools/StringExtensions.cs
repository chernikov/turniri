using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Mvc;
using System.IO;
using turniri.Tools.Video;

namespace turniri.Tools
{
    public static class StringExtension
    {
        private static Random rd = new Random((int)DateTime.Today.Ticks);

        /// <summary>
        /// The regex strip html.
        /// </summary>
        private static readonly Regex RegexStripHtml = new Regex("<[^>]*>", RegexOptions.Compiled);

        private static readonly string[] smiles = { ":-)", ":-(", ":-D", "B-)", ":-O", ";-)", ";-(", "(:|", ":-|", ":-*", ":-P", ":-$", ":^)", "|-)", "|-(", "(inlove)", "]:)", "(talk)", "|-()", ":-&", "(doh)", "x-(", "(wasntme)", "(party)", ":-S", "(mm)", "8-|", ":-X", "(hi)", "(call)", "(devil)", "(angel)", "(envy)", "(wait)", "(hug)", "(makeup)", "(giggle)", "(clap)", "(think)", "(bow)", "(rofl)", "(whew)", "(happy)", "(smirk)", "(nod)", "(shake)", "(punch)", "(emo)", "(ok)", "(n)", "(handshake)", "(h)", "(u)", "(e)", "(f)", "(rain)", "(sun)", "(o)", "(music)", "(movie)", "(ph)", "(coffee)", "(pizza)", "(cash)", "(muscle)", "(cake)", "(beer)", "(d)", "(dance)", "(ninja)", "(*)", "(bandit)", "(bug)", "(drunk)", "(finger)", "(fubar)", "(headbang)", "(heidy)", "(mooning)", "(poolparty)", "(rock)", "(smoking)", "(swear)", "(tmi)", "(toivo)" };

        /// <summary>
        /// Remove all tags from html-text 
        /// </summary>
        /// <param name="source">html-text</param>
        /// <returns></returns>
        public static string StripTags(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            source = source.Replace("&nbsp;", " ");
            source = source.Replace("<br/>", Environment.NewLine);
            source = source.Replace("<br>", Environment.NewLine);
            char[] chArray = new char[source.Length];
            int index = 0;
            bool flag = false;
            for (int i = 0; i < source.Length; i++)
            {
                char ch = source[i];
                if (ch == '<')
                {
                    flag = true;
                }
                else if (ch == '>')
                {
                    flag = false;
                }
                else if (!flag)
                {
                    chArray[index] = ch;
                    index++;
                }
            }
            return new string(chArray, 0, index);

        }

        /// <summary>
        /// Remove all tags from html-text 
        /// </summary>
        /// <param name="source">html-text</param>
        /// <returns></returns>
        public static string StripBBCode(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            char[] chArray = new char[source.Length];
            int index = 0;
            bool flag = false;
            for (int i = 0; i < source.Length; i++)
            {
                char ch = source[i];
                if (ch == '[')
                {
                    flag = true;
                }
                else if (ch == ']')
                {
                    flag = false;
                }
                else if (!flag)
                {
                    chArray[index] = ch;
                    index++;
                }
            }
            return new string(chArray, 0, index);

        }

        /// <summary>
        /// Generate new guid-based name
        /// </summary>
        /// <returns></returns>
        public static string GenerateNewFile()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Check on valid html string (not null StripTags or start with image
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsValidHtmlString(this string source)
        {
            return 
                !string.IsNullOrWhiteSpace(source) && 
                ((!string.IsNullOrWhiteSpace(source.StripTags()) ||
                (source.IndexOf("<img ", StringComparison.InvariantCultureIgnoreCase) != -1)));
        }

       
        /// <summary>
        /// Check if is email correct string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEmail(this string source)
        {
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// Check if is numeric string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string source)
        {
            Regex regex = new Regex(@"[0-9]+", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// Check if is numeric string with need length
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string str, int length)
        {
            Regex regex = new Regex(@"[0-9]+", RegexOptions.Compiled);
            Match match = regex.Match(str);
            return (match.Success && match.Length == str.Length && str.Length == length);
        }

        /// <summary>
        /// Check if is can be ICQ number (just numbers, -, and space)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsICQ(this string source)
        {
            Regex regex = new Regex(@"[0-9,\-,\x20]+", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// Check if is can be correct phone number
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsPhone(this string source)
        {
            Regex regex = new Regex(@"[0-9,\+,\(,\),\-,\x20]+", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// Check if is can be correct url 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUrl(this string source)
        {
            Regex regex = new Regex(@"[a-z,A-Z,\-,_,0-9,\.,\/,?,=,%,а-я,А-Я,:,;,&]+", RegexOptions.Compiled);
            Match match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        /// <summary>
        /// \n to br tags 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static MvcHtmlString NlToBr(this string source)
        { 

            if (string.IsNullOrWhiteSpace(source))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(source.Replace(Environment.NewLine, "<br />"));
        }
        
        /// <summary>
        /// Create random password
        /// </summary>
        /// <param name="passwordLength">length of password</param>
        /// <returns></returns>
        public static string CreateRandomPassword(int passwordLength, string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-")
        {
            char[] chars = new char[passwordLength];
          

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        

        /// <summary>
        /// To Base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64(this string str)
        {
            byte[] byt = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(byt);
        }

        /// <summary>
        /// From base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromBase64(this string str)
        {
            byte[] b = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(b);
        }

        /// <summary>
        /// Clear content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="removeHtml"></param>
        /// <returns></returns>
        public static string CleanContent(string content, bool removeHtml)
        {
            if (removeHtml)
            {
                content = StripHtml(content);
            }

            content =
                content.Replace("\\", string.Empty).Replace("|", string.Empty).Replace("(", string.Empty).Replace(
                    ")", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace("*", string.Empty).
                    Replace("?", string.Empty).Replace("}", string.Empty).Replace("{", string.Empty).Replace(
                        "^", string.Empty).Replace("+", string.Empty);

            var words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var word in
                words.Select(t => t.ToLowerInvariant().Trim()).Where(word => word.Length > 1))
            {
                sb.AppendFormat("{0} ", word);
            }

            return sb.ToString();
        }

        private static string StripHtml(string html)
        {
            return StringIsNullOrWhitespace(html) ? string.Empty : RegexStripHtml.Replace(html, string.Empty).Trim();
        }

        private static bool StringIsNullOrWhitespace(string value)
        {
            return ((value == null) || (value.Trim().Length == 0));
        }
       
        /// <summary>
        /// Make correct phone numbers to one-view base (use russian locale (8 -> 7))
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string ClearPhone(this string phone)
        {
            var str = "";
            foreach (var @char in phone)
            {
                if (@char >= '0' && @char <= '9')
                {
                    str = str + @char;
                }
            }
            if (str.StartsWith("8"))
            {
                str = "7" + str.Substring(1);
            }
            return str;
        }

        /// <summary>
        /// Make teaser (start string) from content
        /// </summary>
        /// <param name="content">string</param>
        /// <param name="length">need length</param>
        /// <param name="more">more chars</param>
        /// <returns></returns>
        public static string Teaser(this string content, int length, string more = "...")
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            if (content.Length < length)
            {
                return content;
            }

            return content.Substring(0, length) + more;
        }

        public static string ForumDateNamed(this DateTime source)
        {
            string date;
            if (source.Date == DateTime.Now.Date)
            {
                date = "Сегодня, ";
            }
            else if (source.Date == DateTime.Now.Date.AddDays(-1))
            {
                date = "Вчера, ";
            }
            else if (source.Date == DateTime.Now.Date.AddDays(-2))
            {
                date = "Позавчера, ";
            } else if (source.Date == DateTime.Now.Date.AddDays(-7))
            {
                date = "Неделю назад, ";
            }
            else if (source.Date > DateTime.Now.Date.AddDays(-7))
            {
                date = source.Date.ToString("dddd") + ", ";
            }
            else if (source.Date == DateTime.Now.Date.AddMonths(-1))
            {
                date = "Месяц назад, ";
            }
            else if (source.Date.Year == DateTime.Now.Date.Year)
            {
                date = source.ToString("dd MMMM") + ", ";
            }
            else
            {
                date = source.ToString("dd MMMM yyyy") + ", ";
            }
            return date + "в " + source.ToString("HH:mm");
        }

        public static string CountWord(this int count, string first, string second, string five)
        {
            if (count % 10 == 1 && (int)(count / 10) != 1)
            {
                return first;
            }
            if (count % 10 > 1 && count % 10 < 5 && ((int)(count / 10) % 10) != 1)
            {
                return second;
            }
            return five;
        }

        public static string GetPreviewPath(this string path, string suffix)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            return (Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + suffix + Path.GetExtension(path)).Replace("\\", "/");
        }

        public static string HtmlToBbCode(this string desc)
        {
            if (string.IsNullOrEmpty(desc))
            {
                return string.Empty;
            }

            desc = Regex.Replace(desc, @"<p>", "", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"&nbsp;", " ", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"</p>", "\n\r", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<br(.*?) />", "\n\r", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<UL[^>]*>", "[ulist]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/UL>", "[/ulist]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<OL[^>]*>", "[olist]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/OL>", "[/olist]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<LI>", "[*]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/li>", "[/*]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<B>", "[b]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/B>>", "[/b]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<STRONG>", "[b]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/STRONG>", "[/b]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<u>", "[u]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/u>", "[/u]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<i>", "[i]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/i>", "[/i]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<em>", "[i]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/em>", "[/i]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<sup>", "[sup]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/sup>", "[/sup]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<sub>", "[sub]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/sub>", "[/sub]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<HR[^>]*>", "[hr]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<STRIKE>", "[strike]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/STRIKE>", "[/strike]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<h1>", "[h1]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/h1>", "[/h1]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<h2>", "[h2]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/h2>", "[/h2]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<h3>", "[h3]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/h3>", "[/h3]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<ol>", "[olist]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/ol>", "[/olist]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<code>", "[code]", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/code>", "[/code]", RegexOptions.IgnoreCase);
            
            var matchAhref = Regex.Matches(desc, "<a.*href=[\",'](?<link>.*?)[\",'][^>]*>(?<name>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (matchAhref.Count > 0)
            {
                foreach (Match match in matchAhref)
                {
                    desc = desc.Replace(match.ToString(), "[url=" + match.Groups[1].Value + "]" + match.Groups[2].Value + "[/url]");
                }
            }
            matchAhref = Regex.Matches(desc, "<a.*href=[\",'](?<link>.*?)[\",'].*>(?<name>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (matchAhref.Count > 0)
            {
                foreach (Match match in matchAhref)
                {
                    desc = desc.Replace(match.ToString(), "[url=" + match.Groups[1].Value + "]" + match.Groups[2].Value + "[/url]");
                }
            }

            var matchImg = Regex.Matches(desc, "<img.*src=[\",'](?<link>.*?)[\",'][^>].*/>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (matchImg.Count > 0)
            {
                foreach (Match match in matchImg)
                {
                    desc = desc.Replace(match.ToString(), "[img]" + match.Groups[1].Value + "[/img]");
                }
            }

            var matchSpanUnderline = Regex.Matches(desc, "<span.*style=[\",'].*text-decoration.*:.*underline;.*[\",'].*>(?<name>.*?)</span>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (matchSpanUnderline.Count > 0)
            {
                foreach (Match match in matchImg)
                {
                    desc = desc.Replace(match.ToString(), "[u]" + match.Groups[1].Value + "[/u]");
                }
            }

            var matchBlockquote = Regex.Matches(desc, "<q.*cite=[\",'](?<link>.*?)[\",'].*>(?<name>.*?)</q>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (matchSpanUnderline.Count > 0)
            {
                foreach (Match match in matchImg)
                {
                    desc = desc.Replace(match.ToString(), "[quote=" + match.Groups[1].Value + "]" + match.Groups[2].Value + "[/quote]");
                }
            }
            //trash 
            desc = Regex.Replace(desc, @"<span[^>]*>", "", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<\/span>", "", RegexOptions.IgnoreCase);
            desc = Regex.Replace(desc, @"<p[^>]*>", "", RegexOptions.IgnoreCase);
            return desc;
        }


        public static string BBCodeToHTML(this string desc)
        {
            if (string.IsNullOrWhiteSpace(desc))
            {
                return string.Empty;
            }

            var exp = new Regex(@"\[b\](.+?)\[/b\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<strong>$1</strong>");

            exp = new Regex(@"\[i\](.+?)\[/i\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<em>$1</em>");

            exp = new Regex(@"\[u\](.+?)\[/u\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<u>$1</u>");

            exp = new Regex(@"\[s\](.+?)\[/s\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<strike>$1</strike>");

            exp = new Regex(@"\[code\](.+?)\[/code\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<code>$1</code>");

            exp = new Regex(@"\[spoiler\](.+?)\[/spoiler\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<span class=\"spoiler-wrapper\" title=\"Spoiler\">$1</span>");

            exp = new Regex(@"\[\*\](.+)\[/\*\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<li>$1</li>");

            exp = new Regex(@"\[olist\](.+)\[/olist\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<ol>$1</ol>");

            exp = new Regex(@"\[ulist\](.+)\[/ulist\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<ul>$1</ul>");

            exp = new Regex(@"\[img\](.+)\[/img\]", RegexOptions.Multiline);
            desc = exp.Replace(desc, "<img src=\"$1\" />");

            exp = new Regex(@"\[img\=([^\]]+)\](.+)\[/img\]", RegexOptions.Multiline);
            desc = exp.Replace(desc, "<img src=\"$1\" alt=\"$2\" />");

            exp = new Regex(@"\[quote\=([^\]]+)\](.+)\[/quote\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<q cite=\"$1\">$2</q>");

            exp = new Regex(@"\[quote[^\]]*](.+)\[/quote\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<q>$1</q>");

            exp = new Regex(@"\[spoiler\=([^\]]+)\](.+)\[/spoiler\]", RegexOptions.Singleline);
            desc = exp.Replace(desc, "<span class=\"spoiler\" title=\"$1\">$2</span>");

            exp = new Regex(@"\[url\=([^\]]+)\](.+)\[/url\]", RegexOptions.Multiline);
            desc = exp.Replace(desc, "<a href=\"$1\">$2</a>");

            var matchVideo = Regex.Matches(desc, @"\[video\](?<url>.*?)\[/video\]", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (matchVideo.Count > 0)
            {
                foreach (Match match in matchVideo)
                {
                    var videoUrl = match.Groups[1].Value;
                    var video = VideoHelper.GetVideoByUrl(videoUrl);
                    desc = desc.Replace(match.ToString(), "<div class=\"video-wrapper\">" + video + "</div>");
                }
            }
            desc = desc.Replace("\n\r", "<br />\n\r");
            desc = desc.Replace("\r\n", "<br />\r\n");
            return desc;
        }

        public static string ReplaceSmiles(this string desc, Func<int, string, string> x)
        {
            var smilesArr = smiles.OrderByDescending(p => p.Length).Select(p => p);

            foreach (var smile in smilesArr)
            {
                
                var i = smiles.ToList().IndexOf(smile);
                if (desc.IndexOf(smile) != -1)
                {
                    desc = desc.Replace(smile, x.Invoke(i + 1, smile));
                }
            }

            return desc;
        }

        public static double Round(this double src, int decimals = 2) 
        {
            return Math.Round(src, decimals);
        }

        public static string ToSteamID(this string source)
        {
            if (source == null) 
            {
                return string.Empty;
            }
            if (source.StartsWith("http://") || source.IndexOf("steamcommunity.com") != -1)
            {
                if (source.IndexOf("7656119") != -1)
                {
                    return source.Substring(source.IndexOf("/profiles/") + "/profiles/".Length);
                }
                if (source.IndexOf("steamcommunity.com/id") != -1)
                {
                    return source.Substring(source.IndexOf("steamcommunity.com/id/") + "steamcommunity.com/id/".Length);
                }
            }
            if (source.IndexOf("7656119") != -1)
            {
                return source.Substring(source.IndexOf("7656119"));
            }
            return source;
        }
    }
}