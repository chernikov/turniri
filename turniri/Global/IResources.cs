using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Global
{
    public interface IResources
    {
        string this[string ObjectName]
        {
            get;
        }
    }
}