using System.ComponentModel;

namespace Labb_4_Uppgift_2.Models
{
    public class Grade
    {
        public virtual int GradeID { get; set; }
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
        [DisplayName("Grade")]
        public string CourseGrade { get; set; }
        public virtual int StudentId { get; set; }
    }
}