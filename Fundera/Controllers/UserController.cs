using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testProject2.Models;

namespace testProject2.Controllers
{
    public class UserController : Controller
    {
        GPcontext db = new GPcontext();

        // GET: User

        //Empty Index
        public ActionResult Index()
        {
            return View();
        }




        public ActionResult login()
        {
            if(Request.Cookies["fullstack"] != null)
            {
                Session["useremail"] = Request.Cookies["fullstack"].Values["email"];
                Session["userrole"] = Request.Cookies["fullstack"].Values["role"];
                return RedirectToAction("timeline");
            }
                return View();
        }
        [HttpPost]
        public ActionResult login(login s , string remeberme)
        {
            login us = db.logins.Where(n => n.email == s.email && n.password == s.password).SingleOrDefault();
            
            //login
            if (us != null)
            {
                //GetHashCode the User 
                user ss = db.users.Where(n => n.email == us.email).SingleOrDefault();

                //checked if The user is blocked Or Not  
                if(ss.blocked == true)
                {
                   return RedirectToAction("blocked");
                }
                TempData["userimage"] = ss.img;
                //Add Session 
                Session.Add("useremail", us.email);
                Session.Add("userrole", ss.role.name);

                //AdditionalMetadataAttribute Cookie   
                if (remeberme == "true")
                {
                    HttpCookie co = new HttpCookie("fullstack");
                    co.Values.Add("email", us.email.ToString());
                    co.Values.Add("role", ss.role.name.ToString());
                    co.Expires = DateTime.Now.AddDays(2);
                    Response.Cookies.Add(co);
                }

                return RedirectToAction("timeline");
            }
            else //  Not login
            {
                ViewBag.status = "invalid username or password";
                return View();
            }

        }

        //logout 
        public ActionResult logout()
        {
            Session["useremail"] = null;
            Session["userrole"] = null;
            Response.Cookies["fullstack"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Home");
        }




        //RegIster Action As User  =>   View
        public ActionResult signup() { 
        
            return View();
        }

        //RegIster Action As User  => Sign Up And Goto To Complete Profile Data
        [HttpPost]
        public ActionResult signup(user u, int userRole , HttpPostedFileBase img)
        {

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    img.SaveAs(Server.MapPath($"~/attach/profilePic/{img.FileName}"));
                    u.img = img.FileName;
                }
                else
                {
                    u.img = "Default.jpg";
                }

                u.role_id = userRole;
                u.blocked = false;

                db.users.Add(u);
                db.SaveChanges();
                Session.Add("useremail", u.email);
                if(userRole == 2)
                {
                    Session.Add("userrole","Funder");
                }
                else if (userRole == 3)
                {
                    Session.Add("userrole", "Student");
                }
                
                login newlogin = new login() { id = u.id, email = u.email, password = u.password };
                db.logins.Add(newlogin);
                db.SaveChanges();
                return RedirectToAction($"profile/{u.id}");
            }
            else
            {
               
                return RedirectToAction("signup");
            }
          
        }

        //Chech if The Email Is Repeate in Database Or not 
        public ActionResult emailChecker(string email)
        {

            user s = db.users.Where(n => n.email == email).SingleOrDefault();
            if(s == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            } else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
           
        }


        public ActionResult profile(int id)
        {
            if (Session["useremail"] == null) { return RedirectToAction("login"); }
            user s = db.users.Where(n => n.id == id).FirstOrDefault();
            return View(s);
        }

        public ActionResult blocked()
        {
            return View();
        }



        /* Tawfiq */
        public ActionResult edit(int id)
        {
                if (Session["useremail"]==null)
                {
                    return RedirectToAction("login");
                }
                user myUser = db.users.Where(u => u.id == id).FirstOrDefault();
                string email = Session["useremail"].ToString();
                user s = db.users.Where(n => n.email == email).FirstOrDefault();
                if (myUser.id == s.id)
                {
                    return View(myUser);
                }
                else
                {
                    return View("Error");
                }
            
        }
        [HttpPost]
        public ActionResult edit(user u, HttpPostedFileBase img)
        {
            user myUser = db.users.Where(us => us.id == u.id).FirstOrDefault();
            if (img != null)
            {
                img.SaveAs(Server.MapPath($"~/attach/profilePic/{img.FileName}"));
                myUser.img = img.FileName;
            }

            myUser.name = u.name;
            myUser.age = u.age;
            myUser.phone = u.phone;
            myUser.address = u.address;
            myUser.confirmPassword = myUser.password;
            myUser.graduation_year = u.graduation_year;
            myUser.aboutMe = u.aboutMe;
            //myUser.College = u.College;
            //myUser.job_title = u.job_title;

            db.SaveChanges();
            return RedirectToAction($"profile/{myUser.id}");
        }

        public ActionResult changePassword(int id)
        {
            if (Session["useremail"] == null)
            {
                return RedirectToAction("login");
            }
            user myUser = db.users.Where(u => u.id == id).FirstOrDefault();
            string email = Session["useremail"].ToString();
            user s = db.users.Where(n => n.email == email).FirstOrDefault();
            if (myUser.id == s.id)  return View(myUser);
            else return View("Error");
        }
        [HttpPost]
        public ActionResult changePassword(user u, string OP)
        {
            user myUser = db.users.Where(us => us.id == u.id).FirstOrDefault();
            login lo = db.logins.Where(n => n.id == u.id).SingleOrDefault();   


            if (OP == myUser.password)
            {
                myUser.password = u.password;
                myUser.confirmPassword = u.confirmPassword;
                lo.password = u.password;
                db.SaveChanges();
                return RedirectToAction($"profile/{myUser.id}");
            }
            else
            {
                ViewBag.mes = "You have Entered wrong PAssword";

                user sameUser = db.users.Where(usr => usr.id == u.id).FirstOrDefault();
                return View(myUser);

            }
        }


        public ActionResult TimeLine()
        {
            //string email = Session["useremail"].ToString();

            if (Session["useremail"].ToString() == null)
            {
                RedirectToAction("login");
            }
            string email = Session["useremail"].ToString();
            user s = db.users.Where(n => n.email == email).FirstOrDefault();
            ViewBag.user = s;
            return View(db.projects.ToList());
        }



    }
}