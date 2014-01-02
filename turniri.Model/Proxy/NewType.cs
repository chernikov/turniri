using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class NewType
    {
        public IEnumerable<New> NewsPage(int page = 1, int itemPerPage = 5)
        {
            return News.OrderByDescending(p => p.AddedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public int CountPage(int itemPerPage = 5)
        {
            return News.Count / itemPerPage + (News.Count % itemPerPage != 0 ? 1 : 0);
        }
	}
}