using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;

namespace turniri.Areas.Default.Controllers
{
    public class PollController : DefaultController
    {
        [Authorize]
        public ActionResult Index(int id, List<int> pollItems)
        {
            var exist = Repository.PollVotes.Any(p => p.PollID == id && p.UserID == CurrentUser.ID);

            if (!exist)
            {
                if (pollItems != null)
                {
                    foreach (var pollItem in pollItems)
                    {
                        var pollVote = new PollVote()
                        {
                            UserID = CurrentUser.ID,
                            PollID = id,
                            PollItemID = pollItem
                        };
                        Repository.CreatePollVote(pollVote);
                    }
                }
                else
                {
                    var pollVote = new PollVote()
                    {
                        UserID = CurrentUser.ID,
                        PollID = id,
                        PollItemID = null
                    };
                    Repository.CreatePollVote(pollVote);
                }
            }
            return Json(new { result = "ok" });
        }

        public ActionResult Voted(int id)
        {
            var poll = Repository.Polls.FirstOrDefault(p => p.ID == id);
            if (poll != null)
            {
                return View(poll);
            }
            return null;
        }
    }
}
