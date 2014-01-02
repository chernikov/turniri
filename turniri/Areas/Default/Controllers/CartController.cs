using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.Info;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Default.Controllers
{
    public class CartController : DefaultController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View(GetCart());
        }

        public ActionResult History()
        {
            var list = Repository.Carts.Where(p => p.UserID == CurrentUser.ID && p.OrderType == (int)Model.Cart.OrderTypes.Delivered);

            return View(list);
        }

        public ActionResult TopCart()
        {
            return View(GetCart());
        }

        [HttpGet]
        public ActionResult Process()
        {
            var cart = GetCart();

            var cartDeliverView = (CartDeliverView)ModelMapper.Map(cart, typeof(Cart), typeof(CartDeliverView));

            if (CurrentUser != null)
            {
                cartDeliverView.User = CurrentUser;
                cartDeliverView.Email = CurrentUser.Email;
                cartDeliverView.Phone = CurrentUser.Phone;
                cartDeliverView.Address = string.Format("{0}, {1}, {2}", CurrentUser.Country, CurrentUser.City, CurrentUser.Address);
            }
            else if (cartDeliverView.IsGold)
            {
                ViewData["message"] = "Не авторизованный пользователь не может купить золотые ТИ";
                return View("CantProcess");
            }
            return View(cartDeliverView);
        }

        [HttpPost]
        public ActionResult Process(CartDeliverView cartDeliverView)
        {
            if (ModelState.IsValid)
            {
                var cart = GetCart();

                cart.Email = cartDeliverView.Email ?? string.Empty;
                cart.Phone = cartDeliverView.Phone ?? string.Empty;
                cart.Address = cartDeliverView.Address ?? string.Empty;
                cart.Notice = cartDeliverView.Notice ?? string.Empty;
                cart.PaymentType = cartDeliverView.PaymentType;
                cart.TotalPrice = cart.TotalSum;
                cart.OrderType = (int)Model.Cart.OrderTypes.Prepared;
                Repository.UpdateCart(cart);

                switch ((Model.Cart.PaymentTypes)cartDeliverView.PaymentType)
                {
                    case Model.Cart.PaymentTypes.GoldMoney:
                        return View("GoldMoney", cart);

                    case Model.Cart.PaymentTypes.Qiwi:
                        var qiwiRecharge = new Recharge()
                        {
                            CartID = cart.ID,
                            GlobalUniqueID = Identity,
                            UserID = CurrentUser != null  ? (int?)CurrentUser.ID : null,
                            Sum = cart.TotalSum,
                            Provider = (int)Recharge.ProviderType.Qiwi,
                            Description = string.Format("Оплата заказа #{0}", cart.ID),
                        };

                        Repository.CreateRecharge(qiwiRecharge);

                        var qiwiRechargeInfo = new QiwiRechargeInfo(Config)
                        {
                            Label = qiwiRecharge.ID,
                            Description = string.Format("Оплата заказа #{0}", cart.ID),
                            To = cart.Phone.ClearPhone().Substring(1),
                            ChargeSum = qiwiRecharge.Sum,
                            Sum = qiwiRecharge.Sum,
                            LifeTime = 6,
                        };
                        
                        return View("Qiwi", qiwiRechargeInfo);
                    case Model.Cart.PaymentTypes.Yandex:
                        var yandexRecharge = new Recharge()
                        {
                            CartID = cart.ID,
                            GlobalUniqueID = Identity,
                            UserID = CurrentUser != null ? (int?)CurrentUser.ID : null,
                            Sum = cart.TotalSum,
                            Provider = (int)Recharge.ProviderType.Yandex,
                            Description = string.Format("Оплата заказа #{0}", cart.ID),
                        };

                        Repository.CreateRecharge(yandexRecharge);
                        var yandexRechargeInfo = new YandexRechargeInfo()
                        {
                            Label = yandexRecharge.ID,
                            ShortDest = string.Format("Оплата заказа #{0}", cart.ID),
                            WritableTargets = false,
                            CommentNeeded = false,
                            QuickpayForm = "shop",
                            Targets = string.Format("Оплата заказа #{0} та turniri.ru", cart.ID),
                            Comment = "",
                            ChargeSum = yandexRecharge.Sum,
                            Sum = yandexRecharge.Sum,
                            Receiver = Config.YandexWallet
                        };
                        return View("Yandex", yandexRechargeInfo);
                }
            }
            if (CurrentUser != null)
            {
                cartDeliverView.User = CurrentUser;
            }
            return View(cartDeliverView);
        }

        public ActionResult AddToCart(int id, int? idVariation)
        {
            var productPrice = Repository.ProductPrices.FirstOrDefault(p => p.ID == id);

            ProductVariation productVariation = Repository.ProductVariations.FirstOrDefault(p => p.ID == idVariation);
            if (productPrice != null)
            {
                ProductCode code = null;
                if (productPrice.Product.IsCodeTyped && !productPrice.Preorder)
                {
                    int? productVariationID = productVariation != null ? (int?)productVariation.ID : null;
                    code = productPrice.ProductCodes.ToList().FirstOrDefault(p => !p.IsReserved && !p.IsSelled && p.ProductVariationID == productVariationID);
                    if (code == null)
                    {
                        return Json(new { result = "error", error = "В данный момент нет доступного товара" }, JsonRequestBehavior.AllowGet);
                    }
                }
                var cart = GetCart();

                var cartProduct = cart.CartProducts.FirstOrDefault(p => p.ProductPriceID == productPrice.ID && p.ProductVariationID == (productVariation != null ? (int?)productVariation.ID : null));

                if (cartProduct == null)
                {
                    cartProduct = new CartProduct()
                    {
                        CartID = cart.ID,
                        ProductPriceID = productPrice.ID,
                        ProductID = productPrice.Product.ID,
                        Price = productPrice.Price,
                        ProductVariationID = productVariation != null ? (int?)productVariation.ID : null,
                        Quantity = 1
                    };
                    Repository.CreateCartProduct(cartProduct);
                }
                else
                {
                    cartProduct.Quantity++;
                    Repository.UpdateCartProductQuantity(cartProduct);
                }
                if (productPrice.Product.IsCodeTyped && !productPrice.Preorder)
                {
                    if (!Repository.ReserveProductCode(code, cartProduct))
                    {
                        Repository.RemoveCartProduct(cartProduct.ID);
                        return Json(new { result = "error", error = "Ошибка при бронировании товара" }, JsonRequestBehavior.AllowGet);
                    };
                }

                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProcessQiwiRecharge(int id)
        {
            var recharge = Repository.Recharges.FirstOrDefault(p => p.ID == id);
            if (recharge != null && recharge.IsSubmitted && recharge.CartID.HasValue)
            {
                var cart = Repository.Carts.FirstOrDefault(p => p.ID == recharge.CartID);
                ProcessCart(cart);
                return Content("OK");
            }
            return Content("ERROR");
        }

        public ActionResult Cheat()
        {
            var cart = GetCart();
            var listOfCodes = new List<ProductCode>();
            var guid = Guid.NewGuid();
            foreach (var item in cart.SubCartProducts)
            {
                if (item.Product.IsCodeTyped)
                {
                    var codes = Repository.ProductCodes.Where(p => p.CartProductID == item.ID);

                    foreach (var code in codes)
                    {
                        if (code.Product.Type == (int)Product.TypeEnum.Code)
                        {
                            listOfCodes.Add(code);
                        }
                    }
                }
            }
            Repository.SubmitMoney(guid);

            if (listOfCodes != null && listOfCodes.Count > 0)
            {
                SendMailOfCodes(cart.ID, "chernikov@gmail.com", listOfCodes);
            }
            return Content("ok");
        }

        public ActionResult UpdateQuantity(int id, int value)
        {
            var cart = GetCart();
            var cartProduct = cart.CartProducts.FirstOrDefault(p => p.ID == id);
            if (cartProduct != null)
            {
                if (cartProduct.Product.IsCodeTyped && !cartProduct.ProductPrice.Preorder)
                {
                    var diff = value - cartProduct.Quantity;
                    if (diff < 0)
                    {
                        if (value <= 0)
                        {
                            Repository.RemoveCartProduct(cartProduct.ID);
                        }
                        else
                        {
                            for (int i = 0; i > diff; i--)
                            {
                                var code = Repository.ProductCodes.FirstOrDefault(p => p.CartProductID == cartProduct.ID);
                                if (code != null)
                                {
                                    Repository.UnReserveProductCode(code);
                                }

                            }
                            cartProduct.Quantity = Repository.ProductCodes.Count(p => p.CartProductID == cartProduct.ID);
                            Repository.UpdateCartProductQuantity(cartProduct);
                        }
                        return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        for (int i = 0; i < diff; i++)
                        {
                            var code = Repository.ProductCodes.FirstOrDefault(p => p.ProductID == cartProduct.ProductID && p.CartProductID == null && p.ProductVariationID == cartProduct.ProductVariationID);
                            if (code != null)
                            {
                                Repository.ReserveProductCode(code, cartProduct);
                            }
                            else
                            {
                                cartProduct.Quantity = Repository.ProductCodes.Count(p => p.CartProductID == cartProduct.ID);
                                Repository.UpdateCartProductQuantity(cartProduct);
                                return Json(new { result = "error", error = "Не доступно больше товара" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        cartProduct.Quantity = Repository.ProductCodes.Count(p => p.CartProductID == cartProduct.ID);
                        Repository.UpdateCartProductQuantity(cartProduct);
                        return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (value <= 0)
                    {
                        Repository.RemoveCartProduct(cartProduct.ID);
                    }
                    else
                    {
                        cartProduct.Quantity = value;
                        Repository.UpdateCartProductQuantity(cartProduct);
                    }
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveCartProduct(int id)
        {
            var cart = GetCart();
            var cartProduct = cart.CartProducts.FirstOrDefault(p => p.ID == id);
            if (cartProduct != null)
            {
                Repository.RemoveCartProduct(cartProduct.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearCart()
        {
            var cart = GetCart();

            Repository.ClearCart(cart.ID);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GoldMoney()
        {
            var cart = GetCart();

            if (cart.OrderType == (int)Model.Cart.OrderTypes.Prepared)
            {
                var moneyDetail = new MoneyDetail()
                {
                    UserID = CurrentUser.ID,
                    CartID = cart.ID,
                    GlobalUniqueID = Identity,
                    SumGold = -cart.TotalPrice,
                    Description = string.Format("Оплата заказа #" + cart.ID),
                };

                var guid = Guid.NewGuid();
                Repository.CreateMoneyDetail(moneyDetail, guid);


                if (Repository.SubmitMoney(guid))
                {
                    var recharge = new Recharge()
                    {
                        UserID = CurrentUser.ID,
                        MoneyDetailID = moneyDetail.ID,
                        Sum = moneyDetail.SumGold,
                        Description = string.Format("Оплата заказа #" + cart.ID),
                        IsSubmitted = true
                    };
                    Repository.CreateRecharge(recharge);

                    ViewBag.NeedToProcess = cart.NeedToProcess;
                    ProcessCart(cart);
                    
                    return View("Success");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult PopupCodeImage(int id)
        {
            var productCode = Repository.ProductCodes.FirstOrDefault(p => p.ID == id);
            if (productCode != null && !string.IsNullOrWhiteSpace(productCode.Image))
            {
                return View(productCode);
            }

            return null;
        }
    }
}
