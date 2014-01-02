using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ProductImage> ProductImages
        {
            get
            {
                return Db.ProductImages;
            }
        }

        public bool CreateProductImage(ProductImage instance)
        {
            if (instance.ID == 0)
            {
                Db.ProductImages.InsertOnSubmit(instance);
                Db.ProductImages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateProductImage(ProductImage instance)
        {
            var cache = Db.ProductImages.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ProductID = instance.ProductID;
				cache.Image = instance.Image;
                Db.ProductImages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveProductImage(int idProductImage)
        {
            ProductImage instance = Db.ProductImages.FirstOrDefault(p => p.ID == idProductImage);
            if (instance != null)
            {
                Db.ProductImages.DeleteOnSubmit(instance);
                Db.ProductImages.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public ProductImage ChangeProductScreenshot(int idProductImage, bool next)
        {
            var currentScreenshot = Db.ProductImages.FirstOrDefault(p => p.ID == idProductImage);

            if (currentScreenshot != null)
            {
                var productID = currentScreenshot.ProductID;

                ProductImage nextScreenshot = null;
                if (next)
                {
                    //next 
                    nextScreenshot = Db.ProductImages.Where(p => p.ProductID == productID && p.ID > currentScreenshot.ID).OrderBy(p => p.ID).FirstOrDefault();
                    //if last - get first
                    if (nextScreenshot == null)
                    {
                        nextScreenshot = Db.ProductImages.Where(p => p.ProductID == productID).OrderBy(p => p.ID).FirstOrDefault();
                    }
                }
                else
                {
                    //next 
                    nextScreenshot = Db.ProductImages.Where(p => p.ProductID == productID && p.ID < currentScreenshot.ID).OrderByDescending(p => p.ID).FirstOrDefault();
                    //if first - get last
                    if (nextScreenshot == null)
                    {
                        nextScreenshot = Db.ProductImages.Where(p => p.ProductID == productID).OrderByDescending(p => p.ID).FirstOrDefault();
                    }
                }
                return nextScreenshot;
            }
            return null;
        }
    }
}