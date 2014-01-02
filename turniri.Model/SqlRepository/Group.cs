using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Group> Groups
        {
            get
            {
                return Db.Groups;
            }
        }

        public bool CreateGroup(Group instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                instance.State = (int)Group.StateType.Registered;
                Db.Groups.InsertOnSubmit(instance);
                Db.Groups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateGroup(Group instance)
        {
            var cache = Db.Groups.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Name = instance.Name;
                cache.Url = instance.Url;
                cache.GameID = instance.GameID;
				cache.LogoPath173 = instance.LogoPath173;
				cache.LogoPath96 = instance.LogoPath96;
				cache.LogoPath84 = instance.LogoPath84;
				cache.LogoPath57 = instance.LogoPath57;
                cache.LogoPath30 = instance.LogoPath30;
				cache.LogoPath26 = instance.LogoPath26;
                cache.LogoPath18 = instance.LogoPath18;
                cache.Description = instance.Description;
				Db.Groups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveGroup(int idGroup)
        {
            var instance = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            if (instance != null)
            {
                instance.State = (int)Group.StateType.Removed;
                Db.Groups.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool PurgeGroup(int idGroup)
        {
            var instance = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            if (instance != null)
            {
                var userGroupHistories = Db.UserGroupHistories.Where(p => p.GroupID == idGroup).ToList();
                Db.UserGroupHistories.DeleteAllOnSubmit(userGroupHistories);
                Db.UserGroupHistories.Context.SubmitChanges();

                Db.Groups.DeleteOnSubmit(instance);
                Db.Groups.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool AcceptGroup(int idGroup)
        {
            var instance = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            if (instance != null)
            {
                instance.State = (int)Group.StateType.Live;
                Db.Groups.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ChangeGroupLeader(int idGroup, int idUser)
        {
            var instance = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            var user = Db.Users.FirstOrDefault(p => p.ID == idUser);
            if (instance != null && user != null)
            {
                instance.UserID = user.ID;
                Db.Groups.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool VisitGroup(int idGroup)
        {
            var instance = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            if (instance != null)
            {
                try
                {
                    instance.VisitCount++;
                    Db.Groups.Context.SubmitChanges();
                    return true;
                }
                catch { }
            }
            return false;
        }

        public bool UpdateGroupMembers(int idGroup, List<int> members)
        {
            var group = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            if (group != null)
            {

                //remove others
                var listForDelete = group.UserGroups.Where(p => !members.Contains(p.UserID));
                var existList = group.UserGroups.Where(p => members.Contains(p.UserID)).Select(p => p.UserID).ToList();
                Db.UserGroups.DeleteAllOnSubmit(listForDelete);
                Db.UserGroups.Context.SubmitChanges();
                //new list
                var newUsers = members.Where(p => !existList.Contains(p)).Select(p => p);
                foreach (var id in newUsers)
                {
                    var user = Db.Users.FirstOrDefault(p => p.ID == id);

                    if (user != null)
                    {
                        var userGroup = new UserGroup
                        {
                            GroupID = group.ID,
                            UserID = user.ID,
                            AddedDate = DateTime.Now
                        };
                        Db.UserGroups.InsertOnSubmit(userGroup);
                    }
                    Db.UserGroups.Context.SubmitChanges();
                }
                return true;
            }
            return false;
        }

        private bool UpdateGroupRating(int idGroup)
        {
            var group = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            if (group != null)
            {
                group.TotalRating = Db.GroupRatings.Where(p => p.GroupID == group.ID).Sum(p => p.Score);
                group.Level = User.LevelByPoints(group.TotalRating);
                Db.Groups.Context.SubmitChanges();
                return true;
            }

            return false;
        }
    }
}