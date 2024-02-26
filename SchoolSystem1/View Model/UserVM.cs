using SchoolSystem1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolSystem1.View_Model
{
    public class UserVM
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LasstName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public double Age { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Img { get; set; }
        public int DepartmentID { get; set; }
        public List<ListOfCourses> ListOfCourses { get; set; }   
    }
}