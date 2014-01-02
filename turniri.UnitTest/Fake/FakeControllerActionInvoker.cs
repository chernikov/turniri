using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;

namespace turniri.UnitTest.Fake
{
    public class FakeControllerActionInvoker<TExpectedResult> : ControllerActionInvoker where TExpectedResult : ActionResult
    {
        protected FakeValueProvider FakeValueProvider { get; set; }

        public FakeControllerActionInvoker()
        {
            FakeValueProvider = new FakeValueProvider();
        }
        
        public FakeControllerActionInvoker(FakeValueProvider fakeValueProvider)
        {
            FakeValueProvider = fakeValueProvider;
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, IList<IActionFilter> filters, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }

        protected override object GetParameterValue(ControllerContext controllerContext, ParameterDescriptor parameterDescriptor)
        {
            var obj = FakeValueProvider[parameterDescriptor.ParameterName];
            if (obj != null) 
            {
                return obj;
            } 
            return parameterDescriptor.DefaultValue;
        }


        protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
        {
            Assert.IsInstanceOf<TExpectedResult>(actionResult);
        }
    }
}
