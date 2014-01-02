using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Round> Rounds
        {
            get
            {
                return Db.Rounds;
            }
        }

        public bool CreateRound(Round instance)
        {
            if (instance.ID == 0)
            {
                instance.Status = (int)Round.RoundStatusEnum.Created;
                instance.AddedDate = DateTime.Now;
                instance.Score1Text = string.Empty;
                instance.Score2Text = string.Empty;
                Db.Rounds.InsertOnSubmit(instance);
                Db.Rounds.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateRound(Round instance)
        {
            var cache = Db.Rounds.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Participant1ID = instance.Participant1ID;
                cache.Participant2ID = instance.Participant2ID;
                Db.Rounds.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool PublishRound(Round instance)
        {
            var cache = Db.Rounds.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Score1 = instance.Score1;
                cache.Score2 = instance.Score2;
                cache.IntroducedResultID = instance.IntroducedResultID;
                cache.PlayedDate = DateTime.Now;
                cache.Status = (int)Round.RoundStatusEnum.Published;
                cache.Extended = instance.Extended;
                Db.Rounds.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool DisputeRound(Round instance)
        {
            var cache = Db.Rounds.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Status = (int)Round.RoundStatusEnum.Disputed;
                Db.Rounds.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool SubmitRound(Round instance)
        {
            var cache = Db.Rounds.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null && cache.Status != (int)Round.RoundStatusEnum.Submit)
            {
                cache.Status = (int)Round.RoundStatusEnum.Submit;
                Db.Rounds.Context.SubmitChanges();

                /* Процессим матч */
                if (instance.Match.TournamentID != null)
                {
                    ProcessMatch(instance.MatchID);
                }
                else
                {
                    ProcessSingleMatch(instance.MatchID);
                }

                return true;
            }
            return false;
        }

        public bool TechSubmitRound(Round instance)
        {
            var cache = Db.Rounds.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                if (instance.Score1 > instance.Score2)
                {
                    cache.Score1 = 3;
                    cache.Score2 = 0;
                }
                else
                {
                    cache.Score1 = 0;
                    cache.Score2 = 3;
                }
                cache.Technical = true;
                cache.Status = (int)Round.RoundStatusEnum.Submit;
                Db.Matches.Context.SubmitChanges();
                ProcessMatch(cache.MatchID);
                return true;
            }
            return false;
        }

        private void CreateAdditionalRound(int idMatch)
        {
            var match = Db.Matches.FirstOrDefault(p => p.ID == idMatch);
            if (match != null)
            {
                var round = new Round
                {
                    MatchID = match.ID,
                    AddedDate = DateTime.Now,
                    Participant1ID = match.Participant1ID,
                    Participant2ID = match.Participant2ID,
                    IsAdditional = true,
                    Score1Text = string.Empty,
                    Score2Text = string.Empty,
                    Status = (int)Round.RoundStatusEnum.Created
                };
                if (match.Rounds.FirstOrDefault() != null)
                {
                    round.ScheduleDate = match.Rounds.First().ScheduleDate;
                }
                Db.Rounds.InsertOnSubmit(round);
                Db.Rounds.Context.SubmitChanges();
            }
        }

        public bool RemoveRound(int idRound)
        {
            Round instance = Db.Rounds.FirstOrDefault(p => p.ID == idRound);
            if (instance != null)
            {
                Db.Rounds.DeleteOnSubmit(instance);
                Db.Rounds.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RollbackRound(int idRound)
        {
            var cache = Db.Rounds.FirstOrDefault(p => p.ID == idRound);
            if (cache != null)
            {
                var match = cache.Match;
                //откатить раунд 
                cache.Status = (int)Round.RoundStatusEnum.Created;
                cache.Score1 = 0;
                cache.Score2 = 0;
                cache.Technical = false;
                cache.Extended = false;
                Db.Rounds.Context.SubmitChanges();

                //откатить матч этого раунда
                match.Winner = null;
                match.Score1 = null;
                match.Score2 = null;
                match.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                match.Technical = false;
                Db.Matches.Context.SubmitChanges();

                //если это был не дополнительный раунд - то удалить дополнительный раунд
                if (!cache.IsAdditional && match.AdditionalRound != null)
                {
                    var additionalRound = match.AdditionalRound;
                    Db.Rounds.DeleteOnSubmit(additionalRound);
                    Db.Rounds.Context.SubmitChanges();
                }
                //откатить все зависимые матчи
                RollbackMatches(match.ID);
                return true;
            }
            return false;
        }

    }
}