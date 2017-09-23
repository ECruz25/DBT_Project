using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBTProject;
using System.IO;

namespace DBTProject.Controllers
{
    public class IncidentsController : Controller
    {
        private ProjectEntities db = new ProjectEntities();

        // GET: Incidents
        public ActionResult Index()
        {
            User User = GetUser();
            Session["Controller"] = "Incidents";

            if (User != null)
            {
                Profile Profile = db.Profiles.Find(User.ProfileID);
                if (Profile.ProfileName == "Admin")
                {
                    var incidents = db.Incidents.Include(i => i.Department).Include(i => i.Status).Include(i => i.Urgency).Include(i => i.User).Include(i => i.User1);
                    return View(incidents.ToList());
                }
                else if(Profile.ProfileName == "Tecnico")
                {
                    var Incidents = from TempIncidents in db.Incidents
                                    where TempIncidents.TechnicianID == User.UserID ||
                                            TempIncidents.UserID == User.UserID
                                    select TempIncidents;
                    return View(Incidents.ToList());
                }
                else if(Profile.ProfileName == "Cliente")
                {
                    var Incidents = from TempIncidents in db.Incidents
                                    where TempIncidents.UserID == User.UserID
                                    select TempIncidents;
                    return View(Incidents.ToList());
                }
                else
                {
                    //Cambiar a que no tiene acceso
                    return RedirectToAction("NoAccess", "Users");
                }
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }

        public ActionResult Export()
        {
            return View();
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
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            ViewBag.TechnicianID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentTitle,IncidentDescription,UrgencyID,DepartmentID")] Incident incident)
        {
            incident.IncidentID = CreateCode(1);
            incident.IncidentCreationDate = DateTime.Today;
            incident.UserID = GetUser().UserID;
            incident.TechnicianID = -1;
            incident.StatusID = GetDefaultStatus().StatusID;

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
            
            var Technicians = from Tempuser in db.Users
                              where Tempuser.ProfileID != 1
                              select Tempuser;

            List<SelectListItem> TechIDs = new List<SelectListItem>();

            foreach (var x in Technicians)
            {
                TechIDs.Add(new SelectListItem
                {
                    Text = x.UserName,
                    Value = x.UserID.ToString()
                });
            }

            ViewBag.TechnicianID = TechIDs;

            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentTitle,IncidentDescription,StatusID,UrgencyID,TechnicianID,DepartmentID")] Incident incident)
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
            if (db.Incidents.Find(id) != null)
            {
                return true;
            }
            return false;
        }

        private User GetUser()
        {
            return Session["User"] as User;
        }

        [HttpPost]
        public ActionResult Export([Bind(Include = "StartDate")] DateTime StartDate, [Bind(Include = "EndDate")] DateTime EndDate)
        {
            ReportByDate(StartDate, EndDate);
            return View("Index", db.Incidents.ToList());
        }

        private ActionResult ReportByDate(DateTime StartingDate, DateTime EndDate)
        {
            var query = from Incidents in db.Incidents
                        where Incidents.IncidentCreationDate >= StartingDate && Incidents.IncidentCreationDate <= EndDate
                        select Incidents;
           
            string str = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            StreamWriter file = new StreamWriter(str + "\\text2.csv");
            file.WriteLine("IncidentID,Title,CreationDate,User,Status,Urgency,Technician");

            foreach(var x in query)
            {
                Status Status = (from TempStatus in db.Status
                                     where TempStatus.StatusID == x.StatusID
                                     select TempStatus).FirstOrDefault();

                Urgency Urgency = (from TempUrgency in db.Urgencies
                                   where TempUrgency.UrgencyID == x.UrgencyID
                                   select TempUrgency).FirstOrDefault();

                User User = (from TempUser in db.Users
                             where TempUser.UserID == x.UserID
                             select TempUser).FirstOrDefault();

                User User2 = (from TempUser in db.Users
                             where TempUser.UserID == x.TechnicianID
                             select TempUser).FirstOrDefault();

                file.WriteLine(x.IncidentID + "," + x.IncidentTitle + "," + x.IncidentCreationDate + "," + User.UserName + 
                                "," + Status.StatusName + "," + Urgency.UrgencyName + ","+ User2.UserName);
            }

            file.Flush();
            file.Close();

            return View("Index", db.Incidents.ToList());
        }

        private Status GetDefaultStatus()
        {
            Status Status = (from TempStatus in db.Status
                             where TempStatus.StatusName == "Nuevo"
                             select TempStatus).FirstOrDefault();

            if(Status!=null)
            {
                return Status;
            }
            return (from TempStatus in db.Status
                    select TempStatus).FirstOrDefault();
        }
    }
}