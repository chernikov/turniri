using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using turniri.Tools;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Tournament> Tournaments
        {
            get
            {
                return Db.Tournaments;
            }
        }

        public IQueryable<Tournament> OpenTournaments
        {
            get
            {
                return Db.Tournaments.Where(p => p.Status != (int)Tournament.StatusEnum.Closed);
            }
        }

        public bool CreateTournament(Tournament instance)
        {
            if (instance.ID == 0)
            {
                instance.Status = (int)Tournament.StatusEnum.Created;
                Db.Tournaments.InsertOnSubmit(instance);
                Db.Tournaments.Context.SubmitChanges();

                var chatRoom = new ChatRoom()
                {
                    TournamentID = instance.ID,
                    Name = "Чат для турнира " + instance.Name,
                    LastUpdate = DateTime.Now
                };
                CreateChatRoom(chatRoom);
                return true;
            }

            return false;
        }

        public bool UpdateTournament(Tournament instance)
        {
            var cache = Db.Tournaments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.PlatformID = instance.PlatformID;
                cache.GameID = instance.GameID;
                cache.ForumID = instance.ForumID;
                cache.Name = instance.Name;
                cache.TournamentType = instance.TournamentType;
                cache.PlayersCount = instance.PlayersCount;
                cache.MinLevel = instance.MinLevel;
                cache.MaxLevel = instance.MaxLevel;
                cache.OpenRegistrationDate = instance.OpenRegistrationDate;
                cache.CloseRegistrationDate = instance.CloseRegistrationDate;
                cache.BeginDate = instance.BeginDate;
                cache.EndDate = instance.EndDate;
                cache.ImagePath = instance.ImagePath;
                cache.Rules = instance.Rules;
                cache.CountRound = instance.CountRound;
                cache.SingleWinPoint = instance.SingleWinPoint;
                cache.SingleDrawPoint = instance.SingleDrawPoint;
                cache.HostGuest = instance.HostGuest;
                cache.DoubleGoalInGuest = instance.DoubleGoalInGuest;
                cache.GroupCount = instance.GroupCount;
                cache.PlayersInGroup = instance.PlayersInGroup;
                cache.ExitFromGroup = instance.ExitFromGroup;
                cache.Toss = instance.Toss;
                cache.IsLive = instance.IsLive;
                cache.Place = instance.Place;
                cache.IsRoundForPoints = instance.IsRoundForPoints;
                cache.IsTeam = instance.IsTeam;
                cache.TeamCount = instance.TeamCount;
                cache.Description = instance.Description;
                cache.IsGroup = instance.IsGroup;
                cache.HotReplacement = instance.HotReplacement;
                cache.MetaDescription = instance.MetaDescription;
                cache.Keywords = instance.Keywords;

                cache.MoneyType = instance.MoneyType;
                cache.Fee = instance.Fee;

                if (cache.TournamentConditionID != null && cache.TournamentConditionID == instance.TournamentConditionID)
                {
                    UpdateTournamentCondition(instance.TournamentCondition);
                }
                else if (cache.TournamentConditionID == null && instance.TournamentConditionID != null)
                {
                    var condition = instance.TournamentCondition;
                    condition.Tournaments.Clear();
                    CreateTournamentCondition(condition);
                    cache.TournamentConditionID = condition.ID;
                }
                else if (cache.TournamentConditionID != null && instance.TournamentConditionID == null)
                {
                    RemoveTournamentCondition(cache.TournamentConditionID.Value);
                }
                Db.Tournaments.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveTournament(int idTournament)
        {
            Tournament instance = Db.Tournaments.FirstOrDefault(p => p.ID == idTournament);
            if (instance != null)
            {
                var list = Db.Notices.Where(p => p.TournamentID == instance.ID).ToList();

                Db.Notices.DeleteAllOnSubmit(list);
                Db.Notices.Context.SubmitChanges();
                var chatRooms = Db.ChatRooms.Where(p => p.TournamentID == instance.ID).ToList();

                foreach (var chatRoom in chatRooms)
                {
                    var forumMessages = Db.ForumMessages.Where(p => p.ChatRoomID == chatRoom.ID);
                    foreach (var forumMessage in forumMessages)
                    {
                        forumMessage.ChatRoomID = null;
                    }
                    Db.ForumMessages.Context.SubmitChanges();
                }
                Db.Tournaments.DeleteOnSubmit(instance);
                Db.Tournaments.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool StartTournament(int idTournament)
        {
            Tournament instance = Db.Tournaments.FirstOrDefault(p => p.ID == idTournament);
            if (instance != null)
            {
                instance.Status = (int)Tournament.StatusEnum.InGame;
                Db.Tournaments.Context.SubmitChanges();
                var canProcessEmpty = instance.TournamentType != (int)Tournament.TournamentTypeEnum.GroupTournament ||
                    (instance.IsGroupAllocated && instance.Matches.Any(p => p.Tour.IsPlayoffType && p.Status == (int)Match.MatchStatusEnum.DefinedPlayers));

                foreach (var match in Db.Matches.Where(p => p.TournamentID == instance.ID).ToList())
                {
                    if (canProcessEmpty)
                    {
                        if (match.IsEmpty)
                        {
                            EmptyMatch(match.ID);
                        }
                    }
                    if (match.IsSingleParticipant && match.Status != (int)Match.MatchStatusEnum.Submit)
                    {
                        SingleParticipantMatch(match.ID);
                    }
                }
                return true;
            }
            return false;
        }

        public bool CreateMatches(int idTournament)
        {
            var tournament = Db.Tournaments.FirstOrDefault(p => p.ID == idTournament);
            if (tournament != null)
            {
                switch ((Tournament.TournamentTypeEnum)tournament.TournamentType)
                {
                    case Tournament.TournamentTypeEnum.RoundRobin:
                        CreateRoundRobinTournament(tournament);
                        break;
                    case Tournament.TournamentTypeEnum.SingleElimination:
                        CreateSingleEliminationTournament(tournament);
                        break;
                    case Tournament.TournamentTypeEnum.DoubleElimination:
                        CreateDoubleEliminationTournament(tournament);
                        break;
                    case Tournament.TournamentTypeEnum.GroupTournament:
                        CreateGroupTournament(tournament);
                        break;
                }
                UpdateToursRecommendedDate(tournament);
                CreateMatchesRounds(tournament);
                tournament.Status = (int)Tournament.StatusEnum.Allocated;
                Db.Tournaments.Context.SubmitChanges();
            }
            return false;
        }

        private bool CreateRoundRobinTournament(Tournament tournament)
        {
            var count = tournament.RegisteredPlayersCount;
            var numberOfMatch = 1;
            var list = new List<Match>();
            var participantsList = new List<int>();

            for (int i = 0; i < count; i++)
            {
                var participant1 = tournament.Participants.OrderBy(p => p.ID).Skip(i).FirstOrDefault();
                if (participant1 != null)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        var participant2 = tournament.Participants.OrderBy(p => p.ID).Skip(j).FirstOrDefault();
                        if (participant2 != null)
                        {
                            var match = CreateMatch(tournament, participant1, participant2, ref numberOfMatch);
                            list.Add(match);
                        }
                    }
                }
            }
            var number = 1;
            while (list.Count > 0)
            {
                var tour = new Tour
                               {
                                   TournamentID = tournament.ID,
                                   Name = number.ToString(),
                                   TourType = (int)Tour.TourTypeEnum.RoundRobin,
                               };
                CreateTour(tour);
                while (true)
                {
                    var match =
                        list.FirstOrDefault(p => !participantsList.Contains(p.Participant1ID.Value) && !participantsList.Contains(p.Participant2ID.Value));
                    if (match != null)
                    {
                        match.TourID = tour.ID;
                        UpdateMatch(match);
                        participantsList.Add(match.Participant1ID.Value);
                        participantsList.Add(match.Participant2ID.Value);
                        list.Remove(match);
                    }
                    else
                    {
                        participantsList = new List<int>();
                        number++;
                        break;
                    }
                }
            }
            return true;
        }

        private bool CreateSingleEliminationTournament(Tournament tournament)
        {
            var count = MatchesMathLogic.GetMin2Pow(tournament.RegisteredPlayersCount);
            if (count > 0)
            {
                var degree = MatchesMathLogic.GetDegree(count);

                var participants = GenerateParticipantList(tournament, count);

                var numberOfMatch = 1;
                var first = true;
                for (int i = degree; i >= 0; i--)
                {
                    var fraction = (int)Math.Pow(2, i - 1);
                    var tour = new Tour
                                   {
                                       TourType = (int)Tour.TourTypeEnum.SingleElimination,
                                       TournamentID = tournament.ID,
                                       Name = fraction > 1 ? "1/" + fraction.ToString() : "финал"
                                   };
                    CreateTour(tour);

                    for (int j = 1; j <= fraction; j++)
                    {
                        Participant participant1 = null;
                        Participant participant2 = null;
                        if (first)
                        {
                            participant1 = participants.Skip(2 * (j - 1)).FirstOrDefault();
                            participant2 = participants.Skip(2 * j - 1).FirstOrDefault();
                        }
                        CreateMatch(tournament, tour, participant1, participant2, (fraction == 1 ? 1 : (int?)null), ref numberOfMatch);
                        if (fraction == 1)
                        {
                            var thirdPlaceTour = new Tour
                            {
                                TourType = (int)Tour.TourTypeEnum.SingleElimination,
                                TournamentID = tournament.ID,
                                Name = "за 3е место"
                            };
                            CreateTour(thirdPlaceTour);
                            CreateMatch(tournament, thirdPlaceTour, null, null, 3, ref numberOfMatch);
                        }
                    }
                    first = false;
                }
                var shift = 0;
                for (int i = degree; i >= 2; i--)
                {
                    var fraction = (int)Math.Pow(2, i - 1);
                    var winNum = fraction + shift;
                    for (int j = 1 + shift; j <= fraction + shift; j = j + 2)
                    {
                        winNum++;
                        var match1 =
                            Matches.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Number == j);
                        var match2 =
                            Matches.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Number == j + 1);
                        var winMatch = Matches.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Number == winNum);
                        if (winMatch != null)
                        {
                            match1.WinMatchID = winMatch.ID;
                            UpdateMatch(match1);
                            match2.WinMatchID = winMatch.ID;
                            UpdateMatch(match2);
                        }
                        //Полуфинал 
                        if (i == 2)
                        {
                            var loseMatch = Matches.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Number == winNum + 1);
                            match1.LoseMatchID = loseMatch.ID;
                            UpdateMatch(match1);
                            match2.LoseMatchID = loseMatch.ID;
                            UpdateMatch(match2);
                        }

                    }
                    shift += fraction;
                }
                return true;
            }
            return false;
        }

        private bool CreateDoubleEliminationTournament(Tournament tournament)
        {
            var count = MatchesMathLogic.GetMin2Pow(tournament.RegisteredPlayersCount);
            if (count > 0)
            {
                var degree = MatchesMathLogic.GetDegree(count);
                var participants = GenerateParticipantList(tournament, count);
                //Матчи первого тура - все в мастерс
                var winMatches = new List<Match>();
                var loseMatches = new List<Match>();
                var bufferWinMatches = new List<Match>();
                var bufferLoseMatches = new List<Match>();
                var numberOfMatch = 1;
                {
                    var fraction = (int)Math.Pow(2, degree - 1);
                    //основной тур
                    var tour = new Tour
                                   {
                                       TourType = (int)Tour.TourTypeEnum.DoubleEliminationMaster,
                                       TournamentID = tournament.ID,
                                       Name = fraction > 1 ? "1/" + fraction.ToString() : "финал"
                                   };
                    CreateTour(tour);
                    for (int j = 1; j <= fraction; j++)
                    {
                        Participant participant1 = null;
                        Participant participant2 = null;
                        participant1 = participants.Skip(2 * (j - 1)).FirstOrDefault();
                        participant2 = participants.Skip(2 * j - 1).FirstOrDefault();
                        var match = CreateMatch(tournament, tour, participant1, participant2, null, ref numberOfMatch);
                        winMatches.Add(match);
                    }
                    //нижний пул 
                    var loseTour = new Tour
                    {
                        TourType = (int)Tour.TourTypeEnum.DoubleEliminationMaster,
                        TournamentID = tournament.ID,
                        Name = (fraction > 1 ? "1/" + fraction.ToString() : "финал") + "(проигр.)"
                    };
                    CreateTour(loseTour);
                    for (int j = 0; j < fraction / 2; j++)
                    {
                        var match = CreateMatch(tournament, loseTour, null, null, null, ref numberOfMatch);
                        loseMatches.Add(match);
                    }
                    //лузеры играют игры внизу
                    for (int j = 0; j < winMatches.Count / 2; j++)
                    {
                        var match1 = winMatches[2 * j];
                        var match2 = winMatches[2 * j + 1];
                        var loseMatch = loseMatches[j];
                        match1.LoseMatchID = loseMatch.ID;
                        UpdateMatch(match1);
                        match2.LoseMatchID = loseMatch.ID;
                        UpdateMatch(match2);
                    }
                }

                Match winFinalGame = null;
                Match loseFinalGame = null;
                var doubleEliminationSwitcher = true;
                while (true)
                {
                    if (winMatches.Count > 1)
                    {
                        var tourCount = winMatches.Count / 2;
                        //1 создаем игры верхнего пула 
                        var tour = new Tour
                                    {
                                        TourType = (int)Tour.TourTypeEnum.DoubleEliminationMaster,
                                        TournamentID = tournament.ID,
                                        Name = tourCount > 1 ? "1/" + tourCount.ToString() : "финал",
                                    };
                        CreateTour(tour);
                        for (int j = 1; j <= tourCount; j++)
                        {
                            var match = CreateMatch(tournament, tour, null, null, null, ref numberOfMatch);
                            if (tourCount == 1)
                            {
                                winFinalGame = match;
                            }
                            bufferWinMatches.Add(match);
                        }
                        //2 победители предыдущего тура выходят на игры этого пула
                        for (int j = 0; j < tourCount; j++)
                        {
                            var match1 = winMatches[2 * j];
                            var match2 = winMatches[2 * j + 1];
                            var winMatch = bufferWinMatches[j];
                            match1.WinMatchID = winMatch.ID;
                            UpdateMatch(match1);
                            match2.WinMatchID = winMatch.ID;
                            UpdateMatch(match2);
                        }

                        //сколько игр? 
                        if ((loseMatches.Count + bufferWinMatches.Count) / 2 < loseMatches.Count)
                        {
                            //2.5 из верхнего пула приходит меньше игроков / делаем тур только для нижнего пула 
                            //тем самым мы уменьшаем колво в нижнем пуле
                            var loseBufferTour = new Tour
                            {
                                TourType = (int)Tour.TourTypeEnum.DoubleEliminationSecondary,
                                TournamentID = tournament.ID,
                                Name = (tourCount > 1 ? "1/" + tourCount.ToString() : "финал") + " (проигр. доп.) ",
                            };
                            CreateTour(loseBufferTour);
                            for (int j = 1; j <= loseMatches.Count / 2; j++)
                            {
                                var match = CreateMatch(tournament, loseBufferTour, null, null, null, ref numberOfMatch);
                                bufferLoseMatches.Add(match);
                            }
                            for (int j = 0; j < loseMatches.Count / 2; j++)
                            {
                                var match1 = loseMatches[2 * j];
                                var match2 = loseMatches[2 * j + 1];
                                var winMatch = bufferLoseMatches[j];
                                match1.WinMatchID = winMatch.ID;
                                UpdateMatch(match1);
                                match2.WinMatchID = winMatch.ID;
                                UpdateMatch(match2);
                            }
                            loseMatches = bufferLoseMatches;
                            bufferLoseMatches = new List<Match>();
                        }
                        //3 создаем игры нижнего пула с заходом из верхнего
                        var loseTour = new Tour
                        {
                            TourType = (int)Tour.TourTypeEnum.DoubleEliminationSecondary,
                            TournamentID = tournament.ID,
                            Name = (tourCount > 1 ? "1/" + tourCount.ToString() : "финал") + "(проигр.)",
                        };
                        CreateTour(loseTour);
                        var loseMatchCount = (loseMatches.Count + bufferWinMatches.Count) / 2;
                        for (int j = 1; j <= loseMatchCount; j++)
                        {
                            var match = CreateMatch(tournament, tour, null, null, (tourCount == 1 ? 2 : (int?)null), ref numberOfMatch);
                            if (tourCount == 1)
                            {
                                loseFinalGame = match;
                            }
                            bufferLoseMatches.Add(match);
                        }

                        //4 победители предыдущего тура нижнего пула выходят на игры этого пула 1-1
                        for (int j = 0; j < loseMatches.Count; j++)
                        {
                            var match = loseMatches[j];
                            var winMatch = bufferLoseMatches[j];
                            match.WinMatchID = winMatch.ID;
                            UpdateMatch(match);
                        }
                        //5 проигравшие из верхнего пула текущего тура выходят на игры этого пула 
                        if (doubleEliminationSwitcher)
                        {
                            for (int j = 0; j < bufferWinMatches.Count; j++)
                            {
                                var match = bufferWinMatches[j];
                                var loseMatch = bufferLoseMatches[bufferLoseMatches.Count - 1 - j];
                                match.LoseMatchID = loseMatch.ID;
                                UpdateMatch(match);
                            }
                            doubleEliminationSwitcher = false;
                        }
                        else
                        {
                            for (int j = 0; j < bufferWinMatches.Count; j++)
                            {
                                var match = bufferWinMatches[j];
                                var loseMatch = bufferLoseMatches[j];
                                match.LoseMatchID = loseMatch.ID;
                                UpdateMatch(match);
                            }
                            doubleEliminationSwitcher = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                    winMatches = bufferWinMatches;
                    bufferWinMatches = new List<Match>();
                    loseMatches = bufferLoseMatches;
                    bufferLoseMatches = new List<Match>();
                }
                if (loseFinalGame != null && winFinalGame != null)
                {
                    var finalTour = new Tour
                                        {
                                            TourType = (int)Tour.TourTypeEnum.DoubleEliminationMaster,
                                            TournamentID = tournament.ID,
                                            Name = "супер-финал",
                                        };
                    CreateTour(finalTour);
                    var match = CreateMatch(tournament, finalTour, null, null, 1, ref numberOfMatch);
                    loseFinalGame.WinMatchID = match.ID;
                    UpdateMatch(loseFinalGame);
                    winFinalGame.WinMatchID = match.ID;
                    UpdateMatch(winFinalGame);
                }
                return true;
            }
            return false;
        }

        private static List<Participant> GenerateParticipantList(Tournament tournament, int count)
        {
            var empty = new List<Participant>();
            for (int i = tournament.RegisteredPlayersCount; i < count; i++)
            {
                empty.Add(null);
            }
            var participants = tournament.Participants.ToList();
            participants.AddRange(empty);
            participants = participants.OrderBy(p => Guid.NewGuid()).ToList();
            return participants;
        }

        private bool CreateGroupTournament(Tournament tournament)
        {
            char numberGroup = 'A';
            var numberOfMatch = 1;
            var shiftParticipants = 0;
            for (int i = 0; i < tournament.GroupCount; i++)
            {
                //Создать группу
                var group = new TournamentGroup
                                {
                                    TournamentID = tournament.ID,
                                    Name = numberGroup.ToString()
                                };
                CreateTournamentGroup(group);
                //создаем групповые игры
                for (int j = 0; j < tournament.PlayersInGroup; j++)
                {
                    var participant1 = tournament.Participants.OrderBy(p => p.ID).Skip(j + shiftParticipants).FirstOrDefault();
                    if (participant1 != null)
                    {
                        for (int k = j + 1; k < tournament.PlayersInGroup; k++)
                        {
                            var participant2 = tournament.Participants.OrderBy(p => p.ID).Skip(k + shiftParticipants).FirstOrDefault();
                            if (participant2 != null)
                            {
                                CreateMatch(tournament, participant1, participant2, group, ref numberOfMatch);
                                participant1.TournamentGroupID = group.ID;
                                UpdateParticipant(participant1);
                                participant2.TournamentGroupID = group.ID;
                                UpdateParticipant(participant2);
                            }
                        }
                    }
                }
                shiftParticipants += tournament.PlayersInGroup;
                numberGroup++;
            }
            //расставляем групповые игры по турам
            foreach (var group in TournamentGroups.Where(p => p.TournamentID == tournament.ID))
            {
                var list = Matches.Where(p => p.TournamentGroupID == group.ID).ToList();
                var participantsList = new List<int>();
                var number = 1;
                while (list.Count > 0)
                {
                    Tour tour = Tours.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Name == number.ToString());
                    if (tour == null)
                    {
                        tour = new Tour
                                    {
                                        TournamentID = tournament.ID,
                                        Name = number.ToString(),
                                        TourType = (int)Tour.TourTypeEnum.Group,
                                    };
                        CreateTour(tour);
                    }
                    while (true)
                    {
                        var match = list.FirstOrDefault(p => !participantsList.Contains(p.Participant1ID.Value) && !participantsList.Contains(p.Participant2ID.Value));
                        if (match != null)
                        {
                            match.TourID = tour.ID;
                            UpdateMatch(match);
                            participantsList.Add(match.Participant1ID.Value);
                            participantsList.Add(match.Participant2ID.Value);
                            list.Remove(match);
                        }
                        else
                        {
                            participantsList = new List<int>();
                            number++;
                            break;
                        }
                    }
                }
            }
            var shift = numberOfMatch - 1;
            //создаем плейофф-игры
            var count = MatchesMathLogic.GetMin2Pow(tournament.GroupCount * tournament.ExitFromGroup);
            if (count > 0)
            {
                var degree = MatchesMathLogic.GetDegree(count);
                for (int i = degree; i >= 0; i--)
                {
                    var fraction = (int)Math.Pow(2, i - 1);
                    var tour = new Tour
                    {
                        TourType = (int)Tour.TourTypeEnum.SingleElimination,
                        TournamentID = tournament.ID,
                        Name = fraction > 1 ? "1/" + fraction.ToString() : "финал"
                    };
                    CreateTour(tour);
                    for (int j = 1; j <= fraction; j++)
                    {
                        CreateMatch(tournament, tour, null, null, (fraction == 1 ? 1 : (int?)null), ref numberOfMatch);
                        if (fraction == 1)
                        {
                            var thirdPlaceTour = new Tour
                            {
                                TourType = (int)Tour.TourTypeEnum.SingleElimination,
                                TournamentID = tournament.ID,
                                Name = "за 3е место"
                            };
                            CreateTour(thirdPlaceTour);
                            CreateMatch(tournament, thirdPlaceTour, null, null, 3, ref numberOfMatch);
                        }
                    }
                }
                for (int i = degree; i >= 2; i--)
                {
                    var fraction = (int)Math.Pow(2, i - 1);
                    var winNum = fraction + shift;
                    for (int j = 1 + shift; j <= fraction + shift; j = j + 2)
                    {
                        winNum++;
                        var match1 =
                            Matches.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Number == j);
                        var match2 =
                            Matches.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Number == j + 1);
                        var winMatch = Matches.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Number == winNum);
                        if (winMatch != null)
                        {
                            match1.WinMatchID = winMatch.ID;
                            UpdateMatch(match1);
                            match2.WinMatchID = winMatch.ID;
                            UpdateMatch(match2);
                        }
                        //Полуфинал 
                        if (i == 2)
                        {
                            var loseMatch = Matches.FirstOrDefault(p => p.TournamentID == tournament.ID && p.Number == winNum + 1);
                            match1.LoseMatchID = loseMatch.ID;
                            UpdateMatch(match1);
                            match2.LoseMatchID = loseMatch.ID;
                            UpdateMatch(match2);
                        }

                    }
                    shift += fraction;
                }
                return true;
            }
            return false;
        }

        private void UpdateToursRecommendedDate(Tournament tournament)
        {
            var toursCount = tournament.Tours.Count();
            var num = 1;
            foreach (var tour in tournament.Tours.OrderBy(p => p.ID))
            {
                tour.RecommendDate = MatchesMathLogic.GetDateLimitSlice(tournament.BeginDate, tournament.EndDate,
                                                                        toursCount, num);
                UpdateTour(tour);
                num++;
            }
        }

        private Match CreateMatch(Tournament tournament, Participant participant1, Participant participant2, ref int numberOfMatch)
        {
            if (participant1 != null && participant2 != null)
            {
                var match = new Match
                {
                    TournamentID = tournament.ID,
                    GameID = tournament.GameID,
                    Participant1ID = participant1.ID,
                    Participant2ID = participant2.ID,
                    Name = participant1.User.Login + " - " + participant2.User.Login,
                    Number = numberOfMatch,
                    Status = (int)Match.MatchStatusEnum.DefinedPlayers
                };
                numberOfMatch++;
                CreateMatch(match);
                return match;
            }
            return null;
        }

        private Match CreateMatch(Tournament tournament, Participant participant1, Participant participant2, TournamentGroup group, ref int numberOfMatch)
        {
            if (participant1 != null && participant2 != null)
            {
                var match = new Match
                {
                    TournamentID = tournament.ID,
                    GameID = tournament.GameID,
                    TournamentGroupID = group.ID,
                    Participant1ID = participant1.ID,
                    Participant2ID = participant2.ID,
                    Name = participant1.User.Login + " - " + participant2.User.Login,
                    Number = numberOfMatch,
                    Status = (int)Match.MatchStatusEnum.DefinedPlayers
                };
                numberOfMatch++;
                CreateMatch(match);
                return match;
            }
            return null;
        }

        private Match CreateMatch(Tournament tournament, Tour tour, Participant participant1, Participant participant2, int? place, ref int numberOfMatch)
        {
            var match = new Match
            {
                TourID = tour.ID,
                TournamentID = tournament.ID,
                GameID = tournament.GameID,
                Participant1ID = (participant1 != null ? participant1.ID : (int?)null),
                Participant2ID = (participant2 != null ? participant2.ID : (int?)null),
                Name = tournament.Name + " (" + tour.Name + ")",
                Status = (participant1 != null && participant2 != null) ? (int)Match.MatchStatusEnum.DefinedPlayers : (int)Match.MatchStatusEnum.Created,
                Number = numberOfMatch
            };
            if (place != null)
            {
                match.Place = place.Value;
            }
            numberOfMatch++;
            CreateMatch(match);
            return match;
        }

        private void CreateMatchesRounds(Tournament tournament)
        {
            foreach (var match in Db.Matches.Where(p => p.TournamentID == tournament.ID))
            {
                CreateRounds(tournament, match);
            }
        }

        private void CreateRounds(Tournament tournament, Match match)
        {
            if (tournament.CountRound == 1)
            {
                var round = new Round
                {
                    MatchID = match.ID,
                    Number = 1,
                    Participant1ID = match.Participant1ID,
                    Participant2ID = match.Participant2ID,
                    ScheduleDate = match.TourID.HasValue ? match.Tour.RecommendDate : null
                };

                CreateRound(round);
            }
            if (tournament.CountRound == 3)
            {
                if (match.Place > 0)
                {
                    var round = new Round
                    {
                        MatchID = match.ID,
                        Number = 1,
                        Participant1ID = match.Participant1ID,
                        Participant2ID = match.Participant2ID,
                        ScheduleDate = match.TourID.HasValue ? match.Tour.RecommendDate : null
                    };

                    CreateRound(round);
                }
                else
                {
                    //домашняя игра для 1го игрока
                    var round1 = new Round
                                     {
                                         MatchID = match.ID,
                                         Participant1ID = match.Participant1ID,
                                         Participant2ID = match.Participant2ID,
                                         Number = 1,
                                         Player1IsHost = tournament.HostGuest,
                                         ScheduleDate = match.TourID.HasValue ? match.Tour.RecommendDate : null
                                     };
                    CreateRound(round1);
                    var round2 = new Round
                                     {
                                         MatchID = match.ID,
                                         Participant1ID = match.Participant1ID,
                                         Participant2ID = match.Participant2ID,
                                         Number = 2,
                                         Player1IsHost = false,
                                         ScheduleDate = match.TourID.HasValue ? match.Tour.RecommendDate : null
                                     };
                    CreateRound(round2);
                }
            }
        }

        public bool ClearMatches(int idTournament)
        {
            var tournament = Db.Tournaments.FirstOrDefault(p => p.ID == idTournament);
            if (tournament != null)
            {
                if (tournament.Status == (int)Tournament.StatusEnum.Created ||
                    tournament.Status == (int)Tournament.StatusEnum.Allocated)
                {
                    var list = tournament.Matches;
                    Db.Matches.DeleteAllOnSubmit(list);
                    Db.Matches.Context.SubmitChanges();
                    var tours = tournament.Tours;
                    Db.Tours.DeleteAllOnSubmit(tours);
                    Db.Tours.Context.SubmitChanges();
                    var groups = tournament.TournamentGroups;
                    foreach (var group in groups)
                    {
                        foreach (var participant in group.Participants)
                        {
                            participant.TournamentGroupID = null;
                            UpdateParticipant(participant);
                        }
                    }
                    Db.TournamentGroups.DeleteAllOnSubmit(groups);
                    Db.TournamentGroups.Context.SubmitChanges();

                    tournament.Status = (int)Tournament.StatusEnum.Created;
                    Db.Tournaments.Context.SubmitChanges();
                    return true;
                }
            }
            return false;
        }

        private void ProcessTournament(int idTournament)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);
            if (tournament != null)
            {
                var canProcessEmpty = tournament.TournamentType != (int)Tournament.TournamentTypeEnum.GroupTournament ||
                    (tournament.IsGroupAllocated && tournament.Matches.Any(p => p.Tour.IsPlayoffType && p.Status == (int)Match.MatchStatusEnum.DefinedPlayers));
                //проверка на проходимость без игры
                foreach (var match in Db.Matches.Where(p => p.TournamentID == tournament.ID).ToList())
                {
                    //только не после прохождения всей группы проверяем / иначе получим везде Empty
                    if (canProcessEmpty)
                    {
                        if (match.IsEmpty)
                        {
                            EmptyMatch(match.ID);
                        }
                    }
                    if (match.IsSingleParticipant && match.Status != (int)Match.MatchStatusEnum.Submit)
                    {
                        SingleParticipantMatch(match.ID);
                    }
                }

                if (tournament.LeagueSeason != null)
                {
                    LeagueSeasonRecalculatePlaces(tournament.LeagueSeason.ID);
                }
            }
        }

        public bool FinishTournament(int idTournament)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);
            // все ли матчи сыграны
            if (tournament.AllGamesPlayed)
            {
                tournament.Status = (int)Tournament.StatusEnum.PlayedOut;
                Db.Tournaments.Context.SubmitChanges();

                //single points 
                AwardUsers(tournament);

                AwardByPlaces(tournament);

                MoneyToSite(tournament);
                return true;
            }
            return false;
        }

        public void MoneyToSite(Tournament tournament)
        {
            var sum = tournament.SumFeeGold;

            if (sum != 0)
            {
                var moneyDetail = new MoneyDetail()
                {
                    TournamentID = tournament.ID,
                    SumGold = -sum,
                    Description = "Остатки от проведения турнира"
                };

                var siteMoney = new MoneyDetail()
                {
                    IsFee = true,
                    SumGold = sum,
                    Description = "Остатки от проведения турнира " + tournament.Name
                };

                var guid = CreateTripleMoneyDetail(moneyDetail, siteMoney);
                SubmitMoney(guid);
            }
        }

        public void ReAwardByPlaces(int idTournament)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);
            // все ли матчи сыграны
            if (tournament.Status == (int)Tournament.StatusEnum.PlayedOut && tournament.Awards.FirstOrDefault(p => !p.IsSpecial) != null && !tournament.Awards.FirstOrDefault(p => !p.IsSpecial).IsAwarded)
            {
                AwardByPlaces(tournament);
            }
        }

        private void AwardByPlaces(Tournament tournament)
        {
            if (tournament.TournamentType == (int)Tournament.TournamentTypeEnum.RoundRobin)
            {
                var list = new List<Tournament.StatisticGroup>();

                foreach (var participant in tournament.SubPlayers)
                {
                    var statistic = new Tournament.StatisticGroup()
                    {
                        TournamentGroup = null,
                        Participant = participant,
                        GuestPoints = tournament.TotalPointWin(participant.ID, false),
                        Points = tournament.TotalWinGame(participant.ID, null) * 3 + tournament.TotalDrawnGame(participant.ID, null),
                        Diff = tournament.TotalPointWin(participant.ID, null) - tournament.TotalPointLose(participant.ID, null),
                        Win = tournament.TotalWinGame(participant.ID, null)
                    };
                    list.Add(statistic);
                }
                var firstPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).FirstOrDefault();
                var secondPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).Skip(1).FirstOrDefault();
                var thirdPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).Skip(2).FirstOrDefault();

                if (firstPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.FirstPlaceAward, firstPlace.Participant.ID);
                }
                if (secondPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.SecondPlaceAward, secondPlace.Participant.ID);
                }
                if (thirdPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.ThirdPlaceAward, thirdPlace.Participant.ID);
                }
            }
            else
            {
                foreach (var match in tournament.Matches.Where(p => p.WinnerID != null && p.Participant1ID != null && p.Participant2ID != null))
                {
                    if (match.Place == 1)
                    {
                        AwardPlayer(match, match.Tournament, match.Tournament.FirstPlaceAward, match.WinnerID);
                        if (!match.Technical)
                        {
                            AwardPlayer(match, match.Tournament, match.Tournament.SecondPlaceAward, match.LoserID);
                        }
                    }
                    if (match.Place == 2) // проигравший финал нижний пула в дабл-элиминейшн
                    {
                        if (!match.Technical)
                        {
                            AwardPlayer(match, match.Tournament, match.Tournament.ThirdPlaceAward, match.LoserID);
                        }
                    }
                    if (match.Place == 3)
                    {
                        AwardPlayer(match, match.Tournament, match.Tournament.ThirdPlaceAward, match.WinnerID);
                    }
                }
            }
        }

        public bool TechFinishTournament(int idTournament)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);
            // все ли матчи сыграны

            var match = tournament.Matches.OrderBy(p => p.ID).FirstOrDefault(p => p.Status == (int)Match.MatchStatusEnum.DefinedPlayers);

            while (match != null)
            {
                TechAllLoseMatch(match.ID);
                match = tournament.Matches.OrderBy(p => p.ID).FirstOrDefault(p => p.Status == (int)Match.MatchStatusEnum.DefinedPlayers);
            }

            tournament.Status = (int)Tournament.StatusEnum.PlayedOut;
            Db.Tournaments.Context.SubmitChanges();

            //single points 
            AwardUsers(tournament);

            if (tournament.TournamentType == (int)Tournament.TournamentTypeEnum.RoundRobin)
            {
                var list = new List<Tournament.StatisticGroup>();

                foreach (var participant in tournament.SubPlayers)
                {
                    var statistic = new Tournament.StatisticGroup()
                    {
                        TournamentGroup = null,
                        Participant = participant,
                        GuestPoints = tournament.TotalPointWin(participant.ID, false),
                        Points = tournament.TotalWinGame(participant.ID, null) * 3 + tournament.TotalDrawnGame(participant.ID, null),
                        Diff = tournament.TotalPointWin(participant.ID, null) - tournament.TotalPointLose(participant.ID, null),
                        Win = tournament.TotalWinGame(participant.ID, null)
                    };
                    list.Add(statistic);
                }
                var firstPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).FirstOrDefault();
                var secondPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).Skip(1).FirstOrDefault();
                var thirdPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).Skip(2).FirstOrDefault();

                if (firstPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.FirstPlaceAward, firstPlace.Participant.ID);
                }
                if (secondPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.SecondPlaceAward, secondPlace.Participant.ID);
                }
                if (thirdPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.ThirdPlaceAward, thirdPlace.Participant.ID);
                }

            }
            return true;
        }

        public bool UndoFinishTournament(int idTournament)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);
            var match = tournament.Matches.OrderBy(p => p.ID).FirstOrDefault(p => p.Technical && p.WinnerID == null && p.Status == (int)Match.MatchStatusEnum.Submit);

            while (match != null)
            {
                match.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                match.Technical = false;
                Db.Matches.Context.SubmitChanges();
                foreach (var round in match.Rounds)
                {
                    round.Status = (int)Round.RoundStatusEnum.Created;
                    round.Technical = false;
                    Db.Rounds.Context.SubmitChanges();
                }
                match = tournament.Matches.OrderBy(p => p.ID).FirstOrDefault(p => p.Technical && p.WinnerID == null && p.Status == (int)Match.MatchStatusEnum.Submit);
            }

            tournament.Status = (int)Tournament.StatusEnum.InGame;
            Db.Tournaments.Context.SubmitChanges();

            //single points 
            UnAwardUsers(tournament);

            return true;

        }

        public bool ResetAwardsForTournament(int idTournament)
        {
            DiscardTournamentAwardMoney(idTournament);
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);
            
            tournament.Status = (int)Tournament.StatusEnum.PlayedOut;
            Db.Tournaments.Context.SubmitChanges();

            foreach (var match in tournament.Matches.Where(p => p.WinnerID != null && p.Participant1ID != null && p.Participant2ID != null && !p.Technical))
            {
                if (match.Place == 1)
                {
                    AwardPlayer(match, match.Tournament, match.Tournament.FirstPlaceAward, match.WinnerID);
                    AwardPlayer(match, match.Tournament, match.Tournament.SecondPlaceAward, match.LoserID);
                }
                if (match.Place == 2) // проигравший финал нижний пула в дабл-элиминейшн
                {
                    AddExpo(match, tournament, tournament.SingleWinPoint, match.Winner, "");
                    AwardPlayer(match, match.Tournament, match.Tournament.ThirdPlaceAward, match.LoserID);
                }
                if (match.Place == 3)
                {
                    AddExpo(match, tournament, tournament.SingleWinPoint, match.Winner, "");
                    AwardPlayer(match, match.Tournament, match.Tournament.ThirdPlaceAward, match.WinnerID);
                }
            }
            if (tournament.TournamentType == (int)Tournament.TournamentTypeEnum.RoundRobin)
            {
                var list = new List<Tournament.StatisticGroup>();

                foreach (var participant in tournament.SubPlayers)
                {
                    var statistic = new Tournament.StatisticGroup()
                    {
                        TournamentGroup = null,
                        Participant = participant,
                        GuestPoints = tournament.TotalPointWin(participant.ID, false),
                        Points = tournament.TotalWinGame(participant.ID, null) * 3 + tournament.TotalDrawnGame(participant.ID, null),
                        Diff = tournament.TotalPointWin(participant.ID, null) - tournament.TotalPointLose(participant.ID, null),
                        Win = tournament.TotalWinGame(participant.ID, null)
                    };
                    list.Add(statistic);
                }
                var firstPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).FirstOrDefault();
                var secondPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).Skip(1).FirstOrDefault();
                var thirdPlace = list.OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).Skip(2).FirstOrDefault();

                if (firstPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.FirstPlaceAward, firstPlace.Participant.ID);
                }
                if (secondPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.SecondPlaceAward, secondPlace.Participant.ID);
                }
                if (thirdPlace != null)
                {
                    AwardPlayer(null, tournament, tournament.ThirdPlaceAward, thirdPlace.Participant.ID);
                }
            }
            return true;
        }


        private void DiscardTournamentAwardMoney(int idTournament)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);

            if (tournament != null)
            {
                var moneyDetails = tournament.MoneyDetails.Where(p => p.SumGold < 0 || p.SumWood < 0 || p.SumCrystal < 0).ToList();

                foreach (var moneyDetail in moneyDetails)
                {
                    var guid = moneyDetail.Global;

                    var moneys = Db.MoneyDetails.Where(p => p.Global == guid).ToList();
                    Db.MoneyDetails.DeleteAllOnSubmit(moneys);
                    Db.MoneyDetails.Context.SubmitChanges();
                };
                if (tournament.IsGroup)
                {
                    foreach (var player in tournament.Participants)
                    {
                        if (player.Group != null)
                        {
                            RecalculateGroupMoney(player.Group.ID);
                        }
                    }
                }
                else
                {
                    foreach (var player in tournament.Participants)
                    {
                        RecalculateUserMoney(player.UserID);
                    }
                }
                RecalculateBalanceSite();
            }
        }

        public bool AllocatePlayoff(int idTournament)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);
            if (tournament != null && tournament.IsGroupAllocated)
            {
                var list = new List<Tournament.StatisticGroup>();
                var stringBuilder = new StringBuilder();
                foreach (var group in tournament.TournamentGroups)
                {
                    foreach (var participant in group.SubPlayers)
                    {
                        var statistic = new Tournament.StatisticGroup()
                        {
                            TournamentGroup = group,
                            Participant = participant,
                            GuestPoints = group.TotalPointWin(participant.ID, false),
                            Points = group.TotalWinGame(participant.ID, null) * 3 + group.TotalDrawnGame(participant.ID, null),
                            Diff = group.TotalPointWin(participant.ID, null) - group.TotalPointLose(participant.ID, null),
                            Win = group.TotalWinGame(participant.ID, null)
                        };
                        list.Add(statistic);
                    }
                }
                var startPlayoffTour = tournament.Tours.Where(p => p.TourType == (int)Tour.TourTypeEnum.SingleElimination).OrderBy(p => p.ID).First();
                var startPlayoffMatches = tournament.Matches.Where(p => p.TourID == startPlayoffTour.ID).ToList();
                foreach (var group in tournament.TournamentGroups)
                {
                    var exitParticipants = list.Where(p => p.TournamentGroup == group).OrderByDescending(p => p.Points).ThenByDescending(p => p.Diff).ThenByDescending(p => p.Win).ThenByDescending(p => p.GuestPoints).Take(tournament.ExitFromGroup).Select(p => p.Participant);

                    foreach (var user in exitParticipants)
                    {
                        DefineParticipantInMatches(user, startPlayoffMatches);
                    }
                }
                tournament.Status = (int)Tournament.StatusEnum.Allocated;
                Db.Tournaments.Context.SubmitChanges();
                return true;

            }
            return false;
        }

        private bool UpdateTournamentRole(int id, List<int> users, string code)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                //delete old
                var list = tournament.UserRoleTournaments.Where(p => p.UserRole.Role.Code == code).ToList();
                Db.UserRoleTournaments.DeleteAllOnSubmit(list);
                Db.UserRoleTournaments.Context.SubmitChanges();
                //create new
                if (users != null)
                {
                    foreach (var idUser in users)
                    {
                        var user = Db.Users.FirstOrDefault(p => p.ID == idUser);
                        if (user != null)
                        {
                            var userRole = user.UserRoles.FirstOrDefault(p => p.Role.Code == code);
                            if (userRole == null)
                            {
                                var role = Roles.FirstOrDefault(p => p.Code == code);

                                if (role != null)
                                {
                                    userRole = new UserRole()
                                    {
                                        RoleID = role.ID,
                                        UserID = user.ID
                                    };
                                    Db.UserRoles.InsertOnSubmit(userRole);
                                    Db.UserRoles.Context.SubmitChanges();
                                }
                            }
                            var userRoleTournament = new UserRoleTournament()
                            {
                                UserRoleID = userRole.ID,
                                TournamentID = tournament.ID
                            };

                            Db.UserRoleTournaments.InsertOnSubmit(userRoleTournament);
                            Db.UserRoleTournaments.Context.SubmitChanges();
                        }
                    }
                }
                //check all and delete UserRole
                var listForDelete = UserRoles.Where(p => p.Role.Code == code && !p.UserRoleTournaments.Any());
                Db.UserRoles.DeleteAllOnSubmit(listForDelete);
                Db.UserRoles.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateTournamentAdmins(int id, List<int> admins)
        {
            return UpdateTournamentRole(id, admins, "tournament_admin");
        }

        public bool UpdateTournamentModerators(int id, List<int> moderators)
        {
            return UpdateTournamentRole(id, moderators, "tournament_moderator");
        }

        public void CloseAllCommentsInTournament(int idTournament, string filesBaseFolder)
        {
            var tournament = Tournaments.FirstOrDefault(p => p.ID == idTournament);

            var commentsForDelete = new List<Comment>();
            foreach (var match in tournament.Matches)
            {
                foreach (var matchComment in match.MatchComments)
                {
                    var comment = matchComment.Comment;

                    if (!string.IsNullOrWhiteSpace(comment.ImagePath))
                    {
                        var path = filesBaseFolder + comment.ImagePath.Replace("/", "\\");

                        var fi = new FileInfo(path);

                        if (fi.Exists)
                        {
                            fi.Delete();
                        }
                        if (!string.IsNullOrWhiteSpace(comment.Text) && !string.IsNullOrWhiteSpace(comment.VideoCode))
                        {
                            commentsForDelete.Add(comment);
                        }
                        else
                        {
                            comment.ImagePath = null;
                            Db.Comments.Context.SubmitChanges();
                        }
                    }
                }
            }
            Db.Comments.DeleteAllOnSubmit(commentsForDelete);
            Db.Comments.Context.SubmitChanges();
        }

        
    }
}