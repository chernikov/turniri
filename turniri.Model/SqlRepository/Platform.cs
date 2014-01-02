using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Platform> Platforms
        {
            get
            {
                return Db.Platforms;
            }
        }

        public bool CreatePlatform(Platform instance)
        {
            if (instance.ID == 0)
            {
                instance.Url = Translit.Translate(instance.Name);
                Db.Platforms.InsertOnSubmit(instance);
                Db.Platforms.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePlatform(Platform instance)
        {
            var cache = Db.Platforms.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                cache.Url = Translit.Translate(instance.Name);
                Db.Platforms.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePlatform(int idPlatform)
        {
            Platform instance = Db.Platforms.FirstOrDefault(p => p.ID == idPlatform);
            if (instance != null)
            {
                Db.Platforms.DeleteOnSubmit(instance);
                Db.Platforms.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}