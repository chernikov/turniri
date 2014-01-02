using System.Linq;
using System.Web.Mvc;
using turniri.Global;
using turniri.Model;
using turniri.Models.Info;
using turniri.Models.ViewModels;

namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin,tournament_admin")]
    public class TeamController : AdminController
    {
        /// <summary>
        /// Команды в турнире
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null)
            {
                if (CurrentUser.InRoles("admin") || CurrentUser.AdminTournaments.Any(p => p.ID == tournament.ID))
                {
                    return View(tournament);
                }
                return RedirectToLoginPage;
            }
            return RedirectToLoginPage;
        }

        public ActionResult Create(int id)
        {
            var teamView = new TeamView()
            {
                TournamentID = id
            };
            return View(teamView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);

            if (team != null)
            {
                var teamView = (TeamView)ModelMapper.Map(team, typeof(Team), typeof(TeamView));
                return View(teamView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(TeamView teamView)
        {
            if (ModelState.IsValid)
            {
                var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == teamView.TournamentID);
                var user = Repository.Users.FirstOrDefault(p => p.ID == teamView.CaptainID);

                if (tournament != null && user != null)
                {
                    var team = (Team)ModelMapper.Map(teamView, typeof(TeamView), typeof(Team));
                    if (team.ID == 0)
                    {
                        Repository.CreateTeam(team, tournament.HotReplacement);
                        var userTeam = new UserTeam()
                        {
                            TeamID = team.ID,
                            UserID = user.ID,
                            IsCaptain = true,
                            Accepted = true
                        };
                        Repository.CreateUserTeam(userTeam);
                        Repository.RegisterParticipant(user.ID, tournament.ID, team.ID);
                    }
                    else
                    {
                        Repository.UpdateTeam(team);
                        var captain = Repository.UserTeams.FirstOrDefault(p => p.TeamID == team.ID && p.IsCaptain);
                        if (captain != null)
                        {
                            if (captain.UserID != user.ID)
                            {
                                captain.UserID = user.ID;
                                Repository.UpdateUserTeam(captain);
                                Repository.ChangeParticipant(captain.Team.Participants.First().ID, user.ID);
                            }
                        }
                    }
                    return RedirectToAction("Index", new { id = tournament.ID });
                }
            }
            return View(teamView);
        }

        public ActionResult Roster(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);
            if (team != null)
            {
                return View(team);
            }
            return RedirectToNotFoundPage;
        }

        
        [HttpGet]
        public ActionResult AddUser(int id)
        {
            var userTeamView = new UserTeamView()
            {
                TeamID = id
            };
            return View(userTeamView);
        }

        [HttpPost]
        public ActionResult AddUser(UserTeamView userTeamView)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == userTeamView.TeamID);
            var user = Repository.Users.FirstOrDefault(p => p.ID == userTeamView.UserID);
            if (team != null && user != null)
            {
                if (user.IsParticipantInTournament(team.Tournament.ID))
                {
                    ModelState.AddModelError("UserLogin", "Игрок уже участвует в турнире в другой команде");
                }
                if (ModelState.IsValid)
                {
                    var userTeam = (UserTeam)ModelMapper.Map(userTeamView, typeof(UserTeamView), typeof(UserTeam));
                    userTeam.Accepted = true;
                    Repository.CreateUserTeam(userTeam);
                    return View("Ok");
                }
            } else {
                ModelState.AddModelError("UserLogin", "Выберите игрока");
            }

            return View(userTeamView);
        }

        public ActionResult DeleteUserTeam(int id)
        {
            var userTeam = Repository.UserTeams.FirstOrDefault(p => p.ID == id);
            if (userTeam != null)
            {
                var teamID = userTeam.TeamID;

                Repository.RemoveUserTeam(userTeam.ID);

                return RedirectToAction("Roster", new { id = teamID });
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SubmitUserTeam(int id)
        {
            var userTeam = Repository.UserTeams.FirstOrDefault(p => p.ID == id);
            if (userTeam != null)
            {
                var teamID = userTeam.TeamID;
                userTeam.Accepted = true;
                Repository.UpdateUserTeam(userTeam);
                return RedirectToAction("Roster", new { id = teamID });
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Close(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);
            if (team != null)
            {
                var tournamentID = team.Tournament.ID;
                Repository.CloseTeam(team.ID);
                return RedirectToAction("Index", new { id = tournamentID });
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AutocompleteUser(string query, int gameID, int? groupID)
        {
            var list = Repository.RegularUsers.Where(p => p.UserGames.Any(r => r.GameID == gameID));

            if (groupID.HasValue)
            {
                list = list.Where(p => p.UserGroups.Any(r => r.GroupID == groupID.Value));
            }

            var data = list.Where(r => r.Login.StartsWith(query) || r.Email.StartsWith(query))
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


    }
}
