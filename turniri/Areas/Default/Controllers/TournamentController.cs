using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.Info;

namespace turniri.Areas.Default.Controllers
{
    public class TournamentController : DefaultController
    {
        public ActionResult All()
        {
            var liveTournaments = Repository.Tournaments.Where(p => !p.IsLive);
            return View(liveTournaments);
        }

        public ActionResult List(int type = 1, int page = 1)
        {
            ViewBag.Type = type;
            var list = Repository.Tournaments.Where(p => !p.IsLive);
            switch (type)
            {
                case 1:
                    list = list.Where(p => !p.LeagueSeasonID.HasValue && p.Status == (int)Tournament.StatusEnum.Created || p.Status == (int)Tournament.StatusEnum.Allocated).OrderByDescending(p => p.MoneyType).ThenByDescending(p => p.ID);
                    break;
                case 2:
                    list = list.Where(p => !p.LeagueSeasonID.HasValue && p.Status == (int)Tournament.StatusEnum.InGame).OrderByDescending(p => p.MoneyType).ThenByDescending(p => p.ID); ;
                    break;
                case 3:
                    list = list.Where(p => !p.LeagueSeasonID.HasValue && p.Status == (int)Tournament.StatusEnum.PlayedOut).OrderByDescending(p => p.ID);
                    break;
                case 4:
                    if (CurrentUser != null)
                    {
                        list = CurrentUser.AllTournaments;
                    }
                    else
                    {
                        return null;
                    }
                    break;
            }
            var data = new PageableData<Tournament>();

            data.Init(list, page, "List");
            return View(data);
        }

        public ActionResult Index(string platformUrl, string gameUrl, string url, int? matchID = null, int? groupID = null, bool? playOff = null, bool? rulesOn = null)
        {
            ViewBag.PlayOff = playOff;
            ViewBag.RulesOn = rulesOn;
            var tournament = Repository.Tournaments.FirstOrDefault(p => string.Compare(p.Url, url, true) == 0);
            if (tournament != null)
            {
                if (matchID != null)
                {
                    var match = Repository.Matches.FirstOrDefault(p => p.ID == matchID);
                    if (match != null)
                    {
                        ViewBag.MatchID = match.ID;
                        ViewBag.GroupID = match.TournamentGroupID;
                        if (match.Tournament.TournamentType == (int)Tournament.TournamentTypeEnum.GroupTournament)
                        {
                            ViewBag.PlayOff = match.Tour.IsPlayoffType ? (bool?)true : null;
                        }
                        else
                        {
                            ViewBag.PlayOff = null;
                        }
                    }
                }
                if (groupID != null)
                {
                    ViewBag.GroupID = groupID;
                }
                return View(tournament);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult UserRating(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null)
            {
                ViewBag.Game = tournament.Game;
                return View(tournament.Participants.ToList());
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Teams(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null)
            {
                return View(tournament.Participants.ToList());
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult StatisticRoundRobin(int id, bool? HomeGuest)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                var matches = tournament.Matches.Where(p => p.WinnerID != null);

                var statistics = new List<TournamentStatistic>();

                foreach (var participant in tournament.Participants)
                {
                    var statistic = new TournamentStatistic
                    {
                        Participant = participant,
                        TotalGamed = tournament.TotalGamed(participant.ID, HomeGuest),
                        Exp = tournament.TotalExp(participant.ID, HomeGuest),
                        WinCount = tournament.TotalWinGame(participant.ID, HomeGuest),
                        LoseCount = tournament.TotalLoseGame(participant.ID, HomeGuest),
                        DrawnCount = tournament.TotalDrawnGame(participant.ID, HomeGuest),
                        PointWinCount = tournament.TotalPointWin(participant.ID, HomeGuest),
                        PointLoseCount = tournament.TotalPointLose(participant.ID, HomeGuest)
                    };
                    statistics.Add(statistic);
                }

                ViewBag.ID = id;
                ViewBag.IsHomeGuest = tournament.HostGuest;
                ViewBag.Tournament = tournament;
                return View(statistics);
            }
            return null;
        }

        public ActionResult Group(int id)
        {
            var group = Repository.TournamentGroups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                return View(group);
            }
            return null;
        }

        public ActionResult GroupPart(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                return View(tournament);
            }
            return null;
        }

        public ActionResult PlayoffPart(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                return View(tournament);
            }
            return null;
        }

        public ActionResult StatisticGroup(int id, bool? HomeGuest)
        {
            var group = Repository.TournamentGroups.FirstOrDefault(p => p.ID == id);
            if (group != null)
            {
                var matches = group.Matches.Where(p => p.WinnerID != null);

                var statistics = new List<TournamentStatistic>();

                foreach (var participant in group.SubPlayers)
                {
                    var statistic = new TournamentStatistic
                    {
                        Participant = participant,
                        TotalGamed = group.TotalGamed(participant.ID, HomeGuest),
                        Exp = group.TotalExp(participant.ID, HomeGuest),
                        GuestPoint = group.TotalPointWin(participant.ID, false),
                        WinCount = group.TotalWinGame(participant.ID, HomeGuest),
                        LoseCount = group.TotalLoseGame(participant.ID, HomeGuest),
                        DrawnCount = group.TotalDrawnGame(participant.ID, HomeGuest),
                        PointWinCount = group.TotalPointWin(participant.ID, HomeGuest),
                        PointLoseCount = group.TotalPointLose(participant.ID, HomeGuest)
                    };
                    statistics.Add(statistic);
                }

                ViewBag.ID = id;
                ViewBag.IsHomeGuest = group.Tournament.HostGuest;
                return View(statistics);
            }
            return null;
        }

        public ActionResult SelectListTours(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                var list = new List<SelectListItem>();
                if (tournament.TournamentType == (int)Tournament.TournamentTypeEnum.RoundRobin)
                {
                    var groupTours = tournament.Tours.Where(p => p.TourType == (int)Tour.TourTypeEnum.Group || p.TourType == (int)Tour.TourTypeEnum.RoundRobin);
                    var count = groupTours.Count();
                    foreach (var tour in groupTours)
                    {
                        list.Add(new SelectListItem()
                        {
                            Value = tour.ID.ToString(),
                            Text = tour.Name,
                            Selected = false,
                        });
                    }
                    if (tournament.CountRound > 1)
                    {
                        foreach (var tour in groupTours)
                        {
                            var num = int.Parse(tour.Name) + count;
                            list.Add(new SelectListItem()
                            {
                                Value = "-" + tour.ID.ToString(),
                                Text = num.ToString(),
                                Selected = false,
                            });
                        }
                    }
                }
                else
                {
                    var tours = tournament.Tours.Where(p => p.Matches.Any());
                    foreach (var tour in tours)
                    {
                        list.Add(new SelectListItem()
                        {
                            Value = tour.ID.ToString(),
                            Text = tour.Name,
                            Selected = false,
                        });
                    }

                }
                return View(list);


            }
            return null;
        }

        public ActionResult Tours(int id)
        {
            if (id > 0)
            {
                var tour = Repository.Tours.FirstOrDefault(p => p.ID == id);
                ViewBag.Spin = 0;
                return View(tour);
            }
            else
            {
                var tour = Repository.Tours.FirstOrDefault(p => p.ID == -id);
                ViewBag.Spin = 1;
                return View(tour);
            }

        }

        public ActionResult SelectListGroups(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                var groups = tournament.TournamentGroups;

                var list = groups.Select(p => new SelectListItem
                {
                    Selected = false,
                    Value = p.ID.ToString(),
                    Text = p.Name
                });
                return View(list);
            }
            return null;
        }

        [Authorize]
        public ActionResult GetPart(int id, Guid? moneyGuid = null, bool force = false)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null && tournament.Status == (int)Tournament.StatusEnum.Created)
            {
                //убираем
                if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Free)
                {
                    return ProcessRegister(id, force, tournament);
                }

                if (moneyGuid != null)
                {
                    return ProcessRegister(id, force, tournament, moneyGuid);
                }
                else
                {
                    var participant = tournament.Participants.FirstOrDefault(p => p.UserID == CurrentUser.ID);
                    if (participant != null)
                    {
                        if (participant.IsTeam)
                        {
                            return Json(new
                            {
                                result = "return-group-money"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new
                            {
                                result = "return-money"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            result = "need-money"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new
            {
                result = "error",
                error = "Регистрация закрыта"
            }, JsonRequestBehavior.AllowGet);
        }

        private ActionResult ProcessRegister(int id, bool force, Tournament tournament, Guid? moneyGuid = null)
        {
            if (TryRemoveParticipant(tournament))
            {
                return Json(new
                {
                    result = "ok",
                    data = "remove",
                    count = tournament.Participants.Count()
                }, JsonRequestBehavior.AllowGet);
            }

            //Добавляет что мы играем в эту игру 
            if (!CurrentUserPlayInGame(tournament, force))
            {
                return Json(new
                {
                    result = "choise"
                }, JsonRequestBehavior.AllowGet);
            }

            //добавляем
            return TryRegisterUserOnTournament(id, tournament, moneyGuid);
        }

        private ActionResult TryRegisterUserOnTournament(int id, Tournament tournament, Guid? moneyGuid = null)
        {
            string condition = string.Empty;
            if (tournament.MaxPlayersCount > tournament.RegisteredPlayersCount)
            {
                if (tournament.CheckConditionsForRegister(CurrentUser, out condition))
                {
                    Repository.RegisterParticipant(CurrentUser.ID, id);
                    var count = Repository.Participants.Count(p => p.TournamentID == tournament.ID);

                    if (moneyGuid.HasValue && !Repository.SubmitMoney(moneyGuid.Value))
                    {
                        Repository.DiscardMoney(moneyGuid.Value);
                        return Json(new
                        {
                            result = "error",
                            error = "Проблема с оплатой"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new
                    {
                        result = "ok",
                        data = "register",
                        count
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (moneyGuid.HasValue)
                    {
                        Repository.DiscardMoney(moneyGuid.Value);
                    }
                    return Json(new
                    {
                        result = "error",
                        error = condition
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (moneyGuid.HasValue)
                {
                    Repository.DiscardMoney(moneyGuid.Value);
                }
                return Json(new
                {
                    result = "error",
                    error = "Все места уже заняты.",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool CurrentUserPlayInGame(Tournament tournament, bool force)
        {
            if (!CurrentUser.UserGames.Any(p => p.GameID == tournament.GameID))
            {
                if (force)
                {
                    Repository.CreateUserGame(new UserGame()
                    {
                        UserID = CurrentUser.ID,
                        GameID = tournament.GameID
                    });
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool TryRemoveParticipant(Tournament tournament)
        {
            var participant = tournament.Participants.FirstOrDefault(p => p.UserID == CurrentUser.ID);
            if (participant != null)
            {
                Repository.RemoveParticipant(participant.ID);
                return true;
            }
            return false;
        }

        public ActionResult FinishTournament(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.FinishTournament(tournament.ID);

                Repository.CloseAllCommentsInTournament(tournament.ID, Server.MapPath("/"));
            }

            return RedirectToAction("Index", new { id });
        }

        public ActionResult Start(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.StartTournament(tournament.ID);
            }
            return RedirectToAction("Index", new { id });
        }

        public ActionResult AllocatePlayoff(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.AllocatePlayoff(tournament.ID);
            }
            return RedirectToAction("Index", new { id });
        }

        public ActionResult AddGame(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null)
            {
                return View(tournament);
            }
            return View("_OK");
        }
    }
}
