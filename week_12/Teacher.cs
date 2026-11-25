using System.Collections.Generic;

namespace week_12
{
    public class Teacher
    {
        public string Name { get; set; } = string.Empty;
        public List<Course> Courses { get; set; } = new List<Course>();

        public string TeacherTitle => $"{Name} 老師（共 {Courses.Count} 門課）";

        public Teacher(string name, List<Course> courses)
        {
            Name = name;
            Courses = courses;
        }
    }
}