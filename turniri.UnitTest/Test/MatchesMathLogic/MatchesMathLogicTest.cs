using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using turniri.Tools;

namespace turniri.UnitTest.Test
{
    [TestFixture]
    public class MatchesMathLogicTest
    {
        [Test]
        public void GetDegree_Get16_Return_4()
        {
            var result = MatchesMathLogic.GetDegree(16);

            Assert.AreEqual(4, result);
        }

        [Test]
        public void GetMin2Pow_Set11_Return16()
        {
            var result = MatchesMathLogic.GetMin2Pow(11);

            Assert.AreEqual(16, result);
        }

        [Test]
        public void GetDateLimitSlice_12juneAnd14june_Get13june()
        {
            var result = MatchesMathLogic.GetDateLimitSlice(new DateTime(2012, 6, 12), new DateTime(2012, 6, 14), 2, 1);

            Assert.AreEqual(new DateTime(2012, 6, 13), result);
        }
    }
}
