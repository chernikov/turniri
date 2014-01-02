using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Tools
{
    public static class MatchesMathLogic
    {
        public static int GetDegree(int number)
        {
            return (int)(Math.Log(number) / Math.Log(2));
        }

        public static int GetMin2Pow(int number)
        {
            return (int)Math.Pow(2, (int)Math.Ceiling(Math.Log(number) / Math.Log(2)));
        }

        public static DateTime GetDateLimitSlice(DateTime beginDate, DateTime endDate, int count, int numSlice)
        {
            if (endDate < beginDate)
            {
                throw new ArgumentOutOfRangeException("endDate must be greater than beginDate");
            }
            var allTime = endDate - beginDate;

            var slice = allTime.TotalSeconds / count;

            return beginDate.AddSeconds(slice*numSlice);
        }
    }
}
