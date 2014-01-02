using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using turniri.Model;
using Ninject;


namespace turniri.Models.ViewModels
{
    public class ProductPriceView
    {
        private IRepository repository
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var _repository = kernel.Get<IRepository>("RequestScoped");
                return _repository;
            }
        }

        public int ID { get; set; }

        public int? PlatformID { get; set; }

        private IEnumerable<Platform> Platforms
        {
            get
            {
                return repository.Platforms.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListPlatforms
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "Без платформы",
                    Selected = !PlatformID.HasValue
                };
                foreach (var platform in Platforms)
                {
                    yield return new SelectListItem()
                    {
                        Value = platform.ID.ToString(),
                        Text = platform.Name,
                        Selected = PlatformID == platform.ID
                    };
                }
            }
        }

        public double Price { get; set; }

        public double? OldPrice { get; set; }

        public bool Preorder { get; set; }
    }
}