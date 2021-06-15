using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testProject2.Models;

namespace testProject2.Controllers
{
    public class projectController : Controller
    {

        GPcontext db = new GPcontext();
        // GET: project

        //create project Action Display View Of create Project
        public ActionResult Index()
        {
            List<catalog> cat = db.catalogs.ToList();
            ViewBag.category = new SelectList(cat, "id", "name");
            return View();
        }

        //Action Create Project
        [HttpPost]
        public ActionResult Index(project p, HttpPostedFileBase img , HttpPostedFileBase video)
        {

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    img.SaveAs(Server.MapPath($"~/attach/{img.FileName}"));
                    p.prototype = img.FileName;
                }
                else
                {
                    p.prototype = "defProject.png";
                }
                video.SaveAs(Server.MapPath($"~/attach/projectVideos/{video.FileName}"));
                p.video = video.FileName;
                p.Proj_date = DateTime.Now;
                //Find User And Pass Id To The Project Id 
                string useremail = Session["useremail"].ToString();
                user u = db.users.Where(n => n.email == useremail).SingleOrDefault();
                p.stud_id = u.id;
                db.projects.Add(p);
                db.SaveChanges();
                return RedirectToAction("DisplayProjects");
            }
            else
            {
                ViewBag.Error = "There Is Issue In The Data";
                return RedirectToAction("Index");
            }

        }

        //Goto To Projects Of the Student 
        public ActionResult DisplayProjects()
        {
            if (Session["useremail"].ToString() == null)
            {
                RedirectToAction("login");
            }
            string useremail = Session["useremail"].ToString();
            user u = db.users.Where(n => n.email == useremail).SingleOrDefault();
            List<project> pp = db.projects.Where(n => n.stud_id == u.id).ToList();
            return View(pp);
        }


        //Delete Project
        public ActionResult delete(int id)
        {
            project p = db.projects.Find(id);
            db.projects.Remove(p);
            db.SaveChanges();
            return RedirectToAction("DisplayProjects");
        }

        //Edite View 
        public ActionResult edite(int id)
        {
            project p = db.projects.Where(n => n.id == id).SingleOrDefault();
            List<catalog> cat = db.catalogs.ToList();
            ViewBag.category = new SelectList(cat, "id", "name");
            return View(p);
        }

        //Edite Project 
        [HttpPost]
        public ActionResult edite(project p, HttpPostedFileBase img, HttpPostedFileBase video)
        {
            project p2 = db.projects.Find(p.id);
            p2.title = p.title;
            p2.bref = p.bref;
            p2.cat_id = p.cat_id;
            p2.description = p.description;
            if (img != null)
            {
                img.SaveAs(Server.MapPath($"~/attach/{img.FileName}"));
                p2.prototype = img.FileName;
            }

            if (video != null)
            {
                video.SaveAs(Server.MapPath($"~/attach/{video.FileName}"));
                p2.video = video.FileName;
            }

            db.SaveChanges();

            return RedirectToAction("DisplayProjects");
        }


        // Project Details 
        // Show Modal For Project Details 
        public ActionResult showDetails(int id)
        {
            project p = db.projects.Find(id);

            ViewBag.pro = p;
            return PartialView();

        }

    }
}