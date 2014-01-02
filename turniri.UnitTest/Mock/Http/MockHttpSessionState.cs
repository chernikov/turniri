using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Moq;

namespace turniri.UnitTest.Mock.Http
{
    public class MockHttpSessionState : Mock<HttpSessionStateBase>
    {
        Dictionary<string, object> sessionStorage;

        public MockHttpSessionState(MockBehavior mockBehavior = MockBehavior.Strict)
            : base(mockBehavior)
        {
            sessionStorage = new Dictionary<string, object>();
            this.Setup(p => p[It.IsAny<string>()]).Returns((string index) => sessionStorage[index]);
            this.Setup(p => p.Add(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>((name, obj) =>
            {
                if (!sessionStorage.ContainsKey(name))
                {
                    sessionStorage.Add(name, obj);
                }
                else
                {
                    sessionStorage[name] = obj;
                }
            });
        }
    }
}
