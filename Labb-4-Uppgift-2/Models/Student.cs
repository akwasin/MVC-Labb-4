using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Labb_4_Uppgift_2.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [DisplayName("Förnamn")]
        public string Name { get; set; }

        [DisplayName("Efternamn")]
        public string Lastname { get; set; }

        public double PersonNumber { get; set; }

        public string Address { get; set; }

        public virtual List<Grade> Grades { get; set; }
    }
}