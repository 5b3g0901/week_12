namespace week_12
{
    public class Course
    {
        private int _credits;

        public string CourseId { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;

        public int Credits
        {
            get => _credits;
            set => _credits = value < 0 ? 0 : value;
        }

        public Course(string courseId, string courseName, string teacherName, int credits)
        {
            CourseId = courseId;
            CourseName = courseName;
            TeacherName = teacherName;
            Credits = credits;
        }

        public override string ToString()
        {
            return $"[{CourseId}] {CourseName}（{TeacherName} / {Credits}學分）";
        }
    }
}
