using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using NUnit.Framework;
using turniri.Global.Auth;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;
using turniri.UnitTest.Mock.Http;
using turniri.UnitTest.Tools;

namespace turniri.IntegrationTest
{
    [TestFixture]
    public class CreateEasyTournamentTest : BaseTournament
    {
        [Test]
        public void CreateSingleEliminationTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 7;

            var tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.SingleElimination);
            CreateMatches(tournamentID);
        }

        [Test]
        public void CreateDoubleTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 16;
            var tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.DoubleElimination);
            CreateMatches(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
            CheckAllRatings();
        }

        [Test]
        public void CreateDoubleTournament_PlayNotAll_CountPlusOne()
        {
            int participantCount = 16;
            var tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.DoubleElimination);
            CreateMatches(tournamentID);
            StartTournament(tournamentID);
            PlayCount(tournamentID, 16);
        }


        [Test]
        public void CreateGroupTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 32;
            var tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.GroupTournament);
            CreateMatches(tournamentID);
            StartTournament(tournamentID);
            PlayCount(tournamentID, 6 * 8);
        }

        [Test]
        public void CreateRoundRobinTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 16;
            var tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.RoundRobin);
            CreateMatches(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, true);
            FinishTournament(tournamentID);
            CheckAllRatings();
        }


        [Test]
        public void CreateDoubleEliminationTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 9;

            var tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.SingleElimination);
            CreateMatches(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID);
            FinishTournament(tournamentID);
            CheckAllRatings();
        }
    }
}
