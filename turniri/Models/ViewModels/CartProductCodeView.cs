using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class CartProductCodeView : ProductCodeView
    {
        public int _ID
        {
            get { return ID; }
            set { ID = value; }
        }

        public int? CartProductID { get; set; }
    }
}