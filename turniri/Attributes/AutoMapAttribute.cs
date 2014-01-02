﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Controllers;

namespace turniri.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoMapAttribute : ActionFilterAttribute
    {
        public Type SourceType { get; private set; }
        public Type DestType { get; private set; }

        public AutoMapAttribute(Type sourceType, Type destType)
        {
            SourceType = sourceType;
            DestType = destType;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var controller = filterContext.Controller as IModelMapperController;
            if (controller == null)
            {
                return;
            }
            var model = filterContext.Controller.ViewData.Model;
            if (model != null && SourceType.IsAssignableFrom(model.GetType()))
            {
                var viewModel = controller.ModelMapper.Map(model, SourceType, DestType);
                filterContext.Controller.ViewData.Model = viewModel;
            }
        }
    }
}