using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class UserGroupView
    {
        private IRepository repository;

        public UserGroupView()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            repository = kernel.Get<IRepository>("RequestScoped");
        }

        public int ID { get; set; }

        public int GroupID { get; set; }

        public int UserID { get; set; }

        public string UserLogin { get; set; }

        public DateTime AddedDate { get; set; }

        public int Status { get; set; }

        public List<Role> Roles
        {
            get
            {
                return repository.UserRoleGroups.Where(p => p.GroupID == GroupID && p.UserRole.UserID == UserID).Select(p => p.UserRole.Role).ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListRoles
        {
            get
            {
                foreach(var role in repository.Roles.Where(p => p.Code.StartsWith("group_"))) 
                {
                    yield return new SelectListItem()
                    {
                        Value = role.ID.ToString(),
                        Text = role.Name,
                        Selected = Roles.Contains(role)
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> SelectListStatus
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = ((int)UserGroup.StatusEnum.Asked).ToString(),
                    Text = "Запрос",
                    Selected = Status == (int)UserGroup.StatusEnum.Asked
                };

                yield return new SelectListItem()
                {
                    Value = ((int)UserGroup.StatusEnum.Granded).ToString(),
                    Text = "Принят",
                    Selected = Status == (int)UserGroup.StatusEnum.Granded
                };
            }
        }
    }
}