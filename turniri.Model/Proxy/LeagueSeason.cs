using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{ 
    public partial class LeagueSeason
    {
        public enum StatusEnum : int
        {
            //Создан
            Created = 0x01,
            //Создан и заполнены все уровни лиги - участники + турниры (если участников нет, то и турниров не надо), все должны участвовать
            Prepared = 0x02,
            //Запуск (все турниры стартуют)
            InPlay = 0x03, 
            //Закончены турниры (создаются матчи) 4/4 - выбывают вниз, 3/4 - соревнуются с аналогичным количеством топ игроков в предыдущей лиге
            FinishedMainTour = 0x04,
            //Приготовлены стыковые матчи
            PreparedOffMatches = 0x05,
            //Играются стыковые матчи
            InPlayOffMatches = 0x06, 
            //Все места разыграны 
            Finished = 0x07
        }

        public bool AnyTournaments
        {
            get
            {
                return Tournaments.Any();
            }
        }

        public string StatusStr
        {
            get
            {
                switch ((StatusEnum)Status)
                {
                    case StatusEnum.Created :
                        return "Создан";
                    case StatusEnum.Prepared:
                        return "Приготовлен";
                    case StatusEnum.InPlay :
                        return "Играются турниры";
                    case StatusEnum.FinishedMainTour:
                        return "Турниры разыграны";
                    case StatusEnum.PreparedOffMatches:
                        return "Приготовлены стыковые";
                    case StatusEnum.InPlayOffMatches:
                        return "Играются стыковые";
                    case StatusEnum.Finished:
                        return "Завершен";
                }
                return string.Empty;
            }
        }

        public bool AnyUnregisteredParticipants
        {
            get
            {
                return LeagueParticipants.Any(p => p.Participant.TournamentID == null);
            }
        }

        public bool AnyMatches
        {
            get
            {
                return LeagueParticipants.Any(p => p.Participant.Tournament != null && p.Participant.Tournament.Matches.Any());
            }
        }

        public bool AllTournamentPlayed
        {
            get
            {
                var all = Tournaments.Count();
                var countPlayed = Tournaments.Count(p => p.Status == (int)Tournament.StatusEnum.PlayedOut);
                var countPlayed2 = Tournaments.Count(p => p.Status == (int)Tournament.StatusEnum.InGame && !p.Matches.Any(r => r.Status != (int)Match.MatchStatusEnum.Empty || r.Status != (int)Match.MatchStatusEnum.Submit));
                var count = Tournaments.Count(p => p.Status == (int)Tournament.StatusEnum.PlayedOut ||
                    p.Status == (int)Tournament.StatusEnum.InGame && !p.Matches.Any(r => r.Status != (int)Match.MatchStatusEnum.Empty || r.Status != (int)Match.MatchStatusEnum.Submit));
                return Tournaments.Count() == Tournaments.Count(p => p.Status == (int)Tournament.StatusEnum.PlayedOut || 
                    (p.Status == (int)Tournament.StatusEnum.InGame && !p.Matches.Any(r => r.Status != (int)Match.MatchStatusEnum.Empty && r.Status != (int)Match.MatchStatusEnum.Submit)));

            }
        }

        public bool AnyOffsMatches
        {
            get
            {
                return Matches.Any();
            }
        }

        public IEnumerable<Match> SubMatches
        {
            get
            {
                return Matches.AsEnumerable();
            }
        }
	}
}