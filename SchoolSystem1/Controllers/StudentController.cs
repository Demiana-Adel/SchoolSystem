using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using SchoolSystem1.Models;
using SchoolSystem1;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolSystem1.View_Model;
using System.Data.Entity;
using Microsoft.SqlServer.Management.Smo;

namespace SchoolSystem1.Controllers
{
    public class StudentController : Controller
    {
        ApplicatioDbContext _DbContext = new ApplicatioDbContext();

        // GET: Student
        public ActionResult AddStudent()
        {
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
        public ActionResult AddStudent(UserVM UserVM, HttpPostedFileBase image1)
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
                    UserVM.ID = user.Id;
                    manager.AddToRole(UserVM.ID, "Student");

                }
            else
            {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
             }

            var listOfCourse = UserVM.ListOfCourses.Where(c => c.IsSelected == true).Select(cc => cc.CourseID).ToList();

                var courses = listOfCourse.Select(c => new StudentsCourses
                {
                    CourseID = c,
                    StudentID = user.Id,
                }).ToList();
                _DbContext.StudentsCourses.AddRange(courses);
                _DbContext.SaveChanges();
       
            return View(UserVM);
        }


        public ActionResult AllStudent()
        {
            var role = _DbContext.Roles.Where(r => r.Name == "Student").SingleOrDefault();
            if (role == null)
            {
                return View("AllStudent");
            }
            var userRole = role.Users.Select(u => u.UserId).ToList();
            var student = _DbContext.Users.Where(u => userRole.Contains(u.Id)).Select(x => new UserVM
            {
                ID = x.Id,
                FirstName = x.FirstName,
                LasstName = x.LasstName,
            }).ToList();
            return View(student);

        }


        public ActionResult Deatails(string id)
        {
            var Students = _DbContext.Users.Where(s => s.Id == id).Select(x => new UserVM
            {
                ID = x.Id,
                FirstName = x.FirstName,
                LasstName = x.LasstName,
                Age = x.Age,
                Email = x.Email,
                Address = x.Address,
                Phone = x.PhoneNumber,
                UserName = x.UserName,
                DepartmentID = x.DepartmentID ?? 0,
                Img = x.Img,

            }).ToList();
            return View(Students);

        }



        public ActionResult Edit(string id)
        {
            var Departments = _DbContext.Departments.ToList();
            ViewBag.Department = new MultiSelectList(Departments, "ID", "Name");
            var Courses = _DbContext.StudentsCourses.Where(sc => sc.StudentID == id).Select(sc => sc.CourseID).ToList();
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
                ID = id,
                FirstName = user.FirstName,
                LasstName =user.LasstName ,
                Address = user.Address,
                Age = user.Age,
                Phone = user.PhoneNumber,
                Email = user.Email,
                DepartmentID = (int)user.DepartmentID,
                Img = user.Img,
                UserName = user.UserName,
                ListOfCourses = listOfCourses,

            };
            return View(model);
        }



        [HttpPost]
        public ActionResult Edit(string id, UserVM Students, HttpPostedFileBase image1)
        {
            var Departments = _DbContext.Departments.ToList();
            ViewBag.Department = new MultiSelectList(Departments, "ID", "Name");
            
                if (ModelState.IsValid)
                {
                    UserInformation user = _DbContext.Users.FirstOrDefault(s => s.Id == id);
                    var studentCourses = _DbContext.StudentsCourses.Where(sc => sc.StudentID == user.Id).ToList();
                    UserStore<IdentityUser> store = new UserStore<IdentityUser>(_DbContext);
                    UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
                    user.Id = Students.ID;
                    user.FirstName = Students.FirstName;
                    user.LasstName = Students.LasstName;
                    user.Age = Students.Age;
                    user.PhoneNumber = Students.Phone;
                    user.Email = Students.Email;
                    user.Address = Students.Address;
                    user.DepartmentID = Students.DepartmentID;
                    user.UserName = Students.UserName;
                    if (image1 != null)
                    {

                        string ImageFileName = Path.GetFileName(image1.FileName);
                        string FolderPath = Path.Combine(Server.MapPath("~/img/"), ImageFileName);
                        image1.SaveAs(FolderPath);
                        user.Img = ImageFileName;
                        manager.AddToRole(user.Id, "Student");
                    }

                    var listOfCourse = Students.ListOfCourses.Where(c => c.IsSelected == true).Select(cc => cc.CourseID).ToList();

                    var courses = listOfCourse.Select(c => new StudentsCourses
                    {
                        CourseID = c,
                        StudentID = Students.ID,
                    }).ToList();
                  
                     _DbContext.StudentsCourses.RemoveRange(studentCourses);
                  
                    _DbContext.StudentsCourses.AddRange(courses);
                    _DbContext.SaveChanges();
                }

            

            return View("Edit" , Students );
        }

        public ActionResult Delete(string id)
        {
            UserStore<IdentityUser> store = new UserStore<IdentityUser>(_DbContext);
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
            manager.RemoveFromRole(id, "Student");
            var std = _DbContext.Users.FirstOrDefault(s => s.Id == id);
            var studentCourses = _DbContext.StudentsCourses.Where(s => s.StudentID == std.Id).ToList();
            _DbContext.Users.Remove(std);
            _DbContext.StudentsCourses.RemoveRange(studentCourses);
            _DbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ViewCourses(string id)
        {
            var stdCourse = _DbContext.StudentsCourses.Where(sc => sc.StudentID == id).Select(dc => dc.CourseID).ToList();
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



























