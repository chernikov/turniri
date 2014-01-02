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

namespace turniri.IntegrationTest.Test
{
    /// <summary>
    /// Только типа футбол
    /// </summary>
    [TestFixture]
    public class RoundRobinSoccerTournaments : SoccerTournaments
    {
        [Test]
        public void CreateRoundRobinTournament_CreateSingleTournament_CountPlusOne()
        {
            int participantCount = 16;
            int tournamentID = GenerateTournament(participantCount, 3, Tournament.TournamentTypeEnum.SingleElimination);
            CreateAwards(tournamentID);
            CreateMatches(tournamentID);
            CreateAvatars(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID);
        }

        [Test]
        public void CreateRoundRobinTournament_CreateSingleTournament_CountPlusOne2()
        {
            int participantCount = 9;
            int tournamentID = GenerateTournament(participantCount, 1, Tournament.TournamentTypeEnum.SingleElimination);
            CreateAwards(tournamentID);
            CreateMatches(tournamentID);
            CreateAvatars(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID);
        }
        [Test]
        public void CreateRoundRobinTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 20;
            int tournamentID = GenerateTournament(participantCount, 3, Tournament.TournamentTypeEnum.RoundRobin);
            CreateAwards(tournamentID);
            CreateMatches(tournamentID);
            CreateAvatars(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID);

            CheckAllRatings();
        }

        [Test]
        public void CreateGroupTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 32;
            int tournamentID = GenerateTournament(participantCount, 3, Tournament.TournamentTypeEnum.GroupTournament);
            CreateAwards(tournamentID);
            CreateMatches(tournamentID);
            CreateAvatars(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
            AllocatePlayoffTournament(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
        }
        
        protected override void PlayAll(int tournamentID, bool enableTech)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            while (PlayRound(tournamentID, rand, enableTech, 4)) { }
        }
    }
}
