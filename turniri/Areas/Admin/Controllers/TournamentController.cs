using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;
using turniri.Models.Info;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class TournamentController : AdminController
    {
        public ActionResult Index(int page = 1, TournamentSearch search = null)
        {
            IQueryable<Tournament> list = null;
            if (CurrentUser.InRoles("admin"))
            {
                list = Repository.Tournaments.OrderByDescending(p => p.ID);
                
            }
            else if (CurrentUser.HasAdminTournament)
            {
                list = CurrentUser.AdminTournaments;
            }
            if (list != null)
            {
                if (search != null)
                {
                    if (!search.ShowLeague && search.State != TournamentSearch.StateEnum.Leagues)
                    {
                        list = list.Where(p => !p.LeagueSeasonID.HasValue);
                    }
                    switch (search.State)
                    {
                        case TournamentSearch.StateEnum.Active:
                            list = list.Where(p => p.Status == (int)Tournament.StatusEnum.Created ||
                                p.Status == (int)Tournament.StatusEnum.Allocated ||
                                p.Status == (int)Tournament.StatusEnum.InGame);
                            break;
                        case TournamentSearch.StateEnum.Archive:
                            list = list.Where(p => p.Status == (int)Tournament.StatusEnum.Closed ||
                                p.Status == (int)Tournament.StatusEnum.PlayedOut);
                            break;
                        case TournamentSearch.StateEnum.Leagues:
                            list = list.Where(p => p.LeagueSeasonID.HasValue);
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(search.SearchString))
                    {
                        list = SearchEngine.Search(search.SearchString, list).AsQueryable();
                    }
                }
                else
                {
                    search = new TournamentSearch()
                    {
                        State = TournamentSearch.StateEnum.All,
                        SearchString = ""
                    };
                }
                ViewBag.Search = search;
                var data = new PageableData<Tournament>();
                data.Init(list, page, "Index");
                return View(data);
            }
            ViewBag.Search = new TournamentSearch()
            {
                State = TournamentSearch.StateEnum.All,
                SearchString = ""
            };
            return View(new PageableData<Tournament>());
        }

        [Authorize(Roles = "admin,game_admin")]
        public ActionResult Create()
        {
            var tournamentView = new TournamentView();
            if (CurrentUser.InRoles("game_admin"))
            {
                tournamentView.InitGameAdmin(CurrentUser);
            }
            return View("Edit", tournamentView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                var tournamentView = (TournamentView)ModelMapper.Map(tournament, typeof(Tournament), typeof(TournamentView));
                if (CurrentUser.InRoles("game_admin"))
                {
                    tournamentView.InitGameAdmin(CurrentUser);
                }
                else if (CurrentUser.InRoles("tournament_admin"))
                {
                    tournamentView.DisablePlatformAndGame = true;
                }
                if (tournamentView.TournamentCondition == null)
                {
                    tournamentView.TournamentCondition = new TournamentConditionView();
                }
                return View(tournamentView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(TournamentView tournamentView)
        {
            var forum = Repository.Forums.FirstOrDefault(p => p.ID == tournamentView.ForumID);
            if (forum == null || (forum.IsFather && string.IsNullOrWhiteSpace(tournamentView.ForumName) && !forum.IsEnd))
            {
                ModelState.AddModelError("Forum", "Выберите топик форума или создайте новый");
            }
            if (tournamentView.MaxPlayersCount < tournamentView.Players.Count)
            {
                ModelState.AddModelError("Participants", "Уупс! Перебрал чуток с количеством");
            }
            if (ModelState.IsValid)
            {
                var tournament = (Tournament)ModelMapper.Map(tournamentView, typeof(TournamentView), typeof(Tournament));

                var baseUrl = Translit.Translate(tournament.Name);
                var url = baseUrl;
                var num = 1;
                bool exist = false;
                do
                {
                    exist = Repository.Tournaments.Any(p => string.Compare(p.Url, url, true) == 0 && p.ID != tournament.ID);
                    if (exist)
                    {
                        url = string.Format("{0}-{1}", baseUrl, num);
                        num++;
                    }
                } while (exist);
                tournament.Url = url;

                if (tournament.ID == 0)
                {
                    Repository.CreateTournament(tournament);
                }
                else
                {
                    Repository.UpdateTournament(tournament);
                }

                Repository.UpdateTournamentAdmins(tournament.ID, tournamentView.Admins);
                Repository.UpdateTournamentModerators(tournament.ID, tournamentView.Moderators);

                if (tournament.Status == (int)Tournament.StatusEnum.Created)
                {
                    Repository.UpdateParticipants(tournament.ID, tournamentView.Players);
                }

                if (forum.IsFather)
                {
                    var newForum = new Forum
                    {
                        ParentID = forum.ID,
                        Name = tournamentView.ForumName,
                        UserID = CurrentUser.ID,
                        IsEnd = true
                    };
                    Repository.CreateForum(newForum);
                    tournament.ForumID = newForum.ID;
                    Repository.UpdateTournament(tournament);
                }
                return RedirectToAction("Index");
            }
            if (CurrentUser.InRoles("game_admin"))
            {
                tournamentView.InitGameAdmin(CurrentUser);
            }
            else if (CurrentUser.InRoles("tournament_admin"))
            {
                tournamentView.DisablePlatformAndGame = true;
            }
            return View(tournamentView);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            if (CurrentUser.ID == 1)
            {
                var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
                if (tournament != null)
                {
                    Repository.RemoveTournament(tournament.ID);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateGamesSelect(int idPlatform)
        {
            var platform = Repository.Platforms.FirstOrDefault(p => p.ID == idPlatform);
            if (platform != null)
            {
                if (CurrentUser.InRoles("game_admin"))
                {
                    var games = CurrentUser.AdminGames.Where(p => p.PlatformID == platform.ID).Select(p => new SelectListItem
                    {
                        Selected = false,
                        Text = p.Name,
                        Value = p.ID.ToString()
                    });
                    return View(games);
                }
                else
                {
                    var games = platform.Games.Select(p => new SelectListItem
                                                               {
                                                                   Selected = false,
                                                                   Text = p.Name,
                                                                   Value = p.ID.ToString()
                                                               });
                    return View(games);
                }
            }
            return null;
        }

        public ActionResult GetForum(int? id)
        {
            ViewBag.CreateTopic = false;
            if (id.HasValue)
            {
                var forum = Repository.Forums.FirstOrDefault(p => p.ID == id.Value);
                if (forum != null)
                {
                    ViewBag.CreateTopic = forum.IsFather;
                }
            }
            var forumList = TournamentView.CreateForumsSelectList(id, Repository);
            ViewBag.ForumID = id;
            return View(forumList);
        }

        public ActionResult Matches(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null)
            {
                return View(tournament);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Players(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null)
            {
                return View(tournament);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult CreateMatches(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                if (tournament.Matches.Count == 0)
                {
                    Repository.CreateMatches(tournament.ID);
                }
                FixMatches(tournament);
                return RedirectToAction("Matches", new { id = tournament.ID });
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult ClearMatches(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.ClearMatches(tournament.ID);
                return RedirectToAction("Matches", new { id = tournament.ID });
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        public ActionResult Toss(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            return View(tournament);
        }

        public ActionResult ChangeParticipants(int participant1ID, int participant2ID)
        {
            Repository.ChangeGroupParticipants(participant1ID, participant2ID);

            return Json(new { result = "ok" });
        }

        public ActionResult ChangeMatchesParticipants(int fromMatchId, bool fromPlayer1, int toMatchId, bool toPlayer1)
        {
            Repository.ChangeMatchParticipant(fromMatchId, fromPlayer1, toMatchId, toPlayer1);

            return Json(new { result = "ok" });
        }

        public ActionResult Start(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.StartTournament(tournament.ID);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SubstituteParticipant(int matchId, bool participant1, int participantID)
        {
            Repository.SubstituteParticipant(matchId, participant1, participantID);
            return Json(new { result = "ok" });
        }

        public ActionResult AutocompleteUser(string query)
        {
            var data = Repository.RegularUsers
                .Where(r => r.Login.StartsWith(query) || r.Email.StartsWith(query))
                .Take(10)
                .ToArray();
            return Json(new
            {
                query = query,
                data = data.Select(p => new
                {
                    ID = p.ID,
                    Label = p.Login
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishTournament(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.FinishTournament(tournament.ID);
                Repository.CloseAllCommentsInTournament(tournament.ID, Server.MapPath("/"));
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult TechFinishTournament(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.TechFinishTournament(tournament.ID);
                Repository.CloseAllCommentsInTournament(tournament.ID, Server.MapPath("/"));
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult UndoFinishTournament(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.UndoFinishTournament(tournament.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ResetAwardsTournament(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.ResetAwardsForTournament(tournament.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult AllocatePlayoff(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                Repository.AllocatePlayoff(tournament.ID);
            }

            return RedirectToAction("Matches", new { id });
        }

        public ActionResult RollbackMatch(int id)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == id);

            if (match.Tournament != null && CurrentUser.IsTournamentAdmin(match.Tournament.ID))
            {
                Repository.RollbackMatch(id);
                return RedirectToAction("Matches", new { id = match.Tournament.ID });
            }
            return RedirectBack(RedirectToAction("Index"));
        }

        public ActionResult TechLose(int id)
        {
            var participant = Repository.Participants.FirstOrDefault(p => p.ID == id);
            if (participant != null)
            {
                var tournament = participant.Tournament;
                var user = participant.User;
                var futureMatches = tournament.Matches.Where(p => (p.Participant1ID == user.ID || p.Participant2ID == user.ID) && p.Status == (int)Match.MatchStatusEnum.DefinedPlayers);

                foreach (var match in futureMatches)
                {
                    if (match.Participant1ID == user.ID)
                    {
                        match.Score1 = 0;
                        match.Score2 = 3;
                    }
                    else
                    {
                        match.Score1 = 3;
                        match.Score2 = 0;
                    }
                    Repository.TechSubmitMatch(match);
                }
                return RedirectToAction("Players", new { id = tournament.ID });
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult Disqualify(int id)
        {
            var participant = Repository.Participants.FirstOrDefault(p => p.ID == id);
            if (participant != null)
            {
                var tournament = participant.Tournament;
                var user = participant.User;
                var matches = tournament.Matches.Where(p => (p.Participant1ID == user.ID || p.Participant2ID == user.ID)).ToList();
                foreach (var match in matches)
                {
                    Repository.RemoveMatch(match.ID); ;
                }
                Repository.RemoveParticipant(participant.ID);
                return RedirectToAction("Players", new { id = tournament.ID });
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult FinishAllPreviousTournament()
        {
            foreach (var tournament in Repository.Tournaments.Where(p => p.Status == (int)Tournament.StatusEnum.Closed || p.Status == (int)Tournament.StatusEnum.PlayedOut))
            {
                Repository.CloseAllCommentsInTournament(tournament.ID, Server.MapPath("/"));
            }
            return RedirectToAction("Index");
        }

        public ActionResult CheckAllCommentFiles()
        {
            var dir = new DirectoryInfo(Server.MapPath("/Media/files/comment"));

            foreach (var file in dir.GetFiles())
            {
                if (file.Name != "." && file.Name != "..")
                {
                    var original = "/Media/files/comment/" + file.Name;

                    var comment = Repository.Comments.Any(p => string.Compare(p.ImagePath, original, true) == 0);

                    if (!comment)
                    {
                        file.Delete();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateNotice(int id)
        {
            var noticeDistributionView = new NoticeDistributionView()
            {
                ForeignID = id
            };

            return View(noticeDistributionView);
        }

        [HttpPost]
        public ActionResult CreateNotice(NoticeDistributionView noticeDistributionView)
        {
            if (ModelState.IsValid)
            {
                var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == noticeDistributionView.ForeignID);

                if (tournament != null)
                {
                    noticeDistributionView._ID = 0;
                    var noticeDistribution = (NoticeDistribution)ModelMapper.Map(noticeDistributionView, typeof(NoticeDistributionView), typeof(NoticeDistribution));

                    noticeDistribution.UserID = CurrentUser.ID;
                    Repository.CreateNoticeDistribution(noticeDistribution);

                    IEnumerable<User> list = null;
                    if (tournament.IsTeam)
                    {
                        list = tournament.SubTeams.SelectMany(p => p.UserTeams).Select(r => r.User).AsEnumerable();
                    }
                    else
                    {
                        list = tournament.Participants.Select(p => p.User).ToList();
                    }

                    foreach (var user in list)
                    {
                        var notice = new Notice()
                        {
                            NoticeDistributionID = noticeDistribution.ID,
                            TournamentID = noticeDistributionView.ForeignID,
                            SenderID = noticeDistribution.UserID,
                            ReceiverID = user.ID,
                            Caption = noticeDistribution.Caption.Replace("%username%", user.Login),
                            Text = noticeDistribution.Text.Replace("%username%", user.Login),
                            Type = (int)Notice.TypeEnum.Tournament,
                            IsCloseForRead = noticeDistribution.IsCloseForRead
                        };
                        Repository.CreateNotice(notice);
                    }
                    return RedirectToAction("Index");
                }

            }
            return View(noticeDistributionView);
        }

        public ActionResult FixTournamentGroups(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null && tournament.TournamentGroups.Any())
            {
                FixMatches(tournament);
            }
            return RedirectToAction("Index");
        }

        private void FixMatches(Tournament tournament)
        {
            foreach (var group in tournament.TournamentGroups)
            {
                var participants = group.Participants.OrderBy(p => p.ID).ToList();
                var participantIDs = participants.Select(p => p.ID).ToList();
                var matches = group.Matches.ToList();
                var matchIDs = matches.Select(p => p.ID).ToList();
                int k = 0;
                if (matches.Any())
                {
                    var currentMatch = matches[k];

                    for (int i = 0; i < participants.Count; i++)
                    {
                        for (int j = i + 1; j < participants.Count; j++)
                        {
                            currentMatch.Participant1ID = participants[i].ID;
                            currentMatch.Participant2ID = participants[j].ID;

                            Repository.UpdateMatch(currentMatch);

                            foreach (var round in currentMatch.Rounds)
                            {
                                round.Participant1ID = participants[i].ID;
                                round.Participant2ID = participants[j].ID;
                                Repository.UpdateRound(round);
                            }
                            k++;
                            if (matches.Count <= k)
                            {
                                break;
                            }
                            currentMatch = matches[k];
                        }
                    }
                }
            }
        }
    }
}
