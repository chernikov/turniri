using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Models.ViewModels
{
    public class ProductRestView
    {
        public int ProductID { get; set; }

        public int? ProductVariationID { get; set; }

        public int Rest { get; set; }
    }
}