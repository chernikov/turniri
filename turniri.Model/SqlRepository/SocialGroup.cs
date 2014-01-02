using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<SocialGroup> SocialGroups
        {
            get
            {
                return Db.SocialGroups;
            }
        }

        public bool CreateSocialGroup(SocialGroup instance)
        {
            if (instance.ID == 0)
            {
                instance.Number = string.Empty;
                Db.SocialGroups.InsertOnSubmit(instance);
                Db.SocialGroups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSocialGroup(SocialGroup instance)
        {
            var cache = Db.SocialGroups.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Provider = instance.Provider;
				cache.Name = instance.Name;
				cache.Number = instance.Number;
                Db.SocialGroups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveSocialGroup(int idSocialGroup)
        {
            SocialGroup instance = Db.SocialGroups.FirstOrDefault(p => p.ID == idSocialGroup);
            if (instance != null)
            {
                Db.SocialGroups.DeleteOnSubmit(instance);
                Db.SocialGroups.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}