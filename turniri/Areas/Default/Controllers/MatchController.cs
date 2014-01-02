using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;
using turniri.Models.ViewModels;

namespace turniri.Areas.Default.Controllers
{
    public class MatchController : DefaultController
    {
        public ActionResult Index(int id)
        {
            Match match = Repository.Matches.FirstOrDefault(p => p.ID == id && p.Status != (int)Match.MatchStatusEnum.Empty);
            if (match != null)
            {
                if (match.IsEmpty)
                {
                    Repository.EmptyMatch(match.ID);
                }
                if (match.IsSingleParticipant && match.Status != (int)Match.MatchStatusEnum.Submit)
                {
                    Repository.SingleParticipantMatch(match.ID);
                    return View("_OK");
                }
                if (match.Status == (int)Match.MatchStatusEnum.Created 
                    && match.Player1 != null && match.Player2 != null)
                {
                    match.Status = (int)Match.MatchStatusEnum.DefinedPlayers;
                    Repository.UpdateStatusMatch(match);
                }
                return View(match);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        public ActionResult CreateComment(int id)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == id);
            ViewBag.AllowImage = true;
            if (match != null)
            {
                var commentView = new CommentView
                {
                    OwnerID = id
                };
                if (match.TournamentID != null)
                {
                    if (match.Tournament.Status == (int)Tournament.StatusEnum.PlayedOut || match.Tournament.Status == (int)Tournament.StatusEnum.Closed)
                    {
                        ViewBag.AllowImage = false;
                    }
                }
                else
                {
                    if (match.Status == (int)Match.MatchStatusEnum.Submit)
                    {
                        ViewBag.AllowImage = false;

                    }
                }
                return View(commentView);
            }
            return null;
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateComment(CommentView commentView)
        {
            if (CurrentUser != null)
            {
                if (string.IsNullOrWhiteSpace(commentView.ImagePath) && string.IsNullOrWhiteSpace(commentView.Text)) 
                {
                    ModelState.AddModelError("Text", "Введите текст комментария");
                }
                if (ModelState.IsValid)
                {
                    Comment comment = CreateBasicComment(commentView);
                    if (comment.ID != 0)
                    {
                        var matchComment = new MatchComment
                        {
                            MatchID = commentView.OwnerID,
                            CommentID = comment.ID
                        };
                        Repository.CreateMatchComment(matchComment);
                    }
                    return View("Ok");
                }
                return View(commentView);
            }
            return null;
        }

        public ActionResult RollbackMatch(int id)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == id);

            if (match.Tournament != null && CurrentUser.IsTournamentAdmin(match.Tournament.ID))
            {
                Repository.RollbackMatch(id);
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
