using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class ProductCodeView
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

        public int ProductID { get; set; }

        public Product Product
        {
            get
            {
                return repository.Products.FirstOrDefault(p => p.ID == ProductID);
            }
        }

        public int ProductPriceID { get; set; }

        public int? ProductVariationID { get; set; }

        private IEnumerable<ProductPrice> ProductPrices
        {
            get
            {
                return repository.ProductPrices.Where(p => p.ProductID == ProductID && !p.IsDeleted).AsEnumerable();
            }
        }

        public IEnumerable<ProductVariation> ProductVariations
        {
            get
            {
                return repository.ProductVariations.Where(p => p.ProductID == ProductID).AsEnumerable();
            }
        }

        public IEnumerable<SelectListItem> SelectListProductPriceID
        {
            get
            {
                foreach (var productPrice in ProductPrices)
                {
                    yield return new SelectListItem() { Value = productPrice.ID.ToString(), Text = productPrice.Name, Selected = ProductPriceID == productPrice.ID };
                }
            }
        }

        public string Code { get; set; }

        public string Image { get; set; }

        public string FullImage
        {
            get
            {
                return string.IsNullOrEmpty(Image) ? "/Media/images/default.png" : Image;
            }
        }
    }
}