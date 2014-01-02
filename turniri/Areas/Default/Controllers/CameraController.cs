using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace turniri.Areas.Default.Controllers
{
    public class CameraController : DefaultController
    {
        //
        // GET: /Default/Camera/

        public ActionResult Index(int id)
        {
            var camera = Repository.Cameras.FirstOrDefault(p => p.ID == id);
            if (camera != null && camera.Enabled)
            {
                return View(camera);
            }
            else
            {
                return View("_OK");
            }
        }

        public ActionResult Match(int id, int? cameraID)
        {
            var match = Repository.Matches.FirstOrDefault(p => p.ID == id);

            if (match != null && match.IsLive) 
            {
                ViewBag.Match = match;
                if (cameraID != null)
                {
                    var camera = match.Cameras.FirstOrDefault(p => p.ID == cameraID && p.Enabled);
                    if (camera != null)
                    {
                        return View(camera);
                    } 
                }
                else
                {
                    var camera = match.Cameras.FirstOrDefault();
                    if (camera != null)
                    {
                        return View(camera);
                    } 
                }
            }
            return View("_OK");
        }
    }
}
