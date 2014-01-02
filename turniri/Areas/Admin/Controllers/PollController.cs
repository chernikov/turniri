using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;


namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,game_admin")]
    public class PollController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            IQueryable<Poll> list = null;
            if (CurrentUser.InRoles("admin"))
            {
                list = Repository.Polls.OrderByDescending(p => p.AddedDate);
            }
            else
            {
                list = Repository.Polls.Where(p => p.UserID == CurrentUser.ID).OrderByDescending(p => p.AddedDate);
            }
            var data = new PageableData<Poll>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var pollView = new PollView()
            {
                UserID = CurrentUser.ID,
                CanUpdateItems = true,
                PollItems = new List<PollItemView>() {
                    new PollItemView() {
                        Description = "Выбор 1",
                    },
                    new PollItemView() {
                        Description = "Выбор 2",
                    }
                }
            };

            return View("Edit", pollView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var poll = Repository.Polls.FirstOrDefault(p => p.ID == id);

            if (poll != null)
            {
                var pollView = (PollView)ModelMapper.Map(poll, typeof(Poll), typeof(PollView));
                return View(pollView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(PollView pollView)
        {
            if (ModelState.IsValid)
            {
                var poll = (Poll)ModelMapper.Map(pollView, typeof(PollView), typeof(Poll));
                poll.UserID = CurrentUser.ID;
                if (poll.ID == 0)
                {
                    Repository.CreatePoll(poll);
                }
                else
                {
                    Repository.UpdatePoll(poll);
                }
                return RedirectToAction("Index");
            }
            return View(pollView);
        }

        public ActionResult Delete(int id)
        {
            var poll = Repository.Polls.FirstOrDefault(p => p.ID == id);
            if (poll != null)
            {
                Repository.RemovePoll(poll.ID);
            }
            return RedirectToAction("Index");
        }


        public ActionResult GetPollItem()
        {
            var pollItemView = new PollItemView();
            return View(pollItemView);
        }
    }
}