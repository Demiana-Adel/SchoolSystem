using SchoolSystem1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolSystem1.View_Model
{
    public class ListOfCourses
    {
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public  string Name  { get; set; }
        public bool IsSelected { get; set; }
      
    }
}