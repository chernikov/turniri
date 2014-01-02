using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class ProductPrice
    {
        public string Name
        {
            get
            {
                return (PlatformID.HasValue ? Platform.Name + ":" : "") + " " + Price.ToString("0,00") + " руб.";
            }
        }

        public bool HasUnselledCodes
        {
            get
            {
                return ProductCodes.Any(p => !p.CartProductID.HasValue);
            }
        }

        public bool IsPreorder
        {
            get
            {
                return (Preorder || !HasUnselledCodes) && Product.Type != (int)Product.TypeEnum.GoldMoney;
            }
        }
	}
}