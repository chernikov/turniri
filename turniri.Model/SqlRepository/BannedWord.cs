using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<BannedWord> BannedWords
        {
            get
            {
                return Db.BannedWords;
            }
        }

        public bool CreateBannedWord(BannedWord instance)
        {
            if (instance.ID == 0)
            {
                Db.BannedWords.InsertOnSubmit(instance);
                Db.BannedWords.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateBannedWord(BannedWord instance)
        {
            var cache = Db.BannedWords.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Word = instance.Word;
				cache.IsCanBeSubWord = instance.IsCanBeSubWord;
                Db.BannedWords.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveBannedWord(int idBannedWord)
        {
            BannedWord instance = Db.BannedWords.FirstOrDefault(p => p.ID == idBannedWord);
            if (instance != null)
            {
                Db.BannedWords.DeleteOnSubmit(instance);
                Db.BannedWords.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}