using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Banner> Banners
        {
            get
            {
                return Db.Banners;
            }
        }

        public bool CreateBanner(Banner instance)
        {
            if (instance.ID == 0)
            {
                Db.Banners.InsertOnSubmit(instance);
                Db.Banners.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateBanner(Banner instance)
        {
            var cache = Db.Banners.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Path = instance.Path;
				cache.Type = instance.Type;
				cache.ImagePath = instance.ImagePath;
				cache.Url = instance.Url;
				cache.IsOn = instance.IsOn;
                Db.Banners.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ViewBanner(int idBanner)
        {
            try
            {
                var cache = Db.Banners.FirstOrDefault(p => p.ID == idBanner);
                if (cache != null)
                {
                    cache.TotalViews++;
                    Db.Banners.Context.SubmitChanges();
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool ClickBanner(int idBanner)
        {
            var cache = Db.Banners.FirstOrDefault(p => p.ID == idBanner);
            if (cache != null)
            {
                var bannerStatistic = new BannerStatistic
                {
                    AddedDate = DateTime.Now,
                    BannerID = cache.ID
                };
                Db.BannerStatistics.InsertOnSubmit(bannerStatistic);
                cache.TotalClicks++;
                Db.Banners.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveBanner(int idBanner)
        {
            Banner instance = Db.Banners.FirstOrDefault(p => p.ID == idBanner);
            if (instance != null)
            {
                Db.Banners.DeleteOnSubmit(instance);
                Db.Banners.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}