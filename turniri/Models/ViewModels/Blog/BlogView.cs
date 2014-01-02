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
	public class BlogView
    {
        public int ID { get; set; }

		public int UserID {get; set; }

        public int? GroupID { get; set; }

        [Required(ErrorMessage = "Введите заголовок")]
		public string Header {get; set; }

        [Required(ErrorMessage = "Введите текст")]
		public string Text {get; set; }

		public string PreviewUrl {get; set; }

        public string FullPreviewUrl 
        {
            get
            {
                return string.IsNullOrWhiteSpace(PreviewUrl) ? "/Media/images/no-preview199.jpg" : PreviewUrl;
            }
        }


        public IEnumerable<Group> Groups
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");

                return repository.UserGroups.Where(p => p.UserID == UserID && p.Status == (int)UserGroup.StatusEnum.Granded).Select(p => p.Group).ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListGroups
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "", 
                    Text = "В личный блог", 
                    Selected = !GroupID.HasValue
                };

                foreach (var item in Groups)
                {
                    yield return new SelectListItem()
                    {
                        Value = item.ID.ToString(),
                        Text = "В блог команды " + item.Name,
                        Selected = GroupID == item.ID
                    };
                }
            }
        }
    }
}