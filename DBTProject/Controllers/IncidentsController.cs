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
    public class IncidentsController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Incidents
        public ActionResult Index()
        {
            var incidents = db.Incidents.Include(i => i.Department).Include(i => i.Status).Include(i => i.Urgency).Include(i => i.User).Include(i => i.User1);
            return View("Index_ByDept", incidents.ToList());
        }

        // GET: Incidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // GET: Incidents/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName");
            ViewBag.UrgencyID = new SelectList(db.Urgencies, "UrgencyID", "UrgencyName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail");
            ViewBag.TechnicianID = new SelectList(db.Users, "UserID", "UserEmail");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentTitle,IncidentDescription,StatusID,UrgencyID,DepartmentID")] Incident incident)
        {
            incident.IncidentCreationDate = DateTime.Today;
            incident.UserID = -1;

            if (ModelState.IsValid)
            {
                db.Incidents.Add(incident);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", incident.DepartmentID);
            ViewBag.UrgencyID = new SelectList(db.Urgencies, "UrgencyID", "UrgencyName", incident.UrgencyID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", incident.UserID);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", incident.DepartmentID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", incident.StatusID);
            ViewBag.UrgencyID = new SelectList(db.Urgencies, "UrgencyID", "UrgencyName", incident.UrgencyID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", incident.UserID);
            ViewBag.TechnicianID = new SelectList(db.Users, "UserID", "UserEmail", incident.TechnicianID);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentID,IncidentTitle,IncidentDescription,IncidentCreationDate,StatusID,UrgencyID,UserID,TechnicianID,DepartmentID")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", incident.DepartmentID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", incident.StatusID);
            ViewBag.UrgencyID = new SelectList(db.Urgencies, "UrgencyID", "UrgencyName", incident.UrgencyID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", incident.UserID);
            ViewBag.TechnicianID = new SelectList(db.Users, "UserID", "UserEmail", incident.TechnicianID);
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = db.Incidents.Find(id);
            db.Incidents.Remove(incident);
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

        private int GetID()
        {

            return -1;
        }
    }
}
