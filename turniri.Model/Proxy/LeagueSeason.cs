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
            //������
            Created = 0x01,
            //������ � ��������� ��� ������ ���� - ��������� + ������� (���� ���������� ���, �� � �������� �� ����), ��� ������ �����������
            Prepared = 0x02,
            //������ (��� ������� ��������)
            InPlay = 0x03, 
            //��������� ������� (��������� �����) 4/4 - �������� ����, 3/4 - ����������� � ����������� ����������� ��� ������� � ���������� ����
            FinishedMainTour = 0x04,
            //������������ �������� �����
            PreparedOffMatches = 0x05,
            //�������� �������� �����
            InPlayOffMatches = 0x06, 
            //��� ����� ��������� 
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
                        return "������";
                    case StatusEnum.Prepared:
                        return "�����������";
                    case StatusEnum.InPlay :
                        return "�������� �������";
                    case StatusEnum.FinishedMainTour:
                        return "������� ���������";
                    case StatusEnum.PreparedOffMatches:
                        return "������������ ��������";
                    case StatusEnum.InPlayOffMatches:
                        return "�������� ��������";
                    case StatusEnum.Finished:
                        return "��������";
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