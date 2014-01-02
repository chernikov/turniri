using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Forum> Forums
        {
            get
            {
                return Db.Forums;
            }
        }

        public bool CreateForum(Forum instance)
        {
            if (instance.ID == 0)
            {
                var lastOrderBy =
                    instance.ParentID.HasValue
                    ? Forums.Where(p => p.ParentID == instance.ParentID).OrderByDescending(p => p.OrderBy).Select(p => p.OrderBy).FirstOrDefault()
                    : Forums.Where(p => !p.ParentID.HasValue).OrderByDescending(p => p.OrderBy).Select(p => p.OrderBy).FirstOrDefault();
                instance.OrderBy = lastOrderBy + 1;
                instance.AddedDate = DateTime.Now;
                instance.Url = Translit.WithPredicateTranslate(instance.Name);
                Db.Forums.InsertOnSubmit(instance);
                Db.Forums.Context.SubmitChanges();

                MoveForum(instance.ID, 1);
                return true;
            }

            return false;
        }

        public bool UpdateForum(Forum instance)
        {
            var cache = Db.Forums.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Name = instance.Name;
                cache.Url = instance.Url;
                cache.ImagePath = instance.ImagePath;
                cache.SubTitle = instance.SubTitle;
                cache.VisitCount = instance.VisitCount;
                cache.PollID = instance.PollID;
                Db.Forums.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveForum(int idForum)
        {
            Forum instance = Db.Forums.FirstOrDefault(p => p.ID == idForum);
            if (instance != null)
            {
                if (instance.ParentID.HasValue)
                {
                    var parentForum = Forums.FirstOrDefault(p => p.ID == instance.ParentID);
                    if (parentForum != null)
                    {
                        foreach (var forForum in Forums.Where(p => p.ParentID == parentForum.ID && p.OrderBy > instance.OrderBy))
                        {
                            forForum.OrderBy--;
                        }
                    }
                }
                else
                {
                    foreach (var forForum in Forums.Where(p => p.ParentID == null && p.OrderBy > instance.OrderBy))
                    {
                        forForum.OrderBy--;
                    }
                }
                if (instance.Forums.Any())
                {
                    var subForums = instance.Forums.ToList();
                    foreach (var subForum in subForums)
                    {
                        RemoveForum(subForum.ID);
                    }
                }

                var list = Db.Notices.Where(p => p.ForumID == instance.ID).ToList();

                Db.Notices.DeleteAllOnSubmit(list);
                Db.Notices.Context.SubmitChanges();

                Db.Forums.DeleteOnSubmit(instance);
                Db.Forums.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        private bool UpdateLastVisitForum(int idForum)
        {
            var instance = Db.Forums.FirstOrDefault(p => p.ID == idForum);
            if (instance != null)
            {
                instance.LastUpdate = DateTime.Now;
                Db.Forums.Context.SubmitChanges();

                if (instance.ParentID != null)
                {
                    UpdateLastVisitForum(instance.ParentID.Value);
                }
                return true;
            }
            return false;
        }

        public bool UpdateVisitForum(int idForum)
        {
            var instance = Db.Forums.FirstOrDefault(p => p.ID == idForum);
            if (instance != null)
            {
                instance.VisitCount++;
                Db.Forums.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateVisitForumNotice(int idForum, int idUser)
        {
            var instance = Db.Notices.FirstOrDefault(p => p.ForumID == idForum && p.ReceiverID == idUser);
            if (instance != null)
            {
                instance.ReadedDate = DateTime.Now;
                Db.Forums.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ChangeParentForum(int id, int idParent)
        {
            var forum = Db.Forums.FirstOrDefault(p => p.ID == id);

            if (forum == null)
            {
                //нету такого - всё пропало
                return false;
            }
            if (idParent == 0)
            {
                //пересортировка в бывшем списке
                ReOrderBeforeMove(forum);
                int lastOrderBy = Forums.Where(p => p.ParentID == null).OrderByDescending(p => p.OrderBy).Select(p => p.OrderBy).FirstOrDefault();
                forum.OrderBy = lastOrderBy + 1;
                //уииии
                forum.ParentID = null;
                Db.Forums.Context.SubmitChanges();
                return true;
            }
            var newParentForum = Db.Forums.FirstOrDefault(p => p.ID == idParent);
            if (newParentForum == null)
            {
                //нету такого - всё пропало
                return false;
            }
            else
            {
                if (forum.ParentID == idParent)
                {
                    //никуда не перемещаем 
                    return true;
                }
                else
                {
                    //пересортировка в бывшем списке
                    ReOrderBeforeMove(forum);
                    //добавить последним 
                    int lastOrderBy = newParentForum.Forums.OrderByDescending(p => p.OrderBy).Select(p => p.OrderBy).FirstOrDefault();
                    forum.OrderBy = lastOrderBy + 1;
                    forum.Forum1 = newParentForum;
                    //уииии
                    Db.Forums.Context.SubmitChanges();
                    return true;
                }
            }
        }

        private void ReOrderBeforeMove(Forum forum)
        {
            var parentForum = forum.ParentForum;
            if (parentForum != null)
            {
                foreach (var forums in parentForum.Forums.Where(w => w.OrderBy > forum.OrderBy))
                {
                    forums.OrderBy--;
                }
            }
            else
            {
                foreach (var forums in Forums.Where(w => w.OrderBy > forum.OrderBy))
                {
                    forums.OrderBy--;
                }
            }
        }

        public bool MoveForum(int id, int placeBefore)
        {
            var forum = Db.Forums.FirstOrDefault(p => p.ID == id);
            if (forum != null)
            {
                if (forum.OrderBy > placeBefore)
                {
                    foreach (var forForum in Forums.Where(w => w.OrderBy >= placeBefore && w.OrderBy < forum.OrderBy && ((w.ParentID == forum.ParentID) || (w.ParentID == null && forum.ParentID == null))))
                    {
                        forForum.OrderBy++;
                    }
                }

                if (forum.OrderBy < placeBefore)
                {
                    foreach (var forCatalog in Forums.Where(w => w.OrderBy > forum.OrderBy && w.OrderBy <= placeBefore && ((w.ParentID == forum.ParentID) || (w.ParentID == null && forum.ParentID == null))))
                    {
                        forCatalog.OrderBy--;
                    }
                }
                forum.OrderBy = placeBefore;
                Db.Forums.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool SetOrderForum(Forum instance)
        {
            var cache = Db.Forums.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.OrderBy = instance.OrderBy;
                Db.Forums.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool SealForum(int idForum)
        {
            var instance = Db.Forums.FirstOrDefault(p => p.ID == idForum);
            if (instance != null)
            {
                instance.IsEnd = true;
                Db.Forums.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}