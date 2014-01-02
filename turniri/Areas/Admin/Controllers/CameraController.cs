using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Models.ViewModels;
using turniri.Model;


namespace turniri.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CameraController : AdminController
    {
        public ActionResult Index(int id)
        {
            var tournament = Repository.Tournaments.FirstOrDefault(p => p.ID == id);
            return View(tournament);
        }

        [HttpGet]
        public ActionResult Main()
        {
            var mainCamera = Repository.MainCameras.FirstOrDefault();
            if (mainCamera == null)
            {
                mainCamera = new MainCamera();
            }
            var mainCameraView = (MainCameraView)ModelMapper.Map(mainCamera, typeof(MainCamera), typeof(MainCameraView));

            return View(mainCameraView);
        }

        [HttpPost]
        public ActionResult Main(MainCameraView mainCameraView)
        {
            if (ModelState.IsValid)
            {
                var mainCamera = (MainCamera)ModelMapper.Map(mainCameraView, typeof(MainCameraView), typeof(MainCamera));

                if (mainCamera.ID == 0)
                {
                    Repository.CreateMainCamera(mainCamera);
                }
                else
                {
                    Repository.UpdateMainCamera(mainCamera);
                }
                TempData["message"] = "Сохранено";
            }
           
            return View(mainCameraView);
        }

        public ActionResult Create(int id)
        {
            var cameraView = new CameraView()
            {
                TournamentID = id
            };

            return View("Edit", cameraView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var camera = Repository.Cameras.FirstOrDefault(p => p.ID == id);

            if (camera != null)
            {
                var cameraView = (CameraView)ModelMapper.Map(camera, typeof(Camera), typeof(CameraView));
                return View(cameraView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CameraView cameraView)
        {
            if (ModelState.IsValid)
            {
                var camera = (Camera)ModelMapper.Map(cameraView, typeof(CameraView), typeof(Camera));
                if (camera.ID == 0)
                {
                    Repository.CreateCamera(camera);
                }
                else
                {
                    Repository.UpdateCamera(camera);
                }
                return RedirectToAction("Index", new { id = camera.TournamentID });
            }
            return View(cameraView);
        }

        public ActionResult Delete(int id)
        {
            var camera = Repository.Cameras.FirstOrDefault(p => p.ID == id);
            if (camera != null)
            {
                var tournamentID = camera.TournamentID;
                Repository.RemoveCamera(camera.ID);
                return RedirectToAction("Index", new { id = tournamentID });
            }
            return RedirectToAction("Index", "Tournament");
        }

    }
}