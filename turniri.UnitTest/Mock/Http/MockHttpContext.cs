using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using turniri.Global.Auth;

namespace turniri.UnitTest.Mock.Http
{
    public class MockHttpContext : Mock<HttpContextBase>
    {
        [Inject]
        public HttpCookieCollection Cookies { get; set; }

        public MockHttpCachePolicy Cache { get; set; }

        public MockHttpBrowserCapabilities Browser { get; set; }

        public MockHttpSessionState SessionState { get; set; }

        public MockHttpServerUtility ServerUtility { get; set; }

        public MockHttpResponse Response { get; set; }

        public MockHttpRequest Request { get; set; }

        public MockHttpContext(MockBehavior mockBehavior = MockBehavior.Strict)
            : this(null, mockBehavior)
        {
        }

        public MockHttpContext(IAuthentication auth, MockBehavior mockBehavior = MockBehavior.Strict)
            : base(mockBehavior)
        {
            //request 
            Browser = new MockHttpBrowserCapabilities(mockBehavior);
            Browser.Setup(b => b.IsMobileDevice).Returns(false);

            Request = new MockHttpRequest(mockBehavior);
            Request.Setup(r => r.Cookies).Returns(Cookies);
            Request.Setup(r => r.ValidateInput());
            Request.Setup(r => r.UserAgent).Returns("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11");
            Request.Setup(r => r.Browser).Returns(Browser.Object);
            this.Setup(p => p.Request).Returns(Request.Object);

            //response
            Cache = new MockHttpCachePolicy(MockBehavior.Loose);
           
            Response = new MockHttpResponse(mockBehavior);
            Response.Setup(r => r.Cookies).Returns(Cookies);
            Response.Setup(r => r.Cache).Returns(Cache.Object);
            this.Setup(p => p.Response).Returns(Response.Object);

            //user
            if (auth != null)
            {
                this.Setup(p => p.User).Returns(() => auth.CurrentUser);
            }
            else
            {
                this.Setup(p => p.User).Returns(new UserProvider("", null));
            }
            //Session State
            SessionState = new MockHttpSessionState(mockBehavior);
            this.Setup(p => p.Session).Returns(SessionState.Object);

            //Server Utility
            ServerUtility = new MockHttpServerUtility(mockBehavior);
            this.Setup(p => p.Server).Returns(ServerUtility.Object);

            //Items
            var items = new ListDictionary();
            this.Setup(p => p.Items).Returns(items);
        }
    }
}
