using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.SqlServer.Management.Smo;
using SchoolSystem1.Models;
using SchoolSystem1.View_Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolSystem1.Controllers
{
    public class ProfController : Controller
    {
        ApplicatioDbContext _DbContext = new ApplicatioDbContext();
        // GET: Prof
        public ActionResult AddProf()
        {
            var role = _DbContext.Roles.Select(r => r.Name).ToList();
            ViewBag.Role = new MultiSelectList(role);
            var Departments = _DbContext.Departments.ToList();
            ViewBag.Department = new MultiSelectList(Departments, "ID", "Name");
            var x = _DbContext.Courses.Select(c => new ListOfCourses
            {
                CourseID = c.ID,
                Name = c.Name,
                IsSelected = false,
            }).ToList();
            var model = new UserVM { ListOfCourses = x };

            return View(model);
        }
        [HttpPost]
        public ActionResult AddProf(UserVM UserVM, HttpPostedFileBase image1)
        {
            var Departments = _DbContext.Departments.ToList();
            ViewBag.Department = new MultiSelectList(Departments, "ID", "Name");
            UserInformation user = new UserInformation();
            UserStore<IdentityUser> store = new UserStore<IdentityUser>(_DbContext);
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
            user.FirstName = UserVM.FirstName;
            user.LasstName = UserVM.LasstName;
            user.Age = UserVM.Age;
            user.PhoneNumber = UserVM.Phone;
            user.Email = UserVM.Email;
            user.Address = UserVM.Address;
            user.UserName = UserVM.UserName;
            user.Email = UserVM.Email;
            user.UserName = user.UserName;
            user.PhoneNumber = UserVM.Phone;
            user.DepartmentID = UserVM.DepartmentID;
            IdentityResult result = manager.Create(user, UserVM.Password);
            if (result.Succeeded || image1.ContentLength > 0)
            {

                string ImageFileName = Path.GetFileName(image1.FileName);
                string FolderPath = Path.Combine(Server.MapPath("~/img/"), ImageFileName);
                image1.SaveAs(FolderPath);
                user.Img = ImageFileName;

                manager.AddToRole(user.Id, "Prof");

            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item);
                }
            }
            var listOfCourse = UserVM.ListOfCourses.Where(c => c.IsSelected == true).Select(cc => cc.CourseID).ToList();

            var courses = listOfCourse.Select(c => new ProfCourses
            {
                CourseID = c,
                ProfID = user.Id,
            }).ToList();

                _DbContext.ProfCourses.AddRange(courses);
                _DbContext.SaveChanges();
            return View(new UserVM());
        }

        public ActionResult Edit(string id)
        {
            var Departments = _DbContext.Departments.ToList();
            ViewBag.Department = new MultiSelectList(Departments, "ID", "Name");
            var Courses = _DbContext.ProfCourses.Where(sc => sc.ProfID == id).Select(sc => sc.CourseID).ToList();
            var listOfCourses = _DbContext.Courses.Select(d => new ListOfCourses
            {

                CourseID = d.ID,
                Name = d.Name,
                IsSelected = false,

            }).ToList();
            foreach (var coursess in listOfCourses)
            {
                if (Courses.Contains(coursess.CourseID))
                {
                    coursess.IsSelected = true;
                }
            }

            var user = _DbContext.Users.FirstOrDefault(c => c.Id == id);
            var model = new UserVM
            {
             
                ID = user.Id,
                FirstName = user.FirstName,
                LasstName = user.LasstName,
                Age = user.Age,
                Address = user.Address,
                Phone = user.PhoneNumber,
                Email = user.Email,
                DepartmentID = (int)user.DepartmentID,
                Img = user.Img,
                UserName = user.UserName,
                ListOfCourses = listOfCourses

            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(string id, UserVM prof, HttpPostedFileBase image1)
        {
            var Departments = _DbContext.Departments.ToList();
            ViewBag.Department = new MultiSelectList(Departments, "ID", "Name");
            if (ModelState.IsValid)
            {
                UserInformation user = _DbContext.Users.FirstOrDefault(s => s.Id == id);
                var ProfCourses = _DbContext.ProfCourses.Where(sc => sc.ProfID == user.Id).ToList();
                UserStore<IdentityUser> store = new UserStore<IdentityUser>(_DbContext);
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
                user.Id = prof.ID;
                user.FirstName = prof.FirstName;
                user.LasstName = prof.LasstName;
                user.Age = prof.Age;
                user.PhoneNumber = prof.Phone;
                user.Email = prof.Email;
                user.Address = prof.Address;
                user.DepartmentID = prof.DepartmentID;
                user.UserName = prof.UserName;
                if (image1 != null)
                {

                    string ImageFileName = Path.GetFileName(image1.FileName);
                    string FolderPath = Path.Combine(Server.MapPath("~/img/"), ImageFileName);
                    image1.SaveAs(FolderPath);
                    user.Img = ImageFileName;
                    manager.AddToRole(user.Id, "Prof");
                }
                var listOfCourse = prof.ListOfCourses.Where(c => c.IsSelected == true).Select(cc => cc.CourseID).ToList();

                var courses = listOfCourse.Select(c => new ProfCourses
                {
                    CourseID = c,
                    ProfID = prof.ID,
                }).ToList();
                _DbContext.ProfCourses.RemoveRange(ProfCourses);
                _DbContext.ProfCourses.AddRange(courses);
                _DbContext.SaveChanges();
            }
            return View("Edit" , prof );
        }

        public ActionResult AllProf()
        {
            var role = _DbContext.Roles.Where(r => r.Name == "Prof").SingleOrDefault();
            if (role == null)
            {
                return View ("AllProf");
            }
            var userRole = role.Users.Select(u => u.UserId).ToList();
            var prof = _DbContext.Users.Where(u => userRole.Contains(u.Id)).Select(x => new UserVM
            {
                ID = x.Id,
                FirstName = x.FirstName,
                LasstName = x.LasstName,
            }).ToList();
            return View(prof);

        }
        public ActionResult Deatails(string id) 
        {
            var prof = _DbContext.Users.Where(u => u.Id == id).Select(u => new UserVM
            {
                ID =u.Id,
                FirstName = u.FirstName,
                LasstName=u.LasstName,
                Address = u.Address,
                Age = u.Age,
                UserName = u.UserName,
                Img = u.Img,
                Email = u.Email,
                Phone=u.PhoneNumber ,
                DepartmentID = (int)u.DepartmentID 
            }).ToList();

            return View(prof);
        }
        public ActionResult Delete(string id) 
        {
            UserStore<IdentityUser> store = new UserStore<IdentityUser>(_DbContext);
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
            manager.RemoveFromRole(id, "Prof");
            var prof = _DbContext.Users.FirstOrDefault(p => p.Id == id);
            var profCourses = _DbContext.ProfCourses.Where(s => s.ProfID == prof.Id).ToList();
            _DbContext.Users.Remove(prof);
            _DbContext.ProfCourses.RemoveRange(profCourses);
            _DbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
         
        }
        public ActionResult ViewCourses(string id)
        {
            var stdCourse = _DbContext.ProfCourses.Where(sc => sc.ProfID == id).Select(dc => dc.CourseID).ToList();
            var courses = _DbContext.Courses.Where(c => stdCourse.Contains(c.ID)).Select(c => new CourseVM
            {
                Name = c.Name,
                courseCode = c.courseCode,
                Hours = c.Hours,
            }).ToList();
            return View(courses);
        }
    }
 
}