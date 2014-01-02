using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<GlobalUnique> GlobalUniques
        {
            get
            {
                return Db.GlobalUniques;
            }
        }

        public bool SaveGlobalUnique(GlobalUnique instance)
        {
            try
            {
                var cache = Db.GlobalUniques.FirstOrDefault(p => p.ID == instance.ID);
                if (cache != null)
                {
                    cache.LastDate = DateTime.Now;
                }
                else
                {
                    instance.AddedDate = DateTime.Now;
                    instance.LastDate = DateTime.Now;
                    Db.GlobalUniques.InsertOnSubmit(instance);
                }
                Db.GlobalUniques.Context.SubmitChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}