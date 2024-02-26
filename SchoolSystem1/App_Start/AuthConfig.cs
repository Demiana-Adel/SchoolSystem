
using Fluent.Infrastructure.FluentModel;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolSystem1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SchoolSystem1
{
    public class UserInformation : IdentityUser
    {
        public string FirstName { get; set; }
        public string LasstName { get; set; }
        public string Address { get; set; }
        public double Age { get; set; }
        public string Img { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentID { get; set; }
        public Department Department { get; set; }
    }
    public class ApplicatioDbContext : IdentityDbContext<UserInformation>
    {
        public ApplicatioDbContext() : base("School_System")
        {

        } 
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course > Courses   { get; set; }
        public DbSet<StudentsCourses > StudentsCourses   { get; set; }
        public DbSet<DepartmentsCourses > DepartmentsCourses    { get; set; }
        public DbSet<ProfCourses > ProfCourses    { get; set; }

        public static implicit operator ApplicatioDbContext(ApplicationDbContext v)
        {
            throw new NotImplementedException();
        }
    }
}