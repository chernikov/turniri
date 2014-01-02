using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Subject> Subjects
        {
            get
            {
                return Db.Subjects;
            }
        }

        public bool CreateSubject(Subject instance)
        {
            if (instance.ID == 0)
            {
                Db.Subjects.InsertOnSubmit(instance);
                Db.Subjects.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSubject(Subject instance)
        {
            var cache = Db.Subjects.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                Db.Subjects.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveSubject(int idSubject)
        {
            Subject instance = Db.Subjects.FirstOrDefault(p => p.ID == idSubject);
            if (instance != null)
            {
                Db.Subjects.DeleteOnSubmit(instance);
                Db.Subjects.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}