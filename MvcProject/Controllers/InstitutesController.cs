using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MvcProject.Models;

namespace MvcProject.Controllers
{
    public class InstitutesController : Controller
    {
        private EmployeeInfoEntities db = new EmployeeInfoEntities();

        // GET: Institutes
        public ActionResult Index()
        {
            return View(db.Institutes.ToList());
        }

        public ActionResult DataInsert()
        {
            return View();
        }

       

        [HttpPost]
        public JsonResult DataInsert( string instituteJason)
        {
            var js = new JavaScriptSerializer();

            Institute[] institute = js.Deserialize<Institute[]>(instituteJason);

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Institutes.AddRange(institute);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Json("Data are inserted in Institute Table");
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return Json("There is a Probem arise");
                }


            }
        }

        // GET: Institutes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institute institute = db.Institutes.Find(id);
            if (institute == null)
            {
                return HttpNotFound();
            }
            return View(institute);
        }

        // GET: Institutes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Institutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstituteID,InstituteName,Address,City")] Institute institute)
        {
            if (ModelState.IsValid)
            {
                db.Institutes.Add(institute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(institute);
        }



        // GET: Institutes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institute institute = db.Institutes.Find(id);
            if (institute == null)
            {
                return HttpNotFound();
            }
            return View(institute);
        }

        // POST: Institutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstituteID,InstituteName,Address,City")] Institute institute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(institute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(institute);
        }

        [Authorize(Roles = "Admin")]

        // GET: Institutes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institute institute = db.Institutes.Find(id);
            if (institute == null)
            {
                return HttpNotFound();
            }
            return View(institute);
        }

        // POST: Institutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Institute institute = db.Institutes.Find(id);
            db.Institutes.Remove(institute);
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
