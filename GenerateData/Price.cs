using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData
{
    public class Price
    {

        private static Random rand = new Random((int)DateTime.Now.Ticks);

        public static double GetRandom()
        {
            var value = (rand.Next(10000) + 1000) / 100;

            return (double)value;
        }
    }
}
