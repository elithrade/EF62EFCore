using System;
using System.Collections.Generic;

namespace EF62EFCore
{
    public class Student
    {
        public int StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual StudentInfo Info { get; set; }

        // Collection navigation property
        public ICollection<Teacher> Teachers { get; set; }
    }
}
