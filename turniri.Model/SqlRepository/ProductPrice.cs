using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ProductPrice> ProductPrices
        {
            get
            {
                return Db.ProductPrices;
            }
        }

        public bool CreateProductPrice(ProductPrice instance)
        {
            if (instance.ID == 0)
            {
                Db.ProductPrices.InsertOnSubmit(instance);
                Db.ProductPrices.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateProductPrice(ProductPrice instance)
        {
            var cache = Db.ProductPrices.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ProductID = instance.ProductID;
				cache.PlatformID = instance.PlatformID;
                cache.OldPrice = instance.OldPrice;
				cache.Price = instance.Price;
				cache.Preorder = instance.Preorder;
                Db.ProductPrices.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveProductPrice(int idProductPrice)
        {
            ProductPrice instance = Db.ProductPrices.FirstOrDefault(p => p.ID == idProductPrice);
            if (instance != null)
            {
                if (instance.CartProducts.Any())
                {
                    instance.IsDeleted = true;
                }
                else if (instance.ProductCodes.Any())
                {
                    instance.IsDeleted = true;
                }
                else
                {
                    Db.ProductPrices.DeleteOnSubmit(instance);
                }
                Db.ProductPrices.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}