using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class MatchController : AdminController
    {
        [Authorize(Roles = "admin")]
        public ActionResult Unplayed()
        {
            var unplayedMatches = Repository.Matches.Where(p => p.MessageID != null && (p.Status == (int)Match.MatchStatusEnum.DefinedPlayers ||
                p.Status == (int)Match.MatchStatusEnum.Created) && p.Message.AddedDate < DateTime.Now.AddDays(-14));
            return View(unplayedMatches);
        }

        public ActionResult CountUnplayed()
        {
            var count = Repository.Matches.Count(p => p.MessageID != null && (p.Status == (int)Match.MatchStatusEnum.DefinedPlayers ||
                p.Status == (int)Match.MatchStatusEnum.Created) && p.Message.AddedDate < DateTime.Now.AddDays(-14));
            return View("Count", count);
        }

        public ActionResult Index()
        {
            IQueryable<Tournament> list = null;
            if (CurrentUser.InRoles("admin"))
            {
                var disputedRounds = Repository.Rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Disputed);
                return View(disputedRounds);
            }
            else if (CurrentUser.HasAdminTournament)
            {
                list = CurrentUser.AdminTournaments;
                var disputedRounds = list.SelectMany(p => p.Matches.SelectMany(r => r.Rounds)).Where(p => p.Status == (int)Round.RoundStatusEnum.Disputed);
                return View(disputedRounds);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Count()
        {
            IQueryable<Tournament> list = null;
            if (CurrentUser.InRoles("admin"))
            {
                return View("Count", Repository.Rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Disputed).Count());
            }
            else if (CurrentUser.HasAdminTournament)
            {
                list = CurrentUser.AdminTournaments;
                var disputedRoundsCount = list.SelectMany(p => p.Matches.SelectMany(r => r.Rounds)).Count(p => p.Status == (int)Round.RoundStatusEnum.Disputed);
                return View("Count", disputedRoundsCount);
            }
            return View("Count", 0);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var round = Repository.Rounds.FirstOrDefault(p => p.ID == id);

            if (round != null)
            {
                var roundView = (RoundView)ModelMapper.Map(round, typeof(Round), typeof(RoundView));
                ViewBag.Round = round;
                return View(roundView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(RoundView roundView)
        {

            var round = Repository.Rounds.FirstOrDefault(p => p.ID == roundView.ID);
            if (round != null)
            {

                if (round.Status == (int)Round.RoundStatusEnum.Submit)
                {
                    ModelState.AddModelError("Common", "Игра уже сохранена");
                }
                bool useAdditionalGame;
                if (!roundView.ProcessScoreForGameCategory((Game.GameCategoryEnum)round.Match.Game.GameCategory, out useAdditionalGame))
                {
                    ModelState.AddModelError("Common", "Неверный формат ввода");
                };

                if (roundView.Score1 < 0 || roundView.Score2 < 0)
                {
                    ModelState.AddModelError("Common", "Отрицательные числа нельзя ставить");
                }
                if (round.Match.Tournament != null)
                {
                    //победа поражение - должно быть выставлено
                    if (round.Match.Tournament != null && !round.Match.Tournament.IsRoundForPoints && roundView.Score1 == roundView.Score2)
                    {
                        ModelState.AddModelError("Common", "Выставьте победу одному из игроков");
                    }

                    //победа поражение - если играем 1 матч и это не в группе и не в круговом турнире
                    if (round.Match.Rounds.Count == 1 && (round.Match.Tournament.TournamentType != (int)Tournament.TournamentTypeEnum.RoundRobin && round.Match.Tour.TourType != (int)Tour.TourTypeEnum.Group && round.Match.Tour.TourType != (int)Tour.TourTypeEnum.RoundRobin) && roundView.Score1 == roundView.Score2)
                    {
                        ModelState.AddModelError("Common", "Выставьте победу одному из игроков");
                    }
                }
                else if (round.Match.CountRounds == 1 && roundView.Score1 == roundView.Score2)
                {
                    ModelState.AddModelError("Common", "Выставьте победу одному из игроков");
                }

                //дополнительная игра - не может быть ничьей
                if (round.IsAdditional && roundView.Score1 == roundView.Score2)
                {
                    ModelState.AddModelError("Common", "Выставьте победу одному из игроков");
                }
                //если игра с допонительным временем, то формат должен быть 2+0 и игра будет считаться дополнительной 
                // не для группового тура / не для туров RoundRobin
                if (!roundView.NeedAdditionalTimeFormat(round, useAdditionalGame))
                {
                    ModelState.AddModelError("Common", "Выставьте счет в формате дополнительного времени (например: 2+0) хотя бы для одного из игроков");
                }
                if (ModelState.IsValid)
                {
                    round.Score1 = roundView.Score1;
                    round.Score2 = roundView.Score2;
                    round.Score1Text = roundView.Score1Text;
                    round.Score2Text = roundView.Score2Text;
                    if (round.IsLast && useAdditionalGame)
                    {
                        round.Extended = true;
                    }
                    round.Technical = false;

                    round.IntroducedResultID = CurrentUser.ID;
                    round.PlayedDate = DateTime.Now;
                    Repository.PublishRound(round);
                    Repository.SubmitRound(round);
                    if (round.Match.Status == (int)Match.MatchStatusEnum.Submit)
                    {
                        SocialTool.ProcessSocialWinner(Repository, round.MatchID, HostName, Url, Server, Config);
                    }
                    return RedirectToAction("Index");
                }
                ViewBag.Round = round;
                return View(roundView);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteUnplayed(int id)
        {
            var unplayedMatch = Repository.Matches.FirstOrDefault(p => p.MessageID != null && (p.Status == (int)Match.MatchStatusEnum.DefinedPlayers ||
               p.Status == (int)Match.MatchStatusEnum.Created) && p.Message.AddedDate < DateTime.Now.AddDays(-14) && p.ID == id);

            if (unplayedMatch != null)
            {
                Repository.RemoveMatch(unplayedMatch.ID);
            }
            return RedirectToAction("Unplayed");
        }

        public ActionResult DeleteAllUnplayed()
        {
            var unplayedMatches = Repository.Matches.Where(p => p.MessageID != null && (p.Status == (int)Match.MatchStatusEnum.DefinedPlayers ||
               p.Status == (int)Match.MatchStatusEnum.Created) && p.Message.AddedDate < DateTime.Now.AddDays(-14));
            foreach (var match in unplayedMatches.ToList())
            {
                Repository.RemoveMatch(match.ID);
            }
            return RedirectToAction("Unplayed");
        }


    }
}
