using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData
{
    public class Surname
    {
        public static string[] SurnameArr =  {"Washington", "Adams", "Jefferson", "Madison", "Monroe",
                                            "Jackson", "Van Buren", "Harrison", "Tyler", "Polk", "Taylor", "Fillmore",
                                            "Pierce", "Buchanan", "Lincoln", "Johnson", "Grant", "Hayes", "Garfield", "Arthur",
                                            "Cleveland", "Harrison", "McKinley", "Roosevelt", "Taft", "Wilson", "Harding", "Coolidge",
                                            "Hoover", "Truman", "Eisenhower", "Kennedy", "Johnson", "Nixon", "Ford", "Carter", "Reagan", "Bush", 
                                            "Clinton", "Obama"};

        public static string GetRandom()
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            var index = rand.Next(SurnameArr.Count());
            return SurnameArr[index];
        }

    }
}
