using System.Collections.Generic;

namespace EF62EFCore
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string Name { get; set; }

        // Collection navigation property
        public ICollection<Student> Students { get; set; }
    }

    // EF Core
    // To create a Many-to-Many relationship using Fluent API you have to create a Joining Entity.
    // This joining entity will contain the foreign keys (reference navigation property) for both the other entities.
    // These foreign keys will form the composite primary key for this joining entity.
    public class TeacherStudent
    {
        public int StudentId { get; set; } // Foreign key property
        public Student Student { get; set; } // Reference navigation property

        public int TeacherId { get; set; } // Foreign key property
        public Teacher Teacher { get; set; } // Reference navigation property
    }
}
