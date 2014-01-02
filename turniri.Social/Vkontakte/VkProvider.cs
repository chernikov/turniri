﻿using Krystalware.UploadHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace turniri.Social.Vkontakte
{
    public class VkProvider
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string AuthorizeUri =
            "https://oauth.vk.com/authorize?client_id={0}&redirect_uri={1}&response_type=code";

        public static string SuperAuthorizeUri =
            "https://oauth.vk.com/authorize?client_id={0}&scope=photos,offline,wall,groups&redirect_uri=https://oauth.vk.com/blank.html&response_type=code";


        public static string GetTokenUri =
            "https://oauth.vk.com/access_token?client_id={0}&client_secret={1}&code={2}&redirect_uri={3}";

        public static string GetUserInfoUri =
            "https://api.vkontakte.ru/method/users.get?uids={0}&fields=uid,first_name,last_name,nickname,domain,sex,bdate,city,country,timezone,photo,photo_medium,photo_big,has_mobile,rate,contacts,education,online";

        public static string GetGroupsByIdUri =
           "https://api.vkontakte.ru/method/groups.getById?gid={0}";

        public static string GetPhotoUploadServerUri =
          "https://api.vkontakte.ru/method/photos.getWallUploadServer?access_token={0}";

        public static string PhotosSaveWallPhotoUri =
          "https://api.vkontakte.ru/method/photos.saveWallPhoto?server={0}&photo={1}&hash={2}&access_token={3}";

        public static string PublicUrl =
            "https://api.vkontakte.ru/method/wall.post?owner_id={0}&access_token={1}&from_group=1";
        public IVkAppConfig Config;

        public VkAccessToken AccessToken { get; set; }

        public string Authorize(string redirectTo)
        {
            return string.Format(AuthorizeUri, Config.AppKey, redirectTo);
        }

        public string SuperAuthorize(string redirectTo)
        {
            return string.Format(SuperAuthorizeUri, Config.AppKey, redirectTo);
        }

        public bool GetAccessToken(string Code, string redirectUrl)
        {
            try
            {
                string reqStr = string.Format(GetTokenUri, Config.AppKey, Config.AppSecret, Code, redirectUrl);

                logger.Debug(reqStr);
                WebClient webClient = new WebClient();
                var response = webClient.DownloadString(reqStr);

                logger.Debug(response);
                AccessToken = JsonConvert.DeserializeObject<VkAccessToken>(response);
                return true;
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);
                return false;
            }
        }

        public string GetUserInfo()
        {
            string reqStr = string.Format(GetUserInfoUri, AccessToken.UserId);
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var response = webClient.DownloadString(reqStr);
            logger.Debug("VK User INFO: " + response);
            return response;
        }

        public string GetGroupByIds(string name)
        {
            string reqStr = string.Format(GetGroupsByIdUri, name);
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            return webClient.DownloadString(reqStr);
        }

        public string GetPhotoUploadServer(int groupID)
        {
            string reqStr = string.Format(GetPhotoUploadServerUri,/* groupID,*/ AccessToken.AccessToken);
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            return webClient.DownloadString(reqStr);
        }

        public string SavePhoto(string server, string photo, string hash)
        {
            string reqStr = string.Format(PhotosSaveWallPhotoUri, server, photo, hash, AccessToken.AccessToken);
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            return webClient.DownloadString(reqStr);
        }

        public List<string> UploadPhotos(string[] FilePaths, int groupID)
        {
            var ids = new List<string>();

            var serverResponse = GetPhotoUploadServer(groupID);
            var serverJson = JsonConvert.DeserializeObject<VkPhotoUploadServerResponse>(serverResponse);

            foreach (var file in FilePaths)
            {
                var fileResponse = UploadFilesToRemoteUrl(serverJson.Response.UploadUrl, "photo", file);
                var jsonFile = JsonConvert.DeserializeObject<VkUploadPhotoResponse>(fileResponse);

                var savePhotoResponse = SavePhoto(jsonFile.Server, jsonFile.Photo, jsonFile.Hash);
                var jsonPhoto = JsonConvert.DeserializeObject<VkSavePhotoWallResponse>(savePhotoResponse);

                if (jsonPhoto.Response.Count > 0)
                {
                    ids.Add(jsonPhoto.Response[0].Id);
                }
            }
            return ids;
        }

        public string Publish(ISocialPost post, string groupName)
        {
            var groupResponse = GetGroupByIds(groupName);
            var groups = JsonConvert.DeserializeObject<VkGroupResponse>(groupResponse);

            if (groups.Response.Count > 0)
            {
                var groupId = groups.Response[0].Id * -1;

                string request = string.Format(PublicUrl, groupId, AccessToken.AccessToken);

                List<string> attached = new List<string>();
                if (post.Images != null && post.Images.Count > 0)
                {
                    attached = UploadPhotos(post.Images.ToArray(), groupId);
                }
                //message
                if (!string.IsNullOrWhiteSpace(post.Teaser))
                {
                    request += string.Format("&message={0}", HttpUtility.UrlEncode(post.Teaser));
                }
                var attach = string.Empty;
                if (attached.Count > 0)
                {
                    foreach (var id in attached)
                    {
                        attach += id + ",";
                    }
                }

                if (!string.IsNullOrWhiteSpace(post.Link))
                {
                    attach += post.Link + ",";
                }

                attach = attach.Remove(attach.Count() - 1);

                if (!string.IsNullOrWhiteSpace(attach))
                {
                    request += string.Format("&attachments={0}", attach);
                }

                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                return webClient.DownloadString(request);
            }
            return null;
        }

        public string PreparePublishOnGroupAttachedWall(ISocialPost post, out int groupId, string groupName = null)
        {
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var groupResponse = GetGroupByIds(groupName);
                var groups = JsonConvert.DeserializeObject<VkGroupResponse>(groupResponse);
                if (groups.Response.Count > 0)
                {
                    groupId = groups.Response[0].Id * -1;
                }
                else
                {
                    groupId = 0;
                }
            }
            else
            {
                groupId = 0;
            }
            List<string> attached = new List<string>();
            if (post.Images != null && post.Images.Count > 0)
            {
                attached = UploadPhotos(post.Images.ToArray(), groupId);
            }
            //message
            var attach = string.Empty;
            if (attached.Count > 0)
            {
                foreach (var id in attached)
                {
                    attach += id + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(post.Link))
            {
                attach += post.Link + ",";
            }
            attach = attach.Remove(attach.Count() - 1);
            return attach;
        }


        public string PreparePublishAttachedWall(ISocialPost post)
        {
            var idUser = Int32.Parse(AccessToken.UserId);
            post.Identifier = AccessToken.UserId;

            string request = string.Format(PublicUrl, idUser, AccessToken.AccessToken);
            List<string> attached = new List<string>();
            if (post.Images != null && post.Images.Count > 0)
            {
                attached = UploadPhotos(post.Images.ToArray(), idUser);
            }
            //message
            var attach = string.Empty;
            if (attached.Count > 0)
            {
                foreach (var id in attached)
                {
                    attach += id + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(post.Link))
            {
                attach += post.Link + ",";
            }

            if (!string.IsNullOrWhiteSpace(attach))
            {
                attach = attach.Remove(attach.Count() - 1);
            }
            return attach;
        }


        public string PublishWall(ISocialPost post)
        {
            var idUser = Int32.Parse(AccessToken.UserId);
            post.Identifier = AccessToken.UserId;

            string request = string.Format(PublicUrl, idUser, AccessToken.AccessToken);
            List<string> attached = new List<string>();
            if (post.Images != null && post.Images.Count > 0)
            {
                attached = UploadPhotos(post.Images.ToArray(), idUser);
            }
            //message
            if (!string.IsNullOrWhiteSpace(post.Teaser))
            {
                request += string.Format("&message={0}", post.Teaser);
            }
            var attach = string.Empty;
            if (attached.Count > 0)
            {
                foreach (var id in attached)
                {
                    attach += id + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(post.Link))
            {
                attach += post.Link + ",";
            }

            if (!string.IsNullOrWhiteSpace(attach))
            {
                attach = attach.Remove(attach.Count() - 1);
            }
            if (!string.IsNullOrWhiteSpace(attach))
            {
                request += string.Format("&attachments={0}", attach);
            }
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            return webClient.DownloadString(request);
        }

        public static string UploadFilesToRemoteUrl(string url, string fileName, string filePath)
        {
            var files = new UploadFile[] 
            { 
                new UploadFile(filePath, fileName, "image/jpeg")
            };

            NameValueCollection form = new NameValueCollection();

            string response = HttpUploadHelper.Upload(url, files, form);

            return response;

            /* HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

             string boundary = Guid.NewGuid().ToString().Replace("-","");
             req.ContentType = "multipart/form-data; boundary=" +boundary;
             req.Method = "POST";

             using (var postData = new MemoryStream())
             {
                 string newLine = "\r\n";
                 StreamWriter sw = new StreamWriter(postData);
                 sw.Write("--" + boundary + newLine);
                 sw.Write("Content-Disposition: form-data;name=\"{0}\";filename=\"{1}\"{2}", "fileName", Path.GetFileName(filePath), newLine);
                 sw.Write("Content-Type: image/pjpeg " + newLine + newLine);
                 sw.Flush();

                 using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                 {
                     byte[] buffer = new byte[1024];
                     int bytesRead = 0;
                     while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                     {
                         postData.Write(buffer, 0, bytesRead);
                     }
                     fileStream.Close();
                 }

                 sw.Write(newLine);
                 sw.Write("--{0}--{1}", boundary,
                 newLine);
                 sw.Flush();

                 req.ContentLength = postData.Length;
                 using (Stream s = req.GetRequestStream())
                 {
                     postData.WriteTo(s);
                 }
                 postData.Close();
             }

             WebResponse webResponse = req.GetResponse();

             Stream responceStream = webResponse.GetResponseStream();
             Encoding enc = System.Text.Encoding.UTF8;
             StreamReader loResponseStream = new StreamReader(webResponse.GetResponseStream(), enc);

             string Response = loResponseStream.ReadToEnd();
             webResponse.Close();
             req = null;
             webResponse = null;
             */
        }
    }
}
