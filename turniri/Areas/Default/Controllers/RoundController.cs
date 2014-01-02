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
    public class RoundController : DefaultController
    {
        [HttpPost]
        public ActionResult Publish(RoundView roundView)
        {
            var round = Repository.Rounds.FirstOrDefault(p => p.ID == roundView.ID);
            if (round != null)
            {
                if (round.Status == (int)Round.RoundStatusEnum.Submit)
                {
                    return Json(new
                    {
                        result = "error",
                        error = "Игра уже сохранена"
                    });
                }
                bool useAdditionalGame;
                if (!roundView.ProcessScoreForGameCategory((Game.GameCategoryEnum)round.Match.Game.GameCategory, out useAdditionalGame))
                {
                    return Json(new
                    {
                        result = "error",
                        error = "Неверный формат ввода"
                    });
                };

                if (roundView.Score1 < 0 || roundView.Score2 < 0)
                {
                    return Json(new
                    {
                        result = "error",
                        error = "Отрицательные числа нельзя ставить"
                    });
                }
                if (round.Match.Tournament != null)
                {
                    //победа поражение - должно быть выставлено
                    if (round.Match.Tournament != null && !round.Match.Tournament.IsRoundForPoints && roundView.Score1 == roundView.Score2)
                    {
                        return Json(new
                        {
                            result = "error",
                            error = "Выставьте победу одному из игроков"
                        });
                    }

                    //победа поражение - если играем 1 матч и это не в группе и не в круговом турнире
                    if (round.Match.Rounds.Count == 1 && (round.Match.Tournament.TournamentType != (int)Tournament.TournamentTypeEnum.RoundRobin && round.Match.Tour.TourType != (int)Tour.TourTypeEnum.Group && round.Match.Tour.TourType != (int)Tour.TourTypeEnum.RoundRobin) && roundView.Score1 == roundView.Score2)
                    {
                        return Json(new
                        {
                            result = "error",
                            error = "Выставьте победу одному из игроков"
                        });
                    }
                }
                else if (round.Match.CountRounds == 1 && roundView.Score1 == roundView.Score2)
                {
                    return Json(new
                    {
                        result = "error",
                        error = "Выставьте победу одному из игроков"
                    });
                }

                //дополнительная игра - не может быть ничьей
                if (round.IsAdditional && roundView.Score1 == roundView.Score2)
                {
                    return Json(new
                    {
                        result = "error",
                        error = "Выставьте победу одному из игроков"
                    });
                }
                //если игра с допонительным временем, то формат должен быть 2+0 и игра будет считаться дополнительной 
                // не для группового тура / не для туров RoundRobin
                if (!roundView.NeedAdditionalTimeFormat(round, useAdditionalGame))
                {
                    return Json(new
                    {
                        result = "error",
                        error = "Выставьте счет в формате дополнительного времени (например: 2+0) хотя бы для одного из игроков"
                    });
                }
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
                if (round.Match.TournamentID.HasValue)
                {
                    if (CurrentUser.IsTournamentAdmin(round.Match.TournamentID.Value))
                    {
                        Repository.SubmitRound(round);
                        if (round.Match.Status == (int)Match.MatchStatusEnum.Submit)
                        {
                            SocialTool.ProcessSocialWinner(Repository, round.MatchID, HostName, Url, Server, Config);
                        }
                    }
                    else
                    {
                        CreateNotifyForSubmit(round);
                    }
                }
                else
                {
                    CreateNotifyForSubmit(round);
                }
                return Json(new
                {
                    result = "ok"
                });
            }
            return null;
        }

        private void CreateNotifyForSubmit(Round round)
        {
            var player = round.Player1.User == round.IntroducerResult ? round.Player2 : round.Player1;
            var user = player.User;

            var notice = new Notice()
            {
                ReceiverID = user.ID,
                Caption = "Подтверждение результата",
                MatchID = round.MatchID,
                Type = (int)Notice.TypeEnum.Match,
                Text = NoticeText(round, player)
            };
            Repository.CreateNotice(notice);

            var submitAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("Submit", "Round", new { id = round.ID }),
                Name = "Подтвердить",
                IsResolveNotice = true,
                Direct = false
            };

            var disputeAction = new NoticeAction()
            {
                NoticeID = notice.ID,
                ActionUrl = Url.Action("Dispute", "Round", new { id = round.ID }),
                Name = "Оспорить",
                IsResolveNotice = true,
                Direct = false
            };
            Repository.CreateNoticeAction(submitAction);
            Repository.CreateNoticeAction(disputeAction);
        }

        private string NoticeText(Round round, Participant player)
        {
            if (round.Match.Tournament != null)
            {
                if (round.Match.CountRounds > 1)
                {
                    string roundName = null;
                    if (round.Match.Tournament.HostGuest)
                    {
                        if (round.IsAdditional)
                        {
                            roundName = "дополнительном";
                        }
                        else
                        {
                            if ((round.Player1IsHost && round.Player1 == player) ||
                            (!round.Player1IsHost && round.Player2 == player))
                            {
                                roundName = "домашнем";
                            }
                            else
                            {
                                roundName = "гостевом";
                            }
                        }
                    }
                    else
                    {
                        if (round.IsAdditional)
                        {
                            roundName = "дополнительном";
                        }
                        else
                        {
                            if (round.Match.Round1 == round)
                            {
                                roundName = "первом";
                            }
                            else
                            {
                                roundName = "втором";
                            }
                        }
                    }
                    return string.Format("Ваш соперник установил счет в  {0} раунде матча {1} : \r\n {2} {3} : {4} {5}", roundName, round.Match.Name, round.Player1.ActualName, round.Score1Text, round.Score2Text, round.Player2.ActualName);
                }
                else
                {
                    return string.Format("Ваш соперник установил счет в  матче {0} : \r\n {1} {2} : {3} {4}", round.Match.Name, round.Player1.ActualName, round.Score1Text, round.Score2Text, round.Player2.ActualName);
                }
            }
            else
            {
                if (round.Match.CountRounds > 1)
                {
                    string roundName = null;

                    if (round.IsAdditional)
                    {
                        roundName = "дополнительном";
                    }
                    else
                    {
                        if (round.Match.Round1 == round)
                        {
                            roundName = "первом";
                        }
                        else
                        {
                            roundName = "втором";
                        }
                    }
                    return string.Format("Ваш соперник установил счет в {0} раунде матча {1}: \r\n {2} {3} : {4} {5}", roundName, round.Match.Name, round.Player1.ActualName, round.Score1Text, round.Score2Text, round.Player2.ActualName);
                }
                else
                {
                    return string.Format("Ваш соперник установил счет в  матче {0}: \r\n {1} {2} : {3} {4}", round.Match.Name, round.Player1.ActualName, round.Score1Text, round.Score2Text, round.Player2.ActualName);
                }
            }
        }

        [HttpPost]
        public ActionResult TechSubmit(Match matchView)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == matchView.ID);
            if (match != null)
            {
                foreach (var round in match.Rounds.Where(p => p.Status == (int)Round.RoundStatusEnum.Created).ToList())
                {
                    round.Score1 = matchView.Score1.Value;
                    round.Score2 = matchView.Score2.Value;
                    Repository.TechSubmitRound(round);
                }


                if (match.WinnerID.HasValue)
                {
                    /* notify winner */
                    var winNotice = new Notice()
                    {
                        ReceiverID = match.Winner.UserID,
                        Caption = "Техническая победа",
                        Text = string.Format("Вам присвоена техническая победа в матче с {0}", match.Loser.ActualName),
                        Type = (int)Notice.TypeEnum.Match,
                        MatchID = match.ID
                    };
                    Repository.CreateNotice(winNotice);
                    /* notify loser */
                    var loseNotice = new Notice()
                    {
                        ReceiverID = match.Loser.UserID,
                        Caption = "Техническое поражение",
                        Text = string.Format("Вам присвоено техническое поражение в матче с {0}", match.Winner.ActualName),
                        Type = (int)Notice.TypeEnum.Match,
                        MatchID = match.ID
                    };
                    Repository.CreateNotice(loseNotice);
                }

                return Json(new
                {
                    result = "ok"
                });
            }
            return null;
        }

        [HttpPost]
        public ActionResult TechAllLoserSubmit(int id)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == id);
            if (match != null)
            {
                Repository.TechAllLoseMatch(match.ID);

                /* notify loser */
                var loseNotice1 = new Notice()
                {
                    ReceiverID = match.Player1.UserID,
                    Caption = "Техническое поражение",
                    Text = string.Format("Вам присвоено техническое поражение в матче с {0}", match.Player2.ActualName),
                    Type = (int)Notice.TypeEnum.Match,
                    MatchID = match.ID
                };
                Repository.CreateNotice(loseNotice1);

                var loseNotice2 = new Notice()
                {
                    ReceiverID = match.Player2.UserID,
                    Caption = "Техническое поражение",
                    Text = string.Format("Вам присвоено техническое поражение в матче с {0}", match.Player1.ActualName),
                    Type = (int)Notice.TypeEnum.Match,
                    MatchID = match.ID
                };
                Repository.CreateNotice(loseNotice2);

                return Json(new
                {
                    result = "ok"
                });
            }
            return null;
        }

        [HttpPost]
        public ActionResult Submit(int id)
        {
            var round = Repository.Rounds.FirstOrDefault(p => p.ID == id);
            if (round != null)
            {
                Repository.SubmitRound(round);

                if (round.Match.Status == (int)Match.MatchStatusEnum.Submit)
                {
                    SocialTool.ProcessSocialWinner(Repository, round.MatchID, HostName, Url, Server, Config);
                }
                return Json(new
                {
                    result = "ok"
                });
            }
            return null;
        }

        [HttpPost]
        public ActionResult Dispute(int id)
        {
            var round = Repository.Rounds.FirstOrDefault(p => p.ID == id);
            if (round != null)
            {
                Repository.DisputeRound(round);

                IEnumerable<User> admins = null;
                if (round.Match.Tournament != null)
                {
                    admins = round.Match.Tournament.Admins;
                }
                else
                {
                    admins = Repository.Users.Where(p => p.UserRoles.Count(r => r.Role.Code == "admin") > 0);
                }

                foreach (var admin in admins)
                {
                    var subject = new Subject
                    {
                        Name = string.Format("Оспаривание матча №{0}", round.Match.Number),
                    };
                    Repository.CreateSubject(subject);

                    var message = new Message()
                    {
                        SenderID = CurrentUser.ID,
                        ReceiverID = admin.ID,
                        SubjectID = subject.ID,
                        Text = string.Format("Я ({0}) прошу разобраться в счете <a href=\"/admin/Match/Edit/{1}\"> матча </a>", CurrentUser.Login, round.ID)
                    };
                    SendNewPrivateMessage(subject, message);
                }

                return Json(new
                {
                    result = "ok"
                });
            }
            return null;
        }

        public ActionResult RollbackRound(int id)
        {
            var round = Repository.Rounds.FirstOrDefault(p => p.ID == id);

            if (round.Match.Tournament != null && CurrentUser.IsTournamentAdmin(round.Match.Tournament.ID))
            {
                Repository.RollbackRound(id);
                return Json(new
                {
                    result = "ok",
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                result = "error",
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
