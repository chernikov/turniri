using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<UserRoleGroup> UserRoleGroups
        {
            get
            {
                return Db.UserRoleGroups;
            }
        }

        public bool CreateUserRoleGroup(UserRoleGroup instance)
        {
            if (instance.ID == 0)
            {
                Db.UserRoleGroups.InsertOnSubmit(instance);
                Db.UserRoleGroups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserRoleGroup(int idUserRoleGroup)
        {
            var instance = Db.UserRoleGroups.FirstOrDefault(p => p.ID == idUserRoleGroup);
            if (instance != null)
            {
                Db.UserRoleGroups.DeleteOnSubmit(instance);
                Db.UserRoleGroups.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool SwitchGroupRole(int idUser, int idGroup, int idRole)
        {
            var instance = Db.UserRoleGroups.FirstOrDefault(p => p.GroupID == idGroup 
                && p.UserRole.UserID == idUser && p.UserRole.RoleID == idRole);

            if (instance != null)
            {
                RemoveUserRole(instance.UserRole.ID);
                return true;
            }
            else
            {
                var role = Db.Roles.FirstOrDefault(p => p.ID == idRole);
                var user = Db.Users.FirstOrDefault(p => p.ID == idUser);
                var group = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
                if (role != null && user != null && group != null)
                {
                    var userRole = new UserRole
                    {
                        RoleID = idRole,
                        UserID = idUser
                    };
                    CreateUserRole(userRole);
                    
                    var userRoleGroup = new UserRoleGroup
                    {
                        UserRoleID = userRole.ID,
                        GroupID = idGroup,
                    };
                    CreateUserRoleGroup(userRoleGroup);
                    return true;
                }
            }
            return false;
        }
    }
}