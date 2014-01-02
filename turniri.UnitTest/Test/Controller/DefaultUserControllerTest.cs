using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using turniri.Model;
using turniri.Models.ViewModels.User;
using turniri.Tools;
using turniri.UnitTest.Mock.Http;
using turniri.UnitTest.Tools;

namespace turniri.UnitTest.Test
{
    [TestFixture]
    public class DefaultUserControllerTest
    {
        [Test]
        public void Register_CaptchaIsInCorrect_Fail()
        {
            Mock.MockRepository mockRepository = DependencyResolver.Current.GetService<Mock.MockRepository>();
            var httpContext = new MockHttpContext().Object;
            var controller = DependencyResolver.Current.GetService<turniri.Areas.Default.Controllers.UserController>();

            var route = new RouteData();

            route.Values.Add("controller", "User");
            route.Values.Add("action", "Register");
            route.Values.Add("area", "Default");

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            controller.Session.Add(CaptchaImage.CaptchaValueKey, "1112");
            var registerUserView = new RegisterUserView()
            {
                Captcha = "1111"
            };
            controller.Register(registerUserView);

            mockRepository.Verify(p => p.CreateUser(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public void UserControllerRegister_AgreementNotSet_Fail()
        {
            Mock.MockRepository mockRepository = DependencyResolver.Current.GetService<Mock.MockRepository>();
            var httpContext = new MockHttpContext().Object;
            var controller = DependencyResolver.Current.GetService<turniri.Areas.Default.Controllers.UserController>();

            var route = new RouteData();

            route.Values.Add("controller", "User");
            route.Values.Add("action", "Register");
            route.Values.Add("area", "Default");

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            controller.Session.Add(CaptchaImage.CaptchaValueKey, "1111");
            var registerUserView = new RegisterUserView()
            {
                Captcha = "1111",
                Agreement = false
            };
            controller.Register(registerUserView);

            mockRepository.Verify(p => p.CreateUser(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public void Validate_AllDataIsOk_UserCreating()
        {
            Mock.MockRepository mockRepository = DependencyResolver.Current.GetService<Mock.MockRepository>();
            var httpContext = new MockHttpContext().Object;
            var controller = DependencyResolver.Current.GetService<turniri.Areas.Default.Controllers.UserController>();

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
            mockRepository.Verify(p => p.CreateUser(It.IsAny<User>()), Times.Once());
        }

    }
}
