using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using week_12;

namespace week_12
{
    public partial class MainWindow : Window
    {
        private List<Course> allCourses = new List<Course>();
        private Dictionary<string, Student> studentMap = new Dictionary<string, Student>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {

            allCourses = new List<Course>
            {
                new Course("CS101", "視窗程式設計", "陳定宏", 3),
                new Course("CS102", "物件導向程式設計", "陳定宏", 3),
                new Course("CS201", "嵌入式系統", "林志明", 3),
                new Course("CS202", "微處理機原理", "林志明", 2),
                new Course("CS301", "動態程式語言", "王美玲", 3),
                new Course("CS302", "資料庫系統", "王美玲", 3)
            };

            lbAllCourses.ItemsSource = allCourses;

            HashSet<string> teacherNames = new HashSet<string>(allCourses.Select(c => c.TeacherName));
            List<Teacher> teachers = teacherNames
                .Select(name => new Teacher(name, allCourses.Where(c => c.TeacherName == name).ToList()))
                .ToList();

            tvTeacherCourses.ItemsSource = teachers;

            Student s1 = new Student("S001", "陳小明");
            Student s2 = new Student("S002", "林小華");
            Student s3 = new Student("S003", "王大明");

            studentMap[s1.StudentId] = s1;
            studentMap[s2.StudentId] = s2;
            studentMap[s3.StudentId] = s3;

            cmbStudent.ItemsSource = studentMap.Values.ToList();
            cmbStudent.SelectedIndex = 0;
        }

        private void cmbStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStudent.SelectedItem is Student currentStudent)
            {
                lbSelectedCourses.ItemsSource = currentStudent.SelectedCourses;
            }
        }

        private void tabCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is not TabControl) return;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStudent.SelectedItem is not Student currentStudent) return;

            List<Course> coursesToAdd = new List<Course>();

            if (tabCourses.SelectedIndex == 0)
            {
                if (tvTeacherCourses.SelectedItem is Course selectedCourse)
                {
                    coursesToAdd.Add(selectedCourse);
                }
                else
                {
                    MessageBox.Show("請展開教師節點並點選要加選的課程。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                coursesToAdd = lbAllCourses.SelectedItems.Cast<Course>().ToList();
                if (coursesToAdd.Count == 0)
                {
                    MessageBox.Show("請在左側清單選擇至少一門課程。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            HashSet<string> existingIds = new HashSet<string>(currentStudent.SelectedCourses.Select(c => c.CourseId));
            int addedCount = 0;

            foreach (Course course in coursesToAdd)
            {
                if (!existingIds.Contains(course.CourseId))
                {
                    currentStudent.SelectedCourses.Add(course); 
                    existingIds.Add(course.CourseId);
                    addedCount++;
                }
            }

            if (addedCount == 0)
            {
                MessageBox.Show("所選課程皆已在已選清單中，未重複加選。", "重複選課", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStudent.SelectedItem is not Student currentStudent) return;

            List<Course> coursesToRemove = lbSelectedCourses.SelectedItems.Cast<Course>().ToList();
            if (coursesToRemove.Count == 0)
            {
                MessageBox.Show("請先在右側已選課程清單選取要退選的課程。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            foreach (Course course in coursesToRemove)
            {
                currentStudent.SelectedCourses.Remove(course); 
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStudent.SelectedItem is not Student currentStudent) return;

            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "純文字檔案 (*.txt)|*.txt",
                FileName = $"{currentStudent.StudentId}_{currentStudent.StudentName}_選課紀錄"
            };

            if (dlg.ShowDialog() == true)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"學生：{currentStudent.DisplayName}");
                sb.AppendLine($"匯出時間：{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
                sb.AppendLine(new string('-', 35));

                foreach (Course c in currentStudent.SelectedCourses)
                {
                    sb.AppendLine(c.ToString());
                }

                sb.AppendLine(new string('-', 35));
                sb.AppendLine($"共計 {currentStudent.SelectedCourses.Count} 門課程，總學分：{currentStudent.GetTotalCredits()} 學分");

                File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"已成功儲存 {currentStudent.DisplayName} 的選課資料！", "存檔完成", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}