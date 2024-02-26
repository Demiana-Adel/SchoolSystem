using SchoolSystem1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolSystem1.View_Model
{
    public class CourseVM
    {
        internal List<CourseVM> model;

        public int ID { get; set; }
        public string Name { get; set; }
        public string courseCode { get; set; }
        public int Hours { get; set; }
        public List<ListOfDepartments> ListOfDepartments { get; set; }

      
    }
}