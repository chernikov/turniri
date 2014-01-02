using System.Linq;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;


namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewTypeController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.NewTypes.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var newtypeView = new NewTypeView();

			return View("Edit", newtypeView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  newtype = Repository.NewTypes.FirstOrDefault(p => p.ID == id); 

			if (newtype != null) {
				var newtypeView = (NewTypeView)ModelMapper.Map(newtype, typeof(NewType), typeof(NewTypeView));
				return View(newtypeView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(NewTypeView newtypeView)
        {
            if (ModelState.IsValid)
            {
                var newtype = (NewType)ModelMapper.Map(newtypeView, typeof(NewTypeView), typeof(NewType));
                if (newtype.ID == 0)
                {
                    Repository.CreateNewType(newtype);
                }
                else
                {
                    Repository.UpdateNewType(newtype);
                }
                return RedirectToAction("Index");
            }
            return View(newtypeView);
        }

        public ActionResult Delete(int id)
        {
            var newtype = Repository.NewTypes.FirstOrDefault(p => p.ID == id);
            if (newtype != null)
            {
                Repository.RemoveNewType(newtype.ID);
            }
			return RedirectToAction("Index");
        }

	}
}