using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace turniri.Helpers
{
    public static class LangUrl
    {
        public static MvcHtmlString LangSwitcher(this UrlHelper url, string name, RouteData routeData, string lang)
        {
            var tagBuilder = new TagBuilder("a");

            RouteValueDictionary routeValueDictionary = new RouteValueDictionary(routeData.Values);

            if (routeValueDictionary.ContainsKey("lang") && routeValueDictionary["lang"] as string == lang)
            {
                tagBuilder.AddCssClass("selected");
            }
            else
            {
                routeValueDictionary["lang"] = lang;
            }
            var link = url.RouteUrl(routeValueDictionary);
            tagBuilder.MergeAttribute("href", link);
            tagBuilder.InnerHtml = "<i class=\"myicon-lang-"+lang+"\"></i>" + name;
            return new MvcHtmlString(tagBuilder.ToString());
        }
    }
}