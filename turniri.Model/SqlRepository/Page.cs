using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Page> Pages
        {
            get
            {
                return Db.Pages;
            }
        }

        public bool CreatePage(Page instance)
        {
            if (instance.ID == 0)
            {
                Db.Pages.InsertOnSubmit(instance);
                Db.Pages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePage(Page instance)
        {
            var cache = Db.Pages.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Url = instance.Url;
				cache.Text = instance.Text;
				cache.Description = instance.Description;
				cache.Keywords = instance.Keywords;
                Db.Pages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePage(int idPage)
        {
            Page instance = Db.Pages.FirstOrDefault(p => p.ID == idPage);
            if (instance != null)
            {
                Db.Pages.DeleteOnSubmit(instance);
                Db.Pages.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}