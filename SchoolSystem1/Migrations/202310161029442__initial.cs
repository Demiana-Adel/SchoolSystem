namespace SchoolSystem1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        courseCode = c.String(),
                        Hours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DepartmentsCourses",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DepartmentID, t.CourseID })
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Manager = c.String(),
                        ManagerImg = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StudentsCourses",
                c => new
                    {
                        StudentID = c.String(nullable: false, maxLength: 128),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentID, t.CourseID })
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.IdentityUsers", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.IdentityUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.ProfCourses",
                c => new
                    {
                        ProfID = c.String(nullable: false, maxLength: 128),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfID, t.CourseID })
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.IdentityUsers", t => t.ProfID, cascadeDelete: true)
                .Index(t => t.ProfID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LasstName = c.String(),
                        Address = c.String(),
                        Age = c.Double(nullable: false),
                        Img = c.String(),
                        DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityUsers", t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .Index(t => t.Id)
                .Index(t => t.DepartmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProfCourses", "ProfID", "dbo.IdentityUsers");
            DropForeignKey("dbo.ProfCourses", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.StudentsCourses", "StudentID", "dbo.IdentityUsers");
            DropForeignKey("dbo.AspNetUserRoles", "IdentityUser_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.AspNetUserLogins", "IdentityUser_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.AspNetUserClaims", "IdentityUser_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.StudentsCourses", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.DepartmentsCourses", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.DepartmentsCourses", "CourseID", "dbo.Courses");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentID" });
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProfCourses", new[] { "CourseID" });
            DropIndex("dbo.ProfCourses", new[] { "ProfID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.IdentityUsers", "UserNameIndex");
            DropIndex("dbo.StudentsCourses", new[] { "CourseID" });
            DropIndex("dbo.StudentsCourses", new[] { "StudentID" });
            DropIndex("dbo.DepartmentsCourses", new[] { "CourseID" });
            DropIndex("dbo.DepartmentsCourses", new[] { "DepartmentID" });
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProfCourses");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.IdentityUsers");
            DropTable("dbo.StudentsCourses");
            DropTable("dbo.Departments");
            DropTable("dbo.DepartmentsCourses");
            DropTable("dbo.Courses");
        }
    }
}
