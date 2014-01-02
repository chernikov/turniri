using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Distribution> Distributions
        {
            get
            {
                return Db.Distributions;
            }
        }

        public bool CreateDistribution(Distribution instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Distributions.InsertOnSubmit(instance);
                Db.Distributions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateDistribution(Distribution instance)
        {
            var cache = Db.Distributions.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.Name = instance.Name;
				cache.Subject = instance.Subject;
				cache.Body = instance.Body;
				cache.IsStart = instance.IsStart;
                Db.Distributions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool StartDistribution(int idDistribution)
        {
            var cache = Db.Distributions.FirstOrDefault(p => p.ID == idDistribution);
            if (cache != null)
            {
                cache.IsStart = true;
                Db.Distributions.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ClearDistribution(int idDistribution)
        {
            var mails = Db.Mails.Where(p => p.DistributionID == idDistribution);
            Db.Mails.DeleteAllOnSubmit(mails);
            Db.Mails.Context.SubmitChanges();

            var cache = Db.Distributions.FirstOrDefault(p => p.ID == idDistribution);
            if (cache != null)
            {
                cache.IsStart = false;
                Db.Distributions.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveDistribution(int idDistribution)
        {
            Distribution instance = Db.Distributions.FirstOrDefault(p => p.ID == idDistribution);
            if (instance != null)
            {
                Db.Distributions.DeleteOnSubmit(instance);
                Db.Distributions.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}