using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.UnitTest.Fake
{
    public class FakeValueProvider
    {
        protected Dictionary<string, object> Values { get; set; }

        public FakeValueProvider()
        {
            Values = new Dictionary<string, object>();
        }

        public object this[string index] 
        {
            get 
            {
                if (Values.ContainsKey(index))
                {
                    return Values[index];
                }
                return null;
            }

            set
            {
                if (Values.ContainsKey(index))
                {
                    Values[index] = value;
                }
                else
                {
                    Values.Add(index, value);
                }
            }
        }
    }
}
