using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Global.Config;

namespace turniri.Models.Info
{
    public class QiwiRechargeInfo
    {
        public string To { get; set; }

        public double Sum { get; set; }

        public double ChargeSum { get; set; }

        public string Description { get; set; }

        public int Label { get; set; }

        public int LifeTime { get; set; }

        public string QiwiID { get; set; }

        public QiwiRechargeInfo(IConfig config)
        {
            LifeTime = 72;
            QiwiID = config.QiwiID;
        }
    }
}