using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using turniri.Model;

namespace turniri.UnitTest.Mock
{
    public partial class MockRepository
    {
        public List<User> Users { get; set; }

        public void GenerateUsers()
        {
            Users = new List<User>();

            var admin = new User()
            {
                ID = 1,
                Login = "admin",
                ActivatedDate = DateTime.Now,
                ActivatedLink = "",
                Email = "admin",
                FirstName = "",
                LastName = "",
                Password = "password",
                LastVisitDate = DateTime.Now,
            };

            var role = Roles.First(p => p.Code == "admin");
            var userRole = new UserRole()
            {
                User = admin,
                UserID = admin.ID,
                Role = role,
                RoleID = role.ID
            };

            admin.UserRoles = 
                new EntitySet<UserRole>() {
                    userRole
                };
            Users.Add(admin);

            Users.Add(new User()
            {
                ID = 2,
                Login = "chernikov",
                ActivatedDate = DateTime.Now,
                ActivatedLink = "",
                Email = "chernikov@gmail.com",
                FirstName = "Andrey",
                LastName = "Chernikov",
                Password = "password2",
                LastVisitDate = DateTime.Now
            });

            this.Setup(p => p.Users).Returns(Users.AsQueryable());
            this.Setup(p => p.GetUser(It.IsAny<string>())).Returns((string email) => 
                Users.FirstOrDefault(p => string.Compare(p.Email, email, 0) == 0));

            this.Setup(p => p.CreateUser(It.IsAny<User>())).Returns(null);
            this.Setup(p => p.RemoveUser(It.IsAny<int>())).Returns(null);
            this.Setup(p => p.UpdateUser(It.IsAny<User>())).Returns(null);
        }
    }
}
