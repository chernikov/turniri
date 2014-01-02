using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Social;

namespace turniri.Model
{
    public partial class SocialPost : ISocialPost
    {
        public IList<string> Images
        {
            get
            {
                return SocialPostImages.Select(p => p.PhotoUrl).ToList();
            }
          
        }

        public SocialPost Copy()
        {
            var post = new SocialPost()
            {
                UserID = this.UserID,
                Preview = this.Preview,
                Title = this.Title,
                Teaser = this.Teaser,
                Link = this.Link,
            };

            foreach (var image in SocialPostImages.ToList()) 
            {
                post.SocialPostImages.Add(image);
            }
            return post;
        }

        public string ProviderShortName
        {
            get
            {
                switch ((Social.ProviderType)Provider)
                {
                    case Social.ProviderType.facebook :
                        return "fb";
                    case Social.ProviderType.google :
                        return "g";
                    case Social.ProviderType.twitter:
                        return "tw";
                    case Social.ProviderType.vk :
                        return "vk";
                }
                return string.Empty;
            }
        }
    }
}