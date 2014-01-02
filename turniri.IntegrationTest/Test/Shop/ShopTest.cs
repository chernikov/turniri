using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using turniri.Global.Auth;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;
using turniri.UnitTest.Mock.Http;
using turniri.UnitTest.Tools;
using GenerateData;
using turniri.IntegrationTest.Tools;
using System.IO;

namespace turniri.IntegrationTest
{
    [TestFixture]
    public class ShopTest
    {
        /*
        private static Random rand = new Random((int)DateTime.Now.Ticks);

        protected static Imaginarium GetImaginarium()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var imaginarium = kernel.Get<Imaginarium>();
            return imaginarium;
        }

        [Test]
        public void CreateProducts()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    var kernel = DependencyResolver.Current.GetService<IKernel>();
                    var repository = kernel.Get<IRepository>("RequestScoped");
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
            var catalog = repository.Catalogs.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            var game = repository.Games.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

            var imaginarium = GetImaginarium();

            var file = Imaginarium.GetRandomSourceImage();

            using (var fs = new FileStream(file, FileMode.Open))
            {
                var product = new Product()
                {
                    Name = GenerateData.Name.GetRandom() + " " + GenerateData.Team.GetRandom(),
                    Price = GenerateData.Price.GetRandom(),
                    OldPrice = rand.Next(2) % 2 == 0 ? (double?)GenerateData.Price.GetRandom() : null,
                    GameID = rand.Next(2) % 2 == 0 ? (int?)game.ID : null,
                    CatalogID = catalog.ID,
                    SmsName = "SMS_NAME",
                    IsAction = rand.Next(2) % 2 == 0,
                    IsNew = rand.Next(2) % 2 == 0,
                    IsPromo = rand.Next(10) % 10 == 0,
                    PreOrder = rand.Next(2) % 2 == 0,
                    OnMain = rand.Next(2) % 2 == 0,
                    ImagePath189 = imaginarium.MakePreview(fs, "/Media/files/product/", "ProductImage189Size"),
                    ImagePath103 = imaginarium.MakePreview(fs, "/Media/files/product/", "ProductImage103Size"),
                    ImagePath144v = imaginarium.MakePreview(fs, "/Media/files/product/", "ProductImage144vSize"),
                    ImagePath47 = imaginarium.MakePreview(fs, "/Media/files/product/", "ProductImage47Size"),
                    ImagePath22 = imaginarium.MakePreview(fs, "/Media/files/product/", "ProductImage22Size"),
                    Description = Textarium.GetRandomText(2)
                };
                product.Url = Translit.Translate(product.Name);

                repository.CreateProduct(product);

                Console.WriteLine("Создан продукт " + product.Name);
            }
        }*/
    }
}
