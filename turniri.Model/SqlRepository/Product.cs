using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Product> Products
        {
            get
            {
                return Db.Products;
            }
        }

        public bool CreateProduct(Product instance)
        {
            if (instance.ID == 0)
            {
                instance.Url = Translit.Translate(instance.Name);
                Db.Products.InsertOnSubmit(instance);
                Db.Products.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateProduct(Product instance)
        {
            var cache = Db.Products.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.GameID = instance.GameID;
                cache.CategoryID = instance.CategoryID;
                cache.VendorID = instance.VendorID;
                cache.Type = instance.Type;
                cache.Name = instance.Name;
                cache.SubTitle = instance.SubTitle;
                cache.Image = instance.Image;
                cache.Description = instance.Description;
                cache.SmsName = instance.SmsName;
                cache.Background = instance.Background;
                cache.ReleaseDate = instance.ReleaseDate;
                cache.SystemRequirements = instance.SystemRequirements;
                cache.Creator = instance.Creator;
                cache.Publisher = instance.Publisher;
                cache.RussianPublisher = instance.RussianPublisher;
                cache.Language = instance.Language;
                cache.AgeRestrict = instance.AgeRestrict;

                cache.IsNew = instance.IsNew;
                cache.IsPromo = instance.IsPromo;
                cache.IsAction = instance.IsAction;
                Db.Products.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveProduct(int idProduct)
        {
            var instance = Db.Products.FirstOrDefault(p => p.ID == idProduct);
            if (instance != null)
            {
                instance.IsDeleted = true;
                Db.Products.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RestoreProduct(int idProduct)
        {
            var instance = Db.Products.FirstOrDefault(p => p.ID == idProduct);
            if (instance != null)
            {
                instance.IsDeleted = false;
                Db.Products.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateVisitCountProduct(int idProduct)
        {
            var instance = Db.Products.FirstOrDefault(p => p.ID == idProduct);
            if (instance != null)
            {
                instance.VisitCount++;
                Db.Products.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ClearProductPrices(int idProduct, List<int> exist)
        {
            var listForDelete = Db.ProductPrices.Where(p => p.ProductID == idProduct && !exist.Contains(p.ID)).ToList();

            foreach (var price in listForDelete)
            {
                RemoveProductPrice(price.ID);
            }
            Db.ProductPrices.Context.SubmitChanges();
            return true;
        }

        public bool ClearProductImages(int idProduct, List<int> exist)
        {
            var listForDelete = Db.ProductImages.Where(p => p.ProductID == idProduct);
            Db.ProductImages.DeleteAllOnSubmit(listForDelete);
            Db.ProductImages.Context.SubmitChanges();
            return true;
        }

        public bool ClearProductVariations(int idProduct, List<int> exist)
        {
            var listForDelete = Db.ProductVariations.Where(p => p.ProductID == idProduct && !exist.Contains(p.ID)).ToList();
            foreach (var price in listForDelete)
            {
                RemoveProductVariation(price.ID);
            }
            Db.ProductVariations.Context.SubmitChanges();
            return true;
        }

        public bool ClearProductVideos(int idProduct, List<int> exist)
        {
            var listForDelete = Db.ProductVideos.Where(p => p.ProductID == idProduct && !exist.Contains(p.ID));
            Db.ProductVideos.DeleteAllOnSubmit(listForDelete);
            Db.ProductVideos.Context.SubmitChanges();
            return true;
        }

        public bool UpdateProductCatalogs(int idProduct, List<int> catalogs)
        {
            var product = Db.Products.FirstOrDefault(p => p.ID == idProduct);
            if (product != null)
            {
                //remove others
                var listForDelete = product.ProductCatalogs.Where(p => !catalogs.Contains(p.CatalogID));
                var existList = product.ProductCatalogs.Where(p => catalogs.Contains(p.CatalogID)).Select(p => p.CatalogID).ToList();
                Db.ProductCatalogs.DeleteAllOnSubmit(listForDelete);
                Db.ProductCatalogs.Context.SubmitChanges();
                //new list
                var newCatalogs = catalogs.Where(p => !existList.Contains(p)).Select(p => p);
                foreach (var id in newCatalogs)
                {
                    var catalog = Db.Catalogs.FirstOrDefault(p => p.ID == id);

                    if (catalog != null)
                    {
                        var productCatalog = new ProductCatalog
                        {
                            CatalogID = catalog.ID,
                            ProductID = product.ID
                        };
                        Db.ProductCatalogs.InsertOnSubmit(productCatalog);
                        Db.ProductCatalogs.Context.SubmitChanges();
                    }
                }
                return true;
            }
            return false;
        }
    }
}