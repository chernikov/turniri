using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.IntegrationTest.Test
{
    [TestFixture]
    public class SoccerTournaments : BaseTournament
    {
        protected int GenerateTournament(int participantCount, int countRound, Tournament.TournamentTypeEnum type)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var game = repository.Games.Where(p => p.GameCategory == (int)Game.GameCategoryEnum.Soccer).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            return GenerateTournament(participantCount, countRound, game, type);
        }
    }
}
