using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class MainCameraView
    {
        public int ID { get; set; }

		public int? CameraID {get; set; }

        [AllowHtml]
		public string Code {get; set; }

		public bool Enabled {get; set; }


        private List<Camera> Cameras
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.Cameras.Where(p => p.Enabled).ToList();
            }
        }

        public IEnumerable<SelectListItem> CamerasSelectList
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "<<Нет>>",
                    Selected = !CameraID.HasValue
                };
                foreach (var camera in Cameras)
                {
                    yield return new SelectListItem()
                    {
                        Value = camera.ID.ToString(),
                        Text = camera.Desc,
                        Selected = camera.ID == CameraID
                    };
                };
            }
        }

    }
}