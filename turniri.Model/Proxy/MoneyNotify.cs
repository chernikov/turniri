using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class MoneyNotify
    {
        public enum TypeEnum
        {
            Yandex = 0x01,
            Qiwi = 0x02
        }

        public string TypeStr
        {
            get
            {
                switch ((TypeEnum)Type)
                {
                    case TypeEnum.Yandex :
                        return "Yandex";
                    case TypeEnum.Qiwi:
                        return "Qiwi";
                }
                return "Иное";
            }
        }
	}
}