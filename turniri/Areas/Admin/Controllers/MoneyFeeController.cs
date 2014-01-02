using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    public class MoneyFeeController : AdminController
    {
     
        public ActionResult Index()
        {
            var list = Repository.MoneyFees.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var moneyFeeView = new MoneyFeeView();


            return View("Edit", moneyFeeView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.ID == id);

            if (moneyFee != null)
            {
                var moneyFeeView = (MoneyFeeView)ModelMapper.Map(moneyFee, typeof(MoneyFee), typeof(MoneyFeeView));
                return View(moneyFeeView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(MoneyFeeView moneyFeeView)
        {
            if (ModelState.IsValid)
            {
                var moneyFee = (MoneyFee)ModelMapper.Map(moneyFeeView, typeof(MoneyFeeView), typeof(MoneyFee));
                if (moneyFee.ID == 0)
                {
                    Repository.CreateMoneyFee(moneyFee);
                }
                else
                {
                    Repository.UpdateMoneyFee(moneyFee);
                }
                return RedirectToAction("Index");
            }
            return View(moneyFeeView);
        }

        public ActionResult Delete(int id)
        {
            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.ID == id);
            if (moneyFee != null)
            {
                Repository.RemoveMoneyFee(moneyFee.ID);
            }
            return RedirectToAction("Index");
        }
    }
}
