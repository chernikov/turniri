using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Moq;
using NUnit.Framework;
using turniri.Global.Auth;
using turniri.Model;
using turniri.UnitTest.Fake;
using turniri.UnitTest.Mock.Http;

namespace turniri.UnitTest.Test
{
    [TestFixture]
    public class AdminHomeControllerTest
    {
        [Test]
        public void Index_NotAuthorizeGetDefaultView_RedirectToLoginPage()
        {
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            var httpContext = new MockHttpContext(auth).Object;
            var controller = DependencyResolver.Current.GetService<Areas.Admin.Controllers.HomeController>();
            var route = new RouteData();
            route.Values.Add("controller", "Home");
            route.Values.Add("action", "Index");
            route.Values.Add("area", "Admin");

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            var controllerActionInvoker = new FakeControllerActionInvoker<HttpUnauthorizedResult>();
            var result = controllerActionInvoker.InvokeAction(controller.ControllerContext, "Index");
        }

        [Test]
        public void Index_AuthorizeGetDefaultView_GetViewResult()
        {
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            var controller = DependencyResolver.Current.GetService<Areas.Admin.Controllers.HomeController>();
            auth.Login("admin");

            var httpContext = new MockHttpContext(auth).Object;

            var route = new RouteData();
            route.Values.Add("controller", "Home");
            route.Values.Add("action", "Index");
            route.Values.Add("area", "Admin");
            

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            var controllerActionInvoker = new FakeControllerActionInvoker<ViewResult>();
            var result = controllerActionInvoker.InvokeAction(controller.ControllerContext, "Index");
        }
        
    }
}
