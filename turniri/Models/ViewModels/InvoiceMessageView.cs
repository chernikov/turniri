using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.Models.ViewModels
{
    public class InvoiceMessageView
    {
        public int ID { get; set; }

        public int SenderID { get; set; }

        public int ReceiverID { get; set; }

        public bool InitedGroup { get; set; }

        public string ReceiverLogin { get; set; }

        [Required(ErrorMessage = "Введите текст")]
        public string Text { get; set; }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        private List<Group> Groups
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                var user = repository.Users.FirstOrDefault(p => p.ID == SenderID);
                if (user != null)
                {
                    return user.GroupsForInvoiceFor(ReceiverID).ToList();
                }
                return new List<Group>();
            }
        }

        public IEnumerable<SelectListItem> GroupSelectList
        {
            get
            {
                return Groups.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Selected = p.ID == GroupID,
                    Text = p.Name
                });
            }
        }
    }
}