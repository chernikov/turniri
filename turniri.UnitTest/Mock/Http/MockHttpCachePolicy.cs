using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Moq;

namespace turniri.UnitTest.Mock.Http
{
    public class MockHttpCachePolicy : Mock<HttpCachePolicyBase>
    {
        public MockHttpCachePolicy(MockBehavior mockBehavior = MockBehavior.Strict)
            : base(mockBehavior)
        {
            
        }
    }
}
