using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testProject2.Models;

namespace testProject2.Controllers
{


    public class dashboardController : Controller
    {
        GPcontext db = new GPcontext();
       
        public ActionResult Index()
        {
            ViewBag.u = db.users.Where(n =>n.blocked == false).ToList().Count();
            ViewBag.bu = db.users.Where(n=>n.blocked == true).ToList().Count();
            ViewBag.pc = db.projects.ToList().Count();
            string email = Session["useremail"].ToString();
            ViewBag.AdminPic = db.users.Where(n => n.email == email).SingleOrDefault().img;
            ViewBag.AdminName = db.users.Where(n => n.email == email).SingleOrDefault().name;
            return View();
        }

        public ActionResult users()
        {
            ViewBag.users = db.users.ToList();
            return PartialView(db.users.ToList());
        }
        public ActionResult blockedUsers()
        {
            ViewBag.Busers = db.users.ToList().Where(n => n.blocked == true);
            return PartialView();
        }
        public ActionResult projects()
        {
            ViewBag.prs = db.projects.ToList();
            return PartialView();
        }
       
        public JsonResult DeleteProject(int projectId)
        {
            //GPcontext db = new GPcontext();
            bool result = false;
            project pro = db.projects.SingleOrDefault(x => x.id == projectId);

            db.projects.Remove(pro);
            db.SaveChanges();
            result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteUser(int UserId)
        {
            //GPcontext db = new GPcontext();
            bool result = false;
            user u = db.users.SingleOrDefault(x => x.id == UserId);
            List<project> lpro = db.projects.Where(n => n.stud_id == u.id).ToList();
            foreach (var item in lpro)
            {
                db.projects.Remove(item);
            }
            db.users.Remove(u);
            db.SaveChanges();
            result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BlockUser(int UserId)
        {
            //GPcontext db = new GPcontext();
            bool result = false;
            user u = db.users.SingleOrDefault(x => x.id == UserId);
            u.confirmPassword = u.password;
            u.blocked = true;
            db.SaveChanges();
            result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnBlockUser(int UserId)
        {
            //GPcontext db = new GPcontext();
            bool result = false;
            user u = db.users.SingleOrDefault(x => x.id == UserId);
            u.confirmPassword = u.password;
            u.blocked = false;
            db.SaveChanges();
            result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}