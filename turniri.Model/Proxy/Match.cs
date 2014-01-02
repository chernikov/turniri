using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using turniri.Tools;

namespace turniri.Model
{
    public partial class Match
    {
        public enum MatchStatusEnum
        {
            Created = 0x01,
            DefinedPlayers = 0x02,
            Submit = 0x03,
            Empty = 0x04,
            Closed = 0x05
        }

        public string MatchStatusName
        {
            get
            {
                switch ((MatchStatusEnum)Status)
                {
                    case MatchStatusEnum.Created:
                        return "Создан";
                    case MatchStatusEnum.DefinedPlayers:
                        return "Определены игроки";
                    case MatchStatusEnum.Submit:
                        return "Подтвержден";
                    case MatchStatusEnum.Empty:
                        return "Пустой";
                }
                return string.Empty;
            }
        }

        public bool IsStartedMatches
        {
            get
            {
                if (TournamentID.HasValue)
                {
                    if (Tournament.TournamentType == (int)Model.Tournament.TournamentTypeEnum.GroupTournament)
                    {
                        var groupMatches = Tournament.Matches.Count(p => p.TournamentGroupID.HasValue);
                        var count = MatchesMathLogic.GetMin2Pow(Tournament.GroupCount * Tournament.ExitFromGroup) / 2;
                        if (groupMatches < Number && Number <= count + groupMatches)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        var count = MatchesMathLogic.GetMin2Pow(Tournament.RegisteredPlayersCount) / 2;
                        if (Number <= count)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        //FIX: Rechange
        public Participant Player1
        {
            get
            {
                return Participant;
            }
            set
            {
                Participant = value;
            }
        }

        //FIX: Rechange
        public Participant Player2
        {
            get
            {
                return Participant1;
            }
            set
            {
                Participant1 = value;
            }
        }

        //FIX: Rechange
        public Participant Winner
        {
            get
            {
                return Participant2;
            }
            set
            {
                Participant2 = value;
            }
        }

        public int? LoserID
        {
            get
            {
                if (WinnerID.HasValue)
                {
                    if (WinnerID == Participant1ID)
                    {
                        return Participant2ID;
                    }
                    if (WinnerID == Participant2ID)
                    {
                        return Participant1ID;
                    }
                }
                return null;
            }
        }

        public Participant Loser
        {
            get
            {
                if (WinnerID.HasValue)
                {
                    if (WinnerID == Participant1ID)
                    {
                        return Player2;
                    }
                    if (WinnerID == Participant2ID)
                    {
                        return Player1;
                    }
                }
                return null;
            }
        }

        //FIX: Rechange
        public Match WinMatch
        {
            get { return Match2; }
            set { Match2 = value; }
        }

        //FIX: Rechange
        public Match LoseMatch
        {
            get { return Match1; }
            set { Match1 = value; }
        }

        public int CountRounds
        {
            get { return Rounds.Count; }
        }

        public string Score1View
        {
            get
            {

                if (Score1.HasValue)
                {
                    if (Tournament == null)
                    {
                        if (Game.GameType == (int)Game.GameTypeEnum.WinLose)
                        {
                            return Score1.Value > Score2.Value ? "В" : "П";
                        }
                        else
                        {
                            return Score1.Value.ToString();
                        }
                    }
                    else
                    {
                        if (Tournament.IsRoundForPoints)
                        {
                            return Score1.Value.ToString();
                        }
                        else
                        {
                            return Score1.Value > Score2.Value ? "В" : "П";
                        }
                    }
                }
                else
                {
                    return "--";
                }
            }
        }

        public string Score2View
        {
            get
            {

                if (Score2.HasValue)
                {
                    if (Tournament == null)
                    {
                        if (Game.GameType == (int)Game.GameTypeEnum.WinLose)
                        {
                            return Score2.Value > Score1.Value ? "В" : "П";
                        }
                        else
                        {
                            return Score2.Value.ToString();
                        }
                    }
                    else
                    {
                        if (Tournament.IsRoundForPoints)
                        {
                            return Score2.Value.ToString();
                        }
                        else
                        {
                            return Score2.Value > Score1.Value ? "В" : "П";
                        }
                    }
                }
                else
                {
                    return "--";
                }
            }
        }

        public bool IsAdmin(User user)
        {

            return Status != (int)Match.MatchStatusEnum.Created &&
           (user != null && (user.InRoles("admin") || user.AdminTournaments.Any(p => p.ID == TournamentID)));
        }

        public bool CanPublish(User user)
        {
            return Status != (int)Match.MatchStatusEnum.Created &&
                (user != null && (user.InRoles("admin")
                || user.ID == Player1.UserID
                || user.ID == Player2.UserID
                || user.AdminTournaments.Any(p => p.ID == TournamentID)));
        }

        public IEnumerable<Round> SubRounds
        {
            get
            {
                return Rounds.ToList();
            }
        }

        public int CommentCount
        {
            get { return MatchComments.Count; }
        }

        public bool HasCommentImage
        {
            get
            {
                return MatchComments.Where(p => !string.IsNullOrWhiteSpace(p.Comment.ImagePath)).Any();
            }
        }

        public bool HasCommentVideo
        {
            get
            {
                return MatchComments.Where(p => !string.IsNullOrWhiteSpace(p.Comment.VideoUrl)).Any();
            }
        }

        public IEnumerable<Comment> SubComments
        {
            get
            {
                return MatchComments.Select(p => p.Comment).Where(p => p.ParentID == null).OrderBy(p => p.AddedDate);
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (Tournament != null)
                {
                    if (AllPreviousMatches.Count == 0)
                    {
                        if (Participant1ID == null && Participant2ID == null)
                        {
                            return true;
                        }
                    }
                    if (AllPreviousMatches.ToList().Count(p => p.IsEmpty || p.IsSingleParticipant) == AllPreviousMatches.Count
                        && Participant1ID == null && Participant2ID == null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSingleParticipant
        {
            get
            {
                if ((Participant1ID != null && Participant2ID == null) || (Participant2ID != null && Participant1ID == null))
                {
                    if (AllPreviousMatches.ToList().Count(p => p.IsEmpty) == 1)
                    {
                        return true;
                    }
                    if (AllPreviousMatches.ToList().Count(p => p.Status == (int)Match.MatchStatusEnum.Submit) == AllPreviousMatches.Count)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public List<Match> AllPreviousMatches
        {
            get
            {
                return Matches.Union(Matches1).ToList();
            }
        }

        public Round Participant1HostRound
        {
            get
            {
                return Rounds.FirstOrDefault(p => !p.IsAdditional && p.Player1IsHost);
            }
        }

        public Round Participant2HostRound
        {
            get
            {
                return Rounds.FirstOrDefault(p => !p.IsAdditional && !p.Player1IsHost);
            }
        }

        public Round AdditionalRound
        {
            get
            {
                return Rounds.FirstOrDefault(p => p.IsAdditional);
            }
        }

        public Round Round1
        {
            get
            {
                return Rounds.Where(p => !p.IsAdditional).OrderBy(p => p.ID).FirstOrDefault();
            }
        }

        public Round Round2
        {
            get
            {
                return Rounds.Where(p => !p.IsAdditional).OrderBy(p => p.ID).Skip(1).FirstOrDefault();
            }
        }

        public Participant Rival(int idParticipant)
        {
            if (Participant1ID == idParticipant)
            {
                return Player2;
            }
            return Player1;
        }

        public bool IsIncoming(int idUser)
        {
            return Message != null && Message.ReceiverID == idUser;
        }

        public string Stage(int idUser)
        {
            switch (Status)
            {
                case (int)MatchStatusEnum.Created:
                    if (Message.SenderID == idUser)
                    {
                        return "Ожидание ответа соперника";
                    }
                    return "Ожидание вашего ответа";
                case (int)MatchStatusEnum.DefinedPlayers:
                    if (Rounds.Any(p => p.Status == (int)Round.RoundStatusEnum.Published))
                    {
                        if (Rounds.Any(p => p.Status == (int)Round.RoundStatusEnum.Published && p.IntroducedResultID == idUser))
                        {
                            return "Ожидание подтверждения результата соперником";
                        }
                        return "Ожидание вашего подтверждения результата";
                    }
                    if (Rounds.Any(p => p.Status == (int)Round.RoundStatusEnum.Disputed))
                    {
                        return "Результат оспорен";
                    }
                    return "Готовность к игре";
                case (int)MatchStatusEnum.Submit:
                    return "Матч разыгран";
                case (int)MatchStatusEnum.Closed:
                    return "Матч закрыт";
            }
            return string.Empty;
        }

        public string Result(int idUser)
        {
            if (Score1.HasValue && Score2.HasValue)
            {
                if (Score1 == Score2)
                {
                    return string.Format("Ничья ({0} : {1})", Score1, Score2);
                }
                if (Player1.UserID == idUser)
                {
                    if (Score1 > Score2)
                    {
                        return string.Format("Победа ({0} : {1})", Score1View, Score2View);
                    }
                    else
                    {
                        return string.Format("Поражение ({0} : {1})", Score1View, Score2View);
                    }
                }
                if (Player2.UserID == idUser)
                {
                    if (Score2 > Score1)
                    {
                        return string.Format("Победа ({0} : {1})", Score2View, Score1View);
                    }
                    else
                    {
                        return string.Format("Поражение ({0} : {1})", Score2View, Score1View);
                    }
                }
            }
            else if (Status == (int)Match.MatchStatusEnum.Submit && ((Player1 != null && Player1.UserID == idUser) || (Player2 != null && Player2.UserID == idUser)))
            {
                return "Автомат. победа";
            }
            return "- : -";

        }

        public bool IsMatchPlayer(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (user.ID == Player1.UserID || user.ID == Player2.UserID)
            {
                return true;
            }
            return false;
        }

        public string Desc
        {
            get
            {
                return (Player1 != null ? Player1.ActualName : " --") + " - " +
                        (Player2 != null ? Player2.ActualName : "-- ") + (TourID.HasValue ? " (" + Tour.Name + ") " : "");

            }
        }

        public IEnumerable<Camera> SubCameras
        {
            get
            {
                return Cameras.AsEnumerable();
            }
        }

        public IEnumerable<Camera> SubEnableCameras
        {
            get
            {
                return Cameras.Where(p => p.Enabled).AsEnumerable();
            }
        }

        public bool IsLive
        {
            get
            {
                return Cameras.Where(p => p.Enabled).Any();
            }
        }

        public RatingDetail Rating
        {
            get
            {
                return RatingDetails.FirstOrDefault();
            }
        }

        public string InfoSubmitClass
        {
            get
            {
               if (Rounds.Any(p => p.Status == (int)Round.RoundStatusEnum.Published || p.Status == (int)Round.RoundStatusEnum.Disputed)) 
               {
                   return "published";
               };
               if (Status == (int)Match.MatchStatusEnum.Submit)
               {
                   return "submit";
               }
               return string.Empty;
            }
        }
    }
}