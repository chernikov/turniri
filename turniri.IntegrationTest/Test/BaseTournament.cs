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
using turniri.IntegrationTest.Tools;
using System.IO;

namespace turniri.IntegrationTest
{
    public abstract class BaseTournament
    {
        protected virtual void StartTournament(int tournamentID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.Login("chernikov");
            repository.StartTournament(tournamentID);
        }

        protected virtual void FinishTournament(int tournamentID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.Login("chernikov");
            repository.FinishTournament(tournamentID);
        }

        protected virtual void AllocatePlayoffTournament(int tournamentID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.Login("chernikov");
            repository.AllocatePlayoff(tournamentID);
        }
        protected virtual int GenerateTournament(int playersCount, int CountRound, Game game, Tournament.TournamentTypeEnum type)
        {
            return GenerateTournament(playersCount, CountRound, game, type);
        }

        protected virtual int GenerateTournament(int participantCount, Tournament.TournamentTypeEnum type, Tournament.MoneyTypeEnum moneyType = Tournament.MoneyTypeEnum.Free, double? fee = null)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var game = repository.Games.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            return GenerateTournament(participantCount, 1, game, type, moneyType, fee);
        }

        protected virtual int GenerateTournament(int playersCount, int CountRound, Game game, Tournament.TournamentTypeEnum type, Tournament.MoneyTypeEnum moneyType = Tournament.MoneyTypeEnum.Free, double? fee = null)
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

            var participants = game.UserGames.Select(p => p.User);

            Console.WriteLine("Participants = " + participants.Count().ToString());

            var rand = new Random((int)DateTime.Now.Ticks);

            var name = game.Name + " Tournament " + (rand.Next(10) + 10).ToString();

            Console.WriteLine("Name = " + name);

            var file = Imaginarium.GetRandomSourceImage();
            using (var fs = new FileStream(file, FileMode.Open))
            {
                var tournamentView = new TournamentView()
                {
                    ID = 0,
                    Status = (int)Tournament.StatusEnum.Created,
                    GameID = game.ID,
                    PlatformID = game.Platform.ID,
                    Name = name,
                    ImagePath = imaginarium.MakePreview(fs, "/Media/files/tournament/", "TournamentImageSize"),
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
                    Toss = false,
                    IsRoundForPoints = (Game.GameTypeEnum)game.GameType == Game.GameTypeEnum.Points,
                    SingleWinPoint = 100,
                    Rules = "<p>Rules</p>",
                    Players = participants.Take(playersCount).Select(p => p.ID).ToList(),
                    Description = "<p>Descriptions</p>",
                    MoneyType = (int)moneyType,
                    Fee = fee
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
        }


        protected virtual void PlayAll(int tournamentID)
        {
            PlayAll(tournamentID, true);
        }

        protected virtual void PlayAll(int tournamentID, bool enableTech)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            while (PlayRound(tournamentID, rand, enableTech)) { }
        }

        protected virtual void PlayAllTech(int tournamentID)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            while (PlayRound(tournamentID, rand, true, maxNum: 4, allTech:true)) { }
        }

        protected virtual void PlayCount(int tournamentID, int count)
        {
            PlayCount(tournamentID, count, true);
        }

        protected virtual void PlayCount(int tournamentID, int count, bool enableTech)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < count; i++)
            {
                PlayRound(tournamentID, rand, enableTech);
            }
        }

        protected virtual void CreateMatches(int tournamentID)
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
            route.Values.Add("action", "CreateMatches");
            route.Values.Add("area", "Admin");

            var context = new ControllerContext(new RequestContext(httpContext, route), controller);
            controller.ControllerContext = context;

            controller.CreateMatches(tournamentID);

            var tournament = repository.Tournaments.FirstOrDefault(p => p.ID == tournamentID);

            Assert.NotNull(tournament);

            /* Assert.AreEqual(Tournament.StatusEnum.Allocated, (Tournament.StatusEnum)tournament.Status);*/
            var matches = repository.Matches.Count(p => p.TournamentID == tournamentID);
            Console.WriteLine("Matches count = " + matches.ToString());
            Assert.AreNotEqual(0, matches);
        }

        protected virtual void CreateAvatars(int tournamentID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var tournament = repository.Tournaments.FirstOrDefault(p => p.ID == tournamentID);

            foreach (var participant in tournament.Participants)
            {
                participant.ImagePath18 = "/Media/files/avatars/f7aa16e7b4e34b9a9fea466daf4dffc0.jpg";
                participant.ImagePath26 = "/Media/files/avatars/34f6ab40d4ab44abb07e4fc6915cfc45.jpg";
                participant.ImagePath30 = "/Media/files/avatars/9bb24fa5739c43df8f0a0432315564bf.jpg";
                participant.Name = GenerateData.Team.GetRandom();
                repository.SaveParticipant(participant);
            }
        }

        protected virtual void CreateAwards(int tournamentID)
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            var tournament = repository.Tournaments.FirstOrDefault(p => p.ID == tournamentID);

            for (int i = 1; i <= 3; i++)
            {
                var award = new Award()
                {
                    TournamentID = tournament.ID,
                    IconPath = "/Media/files/award/038370cdce954724bdea1010898cd0e9.png",
                    Name = string.Format("За {0} место в турнире {1}", i, tournament.Name),
                    IsSpecial = false,
                    Point = 1000 * (4 - i),
                    Place = i,
                    MoneyGoldPercent = (50 - i * 10),
                    MoneyWood = 1000,
                    MoneyCrystal = 10
                };
                repository.CreateAward(award);
            }

        }

        protected virtual bool PlayRound(int tournamentID, Random rand, bool enableTech, int maxNum = 4, bool allTech = false)
        {

            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            var user = auth.Login("chernikov");


            var tournament = repository.Tournaments.FirstOrDefault(p => p.ID == tournamentID);
            if (tournament != null)
            {
                if (tournament.Status == (int)Tournament.StatusEnum.InGame)
                {
                    var round = tournament.Matches.SelectMany(p => p.Rounds).Where(p => p.Status == (int)Round.RoundStatusEnum.Created && p.Player1 != null && p.Player2 != null).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                    if (round != null)
                    {
                        if (rand.Next() % 5 == 0 && enableTech || allTech)
                        {
                            round.Score1 = rand.Next(2);
                            round.Score2 = rand.Next(2);
                            while (round.Score1 == round.Score2)
                            {
                                round.Score2 = rand.Next(2);
                            }
                            repository.TechSubmitRound(round);
                            Console.WriteLine("TECH ROUND: {0} {1} -- {2} {3}", round.Player1.User.Login, round.Score1, round.Score2, round.Player2.User.Login);
                            return true;
                        }
                        /* round.IntroducerResult = user;*/
                        if (round == round.Match.Round2)
                        {
                            round.Extended = true;
                            round.Score1Text = round.Score1.ToString() + "+0";
                            round.Score2Text = round.Score2.ToString() + "+0";
                        }
                        round.Score1 = rand.Next(maxNum);
                        round.Score2 = rand.Next(maxNum);
                        while (round.Score1 == round.Score2)
                        {
                            round.Score2 = rand.Next(maxNum);
                        }
                        repository.PublishRound(round);
                        repository.SubmitRound(round);
                        if (round.IsAdditional)
                        {
                            Console.WriteLine("ROUND ADDITIONAL: {0} {1} -- {2} {3}", round.Player1.User.Login, round.Score1, round.Score2, round.Player2.User.Login);
                        }
                        else
                        {
                            Console.WriteLine("ROUND : {0} {1} -- {2} {3}", round.Player1.User.Login, round.Score1, round.Score2, round.Player2.User.Login);
                        }
                        return true;
                    }
                }
            }
            return false;
        }


        

        protected virtual void CheckAllRatings()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var repository = kernel.Get<IRepository>("RequestScoped");

            foreach (var rating in repository.Ratings)
            {
                var total = rating.TotalScore;
                var realTotal = rating.RatingDetails.Sum(p => p.Score);

                if (total != realTotal)
                {
                    Console.WriteLine("Rating : {0} RealRating : {1} User : {2}", total, realTotal, rating.User.Login);
                }
            }
        }

        protected static Imaginarium GetImaginarium()
        {
            var kernel = DependencyResolver.Current.GetService<IKernel>();
            var imaginarium = kernel.Get<Imaginarium>();
            return imaginarium;
        }
    }
}
