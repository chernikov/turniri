using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Threading;
using System.Globalization;
using System.Configuration;

using Ninject;
using Ninject.Web.Mvc;
using StackExchange.Profiling;
using turniri.Areas.Admin;
using turniri.Areas.Default;
using turniri.Global.Config;
using turniri.Mappers;
using turniri.Model;
using turniri.Global.Auth;
using turniri.Tools.Mail;
using turniri.Tools;
using Ninject.Web.Common;
using turniri.Tools.Qiwi;


namespace turniri
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801


    public class MvcApplication : NinjectHttpApplication
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static IKernel AppKernel { get; private set; }

        private Thread mailThread { get; set; }

        protected override IKernel CreateKernel()
        {
            var res = new StandardKernel();

            res.Bind<IConfig>().To<Config>();
            res.Bind<IMapper>().To<CommonMapper>();
            res.Bind<turniriDbDataContext>().ToMethod(c => new turniriDbDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString));
            res.Bind<IRepository>().To<SqlRepository>().InTransientScope().Named("TransientScoped");
            res.Bind<IRepository>().To<SqlRepository>().InRequestScope().Named("RequestScoped");


            res.Bind<IAuthentication>().To<CustomAuthentication>().InRequestScope();
            MailSender.config = res.Get<IConfig>();
            AppKernel = res;
            return res;
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("glimpse.axd");
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("elmah.axd/{*pathInfo}");
            routes.IgnoreRoute("media/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });
            routes.IgnoreRoute("{*robots}", new { robots = @"(.*/)?robots.txt(/.*)?" });
            routes.IgnoreRoute("*.asmx");
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (Context.Session != null)
            {
                //Аутентификация
                var auth = AppKernel.Get<IAuthentication>();
                auth.AuthCookieProvider = new HttpContextCookieProvider(Context);
                HttpContext.Current.User = auth.CurrentUser;

                //Культура управляется из Web.config
                var ci = AppKernel.Get<IConfig>().Culture;
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var need301 = false;
            var builder = new UriBuilder(Request.Url);

            if (Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback)
            {
                builder.Host = Request.Url.Host.Substring(4);
                need301 = true;
            }
            if (Request.Url.AbsolutePath.EndsWith("/") && Request.Url.AbsolutePath != "/")
            {
                builder.Path = Request.Url.AbsolutePath.Remove(Request.Url.AbsolutePath.Length - 1);
                need301 = true;
            }
            if (need301)
            {
                Response.StatusCode = 301;
                Response.AddHeader("Location", builder.ToString());
                Response.End();
            }
        }

        protected override void OnApplicationStarted()
        {
            DefaultModelBinder.ResourceClassKey = "Messages";

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var adminArea = new AdminAreaRegistration();
            var adminAreaContext = new AreaRegistrationContext(adminArea.AreaName, RouteTable.Routes);
            adminArea.RegisterArea(adminAreaContext);

            var defaultArea = new DefaultAreaRegistration();
            var defaultAreaContext = new AreaRegistrationContext(defaultArea.AreaName, RouteTable.Routes);
            defaultArea.RegisterArea(defaultAreaContext);

            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();

            mailThread = new Thread(ThreadFunc);
            mailThread.Start();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal) { MiniProfiler.Start(); } //or any number of other checks, up to you 
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop(); //stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
        }

        private static void ThreadFunc()
        {
            while (true)
            {
                try
                {
                    logger.Info("Start mail thread");
                    var workThread = new Thread(PeriodCheck);
                    workThread.Start();
                    var qiwiThread = new Thread(QiwiCheck);
                    qiwiThread.Start();
                    logger.Info("Wait for end mail thread");
                    qiwiThread.Join();
                    workThread.Join();
                    logger.Info("Sleep 60 seconds");
                }
                catch (Exception ex)
                {
                    logger.ErrorException("Thread period error", ex);
                }
                Thread.Sleep(60000);
            }
        }

        private static void PeriodCheck()
        {
            var repository = AppKernel.Get<IRepository>("TransientScoped");
            var config = AppKernel.Get<IConfig>();
            while (MailProcessor.SendNextMail(repository, config)) { }
        }

        private static void QiwiCheck()
        {
            var repository = AppKernel.Get<IRepository>("TransientScoped");
            var config = AppKernel.Get<IConfig>();
            try
            {
                QiwiProcessorJob.Start(repository, config);
            } catch (Exception ex) {
                logger.Error(ex.Message);
            }
        }
    }
}