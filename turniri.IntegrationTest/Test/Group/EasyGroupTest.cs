using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using turniri.Global.Auth;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;
using turniri.UnitTest.Mock.Http;
using turniri.UnitTest.Tools;

namespace turniri.IntegrationTest
{
    [TestFixture]
    public class EasyGroupTest : BaseGroupTest
    {
       
        [Test]
        public void Create55GroupsDota2()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var game = repository.Games.FirstOrDefault(p => p.Name == "DOTA 2");

            var users = game.UserGames.Select(p => p.User).ToList();

            var rand = new Random((int)DateTime.Now.Ticks);

            foreach(var user in users.OrderBy(p => Guid.NewGuid()).Take(55)) 
            {
                int members = rand.Next(4) + 6;

                var groupID = CreateGroup(user.ID, members, game);
                Activate(groupID);
            }
        }

        
    }
}
