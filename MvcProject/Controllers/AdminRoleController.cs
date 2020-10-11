using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class AdminRoleController : Controller
    {
        [Authorize(Roles="Admin")]
        // GET: Admin_Roll
        public ActionResult Index()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            List<Admin_RoleCreation> rollList = new List<Admin_RoleCreation>();
            int loopInt = 1;
            if (HttpContext.User.IsInRole("Admin"))
            {

                foreach (var v in roleManager.Roles.ToList())
                {
                    loopInt++;
                    rollList.Add(new Admin_RoleCreation()
                    {
                        RoleName = v.Name,
                        Id = loopInt

                    });
                }
            }
            else
            {

                foreach (var v in roleManager.Roles.Where(w => w.Name != "Admin").ToList())
                {
                    rollList.Add(new Admin_RoleCreation()
                    {
                        RoleName = v.Name,
                        Id = loopInt

                    });
                }



            }
            return View(rollList);
        }

        public ActionResult RollCreate()
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult UserList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userList = db.Users.ToList();
            return View(userList);
        }

        public ActionResult UserDelete(string id)
        {

            if (string.IsNullOrEmpty(id))
            {

                id = HttpContext.User.Identity.GetUserId();
                ApplicationDbContext db1 = new ApplicationDbContext();

                ApplicationUser au1 = db1.Users.Find(new object[] { id });

                if (au1 != null)
                {
                    db1.Users.Remove(au1);
                    db1.SaveChanges();
                }


                return RedirectToAction("Index", "Home");
            }

            ApplicationDbContext db = new ApplicationDbContext();

            ApplicationUser au = db.Users.Find(new object[] { id });
            db.Users.Remove(au);
            db.SaveChanges();

            return RedirectToAction("UserList");
        }


        [HttpPost]
        public ActionResult Delete(string RoleName)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {

                ApplicationDbContext db = new ApplicationDbContext();
                ApplicationDbContext dbRole = new ApplicationDbContext();

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbRole));
                var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(db));
                try
                {
                    ICollection<IdentityUserRole> IdentityUserRoleList = roleManager.FindByName(RoleName).Users;

                    foreach (IdentityUserRole iur in IdentityUserRoleList)
                    {
                        ApplicationUser au = db.Users.Find(new object[] { iur.UserId });
                        db.Users.Remove(au);
                        db.SaveChanges();

                    }
                }
                catch (Exception ex)
                {

                }

                IdentityRole IdentRoleName = dbRole.Roles.Where(w => w.Name == RoleName).FirstOrDefault();
                dbRole.Roles.Remove(IdentRoleName);
                dbRole.SaveChanges();
                return RedirectToAction("Index", "AdminRole");
            }
            else
            {
                return RedirectToAction("Index", "AdminRole");
            }




        }

        [HttpPost]

        public ActionResult RollCreate(Admin_RoleCreation admin_RollCreation)
        {

            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            IdentityRole idenName = new IdentityRole();
            idenName.Name = admin_RollCreation.RoleName;

            IdentityResult r = roleManager.Create(idenName);
            if (r.Succeeded)
            {

                return RedirectToAction("Index");
            }

            return View();

        }
    }
}