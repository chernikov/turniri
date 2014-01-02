using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserGroup> UserGroups
        {
            get
            {
                return Db.UserGroups;
            }
        }

        public bool CreateUserGroup(UserGroup instance)
        {
            if (instance.ID == 0)
            {
                var exist = Db.UserGroups.Any(p => p.UserID == instance.UserID && p.GroupID == instance.GroupID);
                if (!exist)
                {
                    instance.AddedDate = DateTime.Now;
                    Db.UserGroups.InsertOnSubmit(instance);
                    Db.UserGroups.Context.SubmitChanges();
                    if (instance.Status == (int)UserGroup.StatusEnum.Granded)
                    {
                        //записать в историю трансферов
                        var userGroupHistory = new UserGroupHistory
                        {
                            GroupID = instance.GroupID,
                            UserID = instance.UserID
                        };
                        CreateUserGroupHistory(userGroupHistory);
                    }

                    // если не играет - то добавить игру
                    var anyGame = Db.UserGames.Any(p => p.UserID == instance.UserID && p.GameID == instance.Group.GameID);

                    if (!anyGame)
                    {
                        var userGame = new UserGame() 
                        {
                            GameID = instance.Group.GameID,
                            UserID = instance.UserID
                        };
                        CreateUserGame(userGame);
                    }
                    return true;
                }
            }

            return false;
        }

        public bool RemoveUserGroup(int idUserGroup)
        {
            var instance = Db.UserGroups.FirstOrDefault(p => p.ID == idUserGroup);
            if (instance != null)
            {
                var userGroupHistory = Db.UserGroupHistories.FirstOrDefault(p => p.UserID == instance.UserID
                    && p.GroupID == instance.GroupID
                    && !p.ExitDate.HasValue);
                if (userGroupHistory != null)
                {
                    ExitUserGroupHistory(userGroupHistory);
                }
                Db.UserGroups.DeleteOnSubmit(instance);
                Db.UserGroups.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool AcceptUserGroup(int idUserGroup)
        {
            var instance = Db.UserGroups.FirstOrDefault(p => p.ID == idUserGroup);
            if (instance != null)
            {
                instance.Status = (int)UserGroup.StatusEnum.Granded;

                Db.UserGroups.Context.SubmitChanges();
                //записать в историю трансферов
                var userGroupHistory = new UserGroupHistory
                {
                    GroupID = instance.GroupID,
                    UserID = instance.UserID
                };
                CreateUserGroupHistory(userGroupHistory);


                return true;
            }
            return false;
        }
    }
}