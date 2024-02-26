using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolSystem1.Models
{
    public class Department
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public string ManagerImg { get; set; }
        public ICollection<DepartmentsCourses> DepartmentsCourses { get; set; }
    }
}