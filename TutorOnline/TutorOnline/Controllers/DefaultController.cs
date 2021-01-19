using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TutorOnline.Models;

namespace TutorOnline.Controllers
{
    public class DefaultController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: Default

        public ActionResult SignUp()
        {
            if (Session["endUser"] != null)
            {
                return RedirectToAction("Index", "EndUser/Home");
            }
            else if (Session["tutor"] != null)
            {
                return RedirectToAction("Index", "Tutor/Home");
            }
            else if (Session["admin"] != null)
            {
                return RedirectToAction("Index", "Admin/Home");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if(Session["endUser"]!=null)
            {
                return RedirectToAction("Index", "EndUser/Home");
            }
            else if(Session["tutor"]!=null)
            {
                return RedirectToAction("Index", "Tutor/Home");
            }
            else if(Session["admin"]!=null)
            {
                return RedirectToAction("Index", "Admin/Home");
            }
            return View();
        }

        [HttpGet]
        public JsonResult Account(string userName, string pass)
        {
            var acc = db.Accounts.SingleOrDefault(x => x.UserName == userName && x.Pass == pass);
            if (acc!=null)
            {
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 500, mgs="Tài khoản hoặc mật khẩu không chính xác!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(string userName, string passWord)
        {
            string user = userName;
            string pass = passWord;
            var acc = db.Accounts.SingleOrDefault(x => x.UserName == user && x.Pass == pass);
            if(acc!=null)
            {
                Session["user"] = acc;
                if (acc.AccountType == 3)
                {
                    Session["endUser"] = acc;
                    return RedirectToAction("Index", "EndUser/Home",new { EndUserId = acc.AccountId});
                }
                else if(acc.AccountType==1)
                {
                    Session["tutor"] = acc;
                    return RedirectToAction("Index", "Tutor/Home", new { id = acc.AccountId });
                }
                else if (acc.AccountType == 5)
                {
                    Session["admin"] = acc;
                    return RedirectToAction("Index", "Admin/Home", new { id = acc.AccountId });
                }
            }
            else
            {
                // Đăng nhập không thành công
            }
            return View();
        }
    }
}