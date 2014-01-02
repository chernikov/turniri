using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<NewType> NewTypes
        {
            get
            {
                return Db.NewTypes;
            }
        }

        public bool CreateNewType(NewType instance)
        {
            if (instance.ID == 0)
            {
                Db.NewTypes.InsertOnSubmit(instance);
                Db.NewTypes.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateNewType(NewType instance)
        {
            var cache = Db.NewTypes.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                Db.NewTypes.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveNewType(int idNewType)
        {
            NewType instance = Db.NewTypes.FirstOrDefault(p => p.ID == idNewType);
            if (instance != null)
            {
                Db.NewTypes.DeleteOnSubmit(instance);
                Db.NewTypes.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}