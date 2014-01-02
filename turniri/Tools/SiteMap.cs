using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;
using turniri.Model;

namespace turniri.Tools
{
    public class SiteMap
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public enum ChangeFrequency
        {
            always,
            daily,
            weekly,
            monthly,
            yearly,
            never
        }

        public static XmlDocument CreateSiteMap(string host, IRepository Repository)
        {
            var xmlDoc = new XmlDocument();
            var siteMapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = xmlDoc.CreateElement("urlset");
            var attrXmlnsXsi = xmlDoc.CreateAttribute("xmlns:xsi");
            attrXmlnsXsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
            root.Attributes.Append(attrXmlnsXsi);
            var attSchemaLocation = xmlDoc.CreateAttribute("xsi:schemaLocation");
            attSchemaLocation.Value = siteMapNamespace + " http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd";
            root.Attributes.Append(attSchemaLocation);
            var attrXmlns = xmlDoc.CreateAttribute("xmlns");
            attrXmlns.Value = siteMapNamespace;
            root.Attributes.Append(attrXmlns);

            //игры
            root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/tournaments/", ChangeFrequency.always, DateTime.Now, 1));

            foreach (var game in Repository.Games)
            {
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/game/" + game.Platform.Url + "/" + game.Url, ChangeFrequency.monthly, DateTime.Now, 1));
                //турниры
                foreach (var tournament in game.Tournaments)
                {
                    root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/tournament/" + tournament.Platform.Url + "/" + tournament.Game.Url + "/" + tournament.Url, ChangeFrequency.always, DateTime.Now, 0.8));
                }
            }
            //блоги
            foreach (var blog in Repository.Blogs)
            {
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/blog/" + blog.Url, ChangeFrequency.weekly, DateTime.Now, 0.8));
            }
            //новости
            foreach (var @new in Repository.News)
            {
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/new/" + @new.Url, ChangeFrequency.weekly, DateTime.Now, 0.8));
            }
            //фото
            root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/photos", ChangeFrequency.weekly, DateTime.Now, 1));
            foreach (var photoAlbum in Repository.PhotoAlbums)
            {
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/photo/" + photoAlbum.Url, ChangeFrequency.weekly, DateTime.Now, 0.8));
            }
            
            //видео
            root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/videos", ChangeFrequency.weekly, DateTime.Now, 1));
            foreach (var video in Repository.Videos)
            {
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/video/" + video.Url, ChangeFrequency.weekly, DateTime.Now, 0.8));
            }

            root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/users", ChangeFrequency.weekly, DateTime.Now, 1));
            /*foreach (var user in Repository.RegularUsers)
            {
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/user/" + user.Login, ChangeFrequency.weekly, DateTime.Now, 0.8));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/user/blog/" + user.Login, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/user/photo/" + user.Login, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/user/video/" + user.Login, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/user/friends/" + user.Login, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/user/group/" + user.Login, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/user/award/" + user.Login, ChangeFrequency.weekly, DateTime.Now, 0.6));
            }*/
            root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/groups", ChangeFrequency.always, DateTime.Now, 1));
            foreach (var group in Repository.Groups)
            {
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/group/" + group.Url, ChangeFrequency.weekly, DateTime.Now, 0.8));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/group/blog/" + group.Url, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/group/photo/" + group.Url, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/group/video/" + group.Url, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/group/roster/" + group.Url, ChangeFrequency.weekly, DateTime.Now, 0.6));
                root.AppendChild(CreateUrl(xmlDoc, "http://" + host + "/group/award/" + group.Url, ChangeFrequency.weekly, DateTime.Now, 0.6));
            }

            var xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmldecl);
            xmlDoc.AppendChild(root);
            return xmlDoc;
        }

        public static XmlElement CreateUrl(XmlDocument xmlDoc, string loc, ChangeFrequency frequency, DateTime lastModification, double priority)
        {
            var urlMainPage = xmlDoc.CreateElement("url");
            var urlLocMainPage = xmlDoc.CreateElement("loc");
            urlLocMainPage.InnerText = loc;
            var urlChangefreqMainPage = xmlDoc.CreateElement("changefreq");
            urlChangefreqMainPage.InnerText = frequency.ToString();
            var urlLastmodMainPage = xmlDoc.CreateElement("lastmod");
            urlLastmodMainPage.InnerText = lastModification.ToString("yyyy'-'MM'-'dd");
            var urlPriorityMainPage = xmlDoc.CreateElement("priority");
            urlPriorityMainPage.InnerText = priority.ToString(CultureInfo.InvariantCulture);
            urlMainPage.AppendChild(urlLocMainPage);
            urlMainPage.AppendChild(urlChangefreqMainPage);
            urlMainPage.AppendChild(urlLastmodMainPage);
            urlMainPage.AppendChild(urlPriorityMainPage);
            return urlMainPage;
        }
    }
}