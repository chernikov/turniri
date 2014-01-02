using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Models.Info
{
    public class YandexRechargeInfo
    {
        public string Receiver { get; set; }

        public string FormComment { get; set; }

        public string ShortDest { get; set; }

        public bool WritableTargets { get; set; }

        public bool CommentNeeded { get; set; }

        public int Label { get; set; }

        public double Sum { get; set; }

        public double ChargeSum { get; set; }

        public string QuickpayForm { get; set; }

        public string Targets { get; set; }

        public string Comment { get; set; }

    }
}