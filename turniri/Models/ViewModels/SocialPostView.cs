using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class SocialPostView
    {
        public enum Foreign : int
        {
            blog = 0x01,
            @new = 0x02,
            video = 0x03,
            tournament = 0x04,
            photoAlbum = 0x05
        }

        public int ID { get; set; }

        private Model.User User { get; set; }

        public int UserID { get; set; }

        public int? SocialGroupID { get; set; }

        private List<SocialGroup> SocialGroups
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.SocialGroups.Where(p => p.Provider == Provider).ToList();
            }
        }
        public IEnumerable<SelectListItem> SocialGroupSelectList
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "На стену",
                    Selected = !SocialGroupID.HasValue,
                };

                foreach (var socialGroup in SocialGroups)
                {
                    yield return new SelectListItem()
                    {
                        Value = socialGroup.ID.ToString(),
                        Selected = socialGroup.ID == SocialGroupID,
                        Text = socialGroup.Name
                    };
                }
            }
        }

        public int ForeignID { get; set; }

        public int ForeignType { get; set; }

        public int Provider { get; set; }

        public IEnumerable<SelectListItem> ProviderSelectList
        {
            get
            {
                if (User != null)
                {
                    if (User.HasAdvansedSocial(Model.Social.ProviderType.facebook))
                    {
                        yield return new SelectListItem() { Value = ((int)Model.Social.ProviderType.facebook).ToString(), Text = "Facebook", Selected = Provider == (int)Model.Social.ProviderType.facebook };
                    }
                    if (User.HasAdvansedSocial(Model.Social.ProviderType.vk))
                    {
                        yield return new SelectListItem() { Value = ((int)Model.Social.ProviderType.vk).ToString(), Text = "Vk", Selected = Provider == (int)Model.Social.ProviderType.vk };
                    }
                    if (User.HasAdvansedSocial(Model.Social.ProviderType.twitter))
                    {
                        yield return new SelectListItem() { Value = ((int)Model.Social.ProviderType.twitter).ToString(), Text = "Twitter", Selected = Provider == (int)Model.Social.ProviderType.twitter };
                    }
                }
                
            }
        }
       
        public string Title { get; set; }

        public string Preview { get; set; }

        public string Teaser { get; set; }

        public string Link { get; set; }

        public List<SocialPostImageView> SocialPostImages { get; set; }

       
        public SocialPostView()
        {
            Provider = (int)Model.Social.ProviderType.facebook;
        }

        public void InitUser(Model.User user)
        {
            this.User = user;
        }
    }
}