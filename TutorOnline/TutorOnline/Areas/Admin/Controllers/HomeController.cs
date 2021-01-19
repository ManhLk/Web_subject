using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TutorOnline.Models;

namespace TutorOnline.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private DBEntities db = new DBEntities();
        private static int adminId;
        // GET: Admin/Home
        public ActionResult Index(int? id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Default", new { area = "" });
            }
            else if (Session["endUser"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "EndUser" });
            }
            else if(Session["tutor"] != null)
            {
                return RedirectToAction("Index", "Tutor/Home");
            }
            if (id != null)
            {
                adminId = Convert.ToInt32(id.ToString());
            }
            var Admin = db.Administrators.Find(adminId);
            ViewBag.UserName = Admin.AdminName;
            return View();
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session["user"] = null;
            Session["endUser"] = null;
            Session["tutor"] = null;
            Session["admin"] = null;
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult ListRequirement()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Default", new { area = "" });
            }
            else if (Session["endUser"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "EndUser" });
            }
            else if (Session["tutor"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var Admin = db.Administrators.Find(adminId);
            ViewBag.UserName = Admin.AdminName;
            ViewBag.AdminId = Admin.AdminId;
            return View();
        }

        public ActionResult ListRequirement_checking()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Default", new { area = "" });
            }
            else if (Session["endUser"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "EndUser" });
            }
            else if (Session["tutor"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var Admin = db.Administrators.Find(adminId);
            ViewBag.UserName = Admin.AdminName;
            ViewBag.AdminId = Admin.AdminId;
            return View();
        }

        public JsonResult RequirementJson()
        {
            try
            {
                var requirement = (from r in db.Requirements.Where(x => x.RequirementStatus == 0)
                                   select new
                                   {
                                       RequirementId = r.RequirementId,
                                       SubjectId = r.SubjectId,
                                       SubjectName = r.Subject.SujectName,
                                       GradeId = r.GradeId,
                                       GradeName = r.Grade.GradeName,
                                       GenderRequirement = r.GenderRequirement,
                                       Tuition = r.Tuition,
                                       CostAdmin = r.CostAdmin,
                                       RequirementDetail = r.RequirementDetail,
                                       LessionNumber = r.LessionNumber,
                                       TutorNumber = r.TutorNumber,
                                       EndUserId = r.EndUserId,
                                       EndUserName = r.EndUser.EndUserName,
                                       PhoneNumber = r.EndUser.PhoneNumber,
                                       Email = r.EndUser.Email,
                                       LocationId = r.LocationId,
                                       LocationName = r.Location.LocationName
                                   }).ToList();
                return Json(new { code = 200, requirement = requirement, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}