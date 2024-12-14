using Labb_3___SQL___ORM.Data;
using Labb_3___SQL___ORM.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb_3___SQL___ORM
{
    internal class GetData
    {
        public async Task<List<Person>> GetStaff(string selection) // Everyone in a school who is not a student is a staff (most likely)
        {
            using var context = new ERModelleringContext();
            var notStudents = await context.People.Where(p => p.Role != "Student").ToListAsync();
            switch (selection)
            {
                case "All":
                    return notStudents;
                case "Teachers":
                    return notStudents.Where(p => p.Role == "Teacher").ToList();
                case "Administration":
                    return notStudents.Where(p => p.Role != "Teacher").ToList(); // For now just take all non-teachers as admins
                default:
                    return new List<Person>(); // Return an empty list instead of null
            }
        }

        public async Task<List<Student>> GetStudents(string firstOrLast, string ascOrDesc)
        {
            using var context = new ERModelleringContext();

            // Eagerly load the Person navigation property to avoid NullReferenceException
            var students = await context.Students.Include(s => s.Person).ToListAsync();

            switch (firstOrLast)
            {
                case "Get by Firstname":
                    return ascOrDesc == "Ascending"
                        ? students.OrderBy(s => s.Person?.FirstName).ToList()
                        : students.OrderByDescending(s => s.Person?.FirstName).ToList();

                case "Get by Lastname":
                    return ascOrDesc == "Ascending"
                        ? students.OrderBy(s => s.Person?.LastName).ToList()
                        : students.OrderByDescending(s => s.Person?.LastName).ToList();

                default:
                    return new List<Student>();
            }
        }
        public async Task<List<string>> GetClasses()
        {
            using var context = new ERModelleringContext();
            return await context.Classes.Select(c => c.ClassName).ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByClass(string className)
        {
            using var context = new ERModelleringContext();
            return await context.Students
                .Include(s => s.Person)
                .Include(s => s.Class)
                .Where(s => s.Class.ClassName == className)
                .ToListAsync();
        }

        public async Task<List<Grade>> GetLastMonthsGrades()
        {
            using var context = new ERModelleringContext();
            var lastMonth = DateOnly.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")); // Get the date of last month
            return await context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .Include(g => g.Student)
                    .ThenInclude(s => s.Person)
                .Where(g => g.GivenDate > lastMonth)
                .ToListAsync();
        }

        public class CourseStatistics
        {
            public string CourseName { get; set; }
            public string TeacherName { get; set; }
            public double? AverageGrade { get; set; }
            public double? MaxGrade { get; set; }
            public double? MinGrade { get; set; }
        }

        public async Task<List<CourseStatistics>> GetCoursesAndGrades()
        {
            using var context = new ERModelleringContext();

            var courses = await context.Courses
                .Include(c => c.Teacher)
                .ThenInclude(t => t.Person)
                .Include(c => c.Grades)
                .ToListAsync();

            var gradeMap = new Dictionary<string, double> // Map the grade values to their respective GPA
            {
                { "A+", 4.3 }, { "A", 4.0 }, { "A-", 3.7 },
                { "B+", 3.3 }, { "B", 3.0 }, { "B-", 2.7 },
                { "C+", 2.3 }, { "C", 2.0 }, { "C-", 1.7 },
                { "D+", 1.3 }, { "D", 1.0 }, { "F", 0.0 }
            };

            return courses.Select(c => new CourseStatistics
            {
                CourseName = c.CourseName,
                TeacherName = c.Teacher != null
                    ? $"{c.Teacher.Person.FirstName} {c.Teacher.Person.LastName}"
                    : "No Teacher",
                AverageGrade = c.Grades.Any(g => gradeMap.ContainsKey(g.GradeValue)) // Gets the average grade of the course
                    ? c.Grades
                        .Where(g => gradeMap.ContainsKey(g.GradeValue))
                        .Select(g => gradeMap[g.GradeValue])
                        .Average()
                    : (double?)null,
                MaxGrade = c.Grades.Any(g => gradeMap.ContainsKey(g.GradeValue)) // Gets the max grade of the course
                    ? c.Grades
                        .Where(g => gradeMap.ContainsKey(g.GradeValue))
                        .Select(g => gradeMap[g.GradeValue])
                        .Max()
                    : (double?)null,
                MinGrade = c.Grades.Any(g => gradeMap.ContainsKey(g.GradeValue)) // Gets the min grade of the course
                    ? c.Grades
                        .Where(g => gradeMap.ContainsKey(g.GradeValue))
                        .Select(g => gradeMap[g.GradeValue])
                        .Min()
                    : (double?)null
            }).ToList(); // Return the list of CourseStatistics
        }


    }
}
