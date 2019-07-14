using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace EF62EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SchoolContext"].ConnectionString;
            using (var ctx = new SchoolDbContext(connectionString))
            {
                var students = ctx.Students.ToList();
                foreach (var student in students)
                {
                    Console.WriteLine($"{student.FirstMidName} erolled course " +
                        $"{string.Join(", ", student.Enrollments.Select(e => e.Course.Title))} on " +
                        $"{student.EnrollmentDate}");
                }

                List<Student> enumerable = ctx.Students.AsEnumerable().ToList();
                List<Enrollment> enrollments = enumerable
                    .SelectMany(x => x.Enrollments)
                    .Where(e => e.Grade == Grade.A).ToList();

                Console.WriteLine();

                foreach (var enrollment in enrollments)
                {
                    Console.WriteLine($"{enrollment.Student.FirstMidName}" +
                        $"received grade {enrollment.Grade} in " +
                        $"course {enrollment.Course.Title}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
