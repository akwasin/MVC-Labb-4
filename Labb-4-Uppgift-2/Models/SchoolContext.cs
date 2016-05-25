using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Labb_4_Uppgift_2.Models
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public SchoolContext() : base("SchoolDB")
        {
            Database.SetInitializer<SchoolContext>(new SchoolInitializer());
            
            //Database.SetInitializer(new SchoolInitializer());
        }
    }

    public class SchoolInitializer : DropCreateDatabaseAlways<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student>()
            {
               new Student
               {
                   StudentID = 0,
                   Name = "AJ",
                   Lastname = "Karikari",
                   Address = "Vikingavägen",
                   PersonNumber = 8606011234,
                   Grades = new List<Grade>
                   {
                       new Grade {GradeID = 0, CourseName = "HTML", CourseGrade = "VG"},
                       new Grade {GradeID = 1, CourseName = "JavaScript", CourseGrade = "VG"},
                       new Grade {GradeID = 2, CourseName = "C#", CourseGrade = "G"},
                       new Grade {GradeID = 3, CourseName = "Entity Framework", CourseGrade = "VG"},
                       new Grade {GradeID = 4, CourseName = "Projekt", CourseGrade = "VG"},
                   }
               },
              
            };
            foreach (var student in students)
                context.Students.Add(student);
            context.SaveChanges();
        }
    }
}