using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TutorOnline.Models;

namespace TutorOnline.Areas.EndUser.Controllers
{
    public class HomeController : Controller
    {
        private DBEntities db = new DBEntities();
        private static int endUserId;
        // GET: EndUser/Home
        [HttpGet]
        public ActionResult Index(int? EndUserId)
        {
            if(Session["user"]==null)
            {
                return RedirectToAction("Login", "Default", new { area = "" });
            }
            else if(Session["tutor"]!=null)
            {
                return RedirectToAction("Index", "Home", new { area = "Tutor" });
            }
            else if (Session["admin"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            if (EndUserId!=null)
            {
                endUserId = Convert.ToInt32(EndUserId.ToString());
            }
            var EndUser = db.EndUsers.Find(endUserId);
            ViewBag.UserName = EndUser.EndUserName;
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

        [HttpGet] 
        public ActionResult Requirement()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Default", new { area = "" });
            }
            else if (Session["tutor"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Tutor" });
            }
            else if (Session["admin"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var EndUser = db.EndUsers.SingleOrDefault(x => x.EndUserId == endUserId);
            ViewBag.EndUserId = EndUser.EndUserId;
            ViewBag.UserName = EndUser.EndUserName;
            ViewBag.PhoneNumber = EndUser.PhoneNumber;
            ViewBag.Address = EndUser.Location.LocationName;
            return View();
        }

        [HttpGet]
        public ActionResult ListApplication()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Default", new { area = "" });
            }
            else if (Session["tutor"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Tutor" });
            }
            else if (Session["admin"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var EndUser = db.EndUsers.SingleOrDefault(x => x.EndUserId == endUserId);
            ViewBag.EndUserId = EndUser.EndUserId;
            ViewBag.UserName = EndUser.EndUserName;
            return View();
        }

        [HttpGet]
        public JsonResult Grade()
        {
            try
            {
                var grade = (from g in db.Grades
                             select new
                             {
                                 GradeId = g.GradeId,
                                 GradeName = g.GradeName
                             }).ToList();
                return Json(new { code = 200, grade = grade, mgs = "Load dữ liệu lớp thành công!" }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { code = 500, mgs = "Load dữ liệu thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Subject()
        {
            try
            {
                var subject = (from s in db.Subjects
                               select new
                               {
                                   SubjectId = s.SubjectId,
                                   SubjectName = s.SujectName
                               }).ToList();
                return Json(new { code = 200, subject = subject, mgs = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, mgs = "Fail:"+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Location()
        {
            try
            {
                var location = (from l in db.Locations
                                select new
                                {
                                    LocationID = l.LocationID,
                                    LocationName = l.LocationName
                                }).ToList();
                return Json(new { code = 200, location = location, mgs = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, mgs = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddRequirement(int subjectId, int gradeId, int genderRequirement, float tiution, float costAdmin, string requirementDetail, int lessionNumber, int requirementStatus, int endUserId, int locationId)
        {
            try
            {
                var requirement = new Models.Requirement();
                requirement.SubjectId = subjectId;
                requirement.GradeId = gradeId;
                requirement.GenderRequirement = genderRequirement;
                requirement.Tuition = tiution;
                requirement.CostAdmin = costAdmin;
                requirement.RequirementDetail = requirementDetail;
                requirement.LessionNumber = lessionNumber;
                requirement.RequirementStatus = requirementStatus;
                requirement.EndUserId = endUserId;
                requirement.LocationId = locationId;
                requirement.TutorNumber = 0;
                db.Requirements.Add(requirement);
                db.SaveChanges();
                return Json(new { code = 200, mgs = "Tạo yêu cầu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, mgs = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult RequirementJson()
        {
            try
            {
                var requirement = (from r in db.Requirements.Where(x=> x.RequirementStatus==1)
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

        [HttpGet]
        public JsonResult RequirementSearch(int subjectId, int gradeId, int locationId)
        {
            try
            {
                var requirement = (from req in db.Requirements.Where(x => x.SubjectId == subjectId && x.GradeId == gradeId && x.LocationId == locationId)
                                   select new {
                                       RequirementId = req.RequirementId,
                                       SubjectId = req.SubjectId,
                                       SubjectName = req.Subject.SujectName,
                                       GradeId = req.GradeId,
                                       GradeName = req.Grade.GradeName,
                                       GenderRequirement = req.GenderRequirement,
                                       Tuition = req.Tuition,
                                       CostAdmin = req.CostAdmin,
                                       RequirementDetail = req.RequirementDetail,
                                       LessionNumber = req.LessionNumber,
                                       TutorNumber = req.TutorNumber,
                                       EndUserId = req.EndUserId,
                                       EndUserName = req.EndUser.EndUserName,
                                       PhoneNumber = req.EndUser.PhoneNumber,
                                       Email = req.EndUser.Email,
                                       LocationId = req.LocationId,
                                       LocationName = req.Location.LocationName
                                   });
                return Json(new { code = 200, requirement = requirement, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult RequirementDetail(int requirementId)
        {
            try
            {
                var requirement = (from req in db.Requirements.Where(x => x.RequirementId==requirementId)
                                   select new
                                   {
                                       RequirementId = req.RequirementId,
                                       SubjectId = req.SubjectId,
                                       SubjectName = req.Subject.SujectName,
                                       GradeId = req.GradeId,
                                       GradeName = req.Grade.GradeName,
                                       GenderRequirement = req.GenderRequirement,
                                       Tuition = req.Tuition,
                                       CostAdmin = req.CostAdmin,
                                       RequirementDetail = req.RequirementDetail,
                                       LessionNumber = req.LessionNumber,
                                       TutorNumber = req.TutorNumber,
                                       EndUserId = req.EndUserId,
                                       EndUserName = req.EndUser.EndUserName,
                                       PhoneNumber = req.EndUser.PhoneNumber,
                                       Email = req.EndUser.Email,
                                       LocationId = req.LocationId,
                                       LocationName = req.Location.LocationName
                                   });
                return Json(new { code = 200, requirement = requirement, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult RequirementByEndUserId(int endUserId)
        {
            try
            {
                var requirement = (from req in db.Requirements.Where(x => x.EndUserId == endUserId && x.RequirementStatus==1)
                                   select new
                                   {
                                       RequirementId = req.RequirementId,
                                       SubjectId = req.SubjectId,
                                       SubjectName = req.Subject.SujectName,
                                       GradeId = req.GradeId,
                                       GradeName = req.Grade.GradeName,
                                       GenderRequirement = req.GenderRequirement,
                                       Tuition = req.Tuition,
                                       CostAdmin = req.CostAdmin,
                                       RequirementDetail = req.RequirementDetail,
                                       LessionNumber = req.LessionNumber,
                                       TutorNumber = req.TutorNumber,
                                       EndUserId = req.EndUserId,
                                       EndUserName = req.EndUser.EndUserName,
                                       PhoneNumber = req.EndUser.PhoneNumber,
                                       Email = req.EndUser.Email,
                                       LocationId = req.LocationId,
                                       LocationName = req.Location.LocationName
                                   });
                return Json(new { code = 200, requirement = requirement, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ListRequirement()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Default", new { area = "" });
            }
            else if (Session["tutor"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Tutor" });
            }
            else if (Session["admin"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var EndUser = db.EndUsers.SingleOrDefault(x => x.EndUserId == endUserId);
            ViewBag.UserName = EndUser.EndUserName;
            return View();
        }

        [HttpGet]
        public JsonResult ApplicationByRequirementId(int requirementId)
        {
            try
            {
                ViewBag.Check = true;
                var application = (from a in db.TutorApplications.Where(x => x.RequirementId == requirementId && x.ApplicationStatus==0)
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
                                       Point = a.Tutor.Point,
                                       AccountBalance = a.Tutor.AccountBalance
                                   }).ToList();
                if (application.Count >= 1) ViewBag.Check = false;
                return Json(new { code = 200, application = application, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult UpdateApplicationStatus(int requirementId, int tutorId, int status)
        {
            try
            {
                var application = new Models.TutorApplication();
                application = db.TutorApplications.SingleOrDefault(x => x.RequirementId == requirementId && x.TutorId == tutorId);
                application.ApplicationStatus = status;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
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
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateTutorBalance(int tutorId, float accountBalance)
        {
            try
            {
                var tutor = db.Tutors.SingleOrDefault(x => x.TutorID == tutorId);
                tutor.AccountBalance = accountBalance;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdataStatusRequirement(int requirementId, int status)
        {
            try
            {
                var requirement = new Models.Requirement();
                requirement = db.Requirements.SingleOrDefault(x => x.RequirementId == requirementId);
                requirement.RequirementStatus = status;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Success!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}