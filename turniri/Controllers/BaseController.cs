using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Ninject.Web.Mvc;
using turniri.Global.Auth;
using turniri.Global.Config;
using turniri.Model;
using System.Web.Routing;
using turniri.Mappers;
using System.IO;
using System;
using System.Web;
using System.Text;
using turniri.Tools.Mail;

namespace turniri.Controllers
{
    public abstract class BaseController : Controller, IModelMapperController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string HostName = string.Empty;

        protected static string NotFoundPage = "~/not-found-page";

        protected static string LoginPage = "~/Login";

        private const string CookieName = "IndentityCookie";

        protected static readonly string SessionCart = "CART_SESSION";

        public IRepository Repository { get; set; }

        [Inject]
        public IAuthentication Auth { get; set; }

        [Inject]
        public IConfig Config { get; set; }

        [Inject]
        public IMapper ModelMapper { get; set; }

        public MainCamera MainCamera
        {
            get
            {
                return Repository.MainCameras.FirstOrDefault();
            }
        }

        public Guid Identity { get; set; }

        protected BaseController()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            Repository = kernel.Get<IRepository>("RequestScoped");
        }

        public User CurrentUser
        {
            get
            {
                if (Auth.CurrentUser.Identity is IUserable)
                {
                    return ((IUserable)Auth.CurrentUser.Identity).User;
                }
                return null;
            }
        }

        public RedirectResult RedirectToNotFoundPage
        {
            get
            {
                return Redirect(NotFoundPage);
            }
        }


        public RedirectResult RedirectToLoginPage
        {
            get
            {
                return Redirect(LoginPage);
            }
        }

        public ActionResult RedirectBack(ActionResult redirectResult)
        {
                return Request.UrlReferrer != null ? 
                    Redirect(Request.UrlReferrer.ToString()) : 
                    redirectResult;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.Url != null)
            {
                HostName = requestContext.HttpContext.Request.Url.Authority;
            }
            if (!requestContext.HttpContext.Request.Browser.Crawler)
            {
                var identityCookie = requestContext.HttpContext.Request.Cookies.Get(CookieName);
                if (identityCookie != null && !string.IsNullOrEmpty(identityCookie.Value))
                {
                    Identity = Guid.Parse(identityCookie.Value as string);
                }
                else
                {
                    Identity = Guid.NewGuid();
                    var identityResponseCookie = new HttpCookie(CookieName)
                    {
                        Value = Identity.ToString("N"),
                        Expires = DateTime.Now.AddYears(1)
                    };

                    requestContext.HttpContext.Response.Cookies.Set(identityResponseCookie);
                }
                var globalUnique = Repository.GlobalUniques.FirstOrDefault(p => p.ID == Identity);

                if (globalUnique == null)
                {
                    globalUnique = new GlobalUnique()
                    {
                        ID = Identity,
                        IP = GetIP(),
                        UserAgent = System.Web.HttpContext.Current.Request.UserAgent
                    };
                }
                Repository.SaveGlobalUnique(globalUnique);
            }
            else
            {
                logger.Debug("Crawler: " + requestContext.HttpContext.Request.UserAgent + "\r\nAsked: " + requestContext.HttpContext.Request.Url.ToString());
            }
            base.Initialize(requestContext);
        }

        protected Stream GetInputStream(string qqfile, out string fileName)
        {
            fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var httpPostedFileBase = Request.Files["qqfile"];
                if (httpPostedFileBase != null)
                {
                    fileName = qqfile;
                    return httpPostedFileBase.InputStream;
                }
            }
            else
            {
                if (qqfile != null)
                {
                    fileName = qqfile;
                    return Request.InputStream;
                }
            }
            return null;
        }

        public IEnumerable<Platform> Platforms
        {
            get { return Repository.Platforms; }
        }

        public virtual Cart GetCart()
        {
            if (Request.Browser.Crawler)
            {
                return new Cart();
            }
            int? id = GetCartID();
            if (id.HasValue)
            {
                int ID = GetCartID().Value;
                var cart = Repository.Carts.FirstOrDefault(p => p.ID == ID);

                if (cart != null)
                {
                    if (cart.OrderType == (int)Cart.OrderTypes.Delivered)
                    {
                        cart = CreateCart();
                    }
                    if (cart.Customer == null && CurrentUser != null)
                    {
                        cart.Customer = CurrentUser;
                        Repository.UpdateCart(cart);
                    }
                    Repository.UpdateLastVisitDateCart(cart);
                    return cart;
                }
            }
            var newCart = CreateCart();
            return newCart;
        }

        protected Cart CreateCart()
        {
            var cart = new Cart()
            {
                OrderType = (int)Cart.OrderTypes.Created,
                GlobalUniqueID = Identity
            };
            Repository.CreateCart(cart);
            SaveCartID(cart.ID);
            return cart;
        }

        protected void SaveCartID(int id)
        {
            var AuthCookie = new HttpCookie(SessionCart);
            AuthCookie.Value = id.ToString();
            AuthCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Set(AuthCookie);
            Session[SessionCart] = id;
        }

        protected int? GetCartID()
        {
            if (Session[SessionCart] != null)
            {
                return (int)Session[SessionCart];
            }
            try
            {
                if (Request == null)
                {
                    logger.Error("Request == null");
                }
                if (Request.Cookies == null)
                {
                    logger.Error("Request.Cookies == null");
                }
                if (Request.Cookies[SessionCart] == null)
                {
                    logger.Error("Request.Cookies[SessionCart] == null");
                }
                if (!string.IsNullOrWhiteSpace(Request.Cookies[SessionCart].Value))
                {
                    int id;
                    if (Int32.TryParse(Request.Cookies[SessionCart].Value, out id))
                    {
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            return null;
        }

        private static string GetIP()
        {
            string ip =
                System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }

        protected void SendMailOfCodes(int id, string Email, IList<ProductCode> list)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table width='100%'  border='1'>");
            sb.Append("<thead><tr><th  width='50%'>Игра</th><th width='50%'>Код активации</th></tr></thead>");
            foreach (var code in list)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", code.Product.Name, code.Code);
            }
            sb.Append("</table>");
            NotifyMail.SendNotify<int>(Config, "ProductCodes", Email,
                (u, format) => string.Format(format, id),
                (u, format) => string.Format(format, id, sb.ToString(), HostName),
                                          0);

        }
    }
}
