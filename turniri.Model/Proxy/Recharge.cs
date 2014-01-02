using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class Recharge
    {
        public enum ProviderType : int
        {
            Yandex = 0x01,
            Webmoney = 0x02,
            Robokassa = 0x03, 
            Qiwi = 0x04
        }
	}
}