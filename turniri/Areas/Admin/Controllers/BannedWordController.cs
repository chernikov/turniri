using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;


namespace turniri.Areas.Admin.Controllers
{
    public class BannedWordController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.BannedWords;
            var data = new PageableData<BannedWord>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var bannedwordView = new BannedWordView();

            return View("Edit", bannedwordView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var bannedword = Repository.BannedWords.FirstOrDefault(p => p.ID == id);

            if (bannedword != null)
            {
                var bannedwordView = (BannedWordView)ModelMapper.Map(bannedword, typeof(BannedWord), typeof(BannedWordView));
                return View(bannedwordView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(BannedWordView bannedwordView)
        {
            if (ModelState.IsValid)
            {
                var bannedword = (BannedWord)ModelMapper.Map(bannedwordView, typeof(BannedWordView), typeof(BannedWord));
                if (bannedword.ID == 0)
                {
                    Repository.CreateBannedWord(bannedword);
                }
                else
                {
                    Repository.UpdateBannedWord(bannedword);
                }
                return RedirectToAction("Index");
            }
            return View(bannedwordView);
        }

        public ActionResult Delete(int id)
        {
            var bannedword = Repository.BannedWords.FirstOrDefault(p => p.ID == id);
            if (bannedword != null)
            {
                Repository.RemoveBannedWord(bannedword.ID);
            }
            return RedirectToAction("Index");
        }

    }
}