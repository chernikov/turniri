using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateData
{
    public class Phone
    {
        public enum PhoneType
        {
            SimpleNumbers = 0x01,
            LeaderTwoZeros = 0x02,
            LeaderPlus = 0x03,
            WithBraces = 0x04,
            WithSpaces = 0x05,
            WithHyphens = 0x06

        }

        public static string GetRandom() 
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            string number = string.Empty;
            for (int i = 0; i < 12; i++)
            {
                number += (rand.Next() % 10).ToString();
            }

            var type = (PhoneType)((rand.Next() % 6) + 1);

            switch (type)
            {
                case PhoneType.SimpleNumbers :
                    return number;
                case PhoneType.LeaderTwoZeros :
                    return "00" + number;
                case PhoneType.LeaderPlus :
                    return "+" + number;
                case PhoneType.WithBraces :
                    number = "(" + number;
                    number = number.Insert(4, ")");
                    return number;
                case PhoneType.WithSpaces : 
                    number = number.Insert(10, " ");
                    number = number.Insert(8, " ");
                    number = number.Insert(5, " ");
                    number = number.Insert(3, " ");
                    return number;
                case PhoneType.WithHyphens :
                    number = number.Insert(10, "-");
                    number = number.Insert(8, "-");
                    number = number.Insert(5, "-");
                    number = number.Insert(3, "-");
                    return number;
            }
            return number;
        }
    }
}
