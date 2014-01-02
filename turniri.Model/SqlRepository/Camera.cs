using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Camera> Cameras
        {
            get
            {
                return Db.Cameras;
            }
        }

        public bool CreateCamera(Camera instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Cameras.InsertOnSubmit(instance);
                Db.Cameras.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCamera(Camera instance)
        {
            var cache = Db.Cameras.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.MatchID = instance.MatchID;
				cache.Name = instance.Name;
				cache.Code = instance.Code;
				cache.Enabled = instance.Enabled;
                Db.Cameras.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCamera(int idCamera)
        {
            Camera instance = Db.Cameras.FirstOrDefault(p => p.ID == idCamera);
            if (instance != null)
            {
                Db.Cameras.DeleteOnSubmit(instance);
                Db.Cameras.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}