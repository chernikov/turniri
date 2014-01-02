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
    public class MoneyTournament : BaseTournament
    {
        [Test]
        public void CreateUserTournament_CreateMoneyTournament_CountPlusOne()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            int participantCount = 16;
            var tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.SingleElimination, Tournament.MoneyTypeEnum.Gold, 100);

            var moneyTournament = new MoneyDetail()
            {
                TournamentID = tournamentID,
                SumGold = 3200,
                Description = "Взнос просто так",
                Submited = true
            };
            repository.CreateMoneyDetail(moneyTournament, Guid.NewGuid());
           
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
        }
    }
}
