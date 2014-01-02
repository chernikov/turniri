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


namespace turniri.IntegrationTest
{
    [TestFixture]
    public class Dota2TournamentTest : BaseGroupTest
    {
        private void CreateGroupsDota2(int count = 10)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var game = repository.Games.FirstOrDefault(p => p.Name == "DOTA 2");

            var users = game.UserGames.Select(p => p.User).ToList();

            var rand = new Random((int)DateTime.Now.Ticks);

            foreach (var user in users.OrderBy(p => Guid.NewGuid()).Take(count))
            {
                var groupID = CreateGroup(user.ID, 8, game);
                Activate(groupID);
            }
        }

        [Test]
        public void CreateSingleEliminationTournament_CreateNormalTournament_CountPlusOne()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var game = repository.Games.FirstOrDefault(p => p.Name == "DOTA 2");

            CreateGroupsDota2(20);

            int participantCount = 16;
            int tournamentID = GenerateTournament(participantCount, 1, game, Tournament.TournamentTypeEnum.SingleElimination);
            GenerateTeams(tournamentID);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
            participantCount = 16;
            tournamentID = GenerateTournament(participantCount, 1, game, Tournament.TournamentTypeEnum.DoubleElimination);
            GenerateTeams(tournamentID);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
            participantCount = 32;
            tournamentID = GenerateTournament(participantCount, 1, game, Tournament.TournamentTypeEnum.GroupTournament);
            GenerateTeams(tournamentID);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
            AllocatePlayoffTournament(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
            participantCount = 16;
            tournamentID = GenerateTournament(participantCount, 1, game, Tournament.TournamentTypeEnum.RoundRobin);
            GenerateTeams(tournamentID);
            CreateMatches(tournamentID);
            CreateAwards(tournamentID);
            StartTournament(tournamentID);
            PlayAll(tournamentID, false);
            FinishTournament(tournamentID);
        }

        [Test]
        public void TestReplaceGroups()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var game = repository.Games.FirstOrDefault(p => p.Name == "DOTA 2");

            CreateGroupsDota2(20);

            for (int i = 0; i < 100; i++)
            {
                ExitAndEnterGroup(game.ID);
            }
        }

        protected override int GenerateTournament(int playersCount, int CountRound, Game game, Tournament.TournamentTypeEnum type)
        {
            var imaginarium = GetImaginarium();

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
            var rand = new Random((int)DateTime.Now.Ticks);

            var name = game.Name + " Tournament " + (rand.Next(10) + 10).ToString();

            var file = Imaginarium.GetRandomSourceImage();

            Console.WriteLine("Name = " + name);

            using (var fs = new FileStream(file, FileMode.Open))
            {
                var tournamentView = new TournamentView()
                {
                    ID = 0,
                    Status = (int)Tournament.StatusEnum.Created,
                    GameID = game.ID,
                    PlatformID = game.Platform.ID,
                    Name = name,
                    ImagePath = imaginarium.MakePreview(fs, "/Media/files/tournaments/", "TournamentImageSize"),
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
                    IsGroup = true,
                    TeamCount = 5,
                    HotReplacement = 3,
                    IsRoundForPoints = (Game.GameTypeEnum)game.GameType == Game.GameTypeEnum.Points,
                    SingleWinPoint = 10,
                    Rules = "<p>Rules</p>"
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
                return tournament.ID;
            }
        }

        protected virtual void GenerateTeams(int tournamentID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var tournament = repository.Tournaments.FirstOrDefault(p => p.ID == tournamentID);
            if (tournament != null)
            {
                var countMax = tournament.TeamCount;
                var groups = repository.Groups.Where(p => p.GameID == tournament.GameID)
                    .ToList()
                    .OrderBy(p => Guid.NewGuid())
                    .Take(tournament.MaxPlayersCount.Value);

                foreach (var group in groups)
                {
                    var captain = group.Captains.FirstOrDefault();
                    if (captain != null)
                    {
                        var team = new Team()
                        {
                            Name = group.Name,
                            ImagePath18 = group.FullLogoPath18,
                            ImagePath26 = group.FullLogoPath26,
                            ImagePath30 = group.FullLogoPath30
                        };

                        repository.CreateTeam(team, tournament.HotReplacement);

                        var participant = new Participant
                        {
                            TeamID = team.ID,
                            UserID = captain.ID,
                            TournamentID = tournament.ID
                        };
                        repository.CreateParticipant(participant);

                        var userTeam = new UserTeam()
                        {
                            TeamID = team.ID,
                            UserID = captain.ID,
                            IsCaptain = true,
                            Accepted = true
                        };
                        repository.CreateUserTeam(userTeam);

                        var max = countMax - 1;

                        foreach (var otherUser in group.UserGroups.Where(p => p.Status == (int)UserGroup.StatusEnum.Granded).Take(countMax))
                        {
                            if (otherUser.UserID != captain.ID && max > 0)
                            {
                                var otherUserTeam = new UserTeam()
                                {
                                    TeamID = team.ID,
                                    UserID = otherUser.UserID,
                                    IsCaptain = false,
                                    Accepted = true
                                };
                                repository.CreateUserTeam(otherUserTeam);
                                max--;
                            }
                        }
                        repository.CloseTeam(team.ID);
                    }
                }
            }
        }

        protected virtual void PlayAll(int tournamentID, bool enableTech)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            while (PlayRound(tournamentID, rand, enableTech))
            {
                ReplaceUserInTeam(tournamentID);
                ReplaceUserInTeam(tournamentID);
                ReplaceUserInTeam(tournamentID);
                ReplaceUserInTeam(tournamentID);
            }
        }

        protected virtual void ReplaceUserInTeam(int idTournament)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var tournament = repository.Tournaments.FirstOrDefault(p => p.ID == idTournament);

            if (tournament != null)
            {
                var team = tournament.Participants.Select(p => p.Team).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                if (team.HotReplacement != 0)
                {
                    //жертва
                    var userTeam = team.UserTeams.Where(p => !p.IsCaptain).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                    var captain = team.Captain;

                    var group = captain.GroupByGame(tournament.GameID);

                    //на поле 
                    foreach (var bigMen in group.UserGroups.Where(p => p.Status == (int)UserGroup.StatusEnum.Granded).Select(p => p.User))
                    {
                        if (!bigMen.IsParticipantInTournament(tournament.ID))
                        {
                            Console.WriteLine(string.Format("{0} на банку!!! {1} на поле!", userTeam.User.Login, bigMen.Login));
                            repository.ReplaceUser(userTeam.ID, bigMen.ID);
                            break;
                        }
                    }
                }
            }
        }


        protected virtual void ExitAndEnterGroup(int gameID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");


            var group1 = repository.Groups.Where(p => p.GameID == gameID).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

            var group2 = repository.Groups.Where(p => p.GameID == gameID && p.ID != group1.ID).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();


            var userGroup = group1.UserGroups.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

            var user = userGroup.User;
            repository.RemoveUserGroup(userGroup.ID);

            var userGroupNew = new UserGroup()
            {
                GroupID = group2.ID,
                UserID = user.ID,
                Status = (int)UserGroup.StatusEnum.Granded
            };

            repository.CreateUserGroup(userGroupNew);
        }
    }
}
