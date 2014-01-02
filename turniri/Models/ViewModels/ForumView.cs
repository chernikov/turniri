using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using turniri.Model;
using System.ComponentModel.DataAnnotations;


namespace turniri.Models.ViewModels
{
    public class ForumView
    {
        private Model.User Admin { get; set; }

        public int _ID { get; set; }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public int? ParentID { get; set; }

        public int? UserID { get; set; }

        public int? PollID { get; set; }

        public bool IsEnd { get; set; }

        [Required]
        public string Name { get; set; }

        public string Url { get; set; }

        public string ImagePath { get; set; }

        public string SubTitle { get; set; }

        public string FullImagePath
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath) ? ImagePath : "/Media/images/forum_default_preview.png";
            }
        }

        public ForumMessageView Message { get; set; }

        public List<Poll> Polls
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                if (Admin != null)
                {
                    if (Admin.InRoles("admin"))
                    {
                        return repository.Polls.OrderByDescending(p => p.AddedDate).ToList();
                    }
                    else
                    {
                        return repository.Polls.Where(p => p.UserID == Admin.ID).OrderByDescending(p => p.AddedDate).ToList();
                    }
                }
                return null;
            }
        }

        public IEnumerable<SelectListItem> PollsSelectList
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "Без голосования",
                    Selected = !PollID.HasValue
                };

                foreach (var poll in Polls)
                {
                    yield return new SelectListItem
                    {
                        Value = poll.ID.ToString(),
                        Text = poll.Name,
                        Selected = poll.ID == PollID
                    };
                }
            }
        }

        public void InitAdmin(Model.User user)
        {
            this.Admin = user;
        }
    }
}