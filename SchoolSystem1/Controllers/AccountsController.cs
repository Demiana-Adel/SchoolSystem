using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolSystem1.View_Model;
using SchoolSystem1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolSystem1.Controllers
{
    public class AccountsController : Controller
    {
        ApplicatioDbContext _DbContext = new ApplicatioDbContext();
        // GET: Account
        public ActionResult Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                UserStore<IdentityUser> store = new UserStore<IdentityUser>(_DbContext);
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
                IdentityUser identityUser = manager.FindByName(loginVM.UserName);
                bool found = manager.CheckPassword(identityUser, loginVM.Password);
                if (found)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", " UserName OR Password Is Not Correct");
                }
            }
            return View();
        }
    }
}