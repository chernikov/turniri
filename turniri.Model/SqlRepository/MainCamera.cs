using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<MainCamera> MainCameras
        {
            get
            {
                return Db.MainCameras;
            }
        }

        public bool CreateMainCamera(MainCamera instance)
        {
            if (instance.ID == 0)
            {
                Db.MainCameras.InsertOnSubmit(instance);
                Db.MainCameras.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMainCamera(MainCamera instance)
        {
            var cache = Db.MainCameras.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.CameraID = instance.CameraID;
				cache.Code = instance.Code;
                cache.Enabled = instance.Enabled;
                Db.MainCameras.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveMainCamera(int idMainCamera)
        {
            MainCamera instance = Db.MainCameras.FirstOrDefault(p => p.ID == idMainCamera);
            if (instance != null)
            {
                Db.MainCameras.DeleteOnSubmit(instance);
                Db.MainCameras.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}