using System;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;
using Ninject;
using turniri.Model;
using System.Web.Mvc;

namespace turniri.Global.Auth
{
    public class CustomAuthentication : IAuthentication
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private const string cookieName = "__AUTH_COOKIE";

        public IAuthCookieProvider AuthCookieProvider { get; set; }

        public IRepository Repository { get; set; }

        #region IAuthentication Members

        public User Login(string userName, string Password, bool isPersistent)
        {
            User retUser = Repository.Login(userName, Password);
            if (retUser != null)
            {
                CreateAuthCookie(userName, isPersistent);
            }
            return retUser;
        }

        private void CreateAuthCookie(string userName, bool isPersistent)
        {
            var ticket = new FormsAuthenticationTicket(
               1,
               userName,
               DateTime.Now,
               DateTime.Now.Add(FormsAuthentication.Timeout),
               isPersistent,
               string.Empty,
               FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var AuthCookie = new HttpCookie(cookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            AuthCookieProvider.SetCookie(AuthCookie);
        }

        public User Login(string userName)
        {
            User retUser = Repository.Users.FirstOrDefault(p => string.Compare(p.Login, userName, true) == 0);
            if (retUser != null)
            {
                CreateAuthCookie(userName, false);
            }
            return retUser;
        }

        public void LogOut()
        {
            var httpCookie = new HttpCookie(cookieName)
            {
                Value = string.Empty,
                Expires = DateTime.Now
            };
            AuthCookieProvider.SetCookie(httpCookie);
        }

        private IPrincipal _currentUser;

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = AuthCookieProvider.GetCookie(cookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = new UserProvider(ticket.Name, Repository);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Failed authentication: "  + ex.Message);
                        _currentUser = new UserProvider(null, null);
                    }
                }
                return _currentUser;
            }
        }
        #endregion

        public CustomAuthentication()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            Repository = kernel.Get<IRepository>("RequestScoped");
        }
    }
}