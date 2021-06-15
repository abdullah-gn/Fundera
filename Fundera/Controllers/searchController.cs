using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using testProject2.Models;

namespace testProject2.Controllers
{
    public class searchController : Controller
    {
        GPcontext db = new GPcontext();
        // GET: Project
        //form search by username
        public ActionResult Index()
        {
            return View();
        }
        //searh action
        public ActionResult searchForUser(string name)
        {
            List<user> usersNames = db.users.Where(n => n.name.Contains(name)).ToList();
            return View(usersNames);
        }

        public ActionResult catalogDetails()
        {
            ViewBag.catalog = new SelectList(db.catalogs.ToList(), "id", "name");
            return View();
        }

        public ActionResult showProjects(int? id)
        {
            if (id == null)
            {
                return PartialView(db.projects.ToList());
            }
            else
            {
                return PartialView(db.projects.Where(n => n.cat_id == id).ToList());
            }
        }
        public ActionResult showTitProjects(int? id)
        {
            string title = Request.QueryString["title"];
            string mostliks = Request.QueryString["mostliks"];
            int projectStatus = int.Parse(Request.QueryString["projectstatus"]);
            int filterDate=int.Parse(Request.QueryString["filterDate"]);
            IEnumerable<project> projects;
            void fiterProjectMostLikes()
            {
                if (mostliks == "true")
                {
                    //projects = projects.OrderBy(p => p.likesno).Reverse();
                    projects = projects.OrderByDescending(p => p.likesno);
                }
            }

            void filterByProjectStatus()
            {
                if (projectStatus == 1)//funded
                {
                    projects = projects.Where(p => p.fund_id != null);
                }
                else if(projectStatus == 2)//not funded
                {
                    projects = projects.Where(p => p.fund_id == null);
                }
                else//all
                {
                    projects = projects.Where(p => p.fund_id == null || p.fund_id != null);
                }
            }
             void filterBydate()
            {
                if (filterDate != 0)
                {
                    DateTime currenteDate = DateTime.UtcNow.Date.AddDays((int)filterDate);
                    projects = from proj in db.projects
                               where proj.Proj_date >= currenteDate
                               select proj;
                    
                }
                else
                {
                    projects = projects.Where(p => p.Proj_date!=null);
                }
            }

            if (id == null)
            {
                projects = db.projects.ToList();
                fiterProjectMostLikes();
                filterBydate();
                filterByProjectStatus();
                projects = projects.Where(p => p.title.Contains(title) ||title == null).ToList();
                return PartialView("showProjects", projects); 
            }
            else
            {
                projects = db.projects.ToList();
                fiterProjectMostLikes();
                filterBydate();
                filterByProjectStatus();
                projects = projects.Where(n => n.cat_id == id && n.title.Contains(title) || title == null).ToList();   
                return PartialView("showProjects", projects);
            }
        }
        //-------------------------------------------------------------------
        public ActionResult projectDetails(int id)
        {
            if (Session["useremail"] == null)
                return RedirectToAction("login","user");
            else
            {
                project proj = db.projects.Where(p => p.id == id).FirstOrDefault();
                user Owner = db.users.Where(u => u.id == proj.stud_id).FirstOrDefault();
                ViewBag.ownerId = Owner.id;
                ViewBag.ownerName = Owner.name;
                ViewBag.ownerImg = db.users.Where(u => u.id == proj.stud_id).FirstOrDefault().img;
                TempData["reactionsNo"] = db.project_likes.Where(n => n.proj_id == id).ToList().Count();
                string email = Session["useremail"].ToString();
                user s = db.users.Where(n => n.email == email).FirstOrDefault();
                TempData["checkRaect"] = db.project_likes.Where(n => n.proj_id == id && n.user_id == s.id).FirstOrDefault();
                TempData["checkProjectIsFunded"] = db.projects.Where(n => n.id == id).FirstOrDefault().fund_id;
                TempData["checkUserIsFund"] = db.projects.Where(n => n.id == id && n.fund_id == s.id).FirstOrDefault();
                return View(proj);
            }
    
        }
        public ActionResult addComment(int id)
        {
            string commentContent = Request.QueryString["comment"];
            comment userComment = new comment();
            userComment.proj_id = id;
            string email = Session["useremail"].ToString();
            user s = db.users.Where(n => n.email == email).FirstOrDefault();
            userComment.user_id = s.id;//from session
            ViewBag.userName = db.users.Where(u => u.id == s.id).FirstOrDefault().name;
            userComment.com_content = commentContent;
            userComment.date = DateTime.Now;
            //ViewBag.CurrentUser = db.users.Where(n => n.email == Session["useremail"].ToString()).SingleOrDefault().name;
            db.comments.Add(userComment);
            db.SaveChanges();
            TempData["commentNo"] = db.comments.Where(c => c.proj_id == id).ToList().Count();
            return PartialView(userComment);
        }

        public ActionResult addReact(int id)
        {
            project_likes like = new project_likes();
            like.proj_id = id;
            string email = Session["useremail"].ToString();
            user s = db.users.Where(n => n.email == email).FirstOrDefault();
            like.user_id = s.id;//from session;
            db.project_likes.Add(like);
            project proj = db.projects.Where(p => p.id == id).FirstOrDefault();
            proj.likesno += 1;
            db.SaveChanges();
            int no = db.project_likes.Where(n => n.proj_id == id).ToList().Count();
            TempData["reactionsNo"] = no;
            return PartialView();
        }

        public ActionResult removeReact(int id)
        {
            int proj_id = id;
            string email = Session["useremail"].ToString();
            user s = db.users.Where(n => n.email == email).FirstOrDefault();
            int user_id = s.id;//from session
            project_likes like = db.project_likes.Where(n => n.proj_id == proj_id && n.user_id == user_id).FirstOrDefault();
            db.project_likes.Remove(like);
            project proj = db.projects.Where(p => p.id == id).FirstOrDefault();
            proj.likesno -= 1;
            db.SaveChanges();
            int no = db.project_likes.Where(n => n.proj_id == id).ToList().Count();
            TempData["reactionsNo"] = no;
            return PartialView("addReact");
        }

        public ActionResult addFundToProject(int id)
        {
            project proj = db.projects.Where(p => p.id == id).FirstOrDefault();
            string email = Session["useremail"].ToString();
            user s = db.users.Where(n => n.email == email).FirstOrDefault();
            proj.fund_id = s.id;//from session;
            db.SaveChanges();
            TempData["checkProjectIsFunded"] = db.projects.Where(n => n.id == id).FirstOrDefault().fund_id;
            TempData["checkUserIsFund"] = db.projects.Where(n => n.id == id && n.fund_id == s.id).FirstOrDefault();
            sendEmailForFundingProject(id, TempData["checkProjectIsFunded"]);
            return PartialView("add_remove_FundProject");

        }
        public ActionResult removeProjectFunder(int id)
        {
            project proj = db.projects.Where(p => p.id == id).FirstOrDefault();
            proj.fund_id = null;
            db.SaveChanges();
            string email = Session["useremail"].ToString();
            user s = db.users.Where(n => n.email == email).FirstOrDefault();
            TempData["checkProjectIsFunded"] = db.projects.Where(n => n.id == id).FirstOrDefault().fund_id;
            TempData["checkUserIsFund"] = db.projects.Where(n => n.id == id && n.fund_id == s.id).FirstOrDefault();
            sendEmailForFundingProject(id, TempData["checkProjectIsFunded"]);
            return PartialView("add_remove_FundProject");
        }
        public void sendEmailForFundingProject(int id, object projectStatus)
        {
            try
            {
                var mail = new MailMessage();
                var loginInfo = new NetworkCredential("skick006@gmail.com", "P@ssw0rd2021");
                project proj = db.projects.Where(p => p.id == id).FirstOrDefault();
                user ProjectOwner = db.users.Where(u => u.id == proj.stud_id).FirstOrDefault();
                user projectFunder = db.users.Where(u => u.id == proj.fund_id).FirstOrDefault();
                mail.From = new MailAddress("abdalrahmanosman97@gmail.com", " KICK START");
                mail.To.Add(new MailAddress(ProjectOwner.email));
                if (projectStatus == null)
                {
                    mail.Subject = $"The Funding Removed ";
                    mail.Body = $"Dear: {ProjectOwner.name},\n The Funding Removed From Your Project {proj.title}";
                }
                else
                {
                    mail.Subject = $"New Funder For Your Project";
                    mail.Body = $"Dear: {ProjectOwner.name},\n {projectFunder.name} Fund Your Project {proj.title}, You Can Contact him By {projectFunder.email} ";
                }
                //mail.IsBodyHtml=true;
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
            }
            catch (Exception)
            {
                //problem in send an email
            }
            //https://www.google.com/settings/security/lesssecureapps
        }
    }
}