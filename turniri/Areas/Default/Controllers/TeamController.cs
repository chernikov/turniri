using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;
using turniri.Tools;

namespace turniri.Areas.Default.Controllers
{

    public class TeamController : DefaultController
    {
        private static string TeamFolder = "/Media/files/team/";

        private static string Avatar30Size = "Avatar30Size";
        private static string Avatar26Size = "Avatar26Size";
        private static string Avatar18Size = "Avatar18Size";

        public ActionResult Index(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);

            if (team != null)
            {
                return View(team);
            }
            return null;
        }

        [Authorize]
        public ActionResult Add(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null && tournament.Status == (int)Tournament.StatusEnum.Created)
            {
                //убираем
                var participant = tournament.Participants.FirstOrDefault(p => p.UserID == CurrentUser.ID);
                if (participant != null)
                {
                    Repository.RemoveParticipant(participant.ID);
                    return Json(new
                    {
                        result = "ok",
                        data = "remove",
                        count = tournament.Participants.Count()
                    }, JsonRequestBehavior.AllowGet);
                }

                //добавляем
                string condition = string.Empty;
                if (tournament.MaxPlayersCount > tournament.RegisteredPlayersCount)
                {

                    if (CurrentUser.GroupByGame(tournament.GameID) == null)
                    {
                        condition = "Вы должны состоять в команде играющую эту игру";
                    }
                    else if (!CurrentUser.InRoles("group_captain"))
                    {
                        condition = "Вы должны быть капитаном команды";
                    }
                    else if (!CurrentUser.UserRoles.Any(p => string.Compare(p.Role.Code, "group_captain", true) == 0 && p.UserRoleGroups.Any(r => r.Group.GameID == tournament.GameID)))
                    {
                        condition = "Вы должны быть капитаном команды";
                    }
                    else if (!CurrentUser.CanGetGroupPartRating(tournament))
                    {
                        condition = "Рейтинг вашей команды не соответствует условиям турнира";
                    }
                    else if (CurrentUser.AlreadyGetPartInTeam(tournament))
                    {
                        condition = "Вы уже участвуете в турнире";
                    }
                    if (!string.IsNullOrEmpty(condition))
                    {
                        return Json(new
                        {
                            result = "error",
                            error = condition
                        }, JsonRequestBehavior.AllowGet);
                    }
                    if (tournament.CheckConditionsForRegister(CurrentUser, out condition))
                    {
                        /* разрешаем показать форму */
                        var count = Repository.Participants.Count(p => p.TournamentID == tournament.ID);
                        return Json(new
                        {
                            result = "ok",
                            data = "register",
                            count
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            result = "error",
                            error = condition
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        result = "error",
                        error = "Все места уже заняты.",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                result = "error",
                error = "Регистрация закрыта"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Register(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);

            if (tournament != null && tournament.IsTeam)
            {
                if (tournament.IsGroup)
                {
                    var group = CurrentUser.GroupByGame(tournament.GameID);
                    var teamView = new TeamView()
                    {
                        TournamentID = id,
                        MoneyType = tournament.MoneyType ?? 0x01,
                        GroupID = group.ID,
                        Name = group.Name,
                        ImagePath18 = group.FullLogoPath18,
                        ImagePath26 = group.FullLogoPath26,
                        ImagePath30 = group.FullLogoPath30,
                        CaptainID = CurrentUser.ID,
                        CaptainLogin = CurrentUser.Login
                    };

                    if (tournament.MoneyType != (int)Tournament.MoneyTypeEnum.Free)
                    {
                        teamView.Fee = tournament.Fee;
                        if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold)
                        {
                            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentGroupFee);
                            if (moneyFee != null && moneyFee.PercentFee > 0)
                            {
                                teamView.Fee = tournament.Fee * (1 + moneyFee.PercentFee / 100);
                            }
                            if (group.MoneyGold < teamView.Fee)
                            {
                                teamView.Disabled = true;
                            }
                        }
                        else
                        {
                            if (group.MoneyWood < teamView.Fee)
                            {
                                teamView.Disabled = true;
                            }
                        }
                    }
                    return View(teamView);
                }
                else
                {
                    var teamView = new TeamView()
                    {
                        TournamentID = id,
                        CaptainID = CurrentUser.ID,
                        CaptainLogin = CurrentUser.Login
                    };
                    if (tournament.MoneyType != (int)Tournament.MoneyTypeEnum.Free)
                    {
                        teamView.Fee = tournament.Fee;
                        if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold)
                        {
                            var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentGroupFee);
                            if (moneyFee != null && moneyFee.PercentFee > 0)
                            {
                                teamView.Fee = tournament.Fee * (1 + moneyFee.PercentFee / 100);
                            }
                        }
                    }
                    return View(teamView);
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult Register(TeamView teamView)
        {
            if (ModelState.IsValid)
            {
                var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == teamView.TournamentID);

                if (tournament != null)
                {
                    string condition = string.Empty;
                    if (tournament.CheckConditionsForRegister(CurrentUser, out condition))
                    {
                        var team = (Team)ModelMapper.Map(teamView, typeof(TeamView), typeof(Team));

                        //pay for participant 
                        var guid = Guid.NewGuid();
                        if (tournament.MoneyType != (int)Tournament.MoneyTypeEnum.Free)
                        {
                            if (teamView.GroupID != null)
                            {
                                var group = Repository.Groups.FirstOrDefault(p => p.ID == teamView.GroupID);
                                var user = Repository.Users.FirstOrDefault(p => p.ID == teamView.CaptainID);

                                if (group == null && user == null)
                                {
                                    return View(teamView);
                                }
                                MoneyDetail feeMoneyDetail = null;
                                var moneyDetailGroup = new MoneyDetail()
                               {
                                   Description = "Взнос на турнир " + tournament.Name,
                               };
                                var tournamentMoneyDetail = new MoneyDetail()
                                {
                                    TournamentID = tournament.ID,
                                };
                                if (group != null)
                                {
                                    moneyDetailGroup.GroupID = group.ID;
                                    tournamentMoneyDetail.Description = "Взнос от " + group.Name;
                                }
                                else
                                {
                                    moneyDetailGroup.UserID = user.ID;
                                    tournamentMoneyDetail.Description = "Взнос от " + user.Login;
                                }

                                if (tournament.MoneyType == (int)Tournament.MoneyTypeEnum.Gold)
                                {
                                    var moneyFee = Repository.MoneyFees.FirstOrDefault(p => p.Type == (int)MoneyFee.TypeEnum.TournamentGroupFee);
                                    if (moneyFee != null && moneyFee.PercentFee > 0)
                                    {
                                        moneyDetailGroup.MoneyFeeID = moneyFee.ID;
                                        moneyDetailGroup.SumGold = -(tournament.Fee ?? 0) * (1 + moneyFee.PercentFee / 100);
                                        feeMoneyDetail = new MoneyDetail()
                                        {
                                            IsFee = true,
                                            SumGold = (tournament.Fee ?? 0) * moneyFee.PercentFee / 100
                                        };
                                    }
                                    else
                                    {
                                        moneyDetailGroup.SumGold = -tournament.Fee ?? 0;
                                    }
                                    tournamentMoneyDetail.SumGold = tournament.Fee ?? 0;
                                }
                                else
                                {
                                    moneyDetailGroup.SumWood = -tournament.Fee ?? 0;
                                    tournamentMoneyDetail.SumWood = tournament.Fee ?? 0;
                                }
                                guid = Repository.CreateTripleMoneyDetail(moneyDetailGroup, tournamentMoneyDetail, feeMoneyDetail);
                            }
                        }

                        Repository.CreateTeam(team, tournament.HotReplacement);
                        Repository.SubmitMoney(guid);
                        var userTeam = new UserTeam()
                        {
                            TeamID = team.ID,
                            UserID = CurrentUser.ID,
                            IsCaptain = true,
                            Accepted = true
                        };
                        Repository.CreateUserTeam(userTeam);
                        Repository.RegisterParticipant(CurrentUser.ID, tournament.ID, team.ID);
                        return View("_OK");
                    }
                    else
                    {
                        ModelState.AddModelError("Common", condition);
                    }
                }
            }
            return View(teamView);
        }

        [HttpGet]
        public ActionResult SetName(int id)
        {
            var teamView = new TeamView()
            {
                TournamentID = id
            };
            return View(teamView);
        }

        [HttpPost]
        public ActionResult SetName(TeamView teamView)
        {
            if (ModelState.IsValid)
            {
                var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == teamView.TournamentID);
                if (tournament != null)
                {
                    var team = (Team)ModelMapper.Map(teamView, typeof(TeamView), typeof(Team));
                    Repository.CreateTeam(team, tournament.HotReplacement);
                    var userTeam = new UserTeam()
                    {
                        TeamID = team.ID,
                        UserID = CurrentUser.ID,
                        IsCaptain = true,
                        Accepted = true
                    };
                    Repository.CreateUserTeam(userTeam);
                    Repository.SetTeamInParticipant(CurrentUser.ID, tournament.ID, team.ID);
                    return View("_OK");
                }
            }
            return View(teamView);
        }

        [Authorize]
        public ActionResult TakePart(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);
            if (team != null)
            {
                if (!team.UserTeams.Any(p => p.UserID == CurrentUser.ID))
                {
                    var userTeam = new UserTeam()
                    {
                        TeamID = team.ID,
                        UserID = CurrentUser.ID
                    };
                    Repository.CreateUserTeam(userTeam);
                }
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        [Authorize]
        public ActionResult TakeOffPart(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);
            if (team != null)
            {
                var userTeam = Repository.UserTeams.FirstOrDefault(p => p.TeamID == team.ID && p.UserID == CurrentUser.ID);
                if (userTeam != null)
                {
                    Repository.RemoveUserTeam(userTeam.ID);
                }
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public ActionResult UploadAvatar(string qqfile)
        {
            var fileName = string.Empty;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && PreviewCreator.SupportMimeType(mimeType.Name))
                    {
                        var ms = GetMemoryStream(inputStream);
                        var avatar30Url = MakePreview(ms, TeamFolder, Avatar30Size);
                        var avatar26Url = MakePreview(ms, TeamFolder, Avatar26Size);
                        var avatar18Url = MakePreview(ms, TeamFolder, Avatar18Size);
                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                ImagePath30 = avatar30Url,
                                ImagePath26 = avatar26Url,
                                ImagePath18 = avatar18Url
                            }
                        }, "text/html");
                    }
                    else
                    {
                        return Json(new
                        {
                            success = true,
                            result = "error",
                            error = "Файл не является изображением"
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error", error = "Не удалось загрузить файл" });
        }

        private static MemoryStream GetMemoryStream(Stream inputStream)
        {
            var buffer = new byte[inputStream.Length];
            var ms = new MemoryStream(buffer);
            inputStream.CopyTo(ms);
            return ms;
        }

        private string MakePreview(MemoryStream ms, string folder, string avatarSize, bool grayscale = false)
        {
            var avatarUrl = string.Format("{0}{1}.jpg", folder, StringExtension.GenerateNewFile());
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSavePreview(ms, previewSize, Server.MapPath(avatarUrl), grayscale);
            }
            return avatarUrl;
        }

        [Authorize]
        public ActionResult AcceptUser(int id)
        {
            var userTeam = Repository.UserTeams.FirstOrDefault(p => p.ID == id);
            if (userTeam != null && userTeam.Team.Captain.ID == CurrentUser.ID)
            {
                userTeam.Accepted = true;
                Repository.UpdateUserTeam(userTeam);
                return Json(new { result = "ok" });
            }

            return Json(new { result = "error" });
        }

        [Authorize]
        public ActionResult AddUser(int id, int teamId)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            var team = Repository.Teams.FirstOrDefault(p => p.ID == teamId);

            if (user != null && team != null && team.Captain.ID == CurrentUser.ID)
            {
                var userTeam = new UserTeam()
                {
                    TeamID = team.ID,
                    UserID = user.ID,
                    Accepted = true
                };
                Repository.CreateUserTeam(userTeam);
                return Json(new { result = "ok" });
            }

            return Json(new { result = "error" });
        }

        [Authorize]
        public ActionResult DeclineUser(int id)
        {
            var userTeam = Repository.UserTeams.FirstOrDefault(p => p.ID == id);
            if (userTeam != null && userTeam.Team.Captain.ID == CurrentUser.ID)
            {
                Repository.RemoveUserTeam(userTeam.ID);
            }
            return Json(new { result = "error" });
        }

        [Authorize]
        public ActionResult Close(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);
            if (team != null && team.Captain.ID == CurrentUser.ID && team.AcceptedCount == team.Tournament.TeamCount)
            {
                Repository.CloseTeam(team.ID);
            }
            return Json(new { result = "ok" });
        }

        public ActionResult GroupRoster(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);
            if (team != null)
            {
                var group = CurrentUser.GroupByGame(team.Tournament.GameID);
                ViewBag.Team = team;
                return View(group);
            }
            return null;
        }

        [Authorize]
        public ActionResult ReplaceUser(int inId, int offId)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == inId);
            var userTeam = Repository.UserTeams.FirstOrDefault(p => p.ID == offId);

            if (user != null && userTeam != null && userTeam.Team.Captain.ID == CurrentUser.ID)
            {
                var tournament = userTeam.Team.Tournament;
                if (!user.IsParticipantInTournament(tournament.ID))
                {
                    Repository.ReplaceUser(userTeam.ID, user.ID);
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "error", error = "Нельзя произвести замену" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AutocompleteUser(string query, int gameID)
        {
            var list = Repository.RegularUsers.Where(p => p.UserGames.Any(r => r.GameID == gameID));

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


        [HttpPost]
        public ActionResult AddPlayer(UserTeamView userTeamView)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == userTeamView.TeamID);
            var user = Repository.Users.FirstOrDefault(p => p.ID == userTeamView.UserID);
            var error = string.Empty;
            if (team != null && user != null)
            {
                if (user.IsParticipantInTournament(team.Tournament.ID))
                {
                    error = "Игрок уже участвует в турнире";
                }
                if (string.IsNullOrWhiteSpace(error))
                {
                    var userTeam = (UserTeam)ModelMapper.Map(userTeamView, typeof(UserTeamView), typeof(UserTeam));
                    userTeam.Accepted = true;
                    Repository.CreateUserTeam(userTeam);
                    return Json(new { result = "ok" });
                }
            }
            else
            {
                error = "Выберите игрока";
                ModelState.AddModelError("UserLogin", "");
            }
            return Json(new { result = "error", error });
        }
    }
}
