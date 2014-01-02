using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using turniri.Model;

namespace turniri.UnitTest.Test.Model
{
    [TestFixture]
    public class RoundTest
    {
        [Test]
        public void getIsRoundForScore_ModelIsNull_NoFail()
        {
            var round = new Round();

            Assert.AreEqual(false, round.IsRoundForScore);
        }
    }
}