using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Vendor> Vendors
        {
            get
            {
                return Db.Vendors;
            }
        }

        public bool CreateVendor(Vendor instance)
        {
            if (instance.ID == 0)
            {
                Db.Vendors.InsertOnSubmit(instance);
                Db.Vendors.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateVendor(Vendor instance)
        {
            var cache = Db.Vendors.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Text = instance.Text;
                Db.Vendors.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveVendor(int idVendor)
        {
            Vendor instance = Db.Vendors.FirstOrDefault(p => p.ID == idVendor);
            if (instance != null)
            {
                Db.Vendors.DeleteOnSubmit(instance);
                Db.Vendors.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}