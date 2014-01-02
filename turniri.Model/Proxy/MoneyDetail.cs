using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class MoneyDetail
    {
        public double TotalSumGold
        {
            get
            {
                if (MoneyFee != null)
                {
                    return SumGold * (1 + MoneyFee.PercentFee / 100);
                }
                return SumGold;
            }
        }

        public double TotalSumGoldReturn
        {
            get
            {
                if (MoneyFee != null)
                {
                    return SumGold * (1 - MoneyFee.PercentFee / 100);
                }
                return SumGold;
            }


        }
	}
}