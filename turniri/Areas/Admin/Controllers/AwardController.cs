using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class AwardController : AdminController
    {
        public ActionResult Index(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                ViewBag.Tournament = tournament;
                var list = Repository.Awards.Where(p => p.TournamentID == id).OrderByDescending(p => p.Point);
                return View(list);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Create(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                ViewBag.Tournament = tournament;
                var awardView = new AwardView
                {
                    TournamentID = tournament.ID
                };
                return View("Edit", awardView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var award = Repository.Awards.FirstOrDefault(p => p.ID == id);

            if (award != null)
            {
                var awardView = (AwardView)ModelMapper.Map(award, typeof(Award), typeof(AwardView));
                return View(awardView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(AwardView awardView)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == awardView.TournamentID);

            bool EnableMoney = false;
            if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold && tournament.Fee > 0)
            {
                EnableMoney = true;
                var totalPercent = tournament.Awards.Where(p => p.ID != awardView.ID).Sum(p => p.MoneyGoldPercent);
                if (totalPercent >= 100)
                {
                    EnableMoney = false;
                }
                totalPercent = totalPercent + awardView.MoneyGoldPercent;
                if (EnableMoney && totalPercent > 100)
                {
                    ModelState.AddModelError("MoneyGoldPercent", "Процентов не должно быть в сумме больше 100, а то прогорим");
                }
            }
            if (!EnableMoney && awardView.MoneyGoldPercent > 0)
            {
                awardView.MoneyGoldPercent = 0;
            }
            if (ModelState.IsValid)
            {
                var award = (Award)ModelMapper.Map(awardView, typeof(AwardView), typeof(Award));
                if (award.ID == 0)
                {
                    Repository.CreateAward(award);
                }
                else
                {
                    Repository.UpdateAward(award);
                }
                return RedirectToAction("Index", new { id = award.TournamentID });
            }
            return View(awardView);
        }

        public ActionResult Delete(int id)
        {
            var award = Repository.Awards.FirstOrDefault(p => p.ID == id);
            if (award != null)
            {
                var tournamentId = award.TournamentID;
                Repository.RemoveAward(award.ID);
                return RedirectToAction("Index", new { id = tournamentId });
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        public ActionResult Award(int id)
        {
            var award = Repository.Awards.FirstOrDefault(p => p.ID == id);
            if (award != null)
            {
                var awardView = (AwardView)ModelMapper.Map(award, typeof(Award), typeof(AwardView));
                return View(awardView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Award(AwardView awardView)
        {
            Repository.AwardUser(awardView.ID, awardView.UserID.Value);
            return RedirectToAction("Index", new { id = awardView.TournamentID });
        }

        public ActionResult ReAward(int id)
        {
            Repository.ReAwardByPlaces(id);
            return RedirectToAction("Index", new { id });
        }

        public ActionResult ProcessTournamentWithMatchRatingDetails() 
        {
            foreach (var ratingDetail in Repository.RatingDetails)
            {
                Repository.UpdateRatingDetailTournament(ratingDetail.ID);
            }
            return Content("ok");
        }
    }
}
