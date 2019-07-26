using System.Collections.Generic;

namespace EF62EFCore
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string Name { get; set; }

        // Collection navigation property
        public ICollection<TeacherStudent> TeacherStudents { get; set; }
    }
}
