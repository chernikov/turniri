using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Category> Categories
        {
            get
            {
                return Db.Categories;
            }
        }

        public bool CreateCategory(Category instance)
        {
            if (instance.ID == 0)
            {
                Db.Categories.InsertOnSubmit(instance);
                Db.Categories.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCategory(Category instance)
        {
            var cache = Db.Categories.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Url = instance.Url;
                Db.Categories.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCategory(int idCategory)
        {
            Category instance = Db.Categories.FirstOrDefault(p => p.ID == idCategory);
            if (instance != null)
            {
                Db.Categories.DeleteOnSubmit(instance);
                Db.Categories.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}