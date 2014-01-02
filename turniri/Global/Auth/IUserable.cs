using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using turniri.Model;

namespace turniri.Global.Auth
{
    public interface IUserable : IIdentity
    {
        User User { get; }
    }
}