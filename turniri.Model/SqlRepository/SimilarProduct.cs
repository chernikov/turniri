using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<SimilarProduct> SimilarProducts
        {
            get
            {
                return Db.SimilarProducts;
            }
        }

        public bool UpdateSimilarProducts(int productId, List<int> productsList)
        {
            var product = Db.Products.FirstOrDefault(p => p.ID == productId);
            if (productsList == null)
            {
                productsList = new List<int>();
            }
            if (product != null)
            {
                //remove others
                var listForDelete = product.SimilarProducts.Where(p => !productsList.Contains(p.SimilarProductID));
                var existList = product.SimilarProducts.Where(p => productsList.Contains(p.SimilarProductID)).Select(p => p.SimilarProductID).ToList();
                Db.SimilarProducts.DeleteAllOnSubmit(listForDelete);
                Db.SimilarProducts.Context.SubmitChanges();
                //new list
                var newPersons = productsList.Where(p => !existList.Contains(p)).Select(p => p);
                foreach (var id in newPersons)
                {
                    var similarProduct = Db.Products.FirstOrDefault(p => p.ID == id);

                    if (similarProduct != null)
                    {
                        var similarProductIns = new SimilarProduct
                        {
                            ProductID = product.ID,
                            SimilarProductID = similarProduct.ID
                        };
                        Db.SimilarProducts.InsertOnSubmit(similarProductIns);
                        Db.SimilarProducts.Context.SubmitChanges();
                    }
                }
                return true;
            }
            return false;
        }
    }
}