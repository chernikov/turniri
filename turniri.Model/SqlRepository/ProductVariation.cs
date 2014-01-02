using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ProductVariation> ProductVariations
        {
            get
            {
                return Db.ProductVariations;
            }
        }

        public bool CreateProductVariation(ProductVariation instance)
        {
            if (instance.ID == 0)
            {
                Db.ProductVariations.InsertOnSubmit(instance);
                Db.ProductVariations.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateProductVariation(ProductVariation instance)
        {
            var cache = Db.ProductVariations.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ProductID = instance.ProductID;
				cache.Image = instance.Image;
                Db.ProductVariations.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveProductVariation(int idProductVariation)
        {
            ProductVariation instance = Db.ProductVariations.FirstOrDefault(p => p.ID == idProductVariation);
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
                    Db.ProductVariations.DeleteOnSubmit(instance);
                }
                Db.ProductVariations.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}