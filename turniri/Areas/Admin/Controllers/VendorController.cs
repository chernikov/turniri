using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;


namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,seller")]
    public class VendorController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Vendors.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var vendorView = new VendorView();
			return View("Edit", vendorView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  vendor = Repository.Vendors.FirstOrDefault(p => p.ID == id); 

			if (vendor != null) {
				var vendorView = (VendorView)ModelMapper.Map(vendor, typeof(Vendor), typeof(VendorView));
				return View(vendorView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(VendorView vendorView)
        {
            if (ModelState.IsValid)
            {
                var vendor = (Vendor)ModelMapper.Map(vendorView, typeof(VendorView), typeof(Vendor));
                if (vendor.ID == 0)
                {
                    Repository.CreateVendor(vendor);
                }
                else
                {
                    Repository.UpdateVendor(vendor);
                }
                return RedirectToAction("Index");
            }
            return View(vendorView);
        }

        public ActionResult Delete(int id)
        {
            var vendor = Repository.Vendors.FirstOrDefault(p => p.ID == id);
            if (vendor != null)
            {
                    Repository.RemoveVendor(vendor.ID);
            }
			return RedirectToAction("Index");
        }
	}
}