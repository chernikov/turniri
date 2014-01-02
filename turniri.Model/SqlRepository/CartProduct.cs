using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<CartProduct> CartProducts
        {
            get
            {
                return Db.CartProducts;
            }
        }

        public bool CreateCartProduct(CartProduct instance)
        {
            if (instance.ID == 0)
            {
                Db.CartProducts.InsertOnSubmit(instance);
                Db.CartProducts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCartProductQuantity(CartProduct instance)
        {
            var cache = Db.CartProducts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Quantity = instance.Quantity;
                Db.CartProducts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCartProduct(int idCartProduct)
        {
            CartProduct instance = Db.CartProducts.FirstOrDefault(p => p.ID == idCartProduct);
            if (instance != null)
            {
                Db.CartProducts.DeleteOnSubmit(instance);
                Db.CartProducts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}