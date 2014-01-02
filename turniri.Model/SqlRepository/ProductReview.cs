using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ProductReview> ProductReviews
        {
            get
            {
                return Db.ProductReviews;
            }
        }

        public bool CreateProductReview(ProductReview instance)
        {
            if (instance.ID == 0)
            {
                Db.ProductReviews.InsertOnSubmit(instance);
                Db.ProductReviews.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateProductReview(ProductReview instance)
        {
            var cache = Db.ProductReviews.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ProductID = instance.ProductID;
				cache.UserID = instance.UserID;
				cache.AddedDate = instance.AddedDate;
				cache.Text = instance.Text;
                Db.ProductReviews.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveProductReview(int idProductReview)
        {
            ProductReview instance = Db.ProductReviews.FirstOrDefault(p => p.ID == idProductReview);
            if (instance != null)
            {
                Db.ProductReviews.DeleteOnSubmit(instance);
                Db.ProductReviews.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}