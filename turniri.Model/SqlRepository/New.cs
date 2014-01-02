using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<New> News
        {
            get
            {
                return Db.News;
            }
        }

        public bool CreateNew(New instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.Url = Translit.WithPredicateTranslate(instance.Header);
                Db.News.InsertOnSubmit(instance);
                Db.News.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateNew(New instance)
        {
            var cache = Db.News.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.NewTypeID = instance.NewTypeID;
                cache.IsMain = instance.IsMain;
                cache.PreviewPath = instance.PreviewPath;
                cache.AvatarPreviewPath = instance.AvatarPreviewPath;
                cache.TitlePath = instance.TitlePath;
                cache.AvatarTitlePath = instance.AvatarTitlePath;
                cache.Header = instance.Header;
                cache.SubHeader = instance.SubHeader;
                cache.Text = instance.Text;
                cache.VisitCount = instance.VisitCount;
                Db.News.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveNew(int idNew)
        {
            New instance = Db.News.FirstOrDefault(p => p.ID == idNew);
            if (instance != null)
            {
                Db.News.DeleteOnSubmit(instance);
                Db.News.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public int ToggleNewLike(int idNew, int idUser)
        {
            var @new = Db.News.FirstOrDefault(p => p.ID == idNew);

            if (@new != null)
            {
                var newLike = Db.NewLikes.FirstOrDefault(p => p.NewID == idNew && p.UserID == idUser);

                if (newLike != null)
                {
                    Db.NewLikes.DeleteOnSubmit(newLike);

                }
                else
                {
                    var newNewLike = new NewLike
                    {
                        NewID = @new.ID,
                        UserID = idUser
                    };

                    Db.NewLikes.InsertOnSubmit(newNewLike);
                }
                Db.BlogLikes.Context.SubmitChanges();
                @new.Likes = @new.NewLikes.Count;

                Db.News.Context.SubmitChanges();
                return @new.Likes;
            }

            return 0;
        }

        public bool UpdateVisitNew(int idNew)
        {
            try
            {
                var cache = Db.News.FirstOrDefault(p => p.ID == idNew);
                if (cache != null)
                {
                    cache.VisitCount++;
                    Db.Photos.Context.SubmitChanges();
                    return true;
                }
            }
            catch { }
            return false;

        }

    }
}