using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF62EFCore
{
    public class Course
    {
        // The attribute lets you enter the primary key for the course rather than having the database generate it.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
