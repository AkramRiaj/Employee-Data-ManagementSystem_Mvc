using MvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class DetailsController : Controller
    {
        private EmployeeInfoEntities db = new EmployeeInfoEntities();
        // GET: Details
        public ActionResult Index(int? EmployeeID)
        {
            var singleSelectList = new Details();

            singleSelectList.Employees = db.Employees.ToList();

            if (EmployeeID == null)
            {
                if (Session["EmployeeID"] != null)
                {
                    EmployeeID = Convert.ToInt32(Session["EmployeeID"].ToString());
                }
            }

            if (EmployeeID != null)
            {

                Session["EmployeeID"] = EmployeeID;
                singleSelectList.EmploymentHistories = db.EmploymentHistories.Where(w => w.EmployeeID == EmployeeID.Value).ToList();


            }

           
            return View(singleSelectList);
        }
    }
}