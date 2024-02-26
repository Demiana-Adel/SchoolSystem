using Microsoft.Ajax.Utilities;
using SchoolSystem1.Models;
using SchoolSystem1.View_Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SchoolSystem1.Controllers
{
    public class CourseController : Controller
    {
       ApplicatioDbContext _DbContext = new ApplicatioDbContext();
        // GET: Course
        public ActionResult AddCourse()
        {

           var listOfDepartments = _DbContext.Departments.Select(d=> new ListOfDepartments
           {
           
             DpartmentID =d.ID ,
             Name = d.Name ,
             IsSelected =false ,

           }).ToList();
            var model = new CourseVM { ListOfDepartments =listOfDepartments};
            return View(model);
        }
        [HttpPost]
        public ActionResult AddCourse(CourseVM courseVM)
        {
            if (ModelState.IsValid)
            {
                Course course = new Course();
                course.Name = courseVM.Name;
                course.courseCode = courseVM.courseCode;
                course.Hours = courseVM.Hours;
                var listOfDepartments = courseVM.ListOfDepartments.Where(d => d.IsSelected == true).Select(d => d.DpartmentID).ToList();
                var model = listOfDepartments.Select(d => new DepartmentsCourses
                {
                    DepartmentID = d,
                    CourseID = course.ID,

                }).ToList();
                _DbContext.Courses.Add(course);
                _DbContext.DepartmentsCourses.AddRange(model);
                _DbContext.SaveChanges();
            }
            return View(courseVM);
        }
        public ActionResult Edit(int id)
        {
            var Departments = _DbContext.DepartmentsCourses.Where(dc=>dc.CourseID==id).Select(dc=>dc.DepartmentID).ToList();
            var listOfDepartments = _DbContext.Departments.Select(d => new ListOfDepartments
            {

                DpartmentID =d.ID,
                Name = d.Name,
                IsSelected = false,

            }).ToList();
            foreach (var departments in listOfDepartments)
            {
                if (Departments.Contains(departments.DpartmentID))
                {
                    departments.IsSelected = true;
                }
            }
          
            var model = _DbContext.Courses.Where(c => c.ID == id).ToList();
            var model1 = new CourseVM
            {
                ListOfDepartments =listOfDepartments,
                ID = model.First().ID,
                Name = model.First().Name,
                courseCode = model.First().courseCode,
                Hours = model.First().Hours,
            };
            return View(model1);
        }
        [HttpPost]
        public ActionResult Edit(int id , CourseVM course  )
        {
            var Departments = _DbContext.Departments.ToList();
            var departmentsCourses=_DbContext.DepartmentsCourses.Where(dc=>dc.CourseID==id).ToList();
            var listOfDepartments = course.ListOfDepartments.Where(d => d.IsSelected == true).Select(d => d.DpartmentID).ToList();
          var model =listOfDepartments.Select(d=>new DepartmentsCourses
          {
              DepartmentID=d,
              CourseID =course.ID,
          }).ToList();
            var course1 = _DbContext.Courses.FirstOrDefault(c=>c.ID ==id);
            course1.ID = course.ID ;
            course1.Name  = course.Name  ;
            course1.courseCode  = course.courseCode  ;
            course1.Hours  = course.Hours  ;
           
            _DbContext.DepartmentsCourses.RemoveRange(departmentsCourses);
            _DbContext.DepartmentsCourses.AddRange(model);
            _DbContext.SaveChanges();
            return View("Edit"); 
        }
        
        public ActionResult AllCouses()
        {
            var courses = _DbContext.Courses.Select(c => new CourseVM 
            {
             ID = c.ID,
             Name = c.Name,
            }).ToList();

            return View(courses);
        }
        public ActionResult ViewStudents(int id)
        {

            var stdcourse = _DbContext.StudentsCourses.Where(sc => sc.CourseID == id).Select(sc => sc.StudentID).ToList();
            var students = _DbContext.Users.Where(s => stdcourse.Contains(s.Id)).Select(s => new UserVM
            {
                FirstName = s.FirstName,
                LasstName = s.LasstName,
                Age = s.Age,
                Phone = s.PhoneNumber,
                Email = s.Email,
            }).ToList();
            return View(students);
        }
        public ActionResult Details(int id)
        {
            var course=_DbContext.Courses.Select(c=>new CourseVM
            {
                ID =c.ID,
                Name = c.Name,  
                courseCode=c.courseCode,
                Hours=c.Hours,
            }).Where(c=>c.ID == id).ToList();  
            return View(course);
        }
        public ActionResult Delete(int id)
        {
            
            var course = _DbContext.Courses.FirstOrDefault(c => c.ID ==id);
            var courses = _DbContext.StudentsCourses.Where (c => c.CourseID == course.ID).ToList();

                _DbContext.Courses.Remove(course);
                _DbContext.StudentsCourses.RemoveRange(courses);
            
            _DbContext.SaveChanges();
            return RedirectToAction("AllCouses", "Course" );
        }  




    }
 
}