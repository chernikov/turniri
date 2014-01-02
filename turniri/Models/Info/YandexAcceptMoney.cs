using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace turniri.Models.Info
{
    public class YandexAcceptMoney
    {
        public string notification_type { get; set; }

        public string operation_id { get; set; }

        public string amount { get; set; }

       /* public double Amount
        {
            get
            {
                double tryAmount = 0;
                if (Double.TryParse(amount, NumberStyles.None, CultureInfo.InvariantCulture, out tryAmount))
                {
                    return tryAmount;
                }
                return 0.0;
            }
        }
        */

        public string currency { get; set; }

        public string datetime { get; set; }

        public string sender { get; set; }

        public string codepro { get; set; }

        public string label { get; set; }

        public string sha1_hash { get; set; }
    }
}