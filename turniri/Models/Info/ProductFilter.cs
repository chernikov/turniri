using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;

namespace turniri.Models.Info
{
    public class ProductFilter
    {
        public PageableData<Product> Data { get; set; }

        public int Page { get; set; }

        public string SearchWord { get; set; }

        private IRepository repository
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var _repository = kernel.Get<IRepository>("RequestScoped");
                return _repository;
            }
        }

        public enum PeriodEnum : int
        {
            Before2001 = 0x01,
            From2001To2007 = 0x02,
            From2008To2012 = 0x03,
            From2012ToNow = 0x04,
            New = 0x05
        }

        public enum PriceEnum : int
        {
            Free = 0x01,
            Under500 = 0x02,
            To1000 = 0x03,
            To2000 = 0x04,
            Upper2000 = 0x05
        }

        public List<Catalog> Catalogs
        {
            get
            {
                return repository.Catalogs.ToList();
            }
        }

        public int? CatalogID { get; set; }

        public IEnumerable<SelectListItem> SelectListCatalogs
        {
            get
            {
                foreach (var catalog in Catalogs)
                {
                    yield return new SelectListItem() { Value = catalog.ID.ToString(), Text = catalog.Name, Selected = CatalogID == catalog.ID };
                }
            }
        }

        public string SelectedCatalogDesc
        {
            get
            {
                var catalog = Catalogs.FirstOrDefault(p => p.ID == CatalogID);
                if (catalog != null)
                {
                    return catalog.Name;
                }
                return string.Empty;
            }
        }

        public int? Period { get; set; }

        public IEnumerable<SelectListItem> SelectListPeriod
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)PeriodEnum.Before2001).ToString(), Text = "До 2001 года", Selected = Period == (int)PeriodEnum.Before2001 };
                yield return new SelectListItem() { Value = ((int)PeriodEnum.From2001To2007).ToString(), Text = "2001 - 2006 года", Selected = Period == (int)PeriodEnum.From2001To2007 };
                yield return new SelectListItem() { Value = ((int)PeriodEnum.From2008To2012).ToString(), Text = "2007 - 2011 года", Selected = Period == (int)PeriodEnum.From2008To2012 };
                yield return new SelectListItem() { Value = ((int)PeriodEnum.From2012ToNow).ToString(), Text = "с 2012 года", Selected = Period == (int)PeriodEnum.From2012ToNow };
                yield return new SelectListItem() { Value = ((int)PeriodEnum.New).ToString(), Text = "Новинки", Selected = Period == (int)PeriodEnum.New };
            }
        }

        public string SelectedPeriodDesc
        {
            get
            {
                var selected = SelectListPeriod.FirstOrDefault(p => p.Selected);
                if (selected != null)
                {
                    return selected.Text;
                }
                return string.Empty;
            }
        }

        public int? Price { get; set; }



        public IEnumerable<SelectListItem> SelectListPrice
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)PriceEnum.Free).ToString(), Text = "Бесплатные", Selected = Price == (int)PriceEnum.Free };
                yield return new SelectListItem() { Value = ((int)PriceEnum.Under500).ToString(), Text = "до 500 рублей", Selected = Price == (int)PriceEnum.Under500 };
                yield return new SelectListItem() { Value = ((int)PriceEnum.To1000).ToString(), Text = "до 1000 рублей", Selected = Price == (int)PriceEnum.To1000 };
                yield return new SelectListItem() { Value = ((int)PriceEnum.To2000).ToString(), Text = "до 2000 рублей", Selected = Price == (int)PriceEnum.To2000 };
                yield return new SelectListItem() { Value = ((int)PriceEnum.Upper2000).ToString(), Text = "свыше 2000 рублей", Selected = Price == (int)PriceEnum.Upper2000 };
            }
        }

        public string SelectedPriceDesc
        {
            get
            {
                var selected = SelectListPrice.FirstOrDefault(p => p.Selected);
                if (selected != null)
                {
                    return selected.Text;
                }
                return string.Empty;
            }
        }

        public int Type { get; set; }

        public int Category { get; set; }

        public int TypeCategory
        {
            get
            {
                return Type * 100 + Category;
            }
            set
            {
                Type = value / 100;
                Category = value % 100;
            }
        }

        public string TypeCategoryDesc
        {
            get
            {
                if (Category != 0)
                {
                    var category = Categories.FirstOrDefault(p => p.ID == Category);
                    if (category != null)
                    {
                        return category.Name;
                    }
                }
                if (Type == 2)
                {
                    return "Товары сайта";
                }

                return "";
            }
        }


        private List<Category> Categories
        {
            get
            {
                return repository.Categories.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListCategories
        {
            get
            {
                foreach (var category in Categories)
                {
                    yield return new SelectListItem() { Value = (100 + category.ID).ToString(), Text = category.Name, Selected = TypeCategory == 100 + category.ID };
                }
                yield return new SelectListItem() { Value = "200", Text = "Товары сайта", Selected = TypeCategory == 200 };
            }
        }

        public IEnumerable<Platform> Platforms
        {
            get
            {
                return repository.Platforms.ToList();
            }
        }

        public List<int> SelectedPlatform { get; set; }

        public string SelectedPlatformDesc
        {
            get
            {
                var selected = Platforms.Where(p => SelectedPlatform.Contains(p.ID)).ToList();
                return string.Join(", ", selected.Select(p => p.Name));
            }
        }

        public IEnumerable<SelectListItem> SelectListPlatforms
        {
            get
            {
                foreach (var platform in Platforms)
                {
                    yield return new SelectListItem() { Value = platform.ID.ToString(), Text = platform.Name, Selected = SelectedPlatform.Contains(platform.ID) };
                }
            }
        }

        public ProductFilter()
        {
            Page = 1;
            SelectedPlatform = new List<int>();
        }

        public void Process(IQueryable<Product> products, string action)
        {

            if (TypeCategory != 0)
            {
                if (Type == 1)
                {
                    products = products.Where(p => p.Type == (int)Product.TypeEnum.Code && p.CategoryID == Category);
                    if (CatalogID.HasValue)
                    {
                        products = products.Where(p => p.ProductCatalogs.Any(r => r.CatalogID == CatalogID.Value));
                    }
                }
                else
                {
                    products = products.Where(p => p.Type != (int)Product.TypeEnum.Code);
                }
            }
            else
            {
                if (CatalogID.HasValue)
                {
                    products = products.Where(p => p.ProductCatalogs.Any(r => r.CatalogID == CatalogID.Value));
                }
            }

            if (Period.HasValue)
            {
                switch ((PeriodEnum)Period.Value)
                {
                    case PeriodEnum.Before2001:
                        products = products.Where(p => p.ReleaseDate < DateTime.Parse("2000-12-31"));
                        break;
                    case PeriodEnum.From2001To2007:
                        products = products.Where(p => p.ReleaseDate >= DateTime.Parse("2001-01-01") && p.ReleaseDate < DateTime.Parse("2007-12-31"));
                        break;
                    case PeriodEnum.From2008To2012:
                        products = products.Where(p => p.ReleaseDate >= DateTime.Parse("2008-01-01") && p.ReleaseDate < DateTime.Parse("2011-12-31"));
                        break;
                    case PeriodEnum.From2012ToNow:
                        products = products.Where(p => p.ReleaseDate >= DateTime.Parse("2012-01-01"));
                        break;
                    case PeriodEnum.New:
                        products = products.Where(p => p.IsNew);
                        break;
                }
            }
            if (SelectedPlatform.Any())
            {
                products = products.Where(p => p.ProductPrices.Any(r => r.PlatformID.HasValue && SelectedPlatform.Contains(r.PlatformID.Value)));
            }

            if (Price.HasValue)
            {
                switch ((PriceEnum)Price.Value)
                {
                    case PriceEnum.Free:
                        if (SelectedPlatform.Any())
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price == 0 && r.PlatformID.HasValue && SelectedPlatform.Contains(r.PlatformID.Value)));
                        }
                        else
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price == 0));
                        }
                        break;
                    case PriceEnum.Under500:
                        if (SelectedPlatform.Any())
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price <= 500 && r.PlatformID.HasValue && SelectedPlatform.Contains(r.PlatformID.Value)));
                        }
                        else
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price <= 500));
                        }
                        break;
                    case PriceEnum.To1000:
                        if (SelectedPlatform.Any())
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price <= 1000 && r.PlatformID.HasValue && SelectedPlatform.Contains(r.PlatformID.Value)));
                        }
                        else
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price <= 1000));
                        }
                        break;
                    case PriceEnum.To2000:
                        if (SelectedPlatform.Any())
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price <= 2000 && r.PlatformID.HasValue && SelectedPlatform.Contains(r.PlatformID.Value)));
                        }
                        else
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price <= 2000));
                        }
                        break;
                    case PriceEnum.Upper2000:
                        if (SelectedPlatform.Any())
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price >= 2000 && r.PlatformID.HasValue && SelectedPlatform.Contains(r.PlatformID.Value)));
                        }
                        else
                        {
                            products = products.Where(p => p.ProductPrices.Any(r => r.Price >= 2000));
                        }
                        break;
                }
            }
            products = products.OrderByDescending(p => p.IsPromo ? 1 : 0).AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchWord)) 
            {
                products = SearchEngine.Search(SearchWord, products).AsQueryable();
            }
            Data = new PageableData<Product>();
            Data.Init(products, Page, action);
        }
    }
}