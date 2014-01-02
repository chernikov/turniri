using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;


namespace turniri.Models.ViewModels
{ 
	public class RoundView
    {
        public int ID { get; set; }

		public int? Participant1ID {get; set; }

		public int? Participant2ID {get; set; }

		public int MatchID {get; set; }

        public string MatchName { get; set; }

        public int MatchNumber { get; set; }

        public string MatchTournamentName { get; set; }

        public string MatchTourName { get; set; }

        public int MatchTournamentID { get; set; }

        public ParticipantView Player1 { get; set; }

        public ParticipantView Player2 { get; set; }

		public int Number {get; set; }

		public int Score1 {get; set; }

		public int Score2 {get; set; }

        public string Score1Text { get; set; }

        public string Score2Text { get; set; }

		public int? IntroducedResultID {get; set; }

		public string ScreenshotPath {get; set; }

		public int Status {get; set; }

		public DateTime PlayedDate {get; set; }

        public bool ProcessScoreForGameCategory(Game.GameCategoryEnum gameCategory, out bool additionalGame)
        {
            bool ok = true;
            var score = 0;
            additionalGame = false;
            switch (gameCategory)
            {
                case Game.GameCategoryEnum.Common:
                    additionalGame = false;
                    if (Int32.TryParse(Score1Text, out score))
                    {
                        Score1 = score;
                    }
                    else
                    {
                        ok = false;
                    }
                    if (Int32.TryParse(Score2Text, out score))
                    {
                        Score2 = score;
                    }
                    else
                    {
                        ok = false;
                    }
                    return ok;
                case Game.GameCategoryEnum.Soccer:
                    additionalGame = false;
                    if (Int32.TryParse(Score1Text, out score))
                    {
                        Score1 = score;
                    }
                    else
                    {
                        var temp = Score1Text.Trim().Split('+').ToList();
                        temp.ForEach(p => p = p.Trim());
                        var one = 0;
                        var two = 0;
                        if (temp.Count == 2 && Int32.TryParse(temp[0], out one) && Int32.TryParse(temp[1], out two))
                        {
                            additionalGame = true;
                            Score1 = one + two;
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    if (Int32.TryParse(Score2Text, out score))
                    {
                        Score2 = score;
                    }
                    else
                    {
                        var temp = Score2Text.Trim().Split('+').ToList();
                        temp.ForEach(p => p = p.Trim());
                        var one = 0;
                        var two = 0;
                        if (temp.Count == 2 && Int32.TryParse(temp[0], out one) && Int32.TryParse(temp[1], out two))
                        {
                            additionalGame = true;
                            Score2 = one + two;
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    return ok;
            }
            return false;
        }

        public bool NeedAdditionalTimeFormat(Round round, bool useAdditionalGame)
        {
            if (round.Match.TourID != null)
            {
                if (round.Match.Game.GameCategory == (int)Game.GameCategoryEnum.Soccer && round.Match.Tour.TourType != (int)Tour.TourTypeEnum.RoundRobin && round.Match.Tour.TourType != (int)Tour.TourTypeEnum.Group)
                {
                    //это второй раунд и не использована дополнительная игра
                    if (round.IsLast && !useAdditionalGame)
                    {
                        var match = round.Match;
                        var totalParticipant1 = match.Rounds.Where(p => p.ID != ID).Sum(p => p.Score1) + Score1;
                        var totalParticipant2 = match.Rounds.Where(p => p.ID != ID).Sum(p => p.Score2) + Score2;
                        if (totalParticipant1 == totalParticipant2)
                        {
                            if (match.Tournament != null && match.Tournament.HostGuest && match.Tournament.DoubleGoalInGuest)
                            {
                                var newTotalParticipant1 = totalParticipant1 + match.Rounds.Where(p => !p.Player1IsHost && p.ID != ID).Sum(p => p.Score2) + (!round.Player1IsHost ? Score2 : 0);
                                var newTotalParticipant2 = totalParticipant2 + match.Rounds.Where(p => p.Player1IsHost && p.ID != ID).Sum(p => p.Score1) + (round.Player1IsHost ? Score1 : 0);
                                if (newTotalParticipant1 == newTotalParticipant2)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return true;
        }
    }
}