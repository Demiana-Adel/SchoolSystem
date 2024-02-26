using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolSystem1.Controllers
{
    public class RoleController : Controller
    {
        ApplicatioDbContext _Context=new ApplicatioDbContext();
        // GET: Role
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(string RoleName)
        {
            RoleStore<IdentityRole> store = new RoleStore<IdentityRole>(_Context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(store);
            IdentityRole role =new IdentityRole();
            role.Name = RoleName;
            roleManager.Create(role);
            return RedirectToAction("Index" ,"Home");
        }
    }
}