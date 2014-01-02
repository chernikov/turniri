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
    public class AvatarController : AdminController
    {
        public ActionResult Index(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            if (tournament != null)
            {
                return View(tournament);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var participant = Repository.Participants.FirstOrDefault(p => p.ID == id);
            if (participant != null)
            {
                var participantView = (ParticipantView)ModelMapper.Map(participant, typeof(Participant), typeof(ParticipantView));

                return View(participantView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(ParticipantView participantView)
        {
            if (ModelState.IsValid)
            {
                var participant = (Participant)ModelMapper.Map(participantView, typeof(ParticipantView), typeof(Participant));
                Repository.SaveParticipant(participant);
                return RedirectToAction("Index", new { id = participant.TournamentID });
            }
            return View(participantView);
        }
    }
}
