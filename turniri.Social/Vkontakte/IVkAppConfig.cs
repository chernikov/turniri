using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Vkontakte
{
    public interface IVkAppConfig
    {
        string AppKey { get; }
        string AppSecret { get; }
    }
}
