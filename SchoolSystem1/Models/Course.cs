using SchoolSystem1.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolSystem1.Models
{
    public class Course
    {
        internal readonly object ListOfDepartments;

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string courseCode { get; set; }
        public int Hours { get; set; }
        public ICollection<StudentsCourses> StudentsCourses { get; set; }
        public ICollection<DepartmentsCourses> DepartmentsCourses { get; set; }
    }
}