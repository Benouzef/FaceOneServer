using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaceOneApp.Controllers
{
    public class FaceOneController : Controller
    {
        public ActionResult WebCam()
        {
            return View();
        }
    }
}