using GenerateData;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using turniri.IntegrationTest.Tools;
using turniri.Model;
using turniri.Tools;
using turniri.Tools.Video;

namespace turniri.IntegrationTest.Test.Shop
{
    [TestFixture]
    public class AdvancedShopTest
    {
        private static Random rand = new Random((int)DateTime.Now.Ticks);

        private IList<string> Publishers;

        private IList<string> RussianPublishers;

        private IList<string> Languages;

        private IList<Vendor> Vendors;

        private IList<Category> Categories;

        private IList<Catalog> Catalogs;

        private IList<Platform> Platforms;


        [SetUp]
        public void Setup()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            GeneratePublishers();
            GenerateLanguages();

            Categories = repository.Categories.ToList();
            Catalogs = repository.Catalogs.ToList();
            Platforms = repository.Platforms.ToList();

            CreateVendors();
        }

        protected static Imaginarium GetImaginarium()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var imaginarium = kernel.Get<Imaginarium>();
            return imaginarium;
        }

        private void GeneratePublishers()
        {
            Publishers = new List<string>();
            RussianPublishers = new List<string>();
            Publishers.Add(string.Empty);
            RussianPublishers.Add(string.Empty);
            for (int i = 0; i < 10; i++)
            {
                Publishers.Add(GenerateData.Team.GetRandom());
            }

            for (int i = 0; i < 10; i++)
            {
                RussianPublishers.Add(GenerateData.Team.GetRandom());
            }
        }

        private void GenerateLanguages()
        {
            Languages = new List<string>();
            Languages.Add(string.Empty);
            for (int i = 0; i < 7; i++)
            {
                Languages.Add(GenerateData.Team.GetRandomAdjective());
            }
        }

        public void CreateVendors()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            Vendors = new List<Vendor>();
            Vendors.Add(null);
            for (int i = 0; i < 3; i++)
            {
                var vendor = new Vendor()
                {
                    Name = GenerateData.Team.GetRandom(),
                    Text = Textarium.GetRandomHtmlText(2),
                };
                repository.CreateVendor(vendor);
                Vendors.Add(vendor);
            }
        }

        [Test]
        public void CreateProducts()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    CreateProduct(repository);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
        }

        public void CreateProduct(IRepository repository)
        {
            var game = repository.Games.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

            var imaginarium = GetImaginarium();

            DateTime? randReleaseDate = rand.Next(10) % 10 == 0 ? null : (DateTime?)DateTime.Now.AddYears(-10).AddDays(rand.Next(3653));

            var product = new Product()
            {
                Name = GenerateData.Name.GetRandom() + " " + GenerateData.Team.GetRandom(),
                GameID = rand.Next(2) % 2 == 0 ? (int?)game.ID : null,
                Type = rand.Next(2) % 2 == 0 ? 1 : 3,
                SmsName = "SMS_NAME",
                IsAction = rand.Next(2) % 2 == 0,
                IsNew = rand.Next(2) % 2 == 0,
                IsPromo = rand.Next(10) % 10 == 0,
                Description = Textarium.GetRandomHtmlText(2),
                SubTitle = Textarium.GetRandomText(1).Teaser(300),
                Image = Imaginarium.SaveRandomImage("/Media/files/test/"),
                Background = Imaginarium.SaveRandomImage("/Media/files/test/"),
                Publisher = Publishers.OrderBy(p => Guid.NewGuid()).First(),
                RussianPublisher = RussianPublishers.OrderBy(p => Guid.NewGuid()).First(),
                ReleaseDate = randReleaseDate,
                AgeRestrict = rand.Next(5) + 1,
            };

            var vendor = Vendors.OrderBy(p => Guid.NewGuid()).First();
            if (vendor != null)
            {
                product.VendorID = vendor.ID;
            }

            product.Url = Translit.Translate(product.Name);

            var langNumbers = rand.Next(3) + rand.Next(3) + 1;

            product.Language = string.Join(", ", Languages.OrderBy(p => Guid.NewGuid()).Take(langNumbers));


            if (product.Type == (int)Product.TypeEnum.Code)
            {
                //category
                product.CategoryID = Categories.OrderBy(p => Guid.NewGuid()).First().ID;
            }

            repository.CreateProduct(product);
            if (product.Type == (int)Product.TypeEnum.Code)
            {
                //catalog 
                var catalogNumbers = rand.Next(3) + rand.Next(3) + 1;

                var selectedCatalogs = Catalogs.OrderBy(p => Guid.NewGuid()).Take(catalogNumbers).Select(p => p.ID).ToList();

                repository.UpdateProductCatalogs(product.ID, selectedCatalogs);
                //price

                var platformNumbers = rand.Next(3);

                var selectedPlatforms = Platforms.OrderBy(p => Guid.NewGuid()).Take(platformNumbers);

                if (selectedPlatforms.Any() && product.CategoryID != 4)
                {
                    foreach (var platform in selectedPlatforms)
                    {
                        var productPrice = new ProductPrice()
                        {
                            ProductID = product.ID,
                            Price = GenerateData.Price.GetRandom(),
                            PlatformID = platform.ID,
                            Preorder = rand.Next(10) % 10 == 0
                        };
                        repository.CreateProductPrice(productPrice);
                    }
                }
                else
                {
                    var productPrice = new ProductPrice()
                        {
                            ProductID = product.ID,
                            Price = GenerateData.Price.GetRandom(),
                            PlatformID = null,
                            Preorder = rand.Next(10) % 10 == 0
                        };
                    repository.CreateProductPrice(productPrice);
                }

                var videoNum = rand.Next(3) + rand.Next(3);
                for (int i = 0; i < videoNum; i++)
                {
                    var videoUrl = YoutubeVideo.GetRandom();
                    var videoCode = VideoHelper.GetVideoByUrl(videoUrl);
                    var thumb = VideoHelper.GetVideoThumbByUrl(videoUrl);

                    if (thumb.StartsWith("http"))
                    {
                        thumb = Filerarium.SaveImageFromUrl("/Media/files/test/", thumb);
                    }
                    var productVideo = new ProductVideo()
                    {
                        ProductID = product.ID,
                        VideoUrl = videoUrl,
                        VideoCode = videoCode,
                        VideoThumb = thumb
                    };
                    repository.CreateProductVideo(productVideo);
                }
                //image 
                var imageNum = rand.Next(3) + rand.Next(3);
                for (int i = 0; i < imageNum; i++)
                {
                    var productImage = new ProductImage()
                    {
                        ProductID = product.ID,
                        Image = Imaginarium.SaveRandomImage("/Media/files/test/")
                    };
                    repository.CreateProductImage(productImage);
                }
            }

            if (product.Type == (int)Product.TypeEnum.RealGood)
            {
                var productPrice = new ProductPrice()
                {
                    ProductID = product.ID,
                    Price = GenerateData.Price.GetRandom(),
                    PlatformID = null,
                    Preorder = rand.Next(10) % 10 == 0
                };
                repository.CreateProductPrice(productPrice);
                //variation 
                var imageNum = rand.Next(3) + rand.Next(3);
                for (int i = 0; i < imageNum; i++)
                {
                    var productVariation = new ProductVariation()
                    {
                        ProductID = product.ID,
                        Image = Imaginarium.SaveRandomImage("/Media/files/test/")
                    };
                    repository.CreateProductVariation(productVariation);
                }
            }
            Console.WriteLine("Создан продукт " + product.Name);

            var lowProductPrice = repository.ProductPrices.Where(p => p.ProductID == product.ID).OrderBy(p => p.Price).FirstOrDefault();

            if (lowProductPrice != null && !lowProductPrice.Preorder)
            {
                var countCodes = rand.Next(15) + rand.Next(15) + 2;
                for (int i = 0; i < countCodes; i++)
                {
                    var productCode = new ProductCode()
                    {
                        Code = GenerateCode(),
                        Image = Imaginarium.SaveRandomImage("/Media/files/test/"),
                        ProductID = product.ID
                    };
                    if (repository.ProductVariations.Any(p => p.ProductID == product.ID))
                    {
                        var variation = repository.ProductVariations.Where(p => p.ProductID == product.ID).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                        if (rand.Next(3) % 3 != 0)
                        {
                            productCode.ProductVariationID = variation.ID;
                        }
                    };

                    var productSelectPrice = repository.ProductPrices.Where(p => p.ProductID == product.ID).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                    if (productSelectPrice != null)
                    {
                        productCode.ProductPriceID = productSelectPrice.ID;
                        repository.CreateProductCode(productCode);
                        Console.WriteLine("Создан код " + productCode.Code + " для продукта " + product.Name);
                    }
                }
            }
        }

        private static string GenerateCodePart()
        {
            return StringExtension.CreateRandomPassword(4, "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
        }

        private static string GenerateCode()
        {
            return string.Format("{0}-{1}-{2}-{3}", GenerateCodePart(), GenerateCodePart(), GenerateCodePart(), GenerateCodePart());
        }
    }
}
