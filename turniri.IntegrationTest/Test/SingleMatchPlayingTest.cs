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
using GenerateData;

namespace turniri.IntegrationTest.Test
{
    [TestFixture]
    public class SingleMatchPlayingTest
    {
        [Test]
        public void CreateMatchByFightMessage_WriteFightMatch_CreateSingleMatch()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.Login("chernikov");

            var controller = DependencyResolver.Current.GetService<turniri.Areas.Default.Controllers.MessageController>();
            var countBefore = repository.Matches.Count();
            var httpContext = new MockHttpContext().Object;

            var route = new RouteData();

            route.Values.Add("controller", "Message");
            route.Values.Add("action", "WriteFightMessage");
            route.Values.Add("area", "Default");

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            var platform = repository.Platforms.ToList().OrderBy(p => Guid.NewGuid()).First();
            var me = controller.CurrentUser;
            var rival = repository.RegularUsers.ToList().OrderBy(p => Guid.NewGuid()).First(p => p.ID != me.ID);
            var game = repository.Games.Where(p => p.PlatformID == platform.ID).ToList().OrderBy(p => Guid.NewGuid()).First();

            var fightMessageView = new FightMessageView()
            {
                ID = 0,
                PlatformID = platform.ID,
                SenderID = me.ID,
                ReceiverID = rival.ID,
                ReceiverLogin = rival.Login,
                GameID = game.ID,
                CountRound = 1,
                Text = "Test"
            };
            controller.WriteFightMessage(fightMessageView);
        }

    }
}
