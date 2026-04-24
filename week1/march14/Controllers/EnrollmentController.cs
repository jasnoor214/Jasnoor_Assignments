using Microsoft.AspNetCore.Mvc;
using March_16_Assignment.Models;
using System.Linq;
using System.Collections.Generic;

namespace March_16_Assignment.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly List<Student> _students;
        private readonly List<Course> _courses;

        public EnrollmentController()
        {
            _courses = new()
            {
                new() { CourseId = 1, Title = "Data Structures", Credits = 4, Department = "CSE" },
                new() { CourseId = 2, Title = "Algorithms", Credits = 4, Department = "CSE" },
                new() { CourseId = 3, Title = "Databases", Credits = 3, Department = "CSE" },
                new() { CourseId = 4, Title = "Web Dev", Credits = 3, Department = "IT" },
                new() { CourseId = 5, Title = "OS", Credits = 4, Department = "CSE" }
            };

            _students = new()
            {
                new() { StudentId = 1, Name = "Alice", Branch = "CSE", Enrollments = new()
                {
                    new() { CourseId = 1, Grade = "A", AttemptNumber = 1 },
                    new() { CourseId = 2, Grade = "A-", AttemptNumber = 1 },
                    new() { CourseId = 3, Grade = "B+", AttemptNumber = 1 }
                }},
                new() { StudentId = 2, Name = "Bob", Branch = "CSE", Enrollments = new()
                {
                    new() { CourseId = 1, Grade = "B", AttemptNumber = 1 },
                    new() { CourseId = 4, Grade = "A", AttemptNumber = 1 },
                    new() { CourseId = 5, Grade = "B+", AttemptNumber = 1 }
                }},
                new() { StudentId = 3, Name = "Charlie", Branch = "IT", Enrollments = new()
                {
                    new() { CourseId = 4, Grade = "C", AttemptNumber = 1 },
                    new() { CourseId = 1, Grade = "B-", AttemptNumber = 1 }
                }},
                new() { StudentId = 4, Name = "Diana", Branch = "CSE", Enrollments = new()
                {
                    new() { CourseId = 2, Grade = "A", AttemptNumber = 1 },
                    new() { CourseId = 5, Grade = "F", AttemptNumber = 1 },
                    new() { CourseId = 5, Grade = "B", AttemptNumber = 2 }
                }},
                new() { StudentId = 5, Name = "Eve", Branch = "IT", Enrollments = new() }
            };
        }

        // INDEX PAGE
        public IActionResult Index()
        {
            var students = _students.Select(s => new StudentCoursesVM
            {
                Name = s.Name,
                Branch = s.Branch,
                CourseTitles = s.Enrollments
                    .Select(e => _courses.First(c => c.CourseId == e.CourseId).Title)
                    .ToList()
            }).ToList();

            return View(students);
        }

        // DETAILS PAGE
        public IActionResult Details(int studentId)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var courses = student.Enrollments
                .GroupBy(e => e.CourseId)
                .Select(g => g.OrderByDescending(e => e.AttemptNumber).First())
                .Select(e =>
                {
                    var course = _courses.FirstOrDefault(c => c.CourseId == e.CourseId);

                    return new CourseDetailVM
                    {
                        Title = course?.Title,
                        Credits = course?.Credits ?? 0,
                        Department = course?.Department,
                        LatestGrade = e.Grade
                    };
                }).ToList();

            var vm = new StudentDetailVM
            {
                Student = student,
                Courses = courses
            };

            return View(vm);
        }
    }
}