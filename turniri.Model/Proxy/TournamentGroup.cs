using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class TournamentGroup
    {
       public int ParticipantsCount
        {
            get { return Participants.Count(); }
        }


       public IEnumerable<Participant> SubPlayers
        {
            get { return Participants.OrderBy(p => p.ID).ToList(); }
        }


        public int TotalGamed(int idUser, bool? HomeGuest = null)
        {
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant1ID == idUser && p.Player1IsHost) || (p.Participant2ID == idUser && !p.Player1IsHost)));
                }
                else
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant2ID == idUser && p.Player1IsHost) || (p.Participant1ID == idUser && !p.Player1IsHost)));
                }
            }
            else
            {
                return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant1ID == idUser || p.Participant2ID == idUser));
            }
        }
        public int TotalExp(int idUser, bool? HomeGuest = null)
        {
            return Matches.Count(p => p.Status == (int)Match.MatchStatusEnum.Submit && p.WinnerID == idUser) * Tournament.SingleWinPoint +
                Matches.Count(p => p.Status == (int)Match.MatchStatusEnum.Submit && p.WinnerID == null && (p.Participant1ID == idUser || p.Participant2ID == idUser)) * Tournament.SingleDrawPoint;
        }

        public int TotalWinGame(int idUser, bool? HomeGuest = null)
        {
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant1ID == idUser && p.Player1IsHost && p.Score1 > p.Score2) || (p.Participant2ID == idUser && !p.Player1IsHost && p.Score2 > p.Score1)));
                }
                else
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant2ID == idUser && p.Player1IsHost && p.Score2 > p.Score1) || (p.Participant1ID == idUser && !p.Player1IsHost && p.Score1 > p.Score2)));
                }
            }
            else
            {
                return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant1ID == idUser && p.Score1 > p.Score2) || (p.Participant2ID == idUser && p.Score2 > p.Score1)));
            }
        }

        public int TotalLoseGame(int idUser, bool? HomeGuest = null)
        {
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant1ID == idUser && p.Player1IsHost && p.Score1 < p.Score2) || (p.Participant2ID == idUser && !p.Player1IsHost && p.Score2 < p.Score1)));
                }
                else
                {
                    return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant2ID == idUser && p.Player1IsHost && p.Score2 < p.Score1) || (p.Participant1ID == idUser && !p.Player1IsHost && p.Score1 < p.Score2)));
                }
            }
            else
            {
                return Matches.SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant1ID == idUser && p.Score1 < p.Score2) || (p.Participant2ID == idUser && p.Score2 < p.Score1)));
            }
        }

        public int TotalDrawnGame(int idUser, bool? HomeGuest = null)
        {
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    return Matches.Where(p => !p.Technical).SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant1ID == idUser && p.Player1IsHost) || (p.Participant2ID == idUser && !p.Player1IsHost)) && p.Score1 == p.Score2);
                }
                else
                {
                    return Matches.Where(p => !p.Technical).SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && ((p.Participant2ID == idUser && p.Player1IsHost) || (p.Participant1ID == idUser && !p.Player1IsHost)) && p.Score1 == p.Score2);
                }
            }
            else
            {
                return Matches.Where(p => !p.Technical).SelectMany(p => p.Rounds).Count(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant1ID == idUser || p.Participant2ID == idUser) && p.Score1 == p.Score2);
            }
        }

        public int TotalPointWin(int idUser, bool? HomeGuest = null)
        {
            var rounds = Matches.SelectMany(p => p.Rounds);
            int score;
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant1ID == idUser && p.Player1IsHost)).Sum(p => p.Score1) + rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant2ID == idUser && !p.Player1IsHost)).Sum(p => p.Score2);
                }
                else
                {
                    score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant1ID == idUser && !p.Player1IsHost)).Sum(p => p.Score1) + rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant2ID == idUser && p.Player1IsHost)).Sum(p => p.Score2);
                }
            }
            else
            {
                score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant1ID == idUser)).Sum(p => p.Score1) +
                    rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant2ID == idUser)).Sum(p => p.Score2);
            }
            return score;
        }

        public int TotalPointLose(int idUser, bool? HomeGuest = null)
        {
            var rounds = Matches.SelectMany(p => p.Rounds);
            int score;
            if (HomeGuest.HasValue)
            {
                if (HomeGuest == true)
                {
                    score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant1ID == idUser && p.Player1IsHost)).Sum(p => p.Score2) + rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant2ID == idUser && !p.Player1IsHost)).Sum(p => p.Score1);
                }
                else
                {
                    score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant1ID == idUser && !p.Player1IsHost)).Sum(p => p.Score2) + rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant2ID == idUser && p.Player1IsHost)).Sum(p => p.Score1);
                }
            }
            else
            {
                score = rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant1ID == idUser)).Sum(p => p.Score2) +
                    rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Submit && (p.Participant2ID == idUser)).Sum(p => p.Score1);
            }
            return score;
        }

        public Match RoundMatch(int participant1ID, int participant2ID)
        {
            return Matches.FirstOrDefault(p => (p.Participant1ID == participant1ID && p.Participant2ID == participant2ID) || (p.Participant2ID == participant1ID && p.Participant1ID == participant2ID));
        }

	}
}