using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace turniri.Model
{
    public class PageableData<T, TKey> where T : class
    {
        public static int ItemPerPage = 20;

        public IEnumerable<T> List { get; set; }

        public int PageNo { get; set; }

        public int CountPage { get; set; }

        public string Action { get; set; }

        public void Init(IRepository repository, int page, string action, Func<T, TKey> orderBy)
        {
            Action = action;
            PageNo = page;
            var count = repository.GetTable<T>().Count();
            CountPage = (int)decimal.Remainder(count, ItemPerPage) == 0 ? count / ItemPerPage : count / ItemPerPage + 1;
            List = repository.GetTable<T>().OrderBy(orderBy).Skip((PageNo - 1) * ItemPerPage).Take<T>(ItemPerPage);
        }

        public void Init(IRepository repository, int page, string action, Func<T, TKey> orderBy, Func<T, bool> where)
        {
            Action = action;
            PageNo = page;
            var count = repository.GetTable<T>().Count();
            CountPage = (int)decimal.Remainder(count, ItemPerPage) == 0 ? count / ItemPerPage : count / ItemPerPage + 1;
            List = repository.GetTable<T>().Where(where).OrderBy(orderBy).Skip((PageNo - 1) * ItemPerPage).Take<T>(ItemPerPage);
        }
    }

    public class PageableData<T> where T : class
    {
        public static int ItemPerPageDefault = 20;

        public IEnumerable<T> List { get; set; }

        public int PageNo { get; set; }

        public int CountPage { get; set; }

        public int ItemPerPage { get; set; }

        public string Action { get; set; }

        public void Init(IRepository repository, int page, string action)
        {
            Action = action;
            PageNo = page;
            var count = repository.GetTable<T>().Count();
            CountPage = (int)decimal.Remainder(count, ItemPerPageDefault) == 0 ? count / ItemPerPageDefault : count / ItemPerPageDefault + 1;
            List = repository.GetTable<T>().Skip((PageNo - 1) * ItemPerPageDefault).Take<T>(ItemPerPageDefault);
        }

        public void Init(IRepository repository, int page, string action, Func<T, bool> where)
        {
            Action = action;
            PageNo = page;
            ItemPerPage = ItemPerPageDefault;
            var count = repository.GetTable<T>().Count();
            CountPage = (int)decimal.Remainder(count, ItemPerPageDefault) == 0 ? count / ItemPerPageDefault : count / ItemPerPageDefault + 1;
            List = repository.GetTable<T>().Where(where).Skip((PageNo - 1) * ItemPerPageDefault).Take<T>(ItemPerPageDefault);
        }

        public void Init(IQueryable<T> queryableSet, int page, string action, int itemPerPage = 0)
        {
            if (itemPerPage == 0)
            {
                itemPerPage = ItemPerPageDefault;
            }
            ItemPerPage = itemPerPage;

            Action = action;
            PageNo = page;
            var count = queryableSet.Count();

            CountPage = (int)decimal.Remainder(count, itemPerPage) == 0 ? count / itemPerPage : count / itemPerPage + 1;
            List = queryableSet.Skip((PageNo - 1) * itemPerPage).Take<T>(itemPerPage);
        }
    }
}