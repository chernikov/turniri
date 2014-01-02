using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateData
{
    public class CreditCard
    {
        public static string GetRandom()
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            string number = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                number += (rand.Next() % 10).ToString();
            }
            return number;
        }
    }
}
