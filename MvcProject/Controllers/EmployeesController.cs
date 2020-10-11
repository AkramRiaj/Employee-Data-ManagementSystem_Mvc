using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcProject.Models;
using PagedList;

namespace MvcProject.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeInfoEntities db = new EmployeeInfoEntities();

        // GET: Employees
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        { 

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var employee = from s in db.Employees.Include(e => e.Department).Include(e => e.Job) select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                employee = employee.Where(s => s.Lastname.ToUpper().Contains(searchString.ToUpper())
                ||s.Department.DepartmentName.ToUpper().Contains(searchString.ToUpper()));
            }


            if (searchString != null)
            {

                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            switch (sortOrder)
            {
                case "name_desc":
                    employee = employee.OrderByDescending(s => s.FirstName);
                    break;
                case "name":
                    employee = employee.OrderBy(s => s.Lastname);
                    break;
                case "Date":
                    employee = employee.OrderBy(s => s.Contact);
                    break;
                default:
                    employee = employee.OrderBy(s => s.Address);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(employee.ToPagedList(pageNumber, pageSize));
           
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobTitle");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,Lastname,Address,Contact,DepartmentID,JobID,ImageFile")] Employee employee , HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                ImageFileCreate.SaveAs(Server.MapPath("~/Image") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Image/" + ImageFileCreate.FileName;
                employee.ImageFile = filePath;

                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobTitle", employee.JobID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobTitle", employee.JobID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,Lastname,Address,Contact,DepartmentID,JobID,ImageFile")] Employee employee, HttpPostedFileBase ImageFileCreate)
        {
            if (ImageFileCreate.ContentLength > 0 && ModelState.IsValid)
            {
                System.IO.File.Delete(Server.MapPath(employee.ImageFile));
                ImageFileCreate.SaveAs(Server.MapPath("~/Image") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Image/" + ImageFileCreate.FileName;
                employee.ImageFile = filePath;

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "JobTitle", employee.JobID);
            return View(employee);
        }

        [Authorize(Roles = "Admin")]

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);

            System.IO.File.Delete(Server.MapPath(employee.ImageFile));
            db.Employees.Remove(employee);
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
