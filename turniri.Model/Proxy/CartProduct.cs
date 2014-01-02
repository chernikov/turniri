using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class CartProduct
    {
        public IList<ProductCode> SubCodes
        {
            get
            {
                return ProductCodes.ToList();
            }
        }

        public double Sum
        {
            get
            {
                return Price * Quantity;
            }
        }
	}
}