using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;


namespace turniri.Areas.Admin.Controllers
{
    public class MoneyNotifyController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.MoneyNotifies.OrderByDescending(p => p.ID);
            var data = new PageableData<MoneyNotify>();
            data.Init(list, page, "Index", 200);
            return View(data);
        }

        public ActionResult Item(int id)
        {
            var item = Repository.MoneyNotifies.FirstOrDefault(p => p.ID == id);
            if (item != null)
            {
                return View(item);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Delete(int id)
        {
            var moneynotify = Repository.MoneyNotifies.FirstOrDefault(p => p.ID == id);
            if (moneynotify != null)
            {
                Repository.RemoveMoneyNotify(moneynotify.ID);
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult Clear()
        {
            Repository.ClearMoneyNotices();
            return RedirectToAction("Index");
        }

    }
}