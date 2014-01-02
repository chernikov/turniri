using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Google
{
    public interface IGoogleAppConfig
    {
        string ClientId { get; }

        string ClientSecret { get; }
    }
}
