using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolSystem1.Models
{
    public class DepartmentsCourses
    {
        [Key]
        [Column(Order=1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? DepartmentID { get; set; }
        public Department Department  { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? CourseID { get; set; }

        public Course Course { get; set; }
    }
}