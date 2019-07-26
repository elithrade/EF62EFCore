using System.Data.Entity;

namespace EF62EFCore
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext() : base()
        {
        }

        public SchoolDbContext(string connectionString) : base(connectionString)
        {
            // Database initialization strategies.
            Database.SetInitializer(new SchoolDbInitializer());
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentInfo> Infos { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // HasRequired, HasOptional, or HasMany method to specify the type of relationship this entity participates in.
            // WithRequired, WithOptional, and WithMany method to specify the inverse relationship.

            // Configure one to one relationship between Student and StudentInfo,
            // this meaning that the Student entity object must include the StudentInfo
            // entity object and the StudentInfo entity must include the Student entity. 
            modelBuilder.Entity<Student>()
                .HasRequired(student => student.Info) // EF Core HasOne(s => s.Info)
                .WithRequiredPrincipal(info => info.Student); // EF Core WithOne(i => i.Student)

            // Configure one to many relationship between Course and Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasRequired(enrollment => enrollment.Course) // Enrollment entity has required the Course property
                .WithMany(course => course.Enrollments) // Configure the other end that Course entity includes many Enrollment entities
                .HasForeignKey(enrollment => enrollment.CourseID); // Specify the foreign key

            // Similarly one to many relationship between Student and Enrollment, but configure Student instead of Enrollment
            modelBuilder.Entity<Student>()
                .HasMany(student => student.Enrollments)
                .WithRequired(enrollment => enrollment.Student)
                .HasForeignKey(enrollment => enrollment.StudentID);

            // Many to many relationship between Student and Teacher
            // Configure both the foreign keys in the joining entity as a composite key using Fluent API.
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Teachers)
                .WithMany(t => t.Students)
                .Map(cs =>
                {
                    cs.MapLeftKey("StudentID");
                    cs.MapRightKey("TeacherID");
                    cs.ToTable("StudentTeacher");
                });
        }
    }
}
