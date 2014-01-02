using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{
    public partial class Tournament
    {
        public class StatisticGroup
        {
            public TournamentGroup TournamentGroup { get; set; }

            public int GuestPoints { get; set; }

            public Participant Participant { get; set; }

            public int Points { get; set; }

            public int Diff { get; set; }

            public int Win { get; set; }
        }

        public class LeagueStatisticGroup : StatisticGroup
        {
            public int LevelPoints { get; set; }

            public LeagueParticipant LeagueParticipant { get; set; }

            public LeagueLevel Level { get; set;  }

            
        }

        public enum MoneyTypeEnum
        {
            Free = 0x01,
            Wood = 0x02, 
            Gold = 0x03
        }

        private int number = 0;

        public enum StatusEnum : int
        {
            Created = 0x01,
            Allocated = 0x02,
            InGame = 0x03,
            PlayedOut = 0x04,
            Closed = 0x05
        }

        public enum TournamentTypeEnum : int
        {
            SingleElimination = 0x01,
            DoubleElimination = 0x02,
            RoundRobin = 0x03,
            GroupTournament = 0x04
        }

        public string StatusName
        {
            get
            {
                switch ((StatusEnum)Status)
                {
                    case StatusEnum.Created:
                        return "Регистрация";
                    case StatusEnum.Allocated:
                        return "Приготовлен";
                    case StatusEnum.InGame:
                        return "Играется";
                    case StatusEnum.PlayedOut:
                        return "Разыгран";
                    case StatusEnum.Closed:
                        return "Закрыт";
                }
                return null;
            }
        }

        public string TournamentTypeName
        {
            get
            {
                switch ((TournamentTypeEnum)TournamentType)
                {
                    case TournamentTypeEnum.SingleElimination:
                        return "Сингл-элиминейшн";
                    case TournamentTypeEnum.DoubleElimination:
                        return "Дабл-элиминейшн";
                    case TournamentTypeEnum.RoundRobin:
                        return "Каждый с каждым";
                    case TournamentTypeEnum.GroupTournament:
                        return "Групповой турнир + плейофф";
                }
                return null;
            }
        }

        public string MoneyTypeName 
        {
            get
            {
                if (MoneyType.HasValue)
                {
                    switch ((MoneyTypeEnum)MoneyType.Value)
                    {
                        case MoneyTypeEnum.Free :
                            return "без взноса";
                        case MoneyTypeEnum.Wood :
                            return "белый ТИ";
                        case MoneyTypeEnum.Gold :
                            return "золотой ТИ";
                    }
                }
                return "без взноса";
            }
        }

        public string FullImagePath
        {
            get 
            {
                return ImagePath + "?width=103&height=103&mode=crop";
            }
        }

        public int RegisteredPlayersCount
        {
            get { return Participants.Count; }
        }

        public int? MaxPlayersCount
        {
            get
            {
                if (TournamentType == (int)TournamentTypeEnum.GroupTournament)
                {
                    return GroupCount * PlayersInGroup;
                }
                return PlayersCount;
            }
        }

        public IEnumerable<Match> SubMatches
        {
            get { return Matches.ToList(); }
        }
        public bool AnyMatches
        {
            get { return Matches.Any(); }
        }

        public int MatchesCount
        {
            get { return Matches.Count; }
        }

        public int PlayedMatchesCount
        {
            get
            {
                return Matches.Count(p => p.Status == (int)Match.MatchStatusEnum.Submit || p.Status == (int)Match.MatchStatusEnum.Empty);
            }
        }

        public IEnumerable<TournamentGroup> SubGroups
        {
            get { return TournamentGroups.OrderBy(p => p.ID).ToList(); }
        }

        public IEnumerable<Participant> SubPlayers
        {
            get { return Participants.OrderBy(p => p.ID).ToList(); }
        }

        public string TakePartCondition
        {
            get
            {
                if (MaxLevel.HasValue)
                {
                    return string.Format("УР {0} - {1}", (MinLevel ?? 1), MaxLevel.Value);
                }
                if (MinLevel.HasValue)
                {
                    return string.Format("УР min{0}", MinLevel.Value);
                }
                return "нет";
            }
        }

        public Award FirstPlaceAward
        {
            get
            {
                return Awards.FirstOrDefault(p => p.Place == 1);
            }
        }

        public Award SecondPlaceAward
        {
            get
            {
                return Awards.FirstOrDefault(p => p.Place == 2);
            }
        }

        public Award ThirdPlaceAward
        {
            get
            {
                return Awards.FirstOrDefault(p => p.Place == 3);
            }
        }

        public void ResetMatch()
        {
            if (TournamentType == (int)Tournament.TournamentTypeEnum.GroupTournament)
            {
                number = Matches.Where(p => p.TournamentGroupID == null).OrderBy(p => p.Number).First().Number - 1;
            }
            else
            {
                number = 0;
            }
        }

        public Match NextMatch
        {
            get
            {
                number++;
                return Matches.FirstOrDefault(p => p.Number == number);
            }
        }

        public Match RoundMatch(int participant1ID, int participant2ID)
        {
            return Matches.FirstOrDefault(p => (p.Participant1ID == participant1ID && p.Participant2ID == participant2ID) || (p.Participant2ID == participant1ID && p.Participant1ID == participant2ID));
        }

        public bool IsGetPart(int idUser)
        {
            return Participants.Any(p => p.UserID == idUser);
        }

        public bool IsGetTeam(int idUser)
        {
            return Participants.Any(p => p.UserID == idUser && p.TeamID.HasValue);
        }

        public int TotalGamed(int idParticipant, bool? HomeGuest = null)
        {
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && ((p.Participant1ID == idParticipant && p.Player1IsHost)
                        || (p.Participant2ID == idParticipant && !p.Player1IsHost)));
                }
                else
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && ((p.Participant2ID == idParticipant && p.Player1IsHost)
                        || (p.Participant1ID == idParticipant && !p.Player1IsHost)));
                }
            }
            else
            {
                return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && (p.Participant1ID == idParticipant || p.Participant2ID == idParticipant));
            }
        }

        public int TotalExp(int idParticipant, bool? HomeGuest = null)
        {
            return Matches.Count(p => p.Status == (int)Match.MatchStatusEnum.Submit
                && p.WinnerID == idParticipant && !p.Technical) * SingleWinPoint
                + Matches.Count(p => p.Status == (int)Match.MatchStatusEnum.Submit
                && p.WinnerID == null && !p.Technical) * SingleDrawPoint;
        }

        public int TotalWinGame(int idParticipant, bool? HomeGuest = null)
        {
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && ((p.Participant1ID == idParticipant && p.Player1IsHost && p.Score1 > p.Score2)
                        || (p.Participant2ID == idParticipant && !p.Player1IsHost && p.Score2 > p.Score1)));
                }
                else
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && ((p.Participant2ID == idParticipant && p.Player1IsHost && p.Score2 > p.Score1)
                        || (p.Participant1ID == idParticipant && !p.Player1IsHost && p.Score1 > p.Score2)));
                }
            }
            else
            {
                return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && ((p.Participant1ID == idParticipant && p.Score1 > p.Score2)
                    || (p.Participant2ID == idParticipant && p.Score2 > p.Score1)));
            }
        }

        public int TotalLoseGame(int idParticipant, bool? HomeGuest = null)
        {
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && ((p.Participant1ID == idParticipant && p.Player1IsHost && p.Score1 < p.Score2)
                        || (p.Participant2ID == idParticipant && !p.Player1IsHost && p.Score2 < p.Score1)));
                }
                else
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && ((p.Participant2ID == idParticipant && p.Player1IsHost && p.Score2 < p.Score1)
                        || (p.Participant1ID == idParticipant && !p.Player1IsHost && p.Score1 < p.Score2)));
                }
            }
            else
            {
                return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && ((p.Participant1ID == idParticipant && p.Score1 < p.Score2)
                    || (p.Participant2ID == idParticipant && p.Score2 < p.Score1)));
            }
        }

        public int TotalDrawnGame(int idParticipant, bool? HomeGuest = null)
        {
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    return Matches.Where(p => !p.Technical).SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && ((p.Participant1ID == idParticipant && p.Player1IsHost) || (p.Participant2ID == idParticipant && !p.Player1IsHost))
                        && p.Score1 == p.Score2);
                }
                else
                {
                    return Matches.Where(p => !p.Technical).SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && ((p.Participant2ID == idParticipant && p.Player1IsHost) || (p.Participant1ID == idParticipant && !p.Player1IsHost))
                        && p.Score1 == p.Score2);
                }
            }
            else
            {
                return Matches.Where(p => !p.Technical).SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && (p.Participant1ID == idParticipant || p.Participant2ID == idParticipant)
                    && p.Score1 == p.Score2);
            }
        }

        public int TotalPointWin(int idParticipant, bool? HomeGuest = null)
        {
            var rounds = Matches.SelectMany(p => p.Rounds);
            int score;
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && (p.Participant1ID == idParticipant && p.Player1IsHost)).Sum(p => p.Score1)
                        + rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && (p.Participant2ID == idParticipant && !p.Player1IsHost)).Sum(p => p.Score2);
                }
                else
                {
                    score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && (p.Participant1ID == idParticipant && !p.Player1IsHost)).Sum(p => p.Score1)
                        + rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && (p.Participant2ID == idParticipant && p.Player1IsHost)).Sum(p => p.Score2);
                }
            }
            else
            {
                score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && (p.Participant1ID == idParticipant)).Sum(p => p.Score1) +
                    rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && (p.Participant2ID == idParticipant)).Sum(p => p.Score2);
            }
            return score;
        }

        public int TotalPointLose(int idParticipant, bool? HomeGuest = null)
        {
            var rounds = Matches.SelectMany(p => p.Rounds);
            int score;
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && (p.Participant1ID == idParticipant && p.Player1IsHost)).Sum(p => p.Score2)
                        + rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                        && (p.Participant2ID == idParticipant && !p.Player1IsHost)).Sum(p => p.Score1);
                }
                else
                {
                    score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && (p.Participant1ID == idParticipant && !p.Player1IsHost)).Sum(p => p.Score2)
                    + rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && (p.Participant2ID == idParticipant && p.Player1IsHost)).Sum(p => p.Score1);
                }
            }
            else
            {
                score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && (p.Participant1ID == idParticipant)).Sum(p => p.Score2) +
                    rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit
                    && (p.Participant2ID == idParticipant)).Sum(p => p.Score1);
            }
            return score;
        }

        public List<Tour> SelectableSubTours
        {
            get
            {
                return Tours.Where(p => p.TourType == (int)Tour.TourTypeEnum.Group || p.TourType == (int)Tour.TourTypeEnum.RoundRobin).ToList();
            }
        }

        /// <summary>
        /// Группы разыграны
        /// </summary>
        public bool IsGroupAllocated
        {
            get
            {
                var group = (Matches.Count(p => p.TournamentGroupID.HasValue)
                    == Matches.Count(p => p.TournamentGroupID.HasValue && p.Status == (int)Match.MatchStatusEnum.Submit)
                    && Matches.Count(p => p.TournamentGroupID.HasValue) > 0);
                var playoff = (Matches.Count(p => !p.TournamentGroupID.HasValue)
                    == Matches.Count(p => !p.TournamentGroupID.HasValue && p.Status == (int)Match.MatchStatusEnum.Created || p.Status == (int)Match.MatchStatusEnum.Empty));
                return group && playoff;
            }
        }

        public IEnumerable<Award> WinnerAwards
        {
            get
            {
                return Awards.Where(p => !p.IsSpecial && p.UserAwards.Any()).OrderBy(p => p.Place);
            }
        }

        public IEnumerable<Award> SpecialAwards
        {
            get
            {
                return Awards.Where(p => p.IsSpecial && p.UserAwards.Any());
            }
        }

        public bool Has2SuperGameTour
        {
            get
            {
                return Tours.Any(p => p.TourType == (int)Tour.TourTypeEnum.DoubleEliminationSuperFinal);
            }
        }

        public IQueryable<User> Admins
        {
            get
            {
                return UserRoleTournaments.Where(p => p.UserRole.Role.Code == "tournament_admin").Select(p => p.UserRole.User).AsQueryable();
            }
        }

        public IQueryable<User> AllAdmins
        {
            get
            {
                var list =  UserRoleTournaments.Where(p => p.UserRole.Role.Code == "tournament_admin").Select(p => p.UserRole.User).AsQueryable();
                list = list.Union(Game.UserRoleGames.Where(p => p.UserRole.Role.Code == "game_admin").Select(p => p.UserRole.User).AsQueryable());
                return list.Distinct();
            }
        }

        public IQueryable<User> Moderators
        {
            get
            {
                return UserRoleTournaments.Where(p => p.UserRole.Role.Code == "tournament_moderator").Select(p => p.UserRole.User).AsQueryable();
            }
        }

        public IQueryable<User> AllModerators
        {
            get
            {
                var list = UserRoleTournaments.Where(p => p.UserRole.Role.Code == "tournament_moderator").Select(p => p.UserRole.User).AsQueryable();
                list = list.Union(Game.UserRoleGames.Where(p => p.UserRole.Role.Code == "game_moderatory").Select(p => p.UserRole.User).AsQueryable());
                return list.Distinct();
            }
        }

        public bool CheckConditionsForRegister(User user, out string condition)
        {

            if (user != null)
            {
                //проверка на уровень игрока
                if (!user.CanGetPartPlace(this))
                {
                    condition = "Нет мест";
                    return false;
                }
                //проверка на уровень игрока
                if (!user.CanGetPartRating(this))
                {
                    condition = string.Format("Уровень игрока должен быть {0}", TakePartCondition);
                    return false;
                }
                //проверка на заполненность данных

                if (TournamentCondition != null)
                {
                    var fields = new List<string>();
                    if (TournamentCondition.FirstName && string.IsNullOrWhiteSpace(user.FirstName))
                    {
                        fields.Add("имя");
                    }
                    if (TournamentCondition.LastName && string.IsNullOrWhiteSpace(user.LastName))
                    {
                        fields.Add("фамилию");
                    }
                    if (TournamentCondition.ICQ && string.IsNullOrWhiteSpace(user.ICQ))
                    {
                        fields.Add("ICQ");
                    }
                    if (TournamentCondition.Skype && string.IsNullOrWhiteSpace(user.Skype))
                    {
                        fields.Add("Skype");
                    }
                    if (TournamentCondition.Vk && string.IsNullOrWhiteSpace(user.Vk))
                    {
                        fields.Add("Vk");
                    }
                    if (TournamentCondition.PlaystationID && string.IsNullOrWhiteSpace(user.PlaystationID))
                    {
                        fields.Add("PlaystationID");
                    }
                    if (TournamentCondition.XboxGametag && string.IsNullOrWhiteSpace(user.XboxGametag))
                    {
                        fields.Add("XBOX Gametag");
                    }
                    if (TournamentCondition.EAAccount && string.IsNullOrWhiteSpace(user.EAAccount))
                    {
                        fields.Add("EA Account");
                    }
                    if (TournamentCondition.SteamAccount && string.IsNullOrWhiteSpace(user.SteamAccount))
                    {
                        fields.Add("Steam Account");
                    }
                    if (TournamentCondition.GarenaAccount && string.IsNullOrWhiteSpace(user.GarenaAccount))
                    {
                        fields.Add("Garena Account");
                    }
                    if (fields.Any())
                    {
                        if (fields.Count() > 1)
                        {
                            condition = string.Format("Заполните в профиле {0} и {1} для того, чтобы принять участие", string.Join(", ", fields.Take(fields.Count() - 1)), fields.LastOrDefault());
                        }
                        else
                        {
                            condition = string.Format("Заполните в профиле {0} для того, чтобы принять участие", fields[0]);
                        }

                        return false;
                    }
                }
                condition = string.Empty;
                return true;
            }

            condition = string.Empty;
            return false;
        }


        public bool AllGamesPlayed
        {
            get
            {
                return Status != (int)Tournament.StatusEnum.PlayedOut
                    && Matches.Count > 0
                    && Matches.Count == Matches.Count(p => p.Status == (int)Match.MatchStatusEnum.Submit || p.Status == (int)Match.MatchStatusEnum.Empty);
            }
        }

        public bool IsAdmin(User user)
        {

            return (user != null && (user.InRoles("admin") || user.AdminTournaments.Any(p => p.ID == ID)));
        }

        public bool IsNeverPlayed(Participant participant)
        {
            return !Matches.SelectMany(p => p.Rounds).Any(p => (p.Player1 == participant || p.Player2 == participant) && p.Status == (int)Round.RoundStatusEnum.Submit)
                && !Matches.Any(p => (p.Player1 == participant || p.Player2 == participant) && p.Status == (int)Match.MatchStatusEnum.Submit);
        }

        public bool AllTeamClosed
        {
            get
            {
                if (IsTeam)
                {
                    var teams = Participants.Select(p => p.Team);
                    return teams.Count() == teams.Count(p => p.IsClosed);
                }
                return true;
            }
        }

        public string TeamNotClosed
        {
            get
            {
                if (IsTeam)
                {
                    var teams = Participants.Select(p => p.Team);

                    return string.Join (",", teams.Where(p => !p.IsClosed).Select(p => p.Name).ToList());
                }
                return string.Empty;
            }
        }

        public IEnumerable<Team> SubTeams
        {
            get
            {
                return Participants.Select(p => p.Team).AsEnumerable();
            }
        }

        public IEnumerable<Camera> SubEnableCameras
        {
            get
            {
                return Cameras.Where(p => p.Enabled).AsEnumerable();
            }
        }

        public IEnumerable<Camera> SubCameras
        {
            get
            {
                return Cameras.AsEnumerable();
            }
        }

        public IEnumerable<SocialPost> SubSocialPosts
        {
            get
            {
                return TournamentSocialPosts.Select(p => p.SocialPost);
            }
        }

        public double SumFeeGold
        {
            get
            {
                return MoneyDetails.Sum(p => p.SumGold);
            }
        }

    }
}