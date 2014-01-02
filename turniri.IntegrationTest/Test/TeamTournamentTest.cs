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
    public class TeamTournamentTest : BaseTournament
    {
        protected override int GenerateTournament(int playersCount, int CountRound, Game game, Tournament.TournamentTypeEnum type)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.Login("chernikov");

            var controller = DependencyResolver.Current.GetService<turniri.Areas.Admin.Controllers.TournamentController>();
            var countBefore = repository.Tournaments.Count();
            var httpContext = new MockHttpContext().Object;

            var route = new RouteData();

            route.Values.Add("controller", "Tournament");
            route.Values.Add("action", "Edit");
            route.Values.Add("area", "Admin");

            ControllerContext context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            var forum = repository.Forums.Where(p => !p.IsEnd && p.Forums.Count() == p.Forums.Count(r => r.IsEnd)).OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            //Generate participants for game
            Console.WriteLine("Game = " + game.Name);
            var gameID = game.ID;
            foreach (var user in repository.RegularUsers.Take(playersCount))
            {
                if (!user.HasGame(game.ID))
                {
                    var userGame = new UserGame()
                    {
                        GameID = gameID,
                        UserID = user.ID
                    };
                    repository.CreateUserGame(userGame);
                }
            }

            game = repository.Games.FirstOrDefault(p => p.ID == gameID);

            var participants = game.UserGames.Select(p => p.User);
            Console.WriteLine("Participants = " + participants.Count().ToString());

            var rand = new Random((int)DateTime.Now.Ticks);

            var name = game.Name + " Tournament " + (rand.Next(10) + 10).ToString();

            Console.WriteLine("Name = " + name);
            var tournamentView = new TournamentView()
            {
                ID = 0,
                Status = (int)Tournament.StatusEnum.Created,
                GameID = game.ID,
                PlatformID = game.Platform.ID,
                Name = name,
                ImagePath = "/Media/files/tournaments/" + StringExtension.GenerateNewFile() + ".jpg",
                ForumID = forum.ID,
                ForumName = name,
                TournamentType = (int)type,
                PlayersCount = playersCount,
                MinLevel = null,
                MaxLevel = null,
                TournamentCondition = new TournamentConditionView(),
                OpenRegistrationDate = DateTime.Now,
                CloseRegistrationDate = DateTime.Now.AddDays(1),
                BeginDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(3),
                CountRound = CountRound,
                HostGuest = true,
                Toss = true,
                IsTeam = true,
                TeamCount = 4,
                IsRoundForPoints = (Game.GameTypeEnum)game.GameType == Game.GameTypeEnum.Points,
                SingleWinPoint = 100,
                HotReplacement = rand.Next(3) + 1,
                Rules = "<p>Rules</p>",
                Players = participants.Take(playersCount).Select(p => p.ID).ToList()
            };

            if (type == Tournament.TournamentTypeEnum.GroupTournament)
            {
                tournamentView.GroupCount = 8;
                tournamentView.PlayersInGroup = 4;
                tournamentView.ExitFromGroup = 2;
            }

            Validator.ValidateObject<TournamentView>(tournamentView);
            controller.Edit(tournamentView);

            var countAfter = repository.Tournaments.Count();

            Assert.AreEqual(countBefore + 1, countAfter);

            var tournament = repository.Tournaments.FirstOrDefault(p => p.Name == name);

            Assert.NotNull(tournament);

            var tournamentParticipants = repository.Participants.Where(p => p.TournamentID == tournament.ID);
            var countParticipants = tournamentParticipants.Count();

            /*   Assert.AreEqual(playersCount, countParticipants);*/

            return tournament.ID;
        }

        protected int GenerateTournament(int playersCount, Tournament.TournamentTypeEnum type)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var game = repository.Games.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            return base.GenerateTournament(playersCount, 3, game, type);
        }

        protected virtual void GenerateTeams(int tournamentID, int countMax = 0)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var tournament = repository.Tournaments.FirstOrDefault(p => p.ID == tournamentID);
             if (countMax == 0)
            {
                countMax = tournament.TeamCount;
            }
            var usersCount = tournament.MaxPlayersCount.Value * tournament.TeamCount;
            foreach (var user in repository.RegularUsers.Take(usersCount))
            {
                if (!user.HasGame(tournament.GameID))
                {
                    var userGame = new UserGame()
                    {
                        GameID = tournament.GameID,
                        UserID = user.ID
                    };
                    repository.CreateUserGame(userGame);
                }
            }

            var participantUsers = repository.RegularUsers.Where(p => p.UserGames.Any(r => r.GameID == tournament.GameID)).ToList();

            if (tournament != null)
            {
                foreach (var player in tournament.Participants)
                {
                    var team = new Model.Team()
                    {
                        Name = GenerateData.Team.GetRandom(),
                        ImagePath18 = "/Media/images/default_avatar_18.png",
                        ImagePath26 = "/Media/images/default_avatar_26.png",
                        ImagePath30 = "/Media/images/default_avatar_30.png",
                    };
                    repository.CreateTeam(team, tournament.HotReplacement);

                    Console.WriteLine("Team : " + team.Name + " for " + tournament.Name);
                    var userTeam = new UserTeam()
                    {
                        TeamID = team.ID,
                        UserID = player.UserID,
                        IsCaptain = true,
                        Accepted = true
                    };
                    repository.CreateUserTeam(userTeam);
                    Console.WriteLine("User " + player.User.Login + " is Captain");

                    repository.SetTeamInParticipant(player.UserID, player.TournamentID.Value, team.ID);

                    int count = 0;
                    do
                    {
                        var user = participantUsers.OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                        if (user != null)
                        {
                            if (!user.AlreadyGetPartInTeam(tournament))
                            {
                                var partUserTeam = new UserTeam()
                                {
                                    TeamID = team.ID,
                                    UserID = user.ID,
                                    Accepted = true,
                                    IsCaptain = false,
                                };
                                repository.CreateUserTeam(partUserTeam);
                                Console.WriteLine("User " + user.Login + " registered to " + team.Name);
                            }
                        }
                        count = repository.UserTeams.Count(p => p.TeamID == team.ID);
                    } while (count < countMax);

                    if (team.AcceptedCount == tournament.TeamCount)
                    {
                        repository.CloseTeam(team.ID);
                    }
                }
            }
        }

        [Test]
        public void CreateSingleEliminationTournament_CreateNormalTournament_CountPlusOne()
        {
            int participantCount = 8;
            int tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.SingleElimination);
            GenerateTeams(tournamentID, 3);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
           /* PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
            CheckAllRatings();*/
            participantCount = 16;
            tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.DoubleElimination);
            GenerateTeams(tournamentID);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
           /* PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
            CheckAllRatings();*/
            participantCount = 32;
            tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.GroupTournament);
            GenerateTeams(tournamentID);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
           PlayAll(tournamentID, false);
            AllocatePlayoffTournament(tournamentID);
            StartTournament(tournamentID);
            /*  PlayAll(tournamentID, false);
             FinishTournament(tournamentID);
             CheckAllRatings();*/
            participantCount = 16;
            tournamentID = GenerateTournament(participantCount, Tournament.TournamentTypeEnum.RoundRobin);
            GenerateTeams(tournamentID);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
           /* PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
            CheckAllRatings();*/
        }
    }
}
