using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace turniri.Global
{
    public interface IErrorList
    {
        IEnumerable<Error> this[string Property] { get;  }
    }
}