using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.Areas.Admin.Controllers
{
    public class MoneyWithdrawController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.MoneyWithdraws.OrderByDescending(p => p.Submitted ? 0 : 1).ThenByDescending(p => p.ID);

            var data = new PageableData<MoneyWithdraw>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Submit(int id)
        {
            Repository.SubmitMoneyWithdraw(id);
            return RedirectToAction("Index");
        }

        public ActionResult Count() 
        {
            var count = Repository.MoneyWithdraws.Count(p => !p.Submitted);
            return View("Count", count);
        }
    }
}
