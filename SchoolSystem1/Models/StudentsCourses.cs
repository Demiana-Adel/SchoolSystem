using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolSystem1.Models
{
    public class StudentsCourses
    {
        [Key]
        [Column(Order = 1)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]

        [ForeignKey("User")]
        public string StudentID { get; set; }
        public IdentityUser User { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? CourseID { get; set; }

        public Course Course  { get; set; }
    }
}