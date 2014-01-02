using System.Linq;
using System.Web.Mvc;
using turniri.Helpers;
using turniri.Models.ViewModels;
using turniri.Controllers;
using turniri.Models.ViewModels.User;
using turniri.Tools.Mail;

namespace turniri.Areas.Default.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Login, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Password", "Пароль введен неверно");
            }
            return View(loginView);
        }

        public ActionResult LoginPartial()
        {
            return View(new LoginViewModel());
        }

        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordView());
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordView forgotPasswordView)
        {
            if (ModelState.IsValid)
            {
                var user =
                    Repository.Users.FirstOrDefault(p => string.Compare(p.Email, forgotPasswordView.Email, true) == 0);
                if (user != null)
                {
                    NotifyMail.SendNotify(Config, "ForgotPassword", user.Email,
                                                (u, format) => string.Format(format, HostName),
                                                (u, format) => string.Format(format, u.Email, u.Password, HostName),
                                                user);
                    return View("ForgotPasswordSuccess");
                }
                ModelState.AddModelError("Email", "Пользователь с таким email не найден");
            }
            return View(forgotPasswordView);
        }
    }

}
