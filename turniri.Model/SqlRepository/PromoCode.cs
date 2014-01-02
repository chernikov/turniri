using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PromoCode> PromoCodes
        {
            get
            {
                return Db.PromoCodes;
            }
        }

        public bool CreatePromoCode(PromoCode instance)
        {
            if (instance.ID == 0)
            {
                Db.PromoCodes.InsertOnSubmit(instance);
                Db.PromoCodes.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePromoCode(PromoCode instance)
        {
            var cache = Db.PromoCodes.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.PromoActionID = instance.PromoActionID;
				cache.Code = instance.Code;
				cache.AddedDate = instance.AddedDate;
				cache.UsedDate = instance.UsedDate;
                Db.PromoCodes.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePromoCode(int idPromoCode)
        {
            PromoCode instance = Db.PromoCodes.FirstOrDefault(p => p.ID == idPromoCode);
            if (instance != null)
            {
                Db.PromoCodes.DeleteOnSubmit(instance);
                Db.PromoCodes.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool GeneratePromoCodes(int promoActionId, int quantity, string code)
        {
            var promoAction = Db.PromoActions.FirstOrDefault(p => p.ID == promoActionId);

            if (promoAction != null)
            {
                var additional = quantity - promoAction.PromoCodes.Count();
                if (additional > 0)
                {
                    for (int i = 0; i < additional; i++)
                    {
                        var newPromoCode = new PromoCode()
                        {
                            PromoActionID = promoAction.ID,
                            AddedDate = DateTime.Now,
                        };
                        if (!string.IsNullOrWhiteSpace(code))
                        {
                            newPromoCode.Code = code;
                        }
                        else
                        {
                            newPromoCode.Code = StringExtension.CreateRandomPassword(15, "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
                        }
                        CreatePromoCode(newPromoCode);
                    }
                }
                else if (additional < 0)
                {
                    var toRemove = -additional;
                    var nonUsedPromoCodes = promoAction.PromoCodes.Where(p => !p.UsedDate.HasValue).ToList();

                    if (nonUsedPromoCodes.Count() < toRemove)
                    {
                        toRemove = nonUsedPromoCodes.Count();
                    }

                    for (int i = 0; i < toRemove; i++)
                    {
                        RemovePromoCode(nonUsedPromoCodes[i].ID);
                    }
                }
                return true;
            }
            return false;
        }
    }
}