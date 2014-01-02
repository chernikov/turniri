using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Match> Matches
        {
            get
            {
                return Db.Matches;
            }
        }

        public bool CreateMatch(Match instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Matches.InsertOnSubmit(instance);
                Db.Matches.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool PlayMatch(int idMatch)
        {
            Match instance = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (instance != null)
            {
                instance.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                Db.Matches.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdateMatch(Match instance)
        {
            var cache = Db.Matches.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Participant1ID = instance.Participant1ID;
                cache.Participant2ID = instance.Participant2ID;
                cache.Score1 = instance.Score1;
                cache.Score2 = instance.Score2;
                cache.WinnerID = instance.WinnerID;
                cache.WinMatchID = instance.WinMatchID;
                cache.LoseMatchID = instance.LoseMatchID;
                cache.Place = instance.Place;
                Db.Matches.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateStatusMatch(Match instance)
        {
            var cache = Db.Matches.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Status = instance.Status;
                Db.Matches.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveMatch(int idMatch)
        {
            Match instance = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (instance != null)
            {
                foreach (var message in Db.Messages.Where(p => p.MatchID == instance.ID).ToList())
                {
                    message.MatchID = null;
                }
                Db.Messages.Context.SubmitChanges();
                var list = Db.Notices.Where(p => p.MatchID == instance.ID).ToList();

                Db.Notices.DeleteAllOnSubmit(list);
                Db.Notices.Context.SubmitChanges();

                Db.Matches.DeleteOnSubmit(instance);
                Db.Matches.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool TechSubmitMatch(Match instance)
        {
            var cache = Db.Matches.FirstOrDefault(p => p.ID == instance.ID);

            foreach (var round in cache.Rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Created).ToList())
            {
                round.Score1 = instance.Score1.Value;
                round.Score2 = instance.Score2.Value;
                this.TechSubmitRound(round);
            }
            return false;
        }

        public bool TechAllLoseMatch(int idMatch)
        {
            var cache = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (cache != null)
            {
                foreach (var round in cache.Rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Created).ToList())
                {
                    round.Score1 = 0;
                    round.Score2 = 0;
                    round.Technical = true;
                    round.Status = (int)Round.RoundStatusEnum.Submit;
                    Db.Rounds.Context.SubmitChanges();
                }

                cache.Technical = true;
                cache.PublishedDate = DateTime.Now;
                cache.Status = (int)Match.MatchStatusEnum.Submit;
                Db.Matches.Context.SubmitChanges();
                ProcessMatch(cache.ID);

                var winMatch = cache.WinMatch;
                if (winMatch != null)
                {
                    if (winMatch.IsSingleParticipant && winMatch.Status != (int)Match.MatchStatusEnum.Submit)
                    {
                        SingleParticipantMatch(winMatch.ID);
                    }
                }
                var loseMatch = cache.LoseMatch;
                if (loseMatch != null)
                {
                    if (loseMatch.IsSingleParticipant && loseMatch.Status != (int)Match.MatchStatusEnum.Submit)
                    {
                        SingleParticipantMatch(loseMatch.ID);
                    }
                }

                return true;
            }
            return false;
        }

        public bool CreateOffMatch(LeagueSeason leagueSeason, LeagueLevel mainLevel, int mainParticipantID, int nextParticipantID)
        {
            var match = new Match()
            {
                GameID = leagueSeason.League.GameID,
                LeagueLevelID = mainLevel.ID,
                LeagueSeasonID = leagueSeason.ID,
                Participant1ID = mainParticipantID,
                Participant2ID = nextParticipantID,
                Number = 1,
                Name = string.Format("Стыковой матч {0} ({1})", mainLevel.Name, leagueSeason.Name),
                Status = (int)Match.MatchStatusEnum.Created,
            };

            CreateMatch(match);
            var league = leagueSeason.League;


            if (league.CountRound == 1)
            {
                var round = new Round
                {
                    MatchID = match.ID,
                    Number = 1,
                    Participant1ID = match.Participant1ID,
                    Participant2ID = match.Participant2ID,
                    ScheduleDate = leagueSeason.EndDate
                };

                CreateRound(round);
            }
            if (league.CountRound == 3)
            {
                //домашняя игра для 1го игрока
                var round1 = new Round
                {
                    MatchID = match.ID,
                    Participant1ID = match.Participant1ID,
                    Participant2ID = match.Participant2ID,
                    Number = 1,
                    Player1IsHost = league.HostGuest,
                    ScheduleDate = leagueSeason.EndDate
                };
                CreateRound(round1);
                var round2 = new Round
                {
                    MatchID = match.ID,
                    Participant1ID = match.Participant1ID,
                    Participant2ID = match.Participant2ID,
                    Number = 2,
                    Player1IsHost = false,
                    ScheduleDate = leagueSeason.EndDate
                };
                CreateRound(round2);
            }
            return true;
        }

        private void ProcessMatch(int idMatch)
        {
            var match = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (match != null)
            {
                if (match.Status != (int)Match.MatchStatusEnum.Submit)
                {
                    var allRoundSubmited = match.Rounds.Count(p => p.Status == (int)Round.RoundStatusEnum.Submit) == match.Rounds.Count();
                    //Проверяем если все матчи расписаны то (!) 
                    if (allRoundSubmited)
                    {
                        var totalParticipant1 = match.Rounds.Sum(p => p.Score1) + match.Rounds.Where(p => p.Technical).Sum(p => p.Score1 * 1000);
                        var totalParticipant2 = match.Rounds.Sum(p => p.Score2) + match.Rounds.Where(p => p.Technical).Sum(p => p.Score2 * 1000);
                        bool? winnerIsParticipant1 = null;
                        if (totalParticipant1 == totalParticipant2)
                        {
                            bool CreateAdditional = true;
                            // проверяем преимущество гола в гостях/дома
                            if (match.Tournament != null && match.Tournament.HostGuest && match.Tournament.DoubleGoalInGuest)
                            {
                                var newTotalParticipant1 = totalParticipant1 + match.Rounds.Where(p => !p.Player1IsHost).Sum(p => p.Score2);
                                var newTotalParticipant2 = totalParticipant2 + match.Rounds.Where(p => p.Player1IsHost).Sum(p => p.Score1);
                                if (newTotalParticipant1 != newTotalParticipant2)
                                {
                                    CreateAdditional = false;
                                    if (newTotalParticipant1 > newTotalParticipant2)
                                    {
                                        winnerIsParticipant1 = true;
                                    }
                                    else
                                    {
                                        winnerIsParticipant1 = false;
                                    }
                                }
                            }
                            if (CreateAdditional)
                            {
                                if (match.Tour == null || match.Tournament == null)
                                {
                                    CreateAdditionalRound(match.ID);
                                    return;
                                }
                                //необходимо ли дополнительный раунд
                                if ((match.Tour.TourType != (int)Tour.TourTypeEnum.RoundRobin && match.Tour.TourType != (int)Tour.TourTypeEnum.Group)/* || match.Tournament.HostGuest*/)
                                {
                                    //если да - то создаем 
                                    CreateAdditionalRound(match.ID);
                                    return;
                                }
                                if (!match.Tournament.IsRoundForPoints)
                                {
                                    CreateAdditionalRound(match.ID);
                                    return;
                                }
                            }
                        }
                        match.Score1 = match.Rounds.Sum(p => p.Score1);
                        match.Score2 = match.Rounds.Sum(p => p.Score2);
                        if (totalParticipant1 == totalParticipant2)
                        {
                            if (winnerIsParticipant1 == true)
                            {
                                match.WinnerID = match.Participant1ID;
                            }
                            if (winnerIsParticipant1 == false)
                            {
                                match.WinnerID = match.Participant2ID;
                            }
                        }
                        else
                        {
                            if (totalParticipant1 > totalParticipant2)
                            {
                                match.Winner = match.Player1;
                            }
                            else
                            {
                                match.Winner = match.Player2;
                            }
                        }
                        if (match.Rounds.Any(p => p.Technical))
                        {
                            match.Technical = true;
                        }
                        match.Status = (int)Match.MatchStatusEnum.Submit;
                        match.PublishedDate = DateTime.Now;
                        Db.Matches.Context.SubmitChanges();
                    }
                    ProcessWinner(match);

                    ProcessMatchRoaster(match);

                    if (match.Tour != null)
                    {
                        if (match.Tour.TourType == (int)Tour.TourTypeEnum.SingleElimination
                            || match.Tour.TourType == (int)Tour.TourTypeEnum.DoubleEliminationMaster
                            || match.Tour.TourType == (int)Tour.TourTypeEnum.DoubleEliminationSecondary)
                        {
                            SetNextMatchPlayers(match);
                        }
                    }
                    if (match.TournamentID.HasValue)
                    {
                        //проверяем на окончание игры
                        ProcessTournament(match.TournamentID.Value);
                    }
                }
            }
        }

        public bool RollbackMatch(int idMatch)
        {
            var cache = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (cache != null)
            {
                //откатить все раунды  
                RemoveMatchesRoundData(cache);

                //откатить матч 
                cache.Winner = null;
                cache.Score1 = null;
                cache.Score2 = null;
                cache.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                cache.Technical = false;
                Db.Matches.Context.SubmitChanges();

                //откатить все зависимые матчи
                RollbackMatches(cache.ID);
                return true;
            }
            return false;
        }

        private void RollbackMatches(int idMatch)
        {
            var match = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (match.WinMatchID.HasValue)
            {
                var winMatch = Db.Matches.FirstOrDefault(p => p.ID == match.WinMatchID.Value);
                if (winMatch.Status == (int)Match.MatchStatusEnum.Created || winMatch.Status == (int)Match.MatchStatusEnum.Submit || winMatch.Status == (int)Match.MatchStatusEnum.DefinedPlayers)
                {
                    //откатить 
                    winMatch.Score1 = null;
                    winMatch.Score2 = null;
                    winMatch.Status = (int)Match.MatchStatusEnum.Created;
                    winMatch.Technical = false;
                    //определить оставшихся игроков
                    ResetParticipant(winMatch);

                    //удалить данные по раундам 
                    RemoveMatchesRoundData(winMatch);

                    RollbackMatches(winMatch.ID);
                }
            }
            if (match.LoseMatchID.HasValue)
            {
                var loseMatch = Db.Matches.FirstOrDefault(p => p.ID == match.LoseMatchID.Value);
                if (loseMatch.Status == (int)Match.MatchStatusEnum.Created || loseMatch.Status == (int)Match.MatchStatusEnum.Submit || loseMatch.Status == (int)Match.MatchStatusEnum.DefinedPlayers)
                {
                    //откатить 
                    loseMatch.Score1 = null;
                    loseMatch.Score2 = null;
                    loseMatch.Status = (int)Match.MatchStatusEnum.Created;
                    loseMatch.Technical = false;
                    //определить оставшихся игроков
                    ResetParticipant(loseMatch);

                    RemoveMatchesRoundData(loseMatch);
                    RollbackMatches(loseMatch.ID);

                }
            }
        }

        private void ResetParticipant(Match match)
        {
            match.Player1 = null;
            match.Player2 = null;
            match.Winner = null;
            foreach (var subMatch in match.AllPreviousMatches)
            {
                if (subMatch.WinMatchID == match.ID && subMatch.WinnerID != null)
                {
                    if (match.Player1 == null)
                    {
                        match.Player1 = subMatch.Winner;
                    }
                    else
                    {
                        match.Player2 = subMatch.Winner;
                    }
                }
                if (subMatch.LoseMatchID == match.ID && subMatch.LoserID != null)
                {
                    if (match.Player1 == null)
                    {
                        match.Player1 = subMatch.Loser;
                    }
                    else
                    {
                        match.Player2 = subMatch.Loser;
                    }
                }
            }

            Db.Matches.Context.SubmitChanges();

        }

        private void RemoveMatchesRoundData(Match match)
        {
            foreach (var round in match.Rounds.ToList())
            {
                //удалить дополнительный раунд
                if (round.IsAdditional)
                {
                    Db.Rounds.DeleteOnSubmit(round);
                }
                else
                {
                    round.Status = (int)Round.RoundStatusEnum.Created;
                    round.Score1 = 0;
                    round.Score2 = 0;
                }
                Db.Rounds.Context.SubmitChanges();
            }
        }

        private void ProcessSingleMatch(int idMatch)
        {
            var match = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (match != null)
            {
                var allRoundSubmited = match.Rounds.Count(p => p.Status == (int)Round.RoundStatusEnum.Submit) == match.Rounds.Count();
                //Проверяем если все матчи расписаны то (!) 
                if (allRoundSubmited)
                {
                    var totalParticipant1 = match.Rounds.Sum(p => p.Score1);
                    var totalParticipant2 = match.Rounds.Sum(p => p.Score2);
                    bool? winnerIsParticipant1 = null;
                    if (totalParticipant1 == totalParticipant2)
                    {
                        //если да - то создаем 
                        CreateAdditionalRound(match.ID);
                        return;
                    }
                    match.Score1 = totalParticipant1;
                    match.Score2 = totalParticipant2;
                    if (totalParticipant1 == totalParticipant2)
                    {
                        if (winnerIsParticipant1 == true)
                        {
                            match.WinnerID = match.Participant1ID;
                        }
                        if (winnerIsParticipant1 == false)
                        {
                            match.WinnerID = match.Participant2ID;
                        }
                    }
                    else
                    {
                        if (totalParticipant1 > totalParticipant2)
                        {
                            match.WinnerID = match.Participant1ID;
                        }
                        else
                        {
                            match.WinnerID = match.Participant2ID;
                        }
                    }
                    match.Status = (int)Match.MatchStatusEnum.Submit;
                    match.PublishedDate = DateTime.Now;
                    Db.Matches.Context.SubmitChanges();

                    ProcessSingleWinner(match);
                }
            }
        }

        private void SetNextMatchPlayers(Match match)
        {
            if (match.WinMatch != null && match.Winner != null)
            {
                //process winner 
                if (match.WinMatch.Player1 == null)
                {
                    match.WinMatch.Player1 = match.Winner;
                    foreach (var round in match.WinMatch.Rounds)
                    {
                        round.Player1 = match.WinMatch.Player1;
                    }
                }
                else
                {
                    match.WinMatch.Player2 = match.Winner;
                    foreach (var round in match.WinMatch.Rounds)
                    {
                        round.Player2 = match.WinMatch.Player2;
                    }
                }
                if (match.WinMatch.Player1 != null && match.WinMatch.Player2 != null)
                {
                    match.WinMatch.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                }

                CreateWinNoticeSetPlayer(match);
               
            }
            if (match.LoseMatch != null && match.Loser != null)
            {
                //process loser 
                if (match.LoseMatch.Player1 == null)
                {
                    match.LoseMatch.Player1 = match.Loser;
                    foreach (var round in match.LoseMatch.Rounds)
                    {
                        round.Player1 = match.LoseMatch.Player1;
                    }
                }
                else
                {
                    match.LoseMatch.Player2 = match.Loser;
                    foreach (var round in match.LoseMatch.Rounds)
                    {
                        round.Player2 = match.LoseMatch.Player2;
                    }
                }
                if (match.LoseMatch.Player1 != null && match.LoseMatch.Player2 != null)
                {
                    match.LoseMatch.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                }
                CreateLoseNoticeSetPlayer(match);
            }
            else
            {
                CreateOffNoticeSetPlayer(match);
            }
            Db.Matches.Context.SubmitChanges();
        }

        private void CreateWinNoticeSetPlayer(Match match)
        {
            var notice = new Notice()
            {
                ReceiverID = match.Winner.UserID,
                Type = (int)Notice.TypeEnum.Match,
                MatchID = match.WinMatchID,
                Caption = "Победа",
                Text = string.Format("Вы проходите дальше в матч {0}", match.WinMatch.Name),
                IsCloseForRead = false
            };

            if (match.WinMatch.Status == (int)Match.MatchStatusEnum.DefinedPlayers)
            {
                notice.Text += "\r\n Ваш соперник: " + match.WinMatch.Rival(match.WinnerID.Value).ActualName;
            }
            CreateNotice(notice);
        }

        private void CreateLoseNoticeSetPlayer(Match match)
        {
            var notice = new Notice()
            {
                ReceiverID = match.Loser.UserID,
                Type = (int)Notice.TypeEnum.Match,
                MatchID = match.LoseMatchID,
                Caption = "Поражение",
                Text = string.Format("Вы проходите дальше в матч {0}", match.LoseMatch.Name),
                IsCloseForRead = false
            };

            if (match.LoseMatch.Status == (int)Match.MatchStatusEnum.DefinedPlayers)
            {
                notice.Text += "\r\n Ваш соперник: " + match.LoseMatch.Rival(match.Loser.ID).ActualName;
            }
            CreateNotice(notice);
        }

        private void CreateOffNoticeSetPlayer(Match match)
        {
            if (match.Loser != null)
            {
                var notice = new Notice()
                {
                    ReceiverID = match.Loser.UserID,
                    Type = (int)Notice.TypeEnum.Tournament,
                    TournamentID = match.TournamentID,
                    Caption = "Поражение",
                    Text = "Вы покидаете турнир",
                    IsCloseForRead = false
                };
                CreateNotice(notice);
            }
        }

        public void CreateOffMatchNoticeSetPlayers(Match match)
        {
            var leagueLevel = match.LeagueLevel;
            var notice = new Notice()
            {
                ReceiverID = match.Player1.UserID,
                Type = (int)Notice.TypeEnum.Match,
                MatchID = match.ID,
                Caption = "Стыковой матч лиги " + leagueLevel.Name,
                Text = string.Format("Вы вышли в стыковой матч лиги {0} уровень {1}. Ваш соперник {2}", leagueLevel.League.Name, leagueLevel.Name, match.Player2.ActualName),
                IsCloseForRead = false
            };
            CreateNotice(notice);
            notice = new Notice()
            {
                ReceiverID = match.Player2.UserID,
                Type = (int)Notice.TypeEnum.Match,
                MatchID = match.ID,
                Caption = "Стыковой матч лиги " + leagueLevel.Name,
                Text = string.Format("Вы вышли в стыковой матч лиги {0} уровень {1}. Ваш соперник {2}", leagueLevel.League.Name, leagueLevel.Name, match.Player1.ActualName),
                IsCloseForRead = false
            };
            CreateNotice(notice);
        }

        private void ProcessWinner(Match match)
        {
            if (match.Place != 0)
            {
                if (match.Place == 1)
                {
                    //определяем нужен ли еще один матч если фулл-дабл-елиминейшн
                    if (match.Tournament.TournamentType == (int)Tournament.TournamentTypeEnum.DoubleElimination && match.Place == 1 &&
                        match.Tour.TourType != (int)Tour.TourTypeEnum.DoubleEliminationSuperFinal)
                    {
                        var winner = Db.Users.FirstOrDefault(p => p.ID == match.WinnerID);
                        if (winner != null)
                        {
                            if (winner.HasLose(match.TournamentID.Value))
                            {
                                //Создаем еще 1 матч 
                                var tour = new Tour
                                {
                                    TournamentID = match.TournamentID.Value,
                                    TourType = (int)Tour.TourTypeEnum.DoubleEliminationSuperFinal,
                                    Name = "супер-финал - 2",
                                    RecommendDate = DateTime.Now
                                };
                                CreateTour(tour);
                                var number = match.Tournament.Matches.Max(p => p.Number) + 1;
                                var newMatch = new Match
                                {
                                    TournamentID = match.TournamentID,
                                    GameID = match.GameID,
                                    TourID = tour.ID,
                                    Player1 = match.Player1,
                                    Player2 = match.Player2,
                                    Number = number,
                                    Place = 1,
                                    Status = (int)Match.MatchStatusEnum.DefinedPlayers,
                                    Name = "Супер-финал"
                                };
                                CreateMatch(newMatch);

                                var newRound = new Round
                                {
                                    Participant1ID = newMatch.Participant1ID,
                                    Participant2ID = newMatch.Participant2ID,
                                    ScheduleDate = tour.RecommendDate,
                                    Status = (int)Round.RoundStatusEnum.Created,
                                    MatchID = newMatch.ID,
                                };
                                CreateRound(newRound);
                            }
                        }
                    }
                }
            }
        }

        private void ProcessSingleWinner(Match match)
        {
            var winner = Db.Participants.FirstOrDefault(p => p.ID == match.WinnerID);
            if (winner != null)
            {
                var loser = match.Loser;

                var ratingWinner = winner.User.Ratings.FirstOrDefault(p => p.GameID == match.GameID);
                var ratingLoser = loser.User.Ratings.FirstOrDefault(p => p.GameID == match.GameID);
                if (ratingWinner != null && ratingLoser != null)
                {
                    var diff = ratingLoser.Level - ratingWinner.Level;
                    var ratingDetail = new RatingDetail
                    {
                        RatingID = ratingWinner.ID,
                        MatchID = match.ID,
                        Score = diff + 20,
                        Description = "Победа в одиночном бою с " + loser.User.Login
                    };
                    CreateRatingDetail(ratingDetail);

                    var ratingLoserDetail = new RatingDetail
                    {
                        RatingID = ratingLoser.ID,
                        MatchID = match.ID,
                        Score = diff - 20,
                        Description = "Проигрыш в одиночном бою с " + winner.User.Login
                    };
                    CreateRatingDetail(ratingLoserDetail);
                }
            }
        }

        private void AwardUsers(Tournament tournament)
        {
            foreach (var match in tournament.Matches.Where(p => p.WinnerID != null && p.Participant1ID != null && p.Participant2ID != null && !p.Technical))
            {
                AddExpo(match, tournament, tournament.SingleWinPoint, match.Winner, "");
            }
            //ничьи
            if (tournament.SingleDrawPoint > 0)
            {
                foreach (var match in tournament.Matches.Where(p => p.WinnerID == null && p.Participant1ID != null && p.Participant2ID != null && !p.Technical))
                {
                    AddExpo(match, tournament, tournament.SingleDrawPoint, match.Player1, "");
                    AddExpo(match, tournament, tournament.SingleDrawPoint, match.Player2, "");
                }
            }
        }

        private void AwardPlayer(Match match, Tournament tournament, Award award, int? idParticipant)
        {
            if (award != null)
            {
                var player = Db.Participants.FirstOrDefault(p => p.ID == idParticipant);
                if (player != null)
                {
                    if (match != null)
                    {
                        award.MatchID = match.ID;
                    }
                    if (player.TeamID.HasValue)
                    {
                        if (match != null)
                        {
                            var list = Db.MatchRoasters.Where(p => p.MatchID == match.ID && p.TeamID == player.TeamID.Value);
                            foreach (var matchRoaster in list)
                            {
                                var userAward = new UserAward()
                                {
                                    AwardID = award.ID,
                                    UserID = matchRoaster.UserID,
                                };
                                CreateUserAward(userAward);
                            }
                        }
                        else
                        {
                            foreach (var userTeam in player.Team.UserTeams)
                            {
                                var userAward = new UserAward()
                                {
                                    AwardID = award.ID,
                                    UserID = userTeam.UserID,
                                };
                                CreateUserAward(userAward);
                            }
                        }
                        //награждаем в команду кубком
                        if (tournament.IsGroup)
                        {
                            var group = player.Team.Captain.GroupByGame(tournament.GameID);
                            if (group != null)
                            {
                                award.GroupID = group.ID;
                            }
                        }
                    }
                    else
                    {
                        var userAward = new UserAward()
                        {
                            AwardID = award.ID,
                            UserID = player.UserID,
                        };
                        CreateUserAward(userAward);

                        //награждаем в команду кубком
                        if (tournament.IsGroup)
                        {
                            var group = player.User.GroupByGame(match.GameID);
                            if (group != null)
                            {
                                award.GroupID = group.ID;
                            }

                        }
                    }
                    award.TeamID = player.TeamID;
                    award.AwardedDate = DateTime.Now;
                    Db.Awards.Context.SubmitChanges();
                    AwardMoney(tournament, award, player);
                    AddExpo(match, tournament, award.Point, player, string.Format("{0} (за {1} место)", tournament.Name, award.Place));
                }
            }
        }

        private void AwardMoney(Tournament tournament, Award award, Participant participant)
        {
            if (tournament.MoneyType != (int)Tournament.MoneyTypeEnum.Free)
            {
                var awardMoneyDetail = new MoneyDetail()
                {
                    TournamentID = tournament.ID,
                    SumGold = -(award.MoneyGoldPercent ?? 0) * tournament.SumFeeGold / 100,
                    SumWood = -(award.MoneyWood ?? 0),
                    SumCrystal = -(award.MoneyCrystal ?? 0)
                };

                MoneyFee moneyFee = null;
                if (tournament.IsGroup)
                {
                    moneyFee = MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentGroupAward);
                }
                else
                {
                    moneyFee = MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentUserAward);
                }

                var payMoneyDetail = new MoneyDetail()
                {
                    SumGold = (award.MoneyGoldPercent ?? 0) * tournament.SumFeeGold / 100,
                    SumWood = (award.MoneyWood ?? 0),
                    SumCrystal = (award.MoneyCrystal ?? 0),
                    Description = string.Format("Приз турнира {0} ({1})", tournament.Name, award.Description)
                };
                MoneyDetail moneyFeeMoneyDetail = null;
                if (moneyFee != null)
                {
                   moneyFeeMoneyDetail  = new MoneyDetail()
                    {
                        IsFee = true,
                        SumGold = moneyFee.PercentFee / 100 * awardMoneyDetail.SumGold
                    };
                    awardMoneyDetail.MoneyFeeID = moneyFee.ID;
                    payMoneyDetail.SumGold = payMoneyDetail.SumGold - moneyFeeMoneyDetail.SumGold;
                    
                }

                if (tournament.IsGroup)
                {
                    payMoneyDetail.GroupID = participant.Group.ID;
                }
                else
                {
                    payMoneyDetail.UserID = participant.UserID;
                }
                var guid = CreateTripleMoneyDetail(awardMoneyDetail, payMoneyDetail, moneyFeeMoneyDetail);
                SubmitMoney(guid);
            }
        }

        private void AddExpo(Match match, Tournament tournament, int point, Participant player, string description)
        {
            if (player.TeamID.HasValue)
            {
                if (match != null)
                {
                    var list = Db.MatchRoasters.Where(p => p.MatchID == match.ID && p.TeamID == player.TeamID.Value);
                    foreach (var matchRoaster in list)
                    {
                        AddExpoToUser(match, tournament, point, description, matchRoaster.User);
                    }
                }
                else
                {
                    foreach (var userTeams in player.Team.UserTeams)
                    {
                        AddExpoToUser(match, tournament, point, description, userTeams.User);
                    }
                }
            }
            else
            {
                AddExpoToUser(match, tournament, point, description, player.User);
            }
            if (tournament.IsGroup && player.Team != null)
            {
                AddExpoToGroup(match, tournament, point, description, player.Team.Group);
            }
        }

        private void AddExpoToUser(Match match, Tournament tournament, int point, string description, User user)
        {
            var rating = user.Ratings.FirstOrDefault(p => p.GameID == tournament.GameID);
            if (rating != null)
            {
                var ratingDetail = new RatingDetail
                {
                    RatingID = rating.ID,
                    Score = point,
                    Description = description
                };
                if (match != null)
                {
                    ratingDetail.MatchID = match.ID;
                }
                if (tournament != null)
                {
                    ratingDetail.TournamentID = tournament.ID;
                }

                CreateRatingDetail(ratingDetail);


            }
        }

        private void AddExpoToGroup(Match match, Tournament tournament, int point, string description, Group group)
        {
            var rating = group.TotalRating;
            if (rating != 0)
            {
                var groupRating = new GroupRating
                {
                    GroupID = group.ID,
                    Score = point,
                    Description = description
                };
                if (match != null)
                {
                    groupRating.MatchID = match.ID;
                }

                CreateGroupRating(groupRating);
            }
        }

        private void UnAwardUsers(Tournament tournament)
        {
            DiscardTournamentAwardMoney(tournament.ID);

            var list = tournament.RatingDetails.ToList();
            foreach (var rating in list)
            {
                var id = rating.RatingID;
                Db.RatingDetails.DeleteOnSubmit(rating);
                Db.RatingDetails.Context.SubmitChanges();
                UpdateRating(id);
            }

            foreach (var award in tournament.Awards)
            {
                award.Team = null;
                award.Match = null;
                award.AwardedDate = null;

                Db.Awards.Context.SubmitChanges();
                var userAwards =  award.UserAwards;
                Db.UserAwards.DeleteAllOnSubmit(userAwards);
                Db.UserAwards.Context.SubmitChanges();
            }
        }

        public bool EmptyMatch(int idMatch)
        {
            var match = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (match != null)
            {
                match.Status = (int)Match.MatchStatusEnum.Empty;
                Db.Matches.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool SingleParticipantMatch(int idMatch)
        {
            var match = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (match != null)
            {
                match.WinnerID = match.Participant1ID ?? match.Participant2ID;
                match.Status = (int)Match.MatchStatusEnum.Submit;
                match.PublishedDate = DateTime.Now;
                Db.Matches.Context.SubmitChanges();
                ProcessWinner(match);
                SetNextMatchPlayers(match);
                Db.Matches.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        private bool DefineParticipantInMatches(Participant participant, List<Match> startPlayoffMatches)
        {
            var firstParticipantMatch = startPlayoffMatches.FirstOrDefault(p => p.Participant1ID == null);
            if (firstParticipantMatch != null)
            {
                firstParticipantMatch.Participant1ID = participant.ID;

                var inDbMatch = Db.Matches.FirstOrDefault(p => p.ID == firstParticipantMatch.ID);
                inDbMatch.Participant1ID = participant.ID;

                foreach (var round in inDbMatch.Rounds)
                {
                    round.Participant1ID = participant.ID;
                }
                Db.Matches.Context.SubmitChanges();
                return true;
            }
            var secondParticipantMatch = startPlayoffMatches.FirstOrDefault(p => p.Participant2ID == null);
            if (secondParticipantMatch != null)
            {
                secondParticipantMatch.Participant2ID = participant.ID;
                var inDbMatch = Db.Matches.FirstOrDefault(p => p.ID == secondParticipantMatch.ID);
                inDbMatch.Participant2ID = participant.ID;
                foreach (var round in inDbMatch.Rounds)
                {
                    round.Participant2ID = participant.ID;
                }
                inDbMatch.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                Db.Matches.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool DropSingleMatchWithMessages(int idMatch)
        {
            var instance = Db.Matches.Where(p => p.ID == idMatch).FirstOrDefault();
            if (instance != null)
            {
                var list = Db.Notices.Where(p => p.MatchID == instance.ID).ToList();

                Db.Notices.DeleteAllOnSubmit(list);
                Db.Notices.Context.SubmitChanges();

                Db.Matches.DeleteOnSubmit(instance);
                Db.Matches.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        private void ProcessMatchRoaster(Match match)
        {
            if (match.Tournament != null && match.Tournament.IsTeam)
            {
                SaveTeam(match, match.Player1);
                SaveTeam(match, match.Player2);
            }
        }

        private void SaveTeam(Match match, Participant participant)
        {
            var team = participant.Team;

            if (team != null)
            {
                foreach (var user in team.SubUsers.Select(p => p.User))
                {
                    var matchRoaster = new MatchRoaster()
                    {
                        MatchID = match.ID,
                        TeamID = team.ID,
                        UserID = user.ID
                    };
                    CreateMatchRoaster(matchRoaster);
                }
            }
        }

    }
}