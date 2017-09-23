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
    public class UrgenciesController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Urgencies
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
                    return View(db.Urgencies.ToList());
                }
                else
                {
                    //Hacer algo que no tiene acceso
                    return RedirectToAction("NoAccess", "Users");
                }
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }
        
        // GET: Urgencies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urgency urgency = db.Urgencies.Find(id);
            if (urgency == null)
            {
                return HttpNotFound();
            }
            return View(urgency);
        }

        // GET: Urgencies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Urgencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UrgencyName,UrgencyDescription")] Urgency urgency)
        {
            urgency.UrgencyID = CreateCode(100);
            if (ModelState.IsValid)
            {
                db.Urgencies.Add(urgency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(urgency);
        }

        // GET: Urgencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urgency urgency = db.Urgencies.Find(id);
            if (urgency == null)
            {
                return HttpNotFound();
            }
            return View(urgency);
        }

        // POST: Urgencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UrgencyID,UrgencyName,UrgencyDescription")] Urgency urgency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(urgency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(urgency);
        }

        // GET: Urgencies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urgency urgency = db.Urgencies.Find(id);
            if (urgency == null)
            {
                return HttpNotFound();
            }
            return View(urgency);
        }

        // POST: Urgencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Urgency urgency = db.Urgencies.Find(id);
            db.Urgencies.Remove(urgency);
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

        private User GetUser()
        {
            return Session["User"] as User;
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
            if (db.Urgencies.Find(id) != null)
            {
                return true;
            }
            return false;
        }
    }
}
