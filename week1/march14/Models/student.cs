using System.Collections.Generic;

namespace March_16_Assignment.Models
{
    public class StudentCoursesVM
    {
        public string Name { get; set; }
        public string Branch { get; set; }
        public List<string> CourseTitles { get; set; }
    }

    public class CourseDetailVM
    {
        public string Title { get; set; }
        public int Credits { get; set; }
        public string Department { get; set; }
        public string LatestGrade { get; set; }
    }

    public class StudentDetailVM
    {
        public Student Student { get; set; }
        public List<CourseDetailVM> Courses { get; set; }
    }
}