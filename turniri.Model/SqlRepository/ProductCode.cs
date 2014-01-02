using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ProductCode> ProductCodes
        {
            get
            {
                return Db.ProductCodes;
            }
        }

        public bool CreateProductCode(ProductCode instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.ProductCodes.InsertOnSubmit(instance);
                Db.ProductCodes.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateProductCode(ProductCode instance)
        {
            var cache = Db.ProductCodes.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ProductID = instance.ProductID;
				cache.Code = instance.Code;
				cache.Image = instance.Image;
				Db.ProductCodes.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ReserveProductCode(ProductCode instance, CartProduct cartProduct)
        {
            var cache = Db.ProductCodes.FirstOrDefault(p => p.ID == instance.ID);
            var cartProductCache = Db.CartProducts.FirstOrDefault(p => p.ID == cartProduct.ID);
            if (cache != null)
            {
                cache.CartProductID = cartProductCache.ID;
                Db.ProductCodes.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UnReserveProductCode(ProductCode instance)
        {
            var cache = Db.ProductCodes.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                var cartProduct = Db.CartProducts.FirstOrDefault(p => p.ID == instance.CartProductID);
                cache.CartProduct = null;
                Db.ProductCodes.Context.SubmitChanges();
                if (cartProduct.Product.IsCodeTyped)
                {
                    cartProduct.Quantity = Db.ProductCodes.Count(p => p.CartProductID == cartProduct.ID);
                    Db.CartProducts.Context.SubmitChanges();
                }
                return true;
            }
            return false;
        }

        public bool SellProductCode(ProductCode instance)
        {
            var cache = Db.ProductCodes.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.IsSelled = true;
                Db.ProductCodes.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveProductCode(int idProductCode)
        {
            ProductCode instance = Db.ProductCodes.FirstOrDefault(p => p.ID == idProductCode);
            if (instance != null)
            {
                Db.ProductCodes.DeleteOnSubmit(instance);
                Db.ProductCodes.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}