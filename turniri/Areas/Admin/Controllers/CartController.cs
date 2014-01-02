using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,seller")]
    public class CartController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.Carts.Where(p => p.OrderType != (int)Cart.OrderTypes.Created && p.OrderType != (int)Cart.OrderTypes.Canceled && p.MoneyDetails.Any()).OrderByDescending(p => p.ID);

            var data = new PageableData<Cart>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Item(int id)
        {
            var item = Repository.Carts.FirstOrDefault(p => p.ID == id);
            if (item != null)
            {
                return View(item);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Count()
        {
            var count = Repository.Carts.Count(p => p.MoneyDetails.Any() && p.CartProducts.Any(r => r.Quantity > r.ProductCodes.Count() && (r.Product.Type == (int)Product.TypeEnum.Code || r.Product.Type == (int)Product.TypeEnum.RealGood)));

            return View("Count", count);
        }

        [HttpGet]
        public ActionResult AddCode(int id)
        {
            var productCart = Repository.CartProducts.FirstOrDefault(p => p.ID == id);

            if (productCart != null)
            {
                var cartProductCodeView = new CartProductCodeView()
                    {
                        CartProductID = productCart.ID,
                        ProductID = productCart.ProductID,
                        ProductPriceID = productCart.ProductPriceID,
                        ProductVariationID = productCart.ProductVariationID
                    };
                return View(cartProductCodeView);
            }

            return null;
        }

        [HttpGet]
        public ActionResult AddReal(int id)
        {
            var productCart = Repository.CartProducts.FirstOrDefault(p => p.ID == id);

            if (productCart != null)
            {
                var count = productCart.Quantity - productCart.ProductCodes.Count;

                for (int i = 0; i < count; i++)
                {
                    var productCode = new ProductCode()
                    {
                        Code = productCart.Product.Name,
                        ProductID = productCart.ProductID,
                        ProductPriceID = productCart.ProductPriceID,
                        ProductVariationID = productCart.ProductVariationID,
                        CartProductID = productCart.ID
                    };

                    Repository.CreateProductCode(productCode);
                }
            }

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddCode(CartProductCodeView cartProductCodeView)
        {
            var code = Repository.ProductCodes.FirstOrDefault(p => string.Compare(p.Code, cartProductCodeView.Code, true) == 0 && p.ID != cartProductCodeView.ID);

            if (code != null)
            {
                return Json(new { result = "error", error = "Такой код уже есть" });
            }

            if (string.IsNullOrWhiteSpace(cartProductCodeView.Code))
            {
                return Json(new { result = "error", error = "Введите код" });
            }

            var productCode = (ProductCode)ModelMapper.Map(cartProductCodeView, typeof(CartProductCodeView), typeof(ProductCode));

            if (productCode.ID == 0)
            {
                Repository.CreateProductCode(productCode);
            }
            else
            {
                Repository.UpdateProductCode(productCode);
            }

            var cart = Repository.Carts.FirstOrDefault(p => p.ID == productCode.CartProduct.CartID);

            if (!cart.NeedToProcess)
            {
                var listOfCodes = new List<ProductCode>();
                var guid = Guid.NewGuid();
                foreach (var item in cart.SubCartProducts)
                {

                    if (item.Product.IsCodeTyped)
                    {
                        if (item.ProductPrice.Preorder)
                        {
                            var codes = Repository.ProductCodes.Where(p => p.CartProductID == item.ID);

                            foreach (var itemCode in codes)
                            {
                                if (itemCode.Product.Type == (int)Product.TypeEnum.Code)
                                {
                                    listOfCodes.Add(itemCode);
                                }
                            }
                        }
                    }
                }
                Repository.SubmitMoney(guid);
                if (listOfCodes != null && listOfCodes.Count > 0)
                {
                    SendMailOfCodes(cart.ID, cart.Email, listOfCodes);
                }
            }
            return Json(new { result = "ok" });
        }

        public ActionResult SendAgain(int id)
        {
            var cart = Repository.Carts.FirstOrDefault(p => p.ID == id);
            if (cart != null && !cart.NeedToProcess)
            {
                var listOfCodes = new List<ProductCode>();
                var guid = Guid.NewGuid();
                foreach (var item in cart.SubCartProducts)
                {

                    if (item.Product.IsCodeTyped)
                    {
                        var codes = Repository.ProductCodes.Where(p => p.CartProductID == item.ID);

                        foreach (var itemCode in codes)
                        {
                            if (itemCode.Product.Type == (int)Product.TypeEnum.Code)
                            {
                                listOfCodes.Add(itemCode);
                            }
                        }
                    }
                }
                Repository.SubmitMoney(guid);

                if (listOfCodes != null && listOfCodes.Count > 0)
                {
                    SendMailOfCodes(cart.ID, cart.Email, listOfCodes);
                }
            }
            return RedirectToAction("Item", new { id });
        }
    }
}
