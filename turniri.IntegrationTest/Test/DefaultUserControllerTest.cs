using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using NUnit.Framework;
using turniri.Model;
using turniri.Models.ViewModels.User;
using turniri.Tools;
using turniri.UnitTest.Mock.Http;
using turniri.UnitTest.Tools;

namespace turniri.IntegrationTest
{
    [TestFixture]
    public class DefaultUserControllerTest
    {
        [Test]
        public void CreateUser_CreateNormalUser_CountPlusOne()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var controller = DependencyResolver.Current.GetService<turniri.Areas.Default.Controllers.UserController>();

            var countBefore = repository.Users.Count();
            var httpContext = new MockHttpContext().Object;

            var route = new RouteData();

            route.Values.Add("controller", "User");
            route.Values.Add("action", "Register");
            route.Values.Add("area", "Default");

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            controller.Session.Add(CaptchaImage.CaptchaValueKey, "1111");

            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Login = "rollinx",
                Email = "rollinx@gmail.com",
                ConfirmEmail = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Captcha = "1111",
                Country = "Россия",
                City = "Москва",
                Phone = "+7(495) 123 45 67",
                BirthdateDay = 12,
                BirthdateMonth = 9,
                BirthdateYear = 1988,
                Agreement = true
            };

            Validator.ValidateObject<RegisterUserView>(registerUserView);
            controller.Register(registerUserView);

            var countAfter = repository.Users.Count();
            Assert.AreEqual(countBefore + 1, countAfter);
        }
    }
}
