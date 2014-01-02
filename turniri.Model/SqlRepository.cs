using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;

namespace turniri.Model
{
    public partial class SqlRepository : IRepository
    {
        [Inject]
        public turniriDbDataContext Db { get; set; }

        public IQueryable<T> GetTable<T>() where T : class
        {
            return Db.GetTable<T>().AsQueryable<T>();
        }
    }
}