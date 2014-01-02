using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<ProductVideo> ProductVideos
        {
            get
            {
                return Db.ProductVideos;
            }
        }

        public bool CreateProductVideo(ProductVideo instance)
        {
            if (instance.ID == 0)
            {
                Db.ProductVideos.InsertOnSubmit(instance);
                Db.ProductVideos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateProductVideo(ProductVideo instance)
        {
            var cache = Db.ProductVideos.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ProductID = instance.ProductID;
				cache.VideoUrl = instance.VideoUrl;
				cache.VideoThumb = instance.VideoThumb;
				cache.VideoCode = instance.VideoCode;
                Db.ProductVideos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveProductVideo(int idProductVideo)
        {
            ProductVideo instance = Db.ProductVideos.FirstOrDefault(p => p.ID == idProductVideo);
            if (instance != null)
            {
                Db.ProductVideos.DeleteOnSubmit(instance);
                Db.ProductVideos.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}