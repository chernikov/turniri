using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Cart> Carts
        {
            get
            {
                return Db.Carts;
            }
        }

        public bool CreateCart(Cart instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.LastVisitDate = DateTime.Now;
                Db.Carts.InsertOnSubmit(instance);
                Db.Carts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateLastVisitDateCart(Cart instance)
        {
            try
            {
                var cache = Db.Carts.FirstOrDefault(p => p.ID == instance.ID);
                if (cache != null)
                {
                    cache.LastVisitDate = DateTime.Now;
                    Db.Carts.Context.SubmitChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        public bool UpdateCart(Cart instance)
        {
            var cache = Db.Carts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.OrderType = instance.OrderType;
                cache.LastVisitDate = DateTime.Now;
				cache.UserID = instance.UserID;
				cache.ManagerID = instance.ManagerID;
				cache.PromoCodeID = instance.PromoCodeID;
				cache.Address = instance.Address;
				cache.Phone = instance.Phone;
				cache.DeliverTime = instance.DeliverTime;
				cache.Cashback = instance.Cashback;
				cache.CreatedByManager = instance.CreatedByManager;
				cache.DeliverCost = instance.DeliverCost;
				cache.Notice = instance.Notice;
				cache.TotalPrice = instance.TotalPrice;
                Db.Carts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCart(int idCart)
        {
            Cart instance = Db.Carts.FirstOrDefault(p => p.ID == idCart);
            if (instance != null)
            {
                Db.Carts.DeleteOnSubmit(instance);
                Db.Carts.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ClearCart(int idCart)
        {
            Cart instance = Db.Carts.FirstOrDefault(p => p.ID == idCart);
            if (instance != null)
            {
                instance.CartProducts.Clear();
                Db.Carts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}