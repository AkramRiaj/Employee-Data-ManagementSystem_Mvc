using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    public class EmploymentHistoriesController : Controller
    {
        private EmployeeInfoEntities db = new EmployeeInfoEntities();

        // GET: EmploymentHistories
        public ActionResult Index()
        {
            var employmentHistories = db.EmploymentHistories.Include(e => e.Certificate).Include(e => e.Employee).Include(e => e.Institute);
            return View(employmentHistories.ToList());
        }

        // GET: EmploymentHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmploymentHistory employmentHistory = db.EmploymentHistories.Find(id);
            if (employmentHistory == null)
            {
                return HttpNotFound();
            }
            return View(employmentHistory);
        }

        // GET: EmploymentHistories/Create
        public ActionResult Create()
        {
            ViewBag.CertificateID = new SelectList(db.Certificates, "CertificateID", "CertificateName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName");
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName");
            return View();
        }

        // POST: EmploymentHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeID,CertificateID,InstituteID,JobStarttDate,JobEndDate")] EmploymentHistory employmentHistory)
        {
            if (ModelState.IsValid)
            {
                db.EmploymentHistories.Add(employmentHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CertificateID = new SelectList(db.Certificates, "CertificateID", "CertificateName", employmentHistory.CertificateID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", employmentHistory.EmployeeID);
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName", employmentHistory.InstituteID);
            return View(employmentHistory);
        }

        // GET: EmploymentHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmploymentHistory employmentHistory = db.EmploymentHistories.Find(id);
            if (employmentHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CertificateID = new SelectList(db.Certificates, "CertificateID", "CertificateName", employmentHistory.CertificateID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", employmentHistory.EmployeeID);
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName", employmentHistory.InstituteID);
            return View(employmentHistory);
        }

        // POST: EmploymentHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,CertificateID,InstituteID,JobStarttDate,JobEndDate")] EmploymentHistory employmentHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employmentHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CertificateID = new SelectList(db.Certificates, "CertificateID", "CertificateName", employmentHistory.CertificateID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", employmentHistory.EmployeeID);
            ViewBag.InstituteID = new SelectList(db.Institutes, "InstituteID", "InstituteName", employmentHistory.InstituteID);
            return View(employmentHistory);
        }

        [Authorize(Roles = "Admin")]
        // GET: EmploymentHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmploymentHistory employmentHistory = db.EmploymentHistories.Find(id);
            if (employmentHistory == null)
            {
                return HttpNotFound();
            }
            return View(employmentHistory);
        }

        // POST: EmploymentHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmploymentHistory employmentHistory = db.EmploymentHistories.Find(id);
            db.EmploymentHistories.Remove(employmentHistory);
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
    }
}
