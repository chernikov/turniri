using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Catalog> Catalogs
        {
            get
            {
                return Db.Catalogs;
            }
        }

        public bool CreateCatalog(Catalog instance)
        {
            if (instance.ID == 0)
            {
                Db.Catalogs.InsertOnSubmit(instance);
                Db.Catalogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateCatalog(Catalog instance)
        {
            var cache = Db.Catalogs.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Name = instance.Name;
                Db.Catalogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveCatalog(int idCatalog)
        {
            var instance = Db.Catalogs.FirstOrDefault(p => p.ID == idCatalog);
            if (instance != null)
            {
                Db.Catalogs.DeleteOnSubmit(instance);
                Db.Catalogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}