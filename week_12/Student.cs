using System.Collections.ObjectModel;
using System.Linq;

namespace week_12
{
    public class Student
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;

        public ObservableCollection<Course> SelectedCourses { get; set; } = new ObservableCollection<Course>();

        public string DisplayName => $"{StudentId} {StudentName}";

        public Student(string studentId, string studentName)
        {
            StudentId = studentId;
            StudentName = studentName;
        }

        public int GetTotalCredits()
        {
            return SelectedCourses.Sum(c => c.Credits);
        }
    }
}