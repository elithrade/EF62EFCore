using Microsoft.EntityFrameworkCore;

namespace EF62EFCore
{
    public class SchoolDbContext : DbContext
    {
        private readonly string _connectionString;

        public SchoolDbContext() : base()
        {
        }

        public SchoolDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentInfo> Infos { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<TeacherStudent> TeacherStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // HasRequired, HasOptional, or HasMany method to specify the type of relationship this entity participates in.
            // WithRequired, WithOptional, and WithMany method to specify the inverse relationship.

            // Configure one to one relationship between Student and StudentInfo,
            // this meaning that the Student entity object must include the StudentInfo
            // entity object and the StudentInfo entity must include the Student entity. 
            modelBuilder.Entity<Student>()
                .HasOne(student => student.Info) // EF Core HasOne(s => s.Info)
                .WithOne(info => info.Student) // EF Core WithOne(i => i.Student)
                .HasForeignKey<StudentInfo>(info => info.ID);

            // Configure one to many relationship between Course and Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(enrollment => enrollment.Course) // Enrollment entity has required the Course property
                .WithMany(course => course.Enrollments) // Configure the other end that Course entity includes many Enrollment entities
                .HasForeignKey(enrollment => enrollment.CourseID); // Specify the foreign key

            // Similarly one to many relationship between Student and Enrollment, but configure Student instead of Enrollment
            modelBuilder.Entity<Student>()
                .HasMany(student => student.Enrollments)
                .WithOne(enrollment => enrollment.Student)
                .HasForeignKey(enrollment => enrollment.StudentID);

            // Many-to-many relationships without an entity class to represent the
            // join table are not yet supported. However, you can represent a many-to-many
            // relationship by including an entity class for the join table and mapping
            // two separate one-to-many relationships.
            modelBuilder.Entity<TeacherStudent>()
                .HasKey(ts => new { ts.TeacherId, ts.StudentId });

            modelBuilder.Entity<TeacherStudent>()
                .HasOne(ts => ts.Student)
                .WithMany(student => student.TeacherStudents)
                .HasForeignKey(ts => ts.StudentId);

            modelBuilder.Entity<TeacherStudent>()
                .HasOne(ts => ts.Teacher)
                .WithMany(teacher => teacher.TeacherStudents)
                .HasForeignKey(ts => ts.TeacherId);
        }
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
