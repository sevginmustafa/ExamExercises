using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassroomProject
{
    public class Classroom
    {
        List<Student> students;

        public int Capacity { get; set; }
        public int Count { get { return students.Count; } }

        public Classroom(int capacity)
        {
            students = new List<Student>();
            Capacity = capacity;
        }

        public string RegisterStudent(Student student)
        {
            if (Capacity > students.Count)
            {
                students.Add(student);
                return $"Added student {student.FirstName} {student.LastName}";
            }
            else
            {
                return "No seats in the classroom";
            }
        }

        public string DismissStudent(string firstName, string lastName)
        {
            Student toRemove = students.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);

            if (toRemove == null)
            {
                return "Student not found";
            }

            students.Remove(toRemove);

            return $"Dismissed student {firstName} {lastName}";
        }

        public string GetSubjectInfo(string subject)
        {
            Student[] info = students.Where(x => x.Subject == subject).ToArray();

            if (info.Length == 0)
            {
                return "No students enrolled for the subject";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Subject: {subject}");
            sb.AppendLine("Students:");

            foreach (var student in info)
            {
                sb.AppendLine($"{student.FirstName} {student.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public int GetStudentsCount()
        {
            return students.Count;
        }

        public Student GetStudent(string firstName, string lastName)
        {
            return students.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
        }
    }
}
