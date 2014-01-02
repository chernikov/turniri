using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Attributes.Validation;
using turniri.Model;
using turniri.Models.Info;


namespace turniri.Models.ViewModels
{
    public class ProductView
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

        public int _ID
        {
            get
            {
                return ID;
            }
            set
            {
                ID = value;
            }
        }

        public int Type { get; set; }

        public IEnumerable<SelectListItem> TypeSelectList
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = ((int)Product.TypeEnum.Code).ToString(),
                    Text = "Код (высылается на Email)",
                    Selected = Type == (int)Product.TypeEnum.Code
                };

                yield return new SelectListItem()
                {
                    Value = ((int)Product.TypeEnum.GoldMoney).ToString(),
                    Text = "Золотое ТИ",
                    Selected = Type == (int)Product.TypeEnum.GoldMoney
                };

                yield return new SelectListItem()
                {
                    Value = ((int)Product.TypeEnum.RealGood).ToString(),
                    Text = "Реальный товар (необходима доставка по адресу)",
                    Selected = Type == (int)Product.TypeEnum.RealGood
                };
            }
        }

        public int ID { get; set; }

        public int? GameID { get; set; }

        public int? VendorID { get; set; }

        public int? CategoryID { get; set; }

        private List<Game> Games
        {
            get
            {
                return repository.Games.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListGameID
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "Не выбрано",
                    Selected = !GameID.HasValue
                };

                foreach (var item in Games)
                {
                    yield return new SelectListItem()
                    {
                        Value = item.ID.ToString(),
                        Text = string.Format("{0} на {1}", item.Name, item.Platform.Name),
                        Selected = GameID == item.ID
                    };
                }
            }
        }

        [Required(ErrorMessage="Введите наименование")]
        public string Name { get; set; }

        public string SubTitle { get; set; }

        [Required(ErrorMessage = "Загрузите изображение")]
        public string Image { get; set; }

        public string FullImage
        {
            get { return string.IsNullOrWhiteSpace(Image) ? "/Media/images/default_game.jpg" : Image; }
        }

        public Dictionary<string, ProductPriceView> ProductPrices { get; set; }
        
        public Dictionary<string, ProductImageView> ProductImages { get; set; }

        public Dictionary<string, ProductVideoView> ProductVideos { get; set; }

        public Dictionary<string, ProductVariationView> ProductVariations { get; set; }

        public string SmsName { get; set; }

        public string IsAction { get; set; }

        public string IsPromo { get; set; }

        public string IsNew { get; set; }

        public string Background { get; set; }

        public string FullBackground
        {
            get
            {
                return string.IsNullOrWhiteSpace(Background) ? "/Media/images/default.png" : Background;
            }
        }

        public DateTime? ReleaseDate { get; set; }

        public string Creator { get; set; }

        public string Publisher { get; set; }

        public string RussianPublisher { get; set; }

        public string Language { get; set; }

        public int? AgeRestrict { get; set; }

        public IEnumerable<SelectListItem> SelectListAgeRestrict
        {
            get
            {
                yield return new SelectListItem() { Value = "", Text = "Не установлено", Selected = !AgeRestrict.HasValue };
                yield return new SelectListItem() { Value = ((int)Product.AgeRestrictType.ZeroPlus).ToString(), Text = "0+", Selected = AgeRestrict == (int)Product.AgeRestrictType.ZeroPlus };
                yield return new SelectListItem() { Value = ((int)Product.AgeRestrictType.SixPlus).ToString(), Text = "6+", Selected = AgeRestrict == (int)Product.AgeRestrictType.SixPlus };
                yield return new SelectListItem() { Value = ((int)Product.AgeRestrictType.TwelvePlus).ToString(), Text = "12+", Selected = AgeRestrict == (int)Product.AgeRestrictType.TwelvePlus };
                yield return new SelectListItem() { Value = ((int)Product.AgeRestrictType.SixteenPlus).ToString(), Text = "16+", Selected = AgeRestrict == (int)Product.AgeRestrictType.SixteenPlus };
                yield return new SelectListItem() { Value = ((int)Product.AgeRestrictType.Adult).ToString(), Text = "18+", Selected = AgeRestrict == (int)Product.AgeRestrictType.Adult };
            }
        }

        public List<int> ProductCatalogs { get; set; }

        private IList<Catalog> Catalogs
        {
            get
            {
                return repository.Catalogs.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListProductCatalogs
        {
            get
            {
                return Catalogs.Select(p => new SelectListItem()
                {
                    Value = p.ID.ToString(),
                    Text = p.Name,
                    Selected = ProductCatalogs.Contains(p.ID)
                });
            }
        }

        private IList<Vendor> Vendors
        {
            get
            {
                return repository.Vendors.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListVendor
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "Не выбрано",
                    Selected = !VendorID.HasValue
                };

                foreach (var vendor in Vendors)
                {
                    yield return new SelectListItem()
                    {
                        Value = vendor.ID.ToString(),
                        Text = vendor.Name,
                        Selected = VendorID == vendor.ID
                    };
                };
            }
        }

        private IList<Category> Categories
        {
            get
            {
                return repository.Categories.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListCategory
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "Не выбрано",
                    Selected = !CategoryID.HasValue
                };

                foreach (var category in Categories)
                {
                    yield return new SelectListItem()
                    {
                        Value = category.ID.ToString(),
                        Text = category.Name,
                        Selected = CategoryID == category.ID
                    };
                };
            }
        }

        [AllowHtml]
        public string Description { get; set; }


        [AllowHtml]
        public string SystemRequirements { get; set; }

        //похожие товары
        public List<int> ProductsList { get; set; }

        public IQueryable<Product> Products
        {
            get
            {
                if (ProductsList != null)
                {
                    return repository.Products.Where(p => ProductsList.Contains(p.ID));
                }
                return null;
            }
        }

        public IEnumerable<SelectListItem> SelectListProducts
        {
            get
            {
                var list = new List<SelectListItem>();
                if (ProductsList != null)
                {
                    foreach (var product in Products)
                    {
                        list.Add(new SelectListItem
                        {
                            Selected = true,
                            Text = product.Name,
                            Value = product.ID.ToString()
                        });
                    }
                }
                return list;
            }
        }


        public ProductView()
        {
            ProductCatalogs = new List<int>();
            ProductPrices = new Dictionary<string, ProductPriceView>();
            ProductImages = new Dictionary<string, ProductImageView>();
            ProductVideos = new Dictionary<string, ProductVideoView>();
            ProductVariations = new Dictionary<string, ProductVariationView>();
        }

    }
}