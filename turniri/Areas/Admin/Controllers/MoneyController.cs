using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;
using turniri.Tools.Qiwi;

namespace turniri.Areas.Admin.Controllers
{
    public class MoneyController : AdminController
    {
        public ActionResult Index(int page = 1, string search = null, bool gold = true, DateTime? beginDate = null, DateTime? endDate = null)
        {
            IQueryable<MoneyDetail> list = Repository.MoneyDetails.OrderByDescending(p => p.ID);
            if (gold)
            {
                list = list.Where(p => p.SumGold != 0).AsQueryable();
            }
            if (beginDate.HasValue)
            {
                list = list.Where(p => p.AddedDate >= beginDate.Value).AsQueryable();
            }
            if (endDate.HasValue)
            {
                list = list.Where(p => p.AddedDate <= endDate.Value).AsQueryable();
            }
            if (search != null)
            {
                list = SearchEngine.Search(search, list).AsQueryable();
            }

            var data = new PageableData<MoneyDetail>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Balance()
        {
            var balance = Repository.BalanceSiteMoney;
            return View(balance);
        }

        public ActionResult RecalculateAll()
        {
            Repository.RecalculateAll();
            return RedirectToAction("Index");
        }

        public ActionResult Submit(Guid guid)
        {
            Repository.SubmitMoney(guid);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveUnsubmitted()
        {
            Repository.RemoveUnsubmitted();
            return RedirectToAction("Index");
        }

        public ActionResult Remove(Guid guid)
        {
            Repository.RemoveMoneyTransaction(guid);
            return RedirectToAction("Index");
        }

        public ActionResult CheckQiwi()
        {
            QiwiProcessorJob.Start(Repository, Config);
            return RedirectToAction("Index");
        }
    }
}
