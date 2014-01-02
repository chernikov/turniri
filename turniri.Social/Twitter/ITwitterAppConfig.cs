using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social.Twitter
{
    public interface ITwitterAppConfig
    {
        string twitterConsumerKey { get; }
        string twitterConsumerSecret { get; }
    }
}
