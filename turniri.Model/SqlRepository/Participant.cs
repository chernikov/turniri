using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {

        public IQueryable<Participant> Participants
        {
            get
            {
                return Db.Participants;
            }
        }

        public bool CreateParticipant(Participant instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Participants.InsertOnSubmit(instance);
                Db.Participants.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool RegisterParticipant(int userID, int tournamentID)
        {
            var tournament = Db.Tournaments.FirstOrDefault(p => p.ID == tournamentID);

            var exist = Db.Participants.FirstOrDefault(p => p.ID == userID && p.TournamentID == tournamentID);
            if (exist == null && tournament != null)
            {
                var participant = new Participant
                                            {
                                                TournamentID = tournamentID,
                                                UserID = userID,
                                                AddedDate = DateTime.Now
                                            };
                Db.Participants.InsertOnSubmit(participant);
                Db.Participants.Context.SubmitChanges();

                var userGame = Db.UserGames.FirstOrDefault(p => p.GameID == tournament.GameID && p.UserID == userID);
                if (userGame == null)
                {
                    var newUserGame = new UserGame
                    {
                        GameID = tournament.GameID,
                        UserID = userID
                    };
                    CreateUserGame(newUserGame);
                }
                return true;
            }
            return false;
        }

        public bool RegisterParticipant(int userID, int tournamentID, int teamID)
        {
            var tournament = Db.Tournaments.FirstOrDefault(p => p.ID == tournamentID);

            var exist = Db.Participants.FirstOrDefault(p => p.ID == userID && p.TournamentID == tournamentID);
            if (exist == null && tournament != null)
            {
                var participant = new Participant
                {
                    TournamentID = tournamentID,
                    UserID = userID,
                    TeamID = teamID,
                    AddedDate = DateTime.Now
                };
                Db.Participants.InsertOnSubmit(participant);
                Db.Participants.Context.SubmitChanges();

                var userGame = Db.UserGames.FirstOrDefault(p => p.GameID == tournament.GameID && p.UserID == userID);
                if (userGame == null)
                {
                    var newUserGame = new UserGame
                    {
                        GameID = tournament.GameID,
                        UserID = userID
                    };
                    CreateUserGame(newUserGame);
                }
                return true;
            }
            return false;
        }

        public bool SetTeamInParticipant(int userID, int tournamentID, int teamID)
        {
            var cache = Db.Participants.FirstOrDefault(p => p.UserID == userID && p.TournamentID == tournamentID);
            if (cache != null)
            {
                cache.TeamID = teamID;
                Db.Participants.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ChangeParticipant(int idParticipant, int idUser)
        {
            var cache = Db.Participants.FirstOrDefault(p => p.ID == idParticipant);
            if (cache != null)
            {
                cache.UserID = idUser;
                Db.Participants.Context.SubmitChanges();
                return true;
            }

            return false;
        }


        public bool SaveParticipant(Participant instance)
        {
            var cache = Db.Participants.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.ImagePath18 = instance.ImagePath18;
                cache.ImagePath26 = instance.ImagePath26;
                cache.ImagePath30 = instance.ImagePath30;
                cache.Name = instance.Name;
                Db.Participants.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateParticipant(Participant instance)
        {
            var cache = Db.Participants.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.TournamentGroupID = instance.TournamentGroupID;
                cache.TournamentID = instance.TournamentID;
                Db.Participants.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateParticipants(int idTournament, List<int> users)
        {
            var tournament = Db.Tournaments.FirstOrDefault(p => p.ID == idTournament);
            if (tournament != null)
            {
                //remove others
                var listForDelete = tournament.Participants.Where(p => !users.Contains(p.UserID));
                var existList = tournament.Participants.Where(p => users.Contains(p.UserID)).Select(p => p.UserID).ToList();
                Db.Participants.DeleteAllOnSubmit(listForDelete);
                Db.Participants.Context.SubmitChanges();
                //new list
                var newUsers = users.Where(p => !existList.Contains(p)).Select(p => p);
                foreach (var id in newUsers)
                {
                    var user = Db.Users.FirstOrDefault(p => p.ID == id);

                    if (user != null)
                    {
                        int? teamID = null;
                        if (tournament.IsGroup && tournament.IsTeam)
                        {
                            var group = Db.UserGroups.FirstOrDefault(p => p.UserID == user.ID && p.Group.GameID == tournament.GameID);

                            if (group != null)
                            {
                                var team = new Team
                                {
                                    Name = group.Group.Name,
                                    ImagePath18 = group.Group.FullLogoPath18,
                                    ImagePath26 = group.Group.FullLogoPath26,
                                    ImagePath30 = group.Group.FullLogoPath30,
                                };

                                CreateTeam(team, tournament.HotReplacement);
                                teamID = team.ID;
                            }
                        }
                        var participant = new Participant
                        {
                            TournamentID = tournament.ID,
                            UserID = user.ID,
                            TeamID = teamID,
                            AddedDate = DateTime.Now
                        };
                        Db.Participants.InsertOnSubmit(participant);
                        Db.Participants.Context.SubmitChanges();
                    }
                }
                return true;
            }
            return false;
        }

        public bool UpdateLeagueParticipants(int idTournament, List<int> leagueParticipants)
        {
            var tournament = Db.Tournaments.FirstOrDefault(p => p.ID == idTournament);
            if (tournament != null)
            {
                var existLeagueParticipants = tournament.Participants.SelectMany(p => p.LeagueParticipants);
                var listForUnBind = existLeagueParticipants.Where(p => !leagueParticipants.Contains(p.ID));
                var listNoMove = existLeagueParticipants.Where(p => leagueParticipants.Contains(p.ID));
                var listNew = leagueParticipants.Where(p => !listNoMove.Any(r => r.ID == p));

                foreach (var item in listForUnBind)
                {
                    var cache = Db.Participants.FirstOrDefault(p => p.ID == item.ParticipantID);
                    if (cache != null)
                    {
                        cache.TournamentID = null;
                        Db.Participants.Context.SubmitChanges();
                    }
                }
                foreach (var item in listNew)
                {
                    var cache = Db.LeagueParticipants.FirstOrDefault(p => p.ID == item);
                    if (cache != null)
                    {
                        cache.Participant.TournamentID = tournament.ID;
                        /*var user = Db.Users.FirstOrDefault(p => p.ID == cache.Participant.UserID);
                        int? teamID = null;
                        if (tournament.IsGroup && tournament.IsTeam)
                        {
                            var group = Db.UserGroups.FirstOrDefault(p => p.UserID == user.ID && p.Group.GameID == tournament.GameID);
                            if (group != null)
                            {
                                var team = new Team
                                {
                                    Name = group.Group.Name,
                                    ImagePath18 = group.Group.FullLogoPath18,
                                    ImagePath26 = group.Group.FullLogoPath26,
                                    ImagePath30 = group.Group.FullLogoPath30,
                                };
                                CreateTeam(team, tournament.HotReplacement);
                                teamID = team.ID;
                            }
                        }

                        cache.Participant.TeamID = teamID;*/
                        Db.Participants.Context.SubmitChanges();
                    }
                }
                return true;
            }
            return false;
        }
        
        public bool ChangeMatchParticipant(int fromMatchId, bool fromMatchParticipant1, int toMatchId, bool toMatchParticipant1)
        {
            var fromMatch = Db.Matches.FirstOrDefault(p => p.ID == fromMatchId);
            var toMatch = Db.Matches.FirstOrDefault(p => p.ID == toMatchId);

            if (fromMatch != null && toMatch != null)
            {
                Participant fromPlayer = null;
                if (fromMatchParticipant1)
                {
                    fromPlayer = fromMatch.Player1;
                }
                else
                {
                    fromPlayer = fromMatch.Player2;
                }

                Participant toParticipant = null;
                if (toMatchParticipant1)
                {
                    toParticipant = toMatch.Player1;
                }
                else
                {
                    toParticipant = toMatch.Player2;
                }

                if (fromMatchParticipant1)
                {
                    fromMatch.Player1 = toParticipant;
                }
                else
                {
                    fromMatch.Player2 = toParticipant;
                }
                if (toMatchParticipant1)
                {
                    toMatch.Player1 = fromPlayer;
                }
                else
                {
                    toMatch.Player2 = fromPlayer;
                }
                Db.Matches.Context.SubmitChanges();
                foreach (var round in fromMatch.Rounds.ToList())
                {
                    round.Participant1ID = fromMatch.Participant1ID;
                    round.Participant2ID = fromMatch.Participant2ID;
                }

                foreach (var round in toMatch.Rounds.ToList())
                {
                    round.Participant1ID = toMatch.Participant1ID;
                    round.Participant2ID = toMatch.Participant2ID;
                }
                Db.Rounds.Context.SubmitChanges();
            }
            return true;
        }

        public bool ChangeGroupParticipants(int idParticipant1, int idParticipant2)
        {
            var matchesPlayer1IsParticipant1 = Db.Matches.Where(p => p.Participant1ID == idParticipant1).ToList();
            var matchesPlayer2IsParticipant1 = Db.Matches.Where(p => p.Participant2ID == idParticipant1).ToList();

            var matchesPlayer1IsParticipant2 = Db.Matches.Where(p => p.Participant1ID == idParticipant2).ToList();
            var matchesPlayer2IsParticipant2 = Db.Matches.Where(p => p.Participant2ID == idParticipant2).ToList();

            var participant1 = Db.Participants.FirstOrDefault(p => p.ID == idParticipant1);
            var participant2 = Db.Participants.FirstOrDefault(p => p.ID == idParticipant2);

            foreach (var match in matchesPlayer1IsParticipant1)
            {
                match.Participant1ID = idParticipant2;
                match.Name = string.Format("{0} - {1}", participant2.User.Login, match.Player2.User.Login);
                foreach (var round in match.Rounds.ToList())
                {
                    round.Participant1ID = idParticipant2;
                    Db.Rounds.Context.SubmitChanges();
                }
                Db.Matches.Context.SubmitChanges();

            }

            foreach (var match in matchesPlayer2IsParticipant1)
            {
                match.Participant2ID = idParticipant2;
                match.Name = string.Format("{0} - {1}", match.Player1.User.Login, participant2.User.Login);
                foreach (var round in match.Rounds.ToList())
                {
                    round.Participant2ID = idParticipant2;
                    Db.Rounds.Context.SubmitChanges();
                }
                Db.Matches.Context.SubmitChanges();
            }

            foreach (var match in matchesPlayer1IsParticipant2)
            {
                match.Participant1ID = idParticipant1;
                match.Name = string.Format("{0} - {1}", participant1.User.Login, match.Player2.User.Login);
                foreach (var round in match.Rounds.ToList())
                {
                    round.Participant1ID = idParticipant1;
                    Db.Rounds.Context.SubmitChanges();
                }
                Db.Matches.Context.SubmitChanges();
            }

            foreach (var match in matchesPlayer2IsParticipant2)
            {
                match.Participant2ID = idParticipant1;
                match.Name = string.Format("{0} - {1}", match.Player1.User.Login, participant1.User.Login);
                foreach (var round in match.Rounds.ToList())
                {
                    round.Participant2ID = idParticipant1;
                    Db.Rounds.Context.SubmitChanges();
                }
                Db.Matches.Context.SubmitChanges();
            }


            var groupID = participant1.TournamentGroupID;
            participant1.TournamentGroupID = participant2.TournamentGroupID;
            participant2.TournamentGroupID = groupID;

            Db.Participants.Context.SubmitChanges();
            return false;
        }

        /// <summary>
        /// Замена участника
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="participant1"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool SubstituteParticipant(int matchId, bool participant1, int userID)
        {
            var match = Db.Matches.FirstOrDefault(p => p.ID == matchId);

            if (match != null)
            {
                Participant fromPlayer;
                if (participant1)
                {
                    fromPlayer = match.Player1;
                }
                else
                {
                    fromPlayer = match.Player2;
                }

                var toUser = Db.Users.FirstOrDefault(p => p.ID == userID);

                if (toUser != null)
                {
                    fromPlayer.UserID = toUser.ID;
                    Db.Participants.Context.SubmitChanges();

                    var tournament = match.Tournament;
                    foreach (var subMatch in tournament.Matches.ToList())
                    {
                        if (subMatch.Tour != null && (subMatch.Tour.TourType == (int)Tour.TourTypeEnum.Group || subMatch.Tour.TourType == (int)Tour.TourTypeEnum.RoundRobin))
                        {
                            subMatch.Name = string.Format("{0} - {1}", subMatch.Player1.User.Login, subMatch.Player2.User.Login);
                        }
                        Db.Matches.Context.SubmitChanges();
                    }
                    return true;
                }
            }
            return false;
        }

        public bool RemoveParticipant(int idParticipant)
        {
            var instance = Db.Participants.FirstOrDefault(p => p.ID == idParticipant);
            if (instance != null)
            {
                if (instance.TeamID.HasValue)
                {
                    var teamID = instance.TeamID;
                    instance.TeamID = null;
                    Db.Participants.Context.SubmitChanges();
                    RemoveTeam(teamID.Value);
                }
                var roundList = instance.Rounds.Union(instance.Rounds1);
                Db.Rounds.DeleteAllOnSubmit(roundList);
                var matchList = instance.Matches.Union(instance.Matches1);
                Db.Matches.DeleteAllOnSubmit(matchList);
                Db.Participants.DeleteOnSubmit(instance);
                Db.Participants.Context.SubmitChanges();
                return true;
            }
            return false;
        }


    }
}