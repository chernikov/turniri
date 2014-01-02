using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class PromoAction
    {
        public enum TypeEnum
        {
            Percentage = 0x01,
            Absolute = 0x02
        }

        public enum TargetEnum
        {
            Both = 0x01,
            TeamSubscription = 0x02,
            IndividualSubscription = 0x03,
        }

        public bool CanChangeReusable
        {
            get
            {
                return !PromoCodes.Any(p => p.UsedDate.HasValue);
            }
        }

        public bool CanRemove
        {
            get
            {
                return !PromoCodes.Any(p => p.UsedDate.HasValue);
            }
        }

        public int UsedCodesCount
        {
            get
            {
                return PromoCodes.Count(p => p.UsedDate.HasValue);
            }
        }

        public int Quantity
        {
            get
            {
                return PromoCodes.Count();
            }
        }

        public IEnumerable<PromoCode> SubPromoCodes
        {
            get
            {
                return PromoCodes.AsEnumerable();
            }
        }
	}
}