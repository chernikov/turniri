using ImageResizer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;
using turniri.Tools.FineUploader;
using turniri.Tools.Video;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,seller")]
    public class ProductController : AdminController
    {
        public ActionResult Index(int page = 1, string search = null)
        {
            IQueryable<Product> list = Repository.Products;

            if (search != null)
            {
                list = SearchEngine.Search(search, list).AsQueryable();
            }

            var data = new PageableData<Product>();
            data.Init(list, page, "Index");
            ViewData["search"] = search;
            return View(data);
        }

        public ActionResult Create()
        {
            var productView = new ProductView();
            productView.ProductPrices = new Dictionary<string, ProductPriceView>();
            productView.ProductPrices.Add(Guid.NewGuid().ToString("N"), new ProductPriceView());
            return View("Edit", productView);
        }

        public ActionResult Edit(int id)
        {
            var product = Repository.Products.FirstOrDefault(p => p.ID == id);

            if (product != null)
            {
                var productView = (ProductView)ModelMapper.Map(product, typeof(Product), typeof(ProductView));
                return View(productView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(ProductView productView)
        {
            if (!productView.ProductPrices.Any())
            {
                ModelState.AddModelError("ProductPrices", "Добавьте хотя бы одну цену");
            }
            if (productView.ProductVideos != null)
            {
                foreach (var productVideo in productView.ProductVideos)
                {
                    productVideo.Value.VideoCode = VideoHelper.GetVideoByUrl(productVideo.Value.VideoUrl);
                    var thumb = VideoHelper.GetVideoThumbByUrl(productVideo.Value.VideoUrl);
                    if (thumb.StartsWith("http"))
                    {
                        productVideo.Value.VideoThumb = SaveImageFromUrl(thumb);
                    }
                    if (string.IsNullOrWhiteSpace(productVideo.Value.VideoThumb))
                    {
                        ModelState.AddModelError("ProductVideos[" + productVideo.Key + "].Value.VideoUrl", "Error");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var product = (Product)ModelMapper.Map(productView, typeof(ProductView), typeof(Product));

                if (product.ID == 0)
                {
                    product.Url = Translit.Translate(product.Name);
                    var any = Repository.Products.Any(p => p.Url == product.Url && p.ID != product.ID);
                    int i = 1;
                    while (any)
                    {
                        product.Url = Translit.Translate(product.Name) + "-" + i.ToString(CultureInfo.InvariantCulture);
                        any = Repository.Products.Any(p => p.Url == product.Url);
                        i++;
                    }
                    Repository.CreateProduct(product);
                }
                else
                {
                    Repository.UpdateProduct(product);
                }

                Repository.UpdateProductCatalogs(product.ID, productView.ProductCatalogs);

                Repository.ClearProductPrices(product.ID, productView.ProductPrices.Select(p => p.Value.ID).ToList());
                if (productView.ProductPrices != null)
                {
                    foreach (var productPriceView in productView.ProductPrices)
                    {
                        var productPrice = (ProductPrice)ModelMapper.Map(productPriceView.Value, typeof(ProductPriceView), typeof(ProductPrice));
                        productPrice.ProductID = product.ID;
                        if (productPrice.ID == 0)
                        {
                            Repository.CreateProductPrice(productPrice);
                        }
                        else
                        {
                            Repository.UpdateProductPrice(productPrice);
                        }
                    }
                }

                Repository.ClearProductImages(product.ID, productView.ProductImages.Select(p => p.Value.ID).ToList());
                if (productView.ProductImages != null)
                {
                    foreach (var productImageView in productView.ProductImages)
                    {
                        var productImage = (ProductImage)ModelMapper.Map(productImageView.Value, typeof(ProductImageView), typeof(ProductImage));
                        productImage.ProductID = product.ID;
                        if (productImage.ID == 0)
                        {
                            Repository.CreateProductImage(productImage);
                        }
                        else
                        {
                            Repository.UpdateProductImage(productImage);
                        }

                    }
                }

                Repository.ClearProductVariations(product.ID, productView.ProductVariations.Select(p => p.Value.ID).ToList());
                if (productView.ProductVariations != null)
                {
                    foreach (var productVariationView in productView.ProductVariations)
                    {
                        var productVariation = (ProductVariation)ModelMapper.Map(productVariationView.Value, typeof(ProductVariationView), typeof(ProductVariation));
                        productVariation.ProductID = product.ID;
                        if (productVariation.ID == 0)
                        {
                            Repository.CreateProductVariation(productVariation);
                        }
                        else
                        {
                            Repository.UpdateProductVariation(productVariation);
                        }
                    }
                }

                Repository.ClearProductVideos(product.ID, productView.ProductVideos.Select(p => p.Value.ID).ToList());
                if (productView.ProductVideos != null)
                {
                    foreach (var productVideoView in productView.ProductVideos)
                    {
                        var productVideo = (ProductVideo)ModelMapper.Map(productVideoView.Value, typeof(ProductVideoView), typeof(ProductVideo));


                        productVideo.ProductID = product.ID;
                        if (productVideo.ID == 0)
                        {
                            Repository.CreateProductVideo(productVideo);
                        }
                        else
                        {
                            Repository.UpdateProductVideo(productVideo);
                        }
                    }
                }
                Repository.UpdateSimilarProducts(product.ID, productView.ProductsList);
                return RedirectToAction("Index");
            }
            return View(productView);
        }

        public ActionResult Delete(int id)
        {
            var product = Repository.Products.FirstOrDefault(p => p.ID == id);

            if (product != null)
            {
                Repository.RemoveProduct(product.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Restore(int id)
        {
            var product = Repository.Products.FirstOrDefault(p => p.ID == id);

            if (product != null)
            {
                Repository.RestoreProduct(product.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Codes(int id)
        {
            var product = Repository.Products.FirstOrDefault(p => p.ID == id);
            if (product != null)
            {
                return View(product);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult CodeTable(int id)
        {
            var product = Repository.Products.FirstOrDefault(p => p.ID == id);
            if (product != null)
            {
                return View(product);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult AddCode(ProductCodeView productCodeView)
        {
            var code = Repository.ProductCodes.FirstOrDefault(p => string.Compare(p.Code, productCodeView.Code, true) == 0 && p.ID != productCodeView.ID);

            if (code != null)
            {
                return Json(new { result = "error", error = "Такой код уже есть" });
            }

            if (string.IsNullOrWhiteSpace(productCodeView.Code))
            {
                return Json(new { result = "error", error = "Введите код" });
            }

            var productCode = (ProductCode)ModelMapper.Map(productCodeView, typeof(ProductCodeView), typeof(ProductCode));
            if (productCode.ID == 0)
            {
                Repository.CreateProductCode(productCode);
            }
            else
            {
                Repository.UpdateProductCode(productCode);
            }
            return Json(new { result = "ok" });
        }

        public ActionResult RestCode(int ProductID, int Rest)
        {
            var product = Repository.Products.FirstOrDefault(p => p.ID == ProductID);

            if (product != null)
            {
                var diff = Rest - product.UnselledCodes;

                if (diff < 0)
                {
                    var list = product.ProductCodes.Where(p => !p.IsSelled).Take(-diff).ToList();

                    foreach (var item in list)
                    {
                        Repository.RemoveProductCode(item.ID);
                    }
                }
                if (diff > 0)
                {
                    var price = product.ProductPrices.FirstOrDefault();
                    for (int i = 0; i < diff; i++)
                    {
                        var code = StringExtension.CreateRandomPassword(16, "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
                        code = code.Insert(12, "-").Insert(8, "-").Insert(4, "-");
                        var productCode = new ProductCode()
                        {
                            Code = code,
                            ProductID = product.ID,
                            ProductPriceID = price.ID
                        };
                        Repository.CreateProductCode(productCode);
                    }
                }

            }


            return Json(new { result = "ok" });
        }

        public ActionResult Rest(ProductRestView productRestView)
        {
            if (ModelState.IsValid)
            {
                var product = Repository.Products.FirstOrDefault(p => p.ID == productRestView.ProductID);

                if (product != null)
                {
                    var diff = productRestView.Rest - product.RestOfCodes(productRestView.ProductVariationID);

                    if (diff < 0)
                    {
                        var list = product.ProductCodes.Where(p => !p.IsSelled && p.ProductVariationID == productRestView.ProductVariationID).Take(-diff).ToList();
                        foreach (var item in list)
                        {
                            Repository.RemoveProductCode(item.ID);
                        }
                    }
                    if (diff > 0)
                    {
                        var price = product.ProductPrices.FirstOrDefault();
                        if (price != null)
                        {
                            for (int i = 0; i < diff; i++)
                            {
                                var code = StringExtension.CreateRandomPassword(16, "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
                                code = code.Insert(12, "-").Insert(8, "-").Insert(4, "-");

                                var productCode = new ProductCode()
                                {
                                    Code = code,
                                    ProductID = product.ID,
                                    ProductPriceID = price.ID,
                                    ProductVariationID = productRestView.ProductVariationID
                                };
                                Repository.CreateProductCode(productCode);
                            }
                        }
                    }
                }
            }



            return RedirectToAction("Codes", new { id = productRestView.ProductID });
        }

        public ActionResult DeleteProductCode(int id)
        {
            var productCode = Repository.ProductCodes.FirstOrDefault(p => p.ID == id);
            if (productCode != null)
            {
                Repository.RemoveProductCode(productCode.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditProductCode(int id)
        {
            var productCode = Repository.ProductCodes.FirstOrDefault(p => p.ID == id);
            if (productCode != null)
            {
                var productCodeView = ModelMapper.Map(productCode, typeof(ProductCode), typeof(ProductCodeView));
                return View(productCodeView);
            }
            return null;
        }

        public ActionResult UnReserveProductCode(int id)
        {
            var productCode = Repository.ProductCodes.FirstOrDefault(p => p.ID == id);
            if (productCode != null)
            {
                var cartProduct = productCode.CartProduct;
                Repository.UnReserveProductCode(productCode);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddProductPrice()
        {
            return View("ProductPriceItem", new KeyValuePair<string, ProductPriceView>(Guid.NewGuid().ToString("N"), new ProductPriceView()));
        }

        public ActionResult AddProductImage(string image)
        {
            return View("ProductImageItem",
                new KeyValuePair<string, ProductImageView>(Guid.NewGuid().ToString("N"),
                    new ProductImageView()
                {
                    Image = image
                }));
        }

        public ActionResult AddProductVariation(string image)
        {
            return View("ProductVariationItem",
                new KeyValuePair<string, ProductVariationView>(Guid.NewGuid().ToString("N"),
                    new ProductVariationView()
                    {
                        Image = image
                    }));
        }

        public ActionResult AddProductVideo()
        {
            return View("ProductVideoItem",
                new KeyValuePair<string, ProductVideoView>(Guid.NewGuid().ToString("N"),
                    new ProductVideoView()));

        }

        public ActionResult ProcessVideo(string url, string key)
        {
            var video = new ProductVideoView()
            {
                VideoUrl = url
            };
            video.VideoCode = VideoHelper.GetVideoByUrl(url);
            var thumb = VideoHelper.GetVideoThumbByUrl(video.VideoUrl);
            if (thumb.StartsWith("http"))
            {
                video.VideoThumb = SaveImageFromUrl(thumb);
            }
            if (string.IsNullOrWhiteSpace(video.VideoThumb))
            {
                ModelState.AddModelError("ProductVideos[" + key + "].Value.VideoUrl", "Error");
            }
            return View("ProductVideoItem", new KeyValuePair<string, ProductVideoView>(key, video));
        }

        public ActionResult SelectProduct(string term)
        {
            var list = Repository.Products.Where(p => !p.IsDeleted);
            var searchList = SearchEngine.Get(term, list);
            return Json(new
            {
                result = "ok",
                data = searchList.Select(p => new
                {
                    id = p.ID,
                    name = p.Name
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetPrices()
        {
            foreach (var product in Repository.Products.ToList())
            {
                if (!product.ProductPrices.Any())
                {
                    var productPrice = new ProductPrice()
                    {
                        ProductID = product.ID,
                        Price = 0,
                        Preorder = true,
                        PlatformID = null
                    };

                    Repository.CreateProductPrice(productPrice);
                }
            }
            return Content("OK");
        }
    }
}
