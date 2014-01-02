using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using turniri.Global.Auth;
using turniri.IntegrationTest.Tools;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;
using turniri.UnitTest.Mock.Http;
using turniri.UnitTest.Tools;
using NLipsum;
using NLipsum.Core;

namespace turniri.IntegrationTest
{
    public abstract class BaseGroupTest : BaseTournament
    {
        public int CreateGroup(int userID, int membersCount, Game game)
        {
            var repository = GetRepository();
            var imaginarium = GetImaginarium();
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.Login("chernikov");

            var controller = DependencyResolver.Current.GetService<turniri.Areas.Admin.Controllers.GroupController>();

            var httpContext = new MockHttpContext().Object;

            var route = new RouteData();

            route.Values.Add("controller", "Group");
            route.Values.Add("action", "Edit");
            route.Values.Add("area", "Admin");

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            //Generate participants for game
            Console.WriteLine("Game = " + game.Name);
            var users = repository.RegularUsers.Where(p => 
                !p.UserGroups.Any(r => r.Group.GameID == game.ID)).ToList().OrderBy(p => Guid.NewGuid()).Take(membersCount);

            var rand = new Random((int)DateTime.Now.Ticks);

            var name = GenerateData.Team.GetRandom();
            var url = Translit.Translate(name);

            Console.WriteLine("Group = " + name);

            var file = Imaginarium.GetRandomSourceImage();

            using (var fs = new FileStream(file, FileMode.Open))
            {
                var groupView = new AdminGroupView()
                {
                    ID = 0,
                    GameID = game.ID,
                    UserID = userID,
                    Name = name,
                    Url = url,
                    Description = LipsumGenerator.Generate(2),
                    LogoPath173 = imaginarium.MakePreview(fs, "/Media/files/groups/", "Group173Size"),
                    LogoPath96 = imaginarium.MakePreview(fs, "/Media/files/groups/", "Group96Size"),
                    LogoPath84 = imaginarium.MakePreview(fs, "/Media/files/groups/", "Group84Size"),
                    LogoPath57 = imaginarium.MakePreview(fs, "/Media/files/groups/", "Group57Size"),
                    LogoPath30 = imaginarium.MakePreview(fs, "/Media/files/groups/", "Group30Size"),
                    LogoPath26 = imaginarium.MakePreview(fs, "/Media/files/groups/", "Group26Size"),
                    LogoPath18 = imaginarium.MakePreview(fs, "/Media/files/groups/", "Group18Size"),
                };

                Validator.ValidateObject<AdminGroupView>(groupView);
                controller.Edit(groupView);

                var group = repository.Groups.OrderByDescending(p => p.ID).FirstOrDefault();

                groupView = (AdminGroupView)controller.ModelMapper.Map(group, typeof(Group), typeof(AdminGroupView));
                
                foreach (var user in users)
                {
                    controller.AddPlayer(new UserGroupView()
                    {
                        GroupID = groupView.ID,
                        UserID = user.ID
                    });
                }

                users = repository.Users.Where(p => p.UserGroups.Any(r => r.GroupID == group.ID)).ToList();

                var captain = users.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                if (captain != null)
                {
                    var userRole = new UserRole()
                    {
                        RoleID = 6 /* captain */,
                        UserID = captain.ID
                    };

                    repository.CreateUserRole(userRole);

                    var userRoleGroup = new UserRoleGroup()
                    {
                        GroupID = groupView.ID,
                        UserRoleID = userRole.ID
                    };

                    repository.CreateUserRoleGroup(userRoleGroup);
                }

                return group.ID;
            }
        }

        protected static IRepository GetRepository()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            return repository;
        }


        protected static Imaginarium GetImaginarium()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var imaginarium = kernel.Get<Imaginarium>();
            return imaginarium;
        }

        public void Activate(int groupID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            repository.AcceptGroup(groupID);
        }

    }
}
