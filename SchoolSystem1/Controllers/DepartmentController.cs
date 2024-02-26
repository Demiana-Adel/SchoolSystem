using SchoolSystem1.Models;
using SchoolSystem1.View_Model;
using SchoolSystem1.ViewModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolSystem1.Controllers
{
    public class DepartmentController : Controller
    {
        ApplicatioDbContext _DbContext = new ApplicatioDbContext();
        // GET: Department
        public ActionResult AddDepartment()
        {
            var model = new DepartmentVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddDepartment(DepartmentVM departmentVM, HttpPostedFileBase ManagerImg)
        {
            Department department = new Department();
            department.ID = departmentVM.ID;
            department.Name = departmentVM.Name;
            department.Manager = departmentVM.Manager;
            if (ManagerImg != null && ManagerImg.ContentLength > 0)
            {
                string ImageFileName = Path.GetFileName(ManagerImg.FileName);
                string FolderPath = Path.Combine(Server.MapPath("/img/"), ImageFileName);
                ManagerImg.SaveAs(FolderPath);
                department.ManagerImg = ImageFileName;
            }

            _DbContext.Departments.Add(department);
            _DbContext.SaveChanges();

            return View();
        }
        public ActionResult Edit(int id)
        {
            var model = _DbContext.Departments.Select(d => new DepartmentVM
            {
                ID = d.ID,
                Name = d.Name,
                Manager = d.Manager,
                ManagerImg = d.ManagerImg,

            }).FirstOrDefault(d => d.ID == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, Department department, HttpPostedFileBase ManagerImg)
        {
            var dept = _DbContext.Departments.FirstOrDefault(d => d.ID == id);
            dept.ID = department.ID;
            dept.Name = department.Name;
            dept.Manager = department.Manager;
            if (ManagerImg != null && ManagerImg.ContentLength > 0)
            {
                string ImageFileName = Path.GetFileName(ManagerImg.FileName);
                string FolderPath = Path.Combine(Server.MapPath("/img/"), ImageFileName);
                ManagerImg.SaveAs(FolderPath);
                dept.ManagerImg = ImageFileName;
            }
            _DbContext.SaveChanges();
            return View("Edit");
        }
        public ActionResult AllDepartment()
        {
            var Departments = _DbContext.Departments.Select(x => new DepartmentVM
            {
                ID = x.ID,
                Name = x.Name,
                Manager = x.Manager,
            }).ToList();
            return View(Departments);
        }
        public ActionResult Details(int id)
        {
            var model = _DbContext.Departments.Select(d => new DepartmentVM
            {
                ID = d.ID,
                Name = d.Name,
                Manager = d.Manager,
            }).Where(d => d.ID == id).ToList();
            return View(model);
        }
        #region Delete
        //public ActionResult Delete(int id)
        //{
        //    var department = _DbContext.Departments.FirstOrDefault(d => d.ID == id);
        //    //null في جدول الستيودنت بdepartmentidهنا بقوله حط ال 
        //    _DbContext.Users.Where(d => d.DepartmentID == id).Equals(null);
        //    var deptCourses = _DbContext.DepartmentsCourses.Where(dc => dc.DepartmentID == id).ToList();
        //    _DbContext.Departments.Remove(department);
        //    _DbContext.DepartmentsCourses.RemoveRange(deptCourses);
        //    _DbContext.SaveChanges();
        //    return RedirectToAction("Index", "Home");
        //}

        #endregion 
        public ActionResult ViewCourses(int id)
        {
            var deptCourse=_DbContext.DepartmentsCourses.Where(dc=>dc.DepartmentID== id).Select(dc=>dc.CourseID).ToList();
            var courses=_DbContext.Courses.Where(c=>deptCourse.Contains(c.ID)).Select(c=>new CourseVM
            {
                Name=c.Name,
                courseCode =c.courseCode ,
                Hours =c.Hours ,
            }).ToList();
            return View (courses);
        }
        public ActionResult ViewStudents(int id)
        {

            var role = _DbContext.Roles.Where(r => r.Name == "Student").SingleOrDefault();
            var userRole = role.Users.Select(u => u.UserId).ToList();
            var students = _DbContext.Users.Where(s =>s.DepartmentID==id && userRole.Contains(s.Id)).Select(s => new UserVM
            {
                FirstName = s.FirstName,
                LasstName = s.LasstName,
                Age = s.Age,
                Phone = s.PhoneNumber,
                Email = s.Email,
            }).ToList();
            return View(students);
        }
        public ActionResult ViewProf(int id)
        {

            var role = _DbContext.Roles.Where(r => r.Name == "Prof").SingleOrDefault();
            var userRole = role.Users.Select(u => u.UserId).ToList();
            var students = _DbContext.Users.Where(s => s.DepartmentID == id && userRole.Contains(s.Id)).Select(s => new UserVM
            {
                FirstName = s.FirstName,
                LasstName = s.LasstName,
                Age = s.Age,
                Phone = s.PhoneNumber,
                Email = s.Email,
            }).ToList();
            return View(students);
        }
    }
}