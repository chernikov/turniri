using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{
    public partial class Round
    {
        public enum RoundStatusEnum
        {
            Created = 0x01,
            Published = 0x02,
            Submit = 0x03,
            Disputed = 0x04
        }

        partial void OnScore1Changing(int value)
        {
            Score1Text = value.ToString();
        }

        partial void OnScore2Changing(int value)
        {
            Score2Text = value.ToString();
        }

        public string RoundStatusName
        {
            get
            {
                switch ((RoundStatusEnum)Status)
                {
                    case RoundStatusEnum.Created:
                        return "Создан";
                    case RoundStatusEnum.Published:
                        return "Опубликован";
                    case RoundStatusEnum.Submit:
                        return "Подтвержден";
                    case RoundStatusEnum.Disputed:
                        return "Отспорен";
                }
                return string.Empty;
            }
        }

        public User IntroducerResult
        {
            get
            {
                return User;
            }
            set
            {
                User = value;
            }
        }

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

        public string Score1View
        {
            get
            {
                if (Status == (int)Round.RoundStatusEnum.Submit)
                {
                    return Match.Tournament == null || Match.Tournament.IsRoundForPoints ? Score1.ToString() : (Score1 > Score2 ? "В" : "П");
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
                if (Status == (int)Round.RoundStatusEnum.Submit)
                {
                    return Match.Tournament == null || Match.Tournament.IsRoundForPoints ? Score2.ToString() : (Score2 > Score1 ? "В" : "П");
                }
                else
                {
                    return "--";
                }
            }
        }

        public bool IsSomeAdmin(User user)
        {
            if (user == null)
            {
                return false;
            }

            return (Match.TournamentID.HasValue && user.IsTournamentAdmin(Match.TournamentID.Value))
                   || user.IsGameAdmin(Match.GameID);
        }

        public bool IsRoundParticipant(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (Player1.UserID == user.ID || Player2.UserID == user.ID)
            {
                return true;
            }
            return false;
        }

        public bool CanPublish(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (Status == (int)turniri.Model.Round.RoundStatusEnum.Created)
            {
                if (IsSomeAdmin(user) || IsRoundParticipant(user))
                {
                    return true;
                }
            }

            return false;
        }
       
        public bool CanSubmit(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (Status == (int)turniri.Model.Round.RoundStatusEnum.Published)
            {
               if (IsSomeAdmin(user))
                {
                    return true;
                }
                if (IsRoundParticipant(user) && user.ID != IntroducedResultID)
                {
                    return true;
                }
            }
            if (Status == (int)turniri.Model.Round.RoundStatusEnum.Disputed)
            {
                if (IsSomeAdmin(user))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanDispute(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (Status == (int)turniri.Model.Round.RoundStatusEnum.Published)
            {
                if (IsRoundParticipant(user) && user.ID != IntroducedResultID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsRoundForScore
        {
            get
            {
                if (Match != null)
                {
                    if (Match.TournamentID.HasValue)
                    {
                        return Match.Tournament.IsRoundForPoints;
                    }
                    else
                    {
                        return Match.Game.GameType == (int)turniri.Model.Game.GameTypeEnum.Points;
                    }
                }
                return false;
            }
        }

        public bool IsReadOnly(User user)
        {
            if (Status == (int)Round.RoundStatusEnum.Submit)
            {
                return true;
            }
            if (Status == (int)Round.RoundStatusEnum.Created)
            {
                return false;
            }
            if (IsSomeAdmin(user))
            {
                return false;
            }
            return true;
        }

        public string GameAdditionalLong
        {
            get
            {
                if (Match.Game.GameCategory == (int)Game.GameCategoryEnum.Soccer)
                {
                    return "Пенальти";
                }
                if (Match.Game.GameCategory == (int)Game.GameCategoryEnum.Common)
                {
                    return "Дополнительная игра";
                }
                return "";
            }

        }

        public string GameAdditionalMiddle
        {
            get
            {
                if (Match.Game.GameCategory == (int)Game.GameCategoryEnum.Soccer)
                {
                    return "Пенальти";
                }
                if (Match.Game.GameCategory == (int)Game.GameCategoryEnum.Common)
                {
                    return "Доп.игра";
                }
                return "";
            }
        }

        public string GameAdditionalShort
        {
            get
            {
                if (Match.Game.GameCategory == (int)Game.GameCategoryEnum.Soccer)
                {
                    return "пен";
                }
                return "";
            }
        }

        public bool IsLast
        {
            get
            {
                if (Match.Round1 != null && Match.Round2 != null) 
                {
                    if (Match.Round1.ID == ID && (Match.Round2.Status == (int)Round.RoundStatusEnum.Submit || Match.Round2.Status == (int)Round.RoundStatusEnum.Published))
                    {
                        return true;
                    }
                    if (Match.Round2.ID == ID && (Match.Round1.Status == (int)Round.RoundStatusEnum.Submit || Match.Round1.Status == (int)Round.RoundStatusEnum.Published))
                    {
                        return true;
                    }
                }
                return false;
               
            }
        }

    }
}