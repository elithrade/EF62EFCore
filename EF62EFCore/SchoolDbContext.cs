using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EF62EFCore
{
    public class SchoolDbContext : DbContext
    {
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
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Configure one to one relationship between Student and StudentInfo
            modelBuilder.Entity<Student>()
                .HasRequired(s => s.Info) // EF Core HasOne(s => s.Info)
                .WithRequiredPrincipal(i => i.Student); // EF Core WithOne(i => i.Student)

            // Configure one to many relationship between Course and Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasRequired(e => e.Course) // Enrollment entity has required the Course property
                .WithMany(c => c.Enrollments) // Configure the other end that Course entity includes many Enrollment entities
                .HasForeignKey(e => e.CourseID); // Specify the foreign key

            // Similarly one to many relationship between Student and Enrollment, but configure Student instead of Enrollment
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Enrollments)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentID);

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
