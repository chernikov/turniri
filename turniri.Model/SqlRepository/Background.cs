using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Background> Backgrounds
        {
            get
            {
                return Db.Backgrounds;
            }
        }

        public bool CreateBackground(Background instance)
        {
            if (instance.ID == 0)
            {
                Db.Backgrounds.InsertOnSubmit(instance);
                Db.Backgrounds.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateBackground(Background instance)
        {
            var cache = Db.Backgrounds.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Path = instance.Path;
				cache.ImagePath = instance.ImagePath;
				cache.IsOn = instance.IsOn;
                Db.Backgrounds.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveBackground(int idBackground)
        {
            Background instance = Db.Backgrounds.FirstOrDefault(p => p.ID == idBackground);
            if (instance != null)
            {
                Db.Backgrounds.DeleteOnSubmit(instance);
                Db.Backgrounds.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}