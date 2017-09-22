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
    public class DepartmentsController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Departments
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
                    return View(db.Departments.ToList());
                }
                else
                {
                    //Hacer algo que no tiene acceso
                    return RedirectToAction("NoAccess", "Users");
                }
            }
            else
            {
                Session["Controller"] = "Departments";
                return RedirectToAction("Login", "Users");
            }
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = (from TempDepartment in db.Departments
                                     where TempDepartment.DepartmentID == id
                                     select TempDepartment).FirstOrDefault();
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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
        protected string CreateCode(int Amount)
        {
            if (Found("D-" + Amount))
            {
                return CreateCode(Amount + 1);
            }
            else
            {
                return "D-" + Amount;
            }
        }   

        protected Boolean Found(string Code)
        {
            Department Class = db.Departments.Find(Code); 

            if (Class != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private User GetUser()
        {
            return Session["User"] as User;
        }
    }
}
