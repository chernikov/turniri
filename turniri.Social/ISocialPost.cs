using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.Social
{
    public interface ISocialPost
    {
        string Title { get; set; }

        string Preview { get; set; }

        string Teaser { get; set; }

        string Link { get; set; }

        IList<string> Images { get; }

        string Identifier { get; set; }
    }
}
