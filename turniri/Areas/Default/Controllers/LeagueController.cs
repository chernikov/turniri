using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.Areas.Default.Controllers
{
    public class LeagueController : DefaultController
    {
        public ActionResult Index(string url)
        {
            var league = Repository.Leagues.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (league != null)
            {
                return View(league);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Level(int id)
        {
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == id);

            if (leagueLevel != null)
            {
                return View(leagueLevel);
            }
            return null;
        }


        public ActionResult Rating(int id, int seasonId)
        {
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == id);
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            if (leagueLevel != null && leagueSeason != null)
            {
                var participants = Repository.LeagueParticipants.Where(p => p.LeagueLevelID == leagueLevel.ID
                    && p.LeagueSeasonID == leagueSeason.ID).OrderBy(p => p.Place).ToList();
                return View(participants);
            }
            return null;
        }

        public ActionResult Matches(int id, int seasonId)
        {
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == id);
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);

            if (leagueLevel != null && leagueSeason != null)
            {
                if (leagueSeason.Status == (int)LeagueSeason.StatusEnum.InPlayOffMatches ||
                    leagueSeason.Status == (int)LeagueSeason.StatusEnum.Finished
                    )
                {
                    //matches
                    var matches = Repository.Matches.Where(p => p.LeagueLevelID == leagueLevel.ID &&
                        p.LeagueSeasonID == leagueSeason.ID);
                    return View(matches.ToList());
                }
            }
            return null;
        }

        public ActionResult Tournaments(int id, int seasonId)
        {
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == id);
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);

            if (leagueLevel != null && leagueSeason != null)
            {
                if (leagueSeason.Status != (int)LeagueSeason.StatusEnum.Created)
                {
                    //matches
                    var tournaments = Repository.Tournaments.Where(p => p.LeagueLevelID == leagueLevel.ID &&
                        p.LeagueSeasonID == leagueSeason.ID);
                    return View(tournaments.ToList());
                }
            }
            return null;
        }


       
                
    }
}
