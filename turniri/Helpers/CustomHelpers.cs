using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Text;
using turniri.Global;

namespace turniri.Helpers
{
    public static class CustomHelpers
    {
        public static MvcHtmlString Nl2Br(this HtmlHelper html, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(input.Replace("\r\n", "<br />\r\n"));
        }

        public static MvcHtmlString PageLinks(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var builder = new StringBuilder();
            for (int i = 1; i <= totalPages; i++)
            {
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    var subBuilder = new TagBuilder("a");
                    subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                    if (i == currentPage)
                    {
                        subBuilder.AddCssClass("selected");
                    }
                    builder.AppendLine(subBuilder.ToString());
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    builder.AppendLine(" ... ");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    builder.AppendLine(" ... ");
                }
            }
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString PageLinksMessage(this HtmlHelper html, int currentPage, int totalPages, int itemPerPage, Func<int, string> pageUrl)
        {
            var builder = new StringBuilder();
            for (int i = 1; i <= totalPages; i++)
            {
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    var subBuilder = new TagBuilder("a");
                    subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    subBuilder.InnerHtml = string.Format("{0}-{1}", (i - 1) * itemPerPage + 1, i * itemPerPage);
                    if (i == currentPage)
                    {
                        subBuilder.AddCssClass("selected");
                    }
                    builder.AppendLine(subBuilder.ToString());
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    builder.AppendLine(" ... ");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    builder.AppendLine(" ... ");
                }
            }
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString PageLinkLancer(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();

            // Previous
            if (currentPage > 1)
            {
                var subBuilder = new TagBuilder("a");
                subBuilder.MergeAttribute("href", pageUrl.Invoke(1));
                subBuilder.InnerHtml = "Предыдущая";
                sb.AppendFormat("<div class=\"prev\">← {0}</div>", subBuilder);
            }
            else
            {
                sb.Append("<div class=\"empty-prev\">&nbsp;</div>");
            }
            sb.Append("<div class=\"current\">");
            for (int i = 1; i <= totalPages; i++)
            {
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {

                    if (i == currentPage)
                    {
                        var subBuilder = new TagBuilder("span");
                        subBuilder.MergeAttribute("style", "font-weight: bold;");
                        subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                        sb.AppendLine(subBuilder.ToString());
                    }
                    else
                    {
                        var subBuilder = new TagBuilder("a");
                        subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                        subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                        sb.AppendLine(subBuilder.ToString());
                    }
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    sb.AppendLine(" ... ");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    sb.AppendLine(" ... ");
                }
            }
            sb.Append("</div>");
            // Next
            if (currentPage < totalPages)
            {
                var subBuilder = new TagBuilder("a");
                subBuilder.MergeAttribute("href", pageUrl.Invoke(totalPages));
                subBuilder.InnerHtml = "Следующая";
                sb.AppendFormat("<div class=\"next\">{0} →</div>", subBuilder);
            }
            else
            {
                sb.Append("<div class=\"empty-next\">&nbsp;</div>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString PageLinksTurniri(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var pageSize = 10;
            var page = currentPage / pageSize - (currentPage % pageSize == 0 ? 1 : 0);
             
            var builder = new StringBuilder();

          
            if (page > 0)
            {
                var subLeftArrowBuilderA = new TagBuilder("a");
                subLeftArrowBuilderA.MergeAttribute("href", pageUrl.Invoke((page - 1) * pageSize + 1));
                subLeftArrowBuilderA.AddCssClass("arrow");
                subLeftArrowBuilderA.InnerHtml = "<span class=\"icon-right-arrow-light-paging sprite\"></span>";
                builder.AppendLine(subLeftArrowBuilderA.ToString());
            }
            for (int i = (page * pageSize)+ 1; i <= ((page + 1) * pageSize); i++)
            {
                if (i <= totalPages)
                {
                    var subBuilder = new TagBuilder("a")
                    {
                        InnerHtml = i.ToString(CultureInfo.InvariantCulture)
                    };
                    subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    if (i == currentPage)
                    {
                        subBuilder.AddCssClass("current");
                    }
                    builder.AppendLine(subBuilder.ToString());
                }
            }
            if (totalPages > ((page+1) * pageSize))
            {
                var subLeftArrowBuilderA = new TagBuilder("a");
                subLeftArrowBuilderA.MergeAttribute("href", pageUrl.Invoke((page + 1) * pageSize + 1));
                subLeftArrowBuilderA.AddCssClass("arrow");
                subLeftArrowBuilderA.InnerHtml = "<span class=\"icon-arrow-light-paging sprite\"></span>";
                builder.AppendLine(subLeftArrowBuilderA.ToString());
            }
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString BreadChunks<T>(this HtmlHelper html, T obj, Func<T, string> url, Func<T, string> name, Func<T, T> parent, string separator) where T : new()
        {
            var builder = new StringBuilder();
            var current = parent.Invoke(obj);
            while (current != null)
            {
                builder.Insert(0, separator);
                var subBuilder = new TagBuilder("a");
                subBuilder.MergeAttribute("href", url.Invoke(current));
                subBuilder.InnerHtml = name.Invoke(current);
                builder.Insert(0, subBuilder.ToString());
                current = parent.Invoke(current);
            }
            return new MvcHtmlString(builder.ToString());
        }

        public static string SetSmiles(this HtmlHelper html, int i, string name)
        {
            var img = i > 9 ? i.ToString() : "0" + i.ToString();
            return "<img src=\"/Media/files/smiles/" + img + ".gif\" title=\"" + name + "\" class=\"smile\"/>";
        }

        public static string SetSmiles(int i, string name)
        {
            var img = i > 9 ? i.ToString() : "0" + i.ToString();
            return "<img src=\"/Media/files/smiles/" + img + ".gif\" title=\"" + name + "\" class=\"smile\"/>";
        }
    }
}