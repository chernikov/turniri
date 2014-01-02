using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class ProductCode
    {
        public bool IsReserved
        {
            get
            {
                if (IsSelled)
                {
                    return true;
                }
                if (CartProductID == null) 
                {
                    return false;
                }
                if ((CartProduct.Cart.OrderType == (int)Cart.OrderTypes.Created || CartProduct.Cart.OrderType == (int)Cart.OrderTypes.Prepared) && CartProduct.Cart.LastVisitDate.AddHours(6) > DateTime.Now)
                {
                    return true;
                }
                if (CartProduct.Cart.Recharges.Any())
                {
                    var recharge = CartProduct.Cart.Recharges.First();

                    if (recharge.AddedDate.AddHours(7) > DateTime.Now)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
	}
}