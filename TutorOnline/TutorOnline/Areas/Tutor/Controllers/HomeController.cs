using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TutorOnline.Models;

namespace TutorOnline.Areas.Tutor.Controllers
{
    public class HomeController : Controller
    {
        private DBEntities db = new DBEntities();
        private static int tutorId;
        // GET: Tutor/Home
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
            else if(Session["admin"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            if (id != null)
            {
                tutorId = Convert.ToInt32(id.ToString());
            }
            var Tutor = db.Tutors.Find(tutorId);
            ViewBag.UserName = Tutor.TutorName;
            return View();
        }
        public ActionResult LogOut()
        {
            Session["user"] = null;
            Session["tutor"] = null;
            Session["endUser"] = null;
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
            else if (Session["admin"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var Tutor = db.Tutors.SingleOrDefault(x => x.TutorID == tutorId);
            ViewBag.UserName = Tutor.TutorName;
            ViewBag.TutorId = Tutor.TutorID;
            ViewBag.AccountBalance = Tutor.AccountBalance;
            return View();
        }

        public ActionResult ListApplication()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Default", new { area = "" });
            }
            else if (Session["endUser"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "EndUser" });
            }
            else if (Session["admin"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var Tutor = db.Tutors.SingleOrDefault(x => x.TutorID == tutorId);
            ViewBag.UserName = Tutor.TutorName;
            ViewBag.TutorId = Tutor.TutorID;
            return View();
        }

        [HttpPost]
        public JsonResult InsertTutorApplication(int requirementId, int tutorId, int applicationStatus)
        {
            try
            {
                var application = new Models.TutorApplication();
                application.RequirementId = requirementId;
                application.TutorId = tutorId;
                application.ApplicationStatus = applicationStatus;
                db.TutorApplications.Add(application);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Ứng tuyển thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateTutorNumber(int requirementId, int tutorNumber)
        {
            try
            {
                var requirement = db.Requirements.SingleOrDefault(x => x.RequirementId == requirementId);
                requirement.TutorNumber = tutorNumber;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateTutorBalance(int tutorId, float accountBalance)
        {
            try
            {
                var tutor = db.Tutors.SingleOrDefault(x => x.TutorID==tutorId);
                tutor.AccountBalance = accountBalance;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ApplicationDetail(int requirementId, int tutorId)
        {
            try
            {
                ViewBag.Check = true;
                var application = (from a in db.TutorApplications.Where(x => x.RequirementId == requirementId && x.TutorId == tutorId)
                                   select new
                                   {
                                       RequirementId = a.RequirementId,
                                       TutorId = a.TutorId,
                                       ApplicationStatus = a.ApplicationStatus,
                                       SubjectId = a.Requirement.SubjectId,
                                       SubjectName = a.Requirement.Subject.SujectName,
                                       GradeId = a.Requirement.GradeId,
                                       GradeName = a.Requirement.Grade.GradeName,
                                       GenderRequirement = a.Requirement.GenderRequirement,
                                       Tuition = a.Requirement.Tuition,
                                       CostAdmin = a.Requirement.CostAdmin,
                                       RequirementDetail = a.Requirement.RequirementDetail,
                                       LessionNumber = a.Requirement.LessionNumber,
                                       TutorNumber = a.Requirement.TutorNumber,
                                       EndUserId = a.Requirement.EndUserId,
                                       EndUserName = a.Requirement.EndUser.EndUserName,
                                       PhoneNumber = a.Requirement.EndUser.PhoneNumber,
                                       Email = a.Requirement.EndUser.Email,
                                       LocationId = a.Requirement.LocationId,
                                       LocationName = a.Requirement.Location.LocationName,
                                       TutorName = a.Tutor.TutorName,
                                       TutorPhoneNumber = a.Tutor.PhoneNumber,
                                       TutorEmail = a.Tutor.Email,
                                       Gender = a.Tutor.Gender,
                                       TutorJob = a.Tutor.Job.JobName,
                                       Point = a.Tutor.Point

                                   }).ToList();
                if (application.Count >= 1) ViewBag.Check = false;
                return Json(new { code = 200, application = application, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public JsonResult ApplicationByRequirementId(int requirementId)
        {
            try
            {
                ViewBag.Check = true;
                var application = (from a in db.TutorApplications.Where(x => x.RequirementId == requirementId)
                                   select new
                                   {
                                       RequirementId = a.RequirementId,
                                       TutorId = a.TutorId,
                                       ApplicationStatus = a.ApplicationStatus,
                                       SubjectId = a.Requirement.SubjectId,
                                       SubjectName = a.Requirement.Subject.SujectName,
                                       GradeId = a.Requirement.GradeId,
                                       GradeName = a.Requirement.Grade.GradeName,
                                       GenderRequirement = a.Requirement.GenderRequirement,
                                       Tuition = a.Requirement.Tuition,
                                       CostAdmin = a.Requirement.CostAdmin,
                                       RequirementDetail = a.Requirement.RequirementDetail,
                                       LessionNumber = a.Requirement.LessionNumber,
                                       TutorNumber = a.Requirement.TutorNumber,
                                       EndUserId = a.Requirement.EndUserId,
                                       EndUserName = a.Requirement.EndUser.EndUserName,
                                       PhoneNumber = a.Requirement.EndUser.PhoneNumber,
                                       Email = a.Requirement.EndUser.Email,
                                       LocationId = a.Requirement.LocationId,
                                       LocationName = a.Requirement.Location.LocationName,
                                       TutorName = a.Tutor.TutorName,
                                       TutorPhoneNumber = a.Tutor.PhoneNumber,
                                       TutorEmail = a.Tutor.Email,
                                       Gender = a.Tutor.Gender,
                                       TutorJob = a.Tutor.Job.JobName,
                                       Point = a.Tutor.Point

                                   }).ToList();
                if (application.Count >= 1) ViewBag.Check = false;
                return Json(new { code = 200, application = application, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public JsonResult ApplicationByTutorId(int tutorId)
        {
            try
            {
                var application = (from a in db.TutorApplications.Where(x => x.TutorId == tutorId)
                                   select new
                                   {
                                       RequirementId = a.RequirementId,
                                       TutorId = a.TutorId,
                                       ApplicationStatus = a.ApplicationStatus,
                                       SubjectId = a.Requirement.SubjectId,
                                       SubjectName = a.Requirement.Subject.SujectName,
                                       GradeId = a.Requirement.GradeId,
                                       GradeName = a.Requirement.Grade.GradeName,
                                       GenderRequirement = a.Requirement.GenderRequirement,
                                       Tuition = a.Requirement.Tuition,
                                       CostAdmin = a.Requirement.CostAdmin,
                                       RequirementDetail = a.Requirement.RequirementDetail,
                                       LessionNumber = a.Requirement.LessionNumber,
                                       TutorNumber = a.Requirement.TutorNumber,
                                       EndUserId = a.Requirement.EndUserId,
                                       EndUserName = a.Requirement.EndUser.EndUserName,
                                       PhoneNumber = a.Requirement.EndUser.PhoneNumber,
                                       Email = a.Requirement.EndUser.Email,
                                       LocationId = a.Requirement.LocationId,
                                       LocationName = a.Requirement.Location.LocationName

                                   }).ToList();
                return Json(new { code = 200, application = application, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteApplication(int requirementId, int tutorId)
        {
            try
            {
                var tutorApplication = new Models.TutorApplication();
                tutorApplication = db.TutorApplications.SingleOrDefault(x => x.RequirementId == requirementId && x.TutorId == tutorId);
                db.TutorApplications.Remove(tutorApplication);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa dữ liệu thành công!" },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}