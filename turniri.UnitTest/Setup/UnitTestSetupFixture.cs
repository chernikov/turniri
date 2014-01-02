using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ninject;
using NUnit.Framework;
using turniri.Global.Auth;
using turniri.Global.Config;
using turniri.Mappers;
using turniri.Model;
using turniri.UnitTest.Fake;
using turniri.UnitTest.Mock;
using turniri.UnitTest.Mock.Http;
using turniri.UnitTest.Tools;

namespace turniri.UnitTest
{
    [SetUpFixture]
    public class UnitTestSetupFixture
    {
        protected static string Sandbox = "../../Sandbox";

        [SetUp]
        public virtual void Setup()
        {
            Console.WriteLine("===============");
            Console.WriteLine("Here we are go!");
            Console.WriteLine("===============");
            InitKernel();
            Console.WriteLine("===============");
            Console.WriteLine("Context Inited=");
            Console.WriteLine("===============");
        }

        [TearDown]
        public virtual void TearDown()
        {
            Console.WriteLine("===============");
            Console.WriteLine("=====BYE!======");
            Console.WriteLine("===============");
        }

        protected virtual IKernel InitKernel()
        {
            var kernel = new StandardKernel();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

            //IConfig
            InitConfig(kernel);
            kernel.Bind<IMapper>().To<CommonMapper>();
            //IRepository
            InitRepository(kernel);
            //Authentication
            InitAuth(kernel);
            return kernel;
        }

        protected virtual void InitAuth(StandardKernel kernel)
        {
            kernel.Bind<HttpCookieCollection>().To<HttpCookieCollection>();
            kernel.Bind<IAuthCookieProvider>().To<FakeAuthCookieProvider>().InThreadScope();
            kernel.Bind<IAuthentication>().ToMethod<CustomAuthentication>(c =>
            {
                var auth = new CustomAuthentication();
                auth.AuthCookieProvider = kernel.Get<IAuthCookieProvider>();
                return auth;
            });
        }

        protected virtual void InitRepository(StandardKernel kernel)
        {
            kernel.Bind<MockRepository>().To<MockRepository>().InThreadScope();
            kernel.Bind<IRepository>().ToMethod(p => kernel.Get<MockRepository>().Object).Named("TransientScoped");
            kernel.Bind<IRepository>().ToMethod(p => kernel.Get<MockRepository>().Object).Named("RequestScoped");
        }

        protected virtual void InitConfig(StandardKernel kernel)
        {
            Console.WriteLine("===============");
            Console.WriteLine("==Init Config==");
            Console.WriteLine("===============");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename =  new FileInfo(Sandbox + "/Web.config").FullName;
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            kernel.Bind<IConfig>().ToMethod(c => new Config(configuration));
        }
    }
}
