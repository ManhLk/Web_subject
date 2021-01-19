using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TutorOnline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["endUser"] != null)
            {
                return RedirectToAction("Index", "EndUser/Home");
            }
            else if (Session["tutor"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Tutor" });
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}