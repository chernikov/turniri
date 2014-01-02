using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using turniri.Attributes;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class NewView
    {
        public int ID { get; set; }

		public int NewTypeID {get; set; }

	    public List<NewType> NewTypes
	    {
	        get 
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.NewTypes.ToList();
            }
	    }

	    public IEnumerable<SelectListItem> NewTypesSelectList
	    {
	        get
	        {
	            return NewTypes.Select(p => new SelectListItem
	                                     {
	                                         Value = p.ID.ToString(),
	                                         Text = p.Name,
	                                         Selected = p.ID == NewTypeID
	                                     });
	        }
	    }

		public bool IsMain {get; set; }

        [RequiredIf("IsMain", true, ErrorMessage = "Добавьте большую картинку")]
		public string PreviewPath {get; set; }

	    public string FullPreviewPath
	    {
	        get { return !string.IsNullOrWhiteSpace(PreviewPath) ? PreviewPath : "/Media/images/no-image.png"; }
	    }

		public string AvatarPreviewPath {get; set; }

		public string TitlePath {get; set; }

		public string AvatarTitlePath {get; set; }

        public string FullAvatarTitlePath
        {
            get { return !string.IsNullOrWhiteSpace(AvatarTitlePath) ? AvatarTitlePath : "/Media/images/no-image.png"; }
        }

        [Required(ErrorMessage = "Введите заголовок")]
		public string Header {get; set; }

		public string SubHeader {get; set; }

        [Required(ErrorMessage = "Введите текст")]
		public string Text {get; set; }

    }
}