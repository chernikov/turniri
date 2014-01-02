using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using turniri.Model;

namespace turniri.IntegrationTest
{
    [TestFixture]
    public class AllTechTournament : BaseTournament
    {
        [Test]
        public void CreateSingleTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 16;
            var tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.SingleElimination);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAllTech(tournamentID);
            FinishTournament(tournamentID);
        }
    }
}
