using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;

namespace turniri.Areas.Admin.Controllers
{
    public class RechargeController : AdminController
    {
        public ActionResult Index(int page = 1, string search = null, bool submitted = false, DateTime? beginDate = null, DateTime? endDate = null)
        {
            IQueryable<Recharge> list = Repository.Recharges.OrderByDescending(p => p.ID);

            if (submitted)
            {
                list = list.Where(p => p.IsSubmitted).AsQueryable();
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

            var data = new PageableData<Recharge>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Balance()
        {
            var balance = Repository.BalanceSiteRecharge;
            return View(balance);
        }

        public ActionResult RemoveUnsubmitted()
        {
            Repository.RemoveUnSubmittedRecharge();
            return RedirectToAction("Index");
        }
    }
}
