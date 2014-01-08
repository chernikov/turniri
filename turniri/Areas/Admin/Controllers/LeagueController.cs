using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;
using ImageResizer;
using turniri.Tools;
using System.IO;
using System.Drawing;
using turniri.Models.Info;


namespace turniri.Areas.Admin.Controllers
{
    public class LeagueController : AdminController
    {
        private static string ForumTopicSize = "ForumTopicSize";

        public ActionResult Index()
        {
            var list = Repository.Leagues.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var leagueView = new LeagueView();
            if (CurrentUser.InRoles("game_admin"))
            {
                leagueView.InitGameAdmin(CurrentUser);
            }
            return View("Edit", leagueView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var league = Repository.Leagues.FirstOrDefault(p => p.ID == id);

            if (league != null)
            {
                var leagueView = (LeagueView)ModelMapper.Map(league, typeof(League), typeof(LeagueView));
                if (CurrentUser.InRoles("game_admin"))
                {
                    leagueView.InitGameAdmin(CurrentUser);
                }
                return View(leagueView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(LeagueView leagueView)
        {
            if (ModelState.IsValid)
            {
                var league = (League)ModelMapper.Map(leagueView, typeof(LeagueView), typeof(League));

                var game = Repository.Games.FirstOrDefault(p => p.ID == leagueView.GameID);


                if (league.ID == 0)
                {
                    if (game != null)
                    {
                        var topicSizes = Config.IconSizes.FirstOrDefault(c => c.Name == ForumTopicSize);
                        var forumImagePath = "/Media/files/forum/" + StringExtension.GenerateNewFile() + ".jpg";
                        var forumImagePathGray = forumImagePath.GetPreviewPath("_g");
                        if (topicSizes != null)
                        {
                            var topicSize = new Size(topicSizes.Width, topicSizes.Height);
                            using (var fs = new FileStream(Server.MapPath(leagueView.Image), FileMode.Open))
                            {
                                ImageBuilder.Current.Build(fs, Server.MapPath(forumImagePath), new ResizeSettings(string.Format("width={0}&height={1}&scale=both&crop=auto", topicSize.Width, topicSize.Height)));
                            }
                            using (var fs = new FileStream(Server.MapPath(forumImagePath), FileMode.Open))
                            {
                                PreviewCreator.CreateAndSaveImage(fs, topicSize, Server.MapPath(forumImagePathGray), true);
                            }
                        }
                        var forum = new Forum()
                        {
                            Name = "Лига",
                            ImagePath = forumImagePath,
                            ParentID = game.ForumID,
                            IsEnd = false,
                            SubTitle = string.Format("Лига {0} по игре {1} ({2})", league.Name, game.Name, game.Platform.Name)
                        };
                        Repository.CreateForum(forum);
                        var forumMailLine = new Forum()
                        {
                            Name = "Основная тема",
                            ImagePath = forumImagePath,
                            ParentID = forum.ID,
                            IsEnd = true,
                            SubTitle = string.Format("Лига {0} по игре {1} ({2})", league.Name, game.Name, game.Platform.Name)
                        };
                        Repository.CreateForum(forumMailLine);
                        league.ForumID = forumMailLine.ID;
                    }
                    Repository.CreateLeague(league);
                }
                else
                {
                    Repository.UpdateLeague(league);
                }
                return RedirectToAction("Index");
            }
            return View(leagueView);
        }

        public ActionResult Delete(int id)
        {
            var league = Repository.Leagues.FirstOrDefault(p => p.ID == id);
            if (league != null)
            {
                Repository.RemoveLeague(league.ID);
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        #region Level
        public ActionResult Levels(int id)
        {
            var league = Repository.Leagues.FirstOrDefault(p => p.ID == id);
            if (league != null)
            {
                ViewBag.League = league;
                var list = league.LeagueLevels.ToList();
                return View(list);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult CreateLevel(int id)
        {
            var leagueLevelView = new LeagueLevelView()
            {
                LeagueID = id
            };
            return View("EditLevel", leagueLevelView);
        }

        [HttpGet]
        public ActionResult EditLevel(int id)
        {
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == id);

            if (leagueLevel != null)
            {
                var leagueLevelView = (LeagueLevelView)ModelMapper.Map(leagueLevel, typeof(LeagueLevel), typeof(LeagueLevelView));
                return View(leagueLevelView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult EditLevel(LeagueLevelView leagueLevelView)
        {
            if (leagueLevelView.Quantity % 4 != 0)
            {
                ModelState.AddModelError("Quantity", "Кол-во игроков в лиге должно быть кратно 4");
            }
            if (ModelState.IsValid)
            {
                var leagueLevel = (LeagueLevel)ModelMapper.Map(leagueLevelView, typeof(LeagueLevelView), typeof(LeagueLevel));
                if (leagueLevel.ID == 0)
                {
                    var league = Repository.Leagues.FirstOrDefault(p => p.ID == leagueLevel.LeagueID);

                    var topicSizes = Config.IconSizes.FirstOrDefault(c => c.Name == ForumTopicSize);
                    var forumImagePath = "/Media/files/forum/" + StringExtension.GenerateNewFile() + ".jpg";
                    var forumImagePathGray = forumImagePath.GetPreviewPath("_g");
                    if (topicSizes != null)
                    {
                        var topicSize = new Size(topicSizes.Width, topicSizes.Height);
                        using (var fs = new FileStream(Server.MapPath(leagueLevelView.Image), FileMode.Open))
                        {
                            ImageBuilder.Current.Build(fs, Server.MapPath(forumImagePath), new ResizeSettings(string.Format("width={0}&height={1}&scale=both&crop=auto", topicSize.Width, topicSize.Height)));
                        }
                        using (var fs = new FileStream(Server.MapPath(forumImagePath), FileMode.Open))
                        {
                            PreviewCreator.CreateAndSaveImage(fs, topicSize, Server.MapPath(forumImagePathGray), true);
                        }
                    }

                    var forum = new Forum()
                    {
                        Name = leagueLevel.Name,
                        ImagePath = forumImagePath,
                        ParentID = league.Forum.ParentID,
                        IsEnd = true,
                        SubTitle = string.Empty
                    };
                    Repository.CreateForum(forum);
                    leagueLevel.ForumID = forum.ID;
                    Repository.CreateLeagueLevel(leagueLevel);
                }
                else
                {
                    Repository.UpdateLeagueLevel(leagueLevel);
                }
                return RedirectToAction("Levels", new { id = leagueLevel.LeagueID });
            }
            return View(leagueLevelView);
        }

        public ActionResult DeleteLevel(int id)
        {
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == id);
            if (leagueLevel != null)
            {
                Repository.RemoveLeagueLevel(leagueLevel.ID);
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        #endregion

        #region Season
        public ActionResult Seasons(int id)
        {
            var league = Repository.Leagues.FirstOrDefault(p => p.ID == id);
            if (league != null)
            {
                ViewBag.League = league;
                var list = league.LeagueSeasons.OrderByDescending(p => p.ID).ToList();
                return View(list);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult CreateSeason(int id)
        {
            var leagueSeasonView = new LeagueSeasonView()
            {
                LeagueID = id
            };
            return View("EditSeason", leagueSeasonView);
        }

        [HttpGet]
        public ActionResult EditSeason(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);

            if (leagueSeason != null)
            {
                var leagueSeasonView = (LeagueSeasonView)ModelMapper.Map(leagueSeason, typeof(LeagueSeason), typeof(LeagueSeasonView));
                return View(leagueSeasonView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult EditSeason(LeagueSeasonView leagueSeasonView)
        {
            if (leagueSeasonView.StartDate < DateTime.Now)
            {
                ModelState.AddModelError("StartDate", "Дата должна быть больше сегодняшней");
            }
            if (leagueSeasonView.EndMainTourDate < leagueSeasonView.StartDate)
            {
                ModelState.AddModelError("EndMainTourDate", "Дата окончания основных турниров должна быть больше стартовой");
            }
            if (leagueSeasonView.EndDate < leagueSeasonView.EndMainTourDate)
            {
                ModelState.AddModelError("EndDate", "Дата окончания должна быть больше окончания основных турниров");
            }

            var last = Repository.LeagueSeasons.Where(p => p.LeagueID == leagueSeasonView.LeagueID && p.ID != leagueSeasonView.ID).OrderByDescending(p => p.EndDate).FirstOrDefault();
            if (last != null && last.EndDate >= leagueSeasonView.StartDate)
            {
                ModelState.AddModelError("StartDate", "Дата должна быть больше даты окончания предыдущего сезона");
            }
            if (ModelState.IsValid)
            {
                var leagueSeason = (LeagueSeason)ModelMapper.Map(leagueSeasonView, typeof(LeagueSeasonView), typeof(LeagueSeason));
                if (leagueSeason.ID == 0)
                {
                    Repository.CreateLeagueSeason(leagueSeason);
                }
                else
                {
                    Repository.UpdateLeagueSeason(leagueSeason);
                }
                return RedirectToAction("Seasons", new { id = leagueSeason.LeagueID });
            }
            return View(leagueSeasonView);
        }

        public ActionResult DeleteSeason(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                Repository.RemoveLeagueSeason(leagueSeason.ID);
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult FillSeason(int id)
        {
            var currentSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (currentSeason != null)
            {
                //распределить по лигам
                var lastSeason = Repository.LeagueSeasons.Where(p => p.LeagueID == currentSeason.LeagueID && p.ID < id).OrderByDescending(p => p.ID).FirstOrDefault();


                if (lastSeason != null && lastSeason.Status == (int)LeagueSeason.StatusEnum.Finished)
                {
                    //работаем
                    var participantList = lastSeason.LeagueParticipants.OrderBy(p => p.LeagueLevel.Level).ThenBy(p => p.Place).ToList();
                    foreach (var participant in participantList)
                    {
                        var newParticipant = new Participant()
                        {
                            UserID = participant.Participant.UserID,
                        };
                        Repository.CreateParticipant(newParticipant);

                        var leagueParticipant = new LeagueParticipant()
                        {
                            LeagueLevelID = participant.LeagueLevelID,
                            LeagueSeasonID = currentSeason.ID,
                            ParticipantID = newParticipant.ID
                        };
                        Repository.CreateLeagueParticipant(leagueParticipant);
                    }
                }
                return RedirectBack(RedirectToAction("Index"));
            }

            return RedirectToNotFoundPage;

        }
        #endregion

        #region Groups
        public ActionResult Groups(int id)
        {
            var league = Repository.Leagues.FirstOrDefault(p => p.ID == id);
            if (league != null)
            {
                ViewBag.League = league;
                if (league.LastSeason.Status == (int)LeagueSeason.StatusEnum.FinishedMainTour ||
                    league.LastSeason.Status == (int)LeagueSeason.StatusEnum.Finished)
                {
                    var list = league.LastSeason.LeagueParticipants.OrderBy(p => p.LeagueLevel.Level).ThenBy(p => p.Place).Select(p => p.Participant.User.Groups.FirstOrDefault(r => r.GameID == league.GameID)).ToList();
                    return View(list);

                }
                else
                {
                    var list = league.Game.Groups.OrderBy(p => p.TotalRating).ToList();
                    return View(list);
                }
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult GroupRow(int id, int seasonId)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null && group != null)
            {
                ViewBag.League = leagueSeason.League;
                return View(group);
            }
            return null;
        }

        public ActionResult SelectListGroupLeagueLevel(int id, int seasonId)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null && group != null)
            {
                ViewBag.ID = id;
                var participant = Repository.LeagueParticipants.FirstOrDefault(p => group.UserID == p.Participant.UserID && p.LeagueSeasonID == leagueSeason.ID);
                var selectList = new List<SelectListItem>();
                selectList.Add(new SelectListItem()
                {
                    Value = "",
                    Text = "Не участвует в лиге",
                    Selected = participant == null
                });
                foreach (var level in leagueSeason.League.SubLevels)
                {
                    selectList.Add(new SelectListItem()
                    {
                        Value = level.ID.ToString(),
                        Text = level.Name,
                        Selected = participant != null && participant.LeagueLevelID == level.ID
                    });
                }
                return View(selectList);
            }
            return null;
        }

        public ActionResult SetGroupLeagueParticipant(int id, int? levelId, int seasonId)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);

            if (leagueSeason != null && group != null)
            {
                var leagueParticipant = Repository.LeagueParticipants
                    .FirstOrDefault(p => p.LeagueSeasonID == leagueSeason.ID
                        && group.UserID == p.Participant.UserID);

                if (leagueParticipant != null && levelId == null)
                {
                    int? participantID = null;
                    int? teamID = null;
                    if (leagueParticipant.Participant != null && !leagueParticipant.Participant.Matches.Any() && !leagueParticipant.Participant.TournamentID.HasValue)
                    {
                        participantID = leagueParticipant.Participant.ID;

                        teamID = leagueParticipant.Participant.TeamID;
                    }
                    Repository.RemoveLeagueParticipant(leagueParticipant.ID);
                    if (participantID.HasValue)
                    {
                        
                        Repository.RemoveParticipant(participantID.Value);
                    }
                    if (teamID.HasValue)
                    {
                        Repository.RemoveTeam(teamID.Value);
                    }
                    return Json(new { result = "ok" });
                }

                if (levelId != null)
                {
                    var level = Repository.LeagueLevels.FirstOrDefault(p => p.ID == levelId.Value);

                    if (level != null)
                    {
                        //нечего менять
                        if (leagueParticipant != null && leagueParticipant.LeagueLevelID == level.ID)
                        {
                            return Json(new { result = "ok" });
                        }

                        //есть ли места?
                        if (level.Quantity <= level.LeagueParticipants.Count(p => p.LeagueSeasonID == leagueSeason.ID))
                        {
                            return Json(new { result = "error", message = "Все места в уровне " + level.Name + " уже разпределены" });
                        }
                        else
                        {
                            //места есть
                            if (leagueParticipant != null)
                            {
                                leagueParticipant.LeagueLevelID = level.ID;
                                Repository.UpdateLeagueParticipant(leagueParticipant);
                            }
                            else
                            {
                                //team 
                                var team = new Team()
                                {
                                    Name = group.Name,
                                    ImagePath18 = group.FullLogoPath18,
                                    ImagePath26 = group.FullLogoPath26,
                                    ImagePath30 = group.FullLogoPath30,
                                };
                                Repository.CreateTeam(team, level.League.HotReplacement);
                                // создаем игрока
                                var participant = new Participant()
                                {
                                    UserID = group.UserID,
                                    TeamID = team.ID
                                };
                                Repository.CreateParticipant(participant);

                                // создаем участника в лиге
                                var newLeagueParticipant = new LeagueParticipant()
                                {
                                    LeagueLevelID = level.ID,
                                    LeagueSeasonID = leagueSeason.ID,
                                    ParticipantID = participant.ID
                                };
                                Repository.CreateLeagueParticipant(newLeagueParticipant);
                            }
                        }
                    }
                    return Json(new { result = "ok" });
                }
            }
            return Json(new { result = "error", message = "Не удалось изменить" });
        }

        public ActionResult SelectListGroupTournament(int id, int seasonId, int? levelId)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == levelId);
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            ViewBag.НasLevel = false;
            if (leagueSeason != null && group != null)
            {
                var user = group.User;
                ViewBag.ID = id;
                var selectList = new List<SelectListItem>();
                if (leagueLevel != null)
                {
                    ViewBag.НasLevel = true;
                    ViewBag.LevelID = leagueLevel.ID;
                    var leagueParticipant = Repository.LeagueParticipants.FirstOrDefault(p => user.ID == p.Participant.UserID && p.LeagueSeasonID == leagueSeason.ID);
                    var tournaments = Repository.Tournaments.Where(p => p.LeagueLevelID == leagueLevel.ID && p.LeagueSeasonID == leagueSeason.ID);
                    selectList.Add(new SelectListItem()
                    {
                        Value = "",
                        Text = "Не участвует в турнире",
                        Selected = leagueParticipant == null || !leagueParticipant.Participant.TournamentID.HasValue
                    });
                    if (tournaments.Any())
                    {
                        foreach (var tournament in tournaments)
                        {
                            selectList.Add(new SelectListItem()
                            {
                                Value = tournament.ID.ToString(),
                                Text = tournament.Name,
                                Selected = leagueParticipant != null && leagueParticipant.Participant.TournamentID == tournament.ID
                            });
                        }
                    }
                }
                else
                {
                    selectList.Add(new SelectListItem()
                    {
                        Value = "",
                        Text = "Не участвует в лиге",
                        Selected = true
                    });
                    return View(selectList);
                }
                return View(selectList);
            }
            return null;
        }

        public ActionResult SetLeagueGroupParticipantTournament(int id, int seasonId, int levelId, int? tournamentId)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == levelId);

            if (group != null && leagueSeason != null && leagueLevel != null)
            {
                var user = group.User;
                var leagueParticipant = Repository.LeagueParticipants.FirstOrDefault(p => p.Participant.UserID == user.ID
                    && p.LeagueLevelID == leagueLevel.ID
                    && p.LeagueSeasonID == leagueSeason.ID);

                if (leagueParticipant != null)
                {
                    if (tournamentId.HasValue)
                    {
                        var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == tournamentId);
                        if (tournament != null)
                        {
                            if (tournament.PlayersCount.HasValue && tournament.RegisteredPlayersCount >= tournament.PlayersCount)
                            {
                                return Json(new { result = "error", message = "Турнир уже распределен" });
                            }
                            else
                            {
                                leagueParticipant.Participant.TournamentID = tournament.ID;
                                var team = new Team
                                {
                                    Name = group.Name,
                                    ImagePath18 = group.FullLogoPath18,
                                    ImagePath26 = group.FullLogoPath26,
                                    ImagePath30 = group.FullLogoPath30,
                                };
                                Repository.CreateTeam(team, tournament.HotReplacement);
                                leagueParticipant.Participant.TeamID = team.ID;
                                Repository.UpdateLeagueParticipant(leagueParticipant);
                            }
                        }
                    }
                    else
                    {
                        if (leagueParticipant.Participant.TournamentID.HasValue)
                        {
                            leagueParticipant.Participant.TournamentID = null;
                            Repository.UpdateLeagueParticipant(leagueParticipant);
                        }
                    }
                    return Json(new { result = "ok" });
                }
            }
            return Json(new { result = "error", message = "Какая-то непонятная ошибка" });
        }

        #endregion

        #region Players
        public ActionResult Players(int id)
        {
            var league = Repository.Leagues.FirstOrDefault(p => p.ID == id);

            if (league != null)
            {
                if (league.IsGroup)
                {
                    return RedirectToAction("Groups", new { id = id });
                }
                ViewBag.League = league;
                if (league.LastSeason.Status != (int)LeagueSeason.StatusEnum.Created &&
                    league.LastSeason.Status != (int)LeagueSeason.StatusEnum.Prepared)
                {
                    var list = league.LastSeason.LeagueParticipants.OrderBy(p => p.LeagueLevel.Level).ThenBy(p => p.Place).Select(p => p.Participant.User).ToList();
                    return View(list);

                }
                else
                {
                    var list = league.Game.Ratings.OrderBy(p => p.TotalScore).Select(p => p.User).ToList();
                    return View(list);
                }
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult PlayerRow(int id, int seasonId)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null && user != null)
            {
                ViewBag.League = leagueSeason.League;
                return View(user);
            }
            return null;
        }

        public ActionResult SelectListUserLeagueLevel(int id, int seasonId)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            ViewBag.HasTournament = false;
            if (leagueSeason != null && user != null)
            {
                ViewBag.ID = id;
                var participant = Repository.LeagueParticipants.FirstOrDefault(p => user.ID == p.Participant.UserID && p.LeagueSeasonID == leagueSeason.ID);
                ViewBag.HasTournament = participant != null && participant.Participant.TournamentID != null;
                var selectList = new List<SelectListItem>();
                selectList.Add(new SelectListItem()
                {
                    Value = "",
                    Text = "Не участвует в лиге",
                    Selected = participant == null
                });
                foreach (var level in leagueSeason.League.SubLevels)
                {
                    selectList.Add(new SelectListItem()
                    {
                        Value = level.ID.ToString(),
                        Text = level.Name,
                        Selected = participant != null && participant.LeagueLevelID == level.ID
                    });
                }
                return View(selectList);
            }
            return null;
        }

        public ActionResult SetUserLeagueParticipant(int id, int? levelId, int seasonId)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null && user != null)
            {
                var leagueParticipant = Repository.LeagueParticipants
                    .FirstOrDefault(p => p.LeagueSeasonID == leagueSeason.ID
                        && user.ID == p.Participant.UserID);

                if (leagueParticipant != null && levelId == null)
                {
                    Repository.RemoveLeagueParticipant(leagueParticipant.ID);
                    return Json(new { result = "ok" });
                }

                if (levelId != null)
                {
                    var level = Repository.LeagueLevels.FirstOrDefault(p => p.ID == levelId.Value);

                    if (level != null)
                    {
                        //нечего менять
                        if (leagueParticipant != null && leagueParticipant.LeagueLevelID == level.ID)
                        {
                            return Json(new { result = "ok" });
                        }

                        //есть ли места?
                        if (level.Quantity <= level.LeagueParticipants.Count(p => p.LeagueSeasonID == leagueSeason.ID))
                        {
                            return Json(new { result = "error", message = "Все места в уровне " + level.Name + " уже разпределены" });
                        }
                        else
                        {
                            //места есть
                            if (leagueParticipant != null)
                            {
                                leagueParticipant.LeagueLevelID = level.ID;
                                Repository.UpdateLeagueParticipant(leagueParticipant);
                            }
                            else
                            {
                                // создаем игрока
                                var participant = new Participant()
                                {
                                    UserID = user.ID
                                };
                                Repository.CreateParticipant(participant);
                                // создаем участника в лиге
                                var newLeagueParticipant = new LeagueParticipant()
                                {
                                    LeagueLevelID = level.ID,
                                    LeagueSeasonID = leagueSeason.ID,
                                    ParticipantID = participant.ID
                                };
                                Repository.CreateLeagueParticipant(newLeagueParticipant);
                            }
                        }
                    }
                    return Json(new { result = "ok" });
                }
            }
            return Json(new { result = "error", message = "Не удалось изменить" });
        }

        public ActionResult SelectListUserTournament(int id, int seasonId, int? levelId)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == levelId);
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            ViewBag.НasLevel = false;
            if (leagueSeason != null && user != null)
            {
                ViewBag.ID = id;
                var selectList = new List<SelectListItem>();
                if (leagueLevel != null)
                {
                    ViewBag.НasLevel = true;
                    ViewBag.LevelID = leagueLevel.ID;
                    var leagueParticipant = Repository.LeagueParticipants.FirstOrDefault(p => user.ID == p.Participant.UserID && p.LeagueSeasonID == leagueSeason.ID);
                    var tournaments = Repository.Tournaments.Where(p => p.LeagueLevelID == leagueLevel.ID && p.LeagueSeasonID == leagueSeason.ID);
                    selectList.Add(new SelectListItem()
                    {
                        Value = "",
                        Text = "Не участвует в турнире",
                        Selected = leagueParticipant == null || !leagueParticipant.Participant.TournamentID.HasValue
                    });
                    if (tournaments.Any())
                    {
                        foreach (var tournament in tournaments)
                        {
                            selectList.Add(new SelectListItem()
                            {
                                Value = tournament.ID.ToString(),
                                Text = tournament.Name,
                                Selected = leagueParticipant != null && leagueParticipant.Participant.TournamentID == tournament.ID
                            });
                        }
                    }
                }
                else
                {
                    selectList.Add(new SelectListItem()
                    {
                        Value = "",
                        Text = "Не участвует в лиге",
                        Selected = true
                    });
                    return View(selectList);
                }
                return View(selectList);
            }
            return null;
        }

        public ActionResult SetLeagueParticipantTournament(int id, int seasonId, int levelId, int? tournamentId)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == levelId);

            if (user != null && leagueSeason != null && leagueLevel != null)
            {
                var leagueParticipant = Repository.LeagueParticipants.FirstOrDefault(p => p.Participant.UserID == user.ID
                    && p.LeagueLevelID == leagueLevel.ID
                    && p.LeagueSeasonID == leagueSeason.ID);

                if (leagueParticipant != null)
                {
                    if (tournamentId.HasValue)
                    {
                        var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == tournamentId);
                        if (tournament != null)
                        {
                            if (tournament.PlayersCount.HasValue && tournament.RegisteredPlayersCount >= tournament.PlayersCount)
                            {
                                return Json(new { result = "error", message = "Турнир уже распределен" });
                            }
                            else
                            {
                                leagueParticipant.Participant.TournamentID = tournament.ID;
                                Repository.UpdateLeagueParticipant(leagueParticipant);
                            }
                        }
                    }
                    else
                    {
                        if (leagueParticipant.Participant.TournamentID.HasValue)
                        {
                            leagueParticipant.Participant.TournamentID = null;
                            Repository.UpdateLeagueParticipant(leagueParticipant);
                        }
                    }
                    return Json(new { result = "ok" });
                }
            }
            return Json(new { result = "error", message = "Какая-то непонятная ошибка" });
        }
        #endregion

        #region Tournaments

        public ActionResult Tournaments(int id)
        {
            var league = Repository.Leagues.FirstOrDefault(p => p.ID == id);
            if (league != null)
            {
                ViewBag.League = league;
                ViewBag.Levels = league.LeagueLevels.ToList();
                var list = league.LeagueSeasons.OrderByDescending(p => p.ID).ToList();
                return View(list);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        public ActionResult AddTournaments(int id, int seasonId)
        {
            var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == id);
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == seasonId);
            var participants = Repository.LeagueParticipants.Count(p => p.LeagueLevelID == id && p.LeagueSeasonID == seasonId);
            var addLeagueTournaments = new AddLeagueTournaments()
            {
                LeagueLevelID = leagueLevel.ID,
                LeagueSeasonID = leagueSeason.ID,
                ParticipantCount = participants,
                LeagueName = leagueSeason.League.Name,
                LeagueSeasonName = leagueSeason.Name,
                LeagueLevelName = leagueLevel.Name,
                AutoFillParticipants = true
            };
            return View(addLeagueTournaments);
        }

        [HttpPost]
        public ActionResult AddTournaments(AddLeagueTournaments addLeagueTournaments)
        {
            if (addLeagueTournaments.Count == 0)
            {
                ModelState.AddModelError("Count", "Количество турниров должно быть больше 0");
            }
            if (addLeagueTournaments.Count > addLeagueTournaments.ParticipantCount)
            {
                ModelState.AddModelError("Count", "Количество турниров не должно быть больше участников");
            }
            if (addLeagueTournaments.Count != 0)
            {
                var minParticipants = addLeagueTournaments.ParticipantCount / addLeagueTournaments.Count;

                if (minParticipants < 3)
                {
                    ModelState.AddModelError("Count", "Турнир не может содержать меньше 3 участников");
                }
            }
            if (ModelState.IsValid)
            {
                var leagueLevel = Repository.LeagueLevels.FirstOrDefault(p => p.ID == addLeagueTournaments.LeagueLevelID);
                var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == addLeagueTournaments.LeagueSeasonID);
                var participants = Repository.LeagueParticipants.Where(p => p.LeagueLevelID == addLeagueTournaments.LeagueLevelID
                    && p.LeagueSeasonID == addLeagueTournaments.LeagueSeasonID).ToList();

                var groups = new List<List<LeagueParticipant>>();
                for (int i = 0; i < addLeagueTournaments.Count; i++)
                {
                    groups.Add(new List<LeagueParticipant>());
                }
                var index = 0;
                var count = addLeagueTournaments.Count;
                var participantsCount = participants.Count;
                var partIndex = 0;
                while (participantsCount > index)
                {
                    partIndex = index % count;
                    groups[partIndex].Add(participants[index]);
                    index++;
                }
                var league = leagueLevel.League;
                var listOfTournaments = new List<Tournament>();
                for (int i = 0; i < addLeagueTournaments.Count; i++)
                {
                    var number = (i + 1).ToRoman();
                    var name = string.Format("{0} {1} {2} {3}", league.Name, leagueSeason.Name, leagueLevel.Name, number);
                    var url = CreateTournamentUrl(name);

                    var tournament = new Tournament()
                    {
                        GameID = league.GameID,
                        ForumID = leagueLevel.ForumID,
                        PlatformID = league.Game.PlatformID,
                        LeagueLevelID = leagueLevel.ID,
                        LeagueSeasonID = leagueSeason.ID,
                        Name = name,
                        Url = url,
                        TournamentType = (int)Tournament.TournamentTypeEnum.RoundRobin,
                        PlayersCount = groups[i].Count,
                        OpenRegistrationDate = leagueSeason.StartDate,
                        CloseRegistrationDate = leagueSeason.StartDate,
                        BeginDate = leagueSeason.StartDate,
                        EndDate = leagueSeason.EndMainTourDate,
                        ImagePath = leagueLevel.Image,
                        Status = (int)Tournament.StatusEnum.Created,
                        CountRound = league.CountRound,
                        IsRoundForPoints = league.IsRoundForPoints,
                        Rules = league.Rules,
                        SingleDrawPoint = league.SingleDrawPoint,
                        SingleWinPoint = league.SingleWinPoint,
                        HostGuest = league.HostGuest,
                        TeamCount = league.TeamCount ?? 0,
                        HotReplacement = league.HotReplacement,
                        Description = league.Description,
                        DoubleGoalInGuest = league.DoubleGoalInGuest,
                        IsTeam = league.IsGroup,
                        IsGroup = league.IsGroup,
                        //todo : fee
                    };

                    Repository.CreateTournament(tournament);

                    listOfTournaments.Add(tournament);
                }
                if (addLeagueTournaments.AutoFillParticipants)
                {
                    for (int i = 0; i < addLeagueTournaments.Count; i++)
                    {
                        Repository.UpdateLeagueParticipants(listOfTournaments[i].ID, groups[i].Select(p => p.ID).ToList());
                    }
                }
                return View("Ok");
            }
            return View(addLeagueTournaments);
        }

        private string CreateTournamentUrl(string name)
        {
            var baseUrl = Translit.Translate(name);
            var url = baseUrl;
            var num = 1;
            bool exist = false;
            do
            {
                exist = Repository.Tournaments.Any(p => string.Compare(p.Url, url, true) == 0);
                if (exist)
                {
                    url = string.Format("{0}-{1}", baseUrl, num);
                    num++;
                }
            } while (exist);
            return url;
        }

        [HttpPost]
        public ActionResult RemoveTournaments(int id, int seasonId)
        {
            var tournaments = Repository.Tournaments.Where(p => p.LeagueLevelID == id && p.LeagueSeasonID == seasonId);

            foreach (var tournament in tournaments)
            {
                Repository.UpdateLeagueParticipants(tournament.ID, new List<int>());
                Repository.RemoveTournament(tournament.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult CreateMatches(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                foreach (var tournament in leagueSeason.Tournaments.ToList())
                {
                    if (tournament.Status == (int)Tournament.StatusEnum.Created)
                    {
                        Repository.CreateMatches(tournament.ID);
                    }
                }
                Repository.UpdateLeagueSeasonStatus(leagueSeason, LeagueSeason.StatusEnum.Prepared);
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult StartSeason(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                foreach (var tournament in leagueSeason.Tournaments.ToList())
                {
                    if (tournament.Status == (int)Tournament.StatusEnum.Allocated)
                    {
                        Repository.StartTournament(tournament.ID);
                    }
                }
                Repository.UpdateLeagueSeasonStatus(leagueSeason, LeagueSeason.StatusEnum.InPlay);

            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult FinishTournaments(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                var list = new List<Tournament.LeagueStatisticGroup>();

                foreach (var tournament in leagueSeason.Tournaments.ToList())
                {
                    if (tournament.Status == (int)Tournament.StatusEnum.InGame)
                    {
                        if (tournament.AllGamesPlayed)
                        {
                            Repository.FinishTournament(tournament.ID);
                        }
                        else
                        {
                            Repository.TechFinishTournament(tournament.ID);
                        }
                    }
                    foreach (var participant in tournament.SubPlayers)
                    {
                        var statistic = new Tournament.LeagueStatisticGroup()
                        {
                            LeagueParticipant = participant.LeagueParticipants.FirstOrDefault(p => p.LeagueLevelID == tournament.LeagueLevelID
                                && p.LeagueSeasonID == tournament.LeagueSeasonID),
                            Participant = participant,
                            Points = tournament.TotalWinGame(participant.ID, null) * 3 + tournament.TotalDrawnGame(participant.ID, null),
                            LevelPoints = participant.TotalRating(leagueSeason.League.GameID),
                            Level = tournament.LeagueLevel
                        };
                        list.Add(statistic);
                    }
                }

                foreach (var level in leagueSeason.League.LeagueLevels)
                {
                    var subStatistic = list.Where(p => p.Level.ID == level.ID);

                    var place = 1;
                    foreach (var subStatisticItem in subStatistic.OrderByDescending(p => p.Points).ThenByDescending(p => p.LevelPoints))
                    {
                        subStatisticItem.LeagueParticipant.Place = place;
                        place++;
                        Repository.UpdateLeagueParticipant(subStatisticItem.LeagueParticipant);
                    }
                }
                Repository.UpdateLeagueSeasonStatus(leagueSeason, LeagueSeason.StatusEnum.FinishedMainTour);
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult CreateOffsMatches(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                if (leagueSeason.Status == (int)LeagueSeason.StatusEnum.FinishedMainTour && !leagueSeason.Matches.Any())
                {
                    var participants = leagueSeason.LeagueParticipants;

                    var levels = leagueSeason.League.LeagueLevels.OrderBy(p => p.Level).ToList();

                    var i = 0;
                    while (levels.Any(p => p.Level > i))
                    {
                        var mainLevel = levels.Where(p => p.Level > i).OrderBy(p => p.Level).FirstOrDefault();
                        if (mainLevel != null)
                        {
                            var nextLevel = levels.Where(p => p.Level > mainLevel.Level).OrderBy(p => p.Level).FirstOrDefault();

                            if (nextLevel != null)
                            {
                                i = mainLevel.Level;
                                //работаем
                                var quantity = mainLevel.Quantity;
                                var quatra = quantity / 4;

                                //3я quatra 
                                var mainParticipants = participants.Where(p => p.LeagueLevelID == mainLevel.ID
                                    && p.Place > quatra * 2 && p.Place <= quatra * 3)
                                    .ToList().OrderBy(p => Guid.NewGuid()).ToList();
                                //1я quatra - следующего уровня
                                var nextParticipants = participants.Where(p => p.LeagueLevelID == nextLevel.ID
                                    && p.Place > quatra && p.Place <= quatra * 2)
                                    .ToList().OrderBy(p => Guid.NewGuid()).ToList();

                                for (int j = 0; j < quatra; j++)
                                {
                                    if (mainParticipants.Count > j && nextParticipants.Count > j)
                                    {
                                        var mainParticipantID = mainParticipants[j].ParticipantID;
                                        var nextParticipantID = nextParticipants[j].ParticipantID;

                                        Repository.CreateOffMatch(leagueSeason, mainLevel, mainParticipantID, nextParticipantID);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    Repository.UpdateLeagueSeasonStatus(leagueSeason, LeagueSeason.StatusEnum.PreparedOffMatches);
                }

                return RedirectBack(RedirectToAction("Index"));
            }
            return RedirectToNotFoundPage;
        }

        #region Matches

        public ActionResult Matches(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);

            if (leagueSeason != null)
            {
                return View(leagueSeason);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult StartOffsMatches(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                foreach (var match in leagueSeason.Matches)
                {
                    if (match.Status == (int)Match.MatchStatusEnum.Created)
                    {
                        if (match.Player1 != null && match.Player2 != null)
                        {
                            match.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                            Repository.UpdateStatusMatch(match);
                            Repository.CreateOffMatchNoticeSetPlayers(match);
                        }
                    }
                }
                Repository.UpdateLeagueSeasonStatus(leagueSeason, LeagueSeason.StatusEnum.InPlayOffMatches);

                return RedirectBack(RedirectToAction("Index"));
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult FinishOffsMatches(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                foreach (var match in leagueSeason.Matches)
                {
                    if (match.Status == (int)Match.MatchStatusEnum.DefinedPlayers)
                    {
                        Repository.TechAllLoseMatch(match.ID);
                    }
                }
                //Стыковые матчи
                foreach (var match in leagueSeason.Matches)
                {
                    if (match.WinnerID != null)
                    {
                        var winner = match.Winner;
                        var loser = match.Loser;

                        var winnerLeagueParticipant = winner.LeagueParticipants.FirstOrDefault(p => p.LeagueSeasonID == leagueSeason.ID);
                        var loserLeagueParticipant = loser.LeagueParticipants.FirstOrDefault(p => p.LeagueSeasonID == leagueSeason.ID);

                        if (winnerLeagueParticipant != null && loserLeagueParticipant != null)
                        {
                            //Победа претендента
                            if (winnerLeagueParticipant.LeagueLevel.Level > loserLeagueParticipant.LeagueLevel.Level)
                            {
                                var bottomLevel = winnerLeagueParticipant.LeagueLevel;
                                var bottomPlace = winnerLeagueParticipant.Place;
                                winnerLeagueParticipant.LeagueLevel = loserLeagueParticipant.LeagueLevel;
                                winnerLeagueParticipant.Place = loserLeagueParticipant.Place;
                                Repository.UpdateLeagueParticipant(winnerLeagueParticipant);

                                loserLeagueParticipant.LeagueLevel = bottomLevel;
                                loserLeagueParticipant.Place = bottomPlace;
                                Repository.UpdateLeagueParticipant(loserLeagueParticipant);
                            }
                        }
                    }
                }
                var participants = leagueSeason.LeagueParticipants;

                //Места
                var levels = leagueSeason.League.LeagueLevels;
                var i = 0;
                while (levels.Any(p => p.Level > i))
                {
                    var mainLevel = levels.Where(p => p.Level > i).OrderBy(p => p.Level).FirstOrDefault();
                    if (mainLevel != null)
                    {
                        var nextLevel = levels.Where(p => p.Level > mainLevel.Level).OrderBy(p => p.Level).FirstOrDefault();

                        if (nextLevel != null)
                        {
                            i = mainLevel.Level;
                            //работаем
                            var quantity = mainLevel.Quantity;
                            var quatra = quantity / 4;

                            //4я quatra 
                            var mainParticipants = participants.Where(p => p.LeagueLevelID == mainLevel.ID
                                && p.Place > quatra * 3)
                                .ToList().OrderBy(p => Guid.NewGuid()).ToList();
                            //1я quatra - следующего уровня
                            var nextParticipants = participants.Where(p => p.LeagueLevelID == nextLevel.ID
                                && p.Place <= quatra)
                                .ToList().OrderBy(p => Guid.NewGuid()).ToList();

                            for (int j = 0; j < quatra; j++)
                            {
                                if (mainParticipants.Count > j && nextParticipants.Count > j)
                                {
                                    var mainParticipant = mainParticipants[j];
                                    var nextParticipant = nextParticipants[j];

                                    var bottomLevel = mainParticipant.LeagueLevel;
                                    var bottomPlace = mainParticipant.Place;
                                    mainParticipant.LeagueLevel = nextParticipant.LeagueLevel;
                                    mainParticipant.Place = nextParticipant.Place;
                                    Repository.UpdateLeagueParticipant(mainParticipant);

                                    nextParticipant.LeagueLevel = bottomLevel;
                                    nextParticipant.Place = bottomPlace;
                                    Repository.UpdateLeagueParticipant(nextParticipant);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                Repository.UpdateLeagueSeasonStatus(leagueSeason, LeagueSeason.StatusEnum.Finished);
                return RedirectBack(RedirectToAction("Index"));
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult RollbackMatch(int id)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == id);

            if (match.LeagueSeason != null)
            {
                Repository.RollbackMatch(id);
                return RedirectToAction("Matches", new { id = match.Tournament.ID });
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult ChangeMatchesParticipants(int fromMatchId, bool fromPlayer1, int toMatchId, bool toPlayer1)
        {
            Repository.ChangeMatchParticipant(fromMatchId, fromPlayer1, toMatchId, toPlayer1);

            return Json(new { result = "ok" });
        }

        #endregion

        #region Tech
        public ActionResult AutoPlay(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                foreach (var tournament in leagueSeason.Tournaments)
                {
                    var rand = new Random((int)DateTime.Now.Ticks);
                    while (PlayRound(tournament.ID, rand, true)) { };
                }
            }
            return Content("Ok");
        }

        public ActionResult AutoPlayMatches(int id)
        {
            var leagueSeason = Repository.LeagueSeasons.FirstOrDefault(p => p.ID == id);
            if (leagueSeason != null)
            {
                foreach (var match in leagueSeason.Matches)
                {
                    var rand = new Random((int)DateTime.Now.Ticks);
                    while (PlayMatchRound(match.ID, rand, true)) { };
                }
            }
            return Content("Ok");
        }
        protected virtual bool PlayRound(int tournamentID, Random rand, bool enableTech, int maxNum = 4, bool allTech = false)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == tournamentID);
            if (tournament != null)
            {
                if (tournament.Status == (int)Tournament.StatusEnum.InGame)
                {
                    var round = tournament.Matches.SelectMany(p => p.Rounds).Where(p => p.Status == (int)Round.RoundStatusEnum.Created && p.Player1 != null && p.Player2 != null).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                    if (round != null)
                    {
                        return PlayRound(rand, enableTech, maxNum, allTech, round);
                    }
                }
            }
            return false;
        }

        protected virtual bool PlayMatchRound(int matchID, Random rand, bool enableTech, int maxNum = 4, bool allTech = false)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == matchID);
            if (match != null)
            {
                if (match.Status == (int)Match.MatchStatusEnum.DefinedPlayers)
                {
                    var round = match.Rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Created && p.Player1 != null && p.Player2 != null).ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                    if (round != null)
                    {
                        return PlayRound(rand, enableTech, maxNum, allTech, round);
                    }
                }
            }
            return false;
        }

        private bool PlayRound(Random rand, bool enableTech, int maxNum, bool allTech, Round round)
        {
            if (rand.Next() % 5 == 0 && enableTech || allTech)
            {
                round.Score1 = rand.Next(2);
                round.Score2 = rand.Next(2);
                while (round.Score1 == round.Score2)
                {
                    round.Score2 = rand.Next(2);
                }
                Repository.TechSubmitRound(round);
                Console.WriteLine("TECH ROUND: {0} {1} -- {2} {3}", round.Player1.User.Login, round.Score1, round.Score2, round.Player2.User.Login);
                return true;
            }
            /* round.IntroducerResult = user;*/
            if (round == round.Match.Round2)
            {
                round.Extended = true;
                round.Score1Text = round.Score1.ToString() + "+0";
                round.Score2Text = round.Score2.ToString() + "+0";
            }
            round.Score1 = rand.Next(maxNum);
            round.Score2 = rand.Next(maxNum);
            while (round.Score1 == round.Score2)
            {
                round.Score2 = rand.Next(maxNum);
            }
            Repository.PublishRound(round);
            Repository.SubmitRound(round);
            if (round.IsAdditional)
            {
                Console.WriteLine("ROUND ADDITIONAL: {0} {1} -- {2} {3}", round.Player1.User.Login, round.Score1, round.Score2, round.Player2.User.Login);
            }
            else
            {
                Console.WriteLine("ROUND : {0} {1} -- {2} {3}", round.Player1.User.Login, round.Score1, round.Score2, round.Player2.User.Login);
            }
            return true;
        }

        #endregion
    }
}