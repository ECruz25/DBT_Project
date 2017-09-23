using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBTProject;

namespace DBTProject.Controllers
{
    public class UsersController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Users
        public ActionResult Index()
        {
            User LoggedUser = GetUser();

            if (LoggedUser != null)
            {
                Profile Profile = (from TempProfile in db.Profiles
                                   where TempProfile.ProfileID == LoggedUser.ProfileID
                                   select TempProfile).FirstOrDefault();

                if (Profile.ProfileName == "Admin")
                {
                    var users = db.Users.Include(u => u.Profile);
                    return View(users.ToList());
                }
                else
                {
                    //Hacer algo que no tiene acceso
                    return RedirectToAction("NoAccess");
                }
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "ProfileName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserEmail,UserPassword,UserName,UserBirthday,ProfileID")] User user)
        {
            user.UserID = CreateCode(db.Users.Count());
            user.UserLastActivity = DateTime.Now.ToString();

            if (UserExists(user.UserEmail))
            {
                ViewBag.Message = "Correo electronico ya en uso";
                return View("NoAccess");
            }
            else if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "ProfileName");
            return View(user);
        }

        public JsonResult CheckUsernameAvailability(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SeachData = db.Users.Where(x => x.UserEmail == userdata).SingleOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        private bool UserExists(string email)
        {
            User user = (from TempUser in db.Users
                         where TempUser.UserEmail == email
                         select TempUser).FirstOrDefault();

            if (user != null)
            {
                return true;
            }
            return false;
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "ProfileName", user.ProfileID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserEmail,UserPassword,UserLastActivity,UserName,ProfileID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "ProfileName", user.ProfileID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            ViewBag.Message = Session["ViewBag"];
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "UserEmail,UserPassword")] User user)
        {
            User UserTemp = (from Users in db.Users
                             where Users.UserEmail == user.UserEmail &&
                             Users.UserPassword == user.UserPassword
                             select Users).FirstOrDefault();

            if (UserTemp == null)
            {
                UserTemp = (from Users in db.Users
                            where Users.UserEmail == user.UserEmail
                            select Users).FirstOrDefault();
                if (UserTemp == null)
                {
                    Session["ViewBag"] = null;
                    return RedirectToAction("SignUp");
                }
                Session["ViewBag"] = "Usuario Incorrecto";
                return RedirectToAction("Login");
            }
            else
            {
                Session["ViewBag"] = null;
                Session["User"] = UserTemp;
                UpdateLastActivity(UserTemp);
                return RedirectToAction("Index", GetController());
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "UserEmail,UserPassword,UserName,UserBirthday")] User user)
        {
            user.UserID = CreateCode(db.Users.Count());
            user.UserLastActivity = DateTime.Now.ToString();
            user.ProfileID = 1;
            if (UserExists(user.UserEmail))
            {
                //modificar para que aparezca un msj
                ViewBag.Message = "Correo electronico ya en uso";
                return View("NoAccess");
            }
            else if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "ProfileName", user.ProfileID);
            return View(user);
        }

        private string GetController()
        {
            return Session["Controller"] as string;
        }

        private User GetUser()
        {
            return Session["User"] as User;
        }

        public ActionResult NoAccess()
        {
            ViewBag.Message = "You don't have the required permissions to acces this page";
            return View("NoAccess");
        }

        public ActionResult LogOut()
        {
            UpdateLastActivity(GetUser());
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }

        private void UpdateLastActivity(User User)
        {
            User Tempuser = db.Users.Find(User.UserID);
            Tempuser.UserLastActivity = DateTime.Now.ToString();
            db.SaveChanges();
        }

        private int CreateCode(int amount)
        {
            if (IdExists(amount))
            {
                return CreateCode(amount + 1);
            }
            else
            {
                return amount;
            }
        }

        private bool IdExists(int id)
        {
            if (db.Users.Find(id) != null)
            {
                return true;
            }
            return false;
        }
    }
}
