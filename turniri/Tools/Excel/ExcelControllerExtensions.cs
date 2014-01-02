using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace turniri.Tools.Extensions.Excel
{
    public static class ExcelControllerExtensions
    {
        public static ActionResult Excel
        (
            this Controller controller,
            IQueryable rows,
            string fileName,
            Dictionary<string, string> headers
        )
        {
            return new ExcelResult(fileName, rows,  headers);
        }
    }
}
